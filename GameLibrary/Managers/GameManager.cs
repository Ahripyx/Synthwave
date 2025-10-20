using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary.Managers;
using Windows.Foundation;
using Windows.Media.PlayTo;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GameLibrary
{
    public class GameManager
    {
        // Referencing Managers
        public PlayerManager Player { get; }
        public EnemyManager Enemies { get; }
        public ProjectileManager Projectiles { get; }
        public HealthBarManager HealthBar { get; }

        public CollectibleManager Collectibles { get; }


        // References to UI elements
        private readonly Grid gridMain;
        private readonly Frame frame;

        // Timers
        private DispatcherTimer gameTimer;
        private DispatcherTimer enemySpawnTimer;
        private DispatcherTimer projectileTimer;
        private DispatcherTimer collectibleTimer;
        private DispatcherTimer enemySpawnAccelerateTimer;

        private double enemySpawnInterval = 2.0;
        private const double MinEnemySpawnInterval = 0.3;
        private const double EnemySpawnDecrement = 0.2;

        private readonly ScoreManager scoreManager;
        public int Score => scoreManager.Score;


        // Constructor
        public GameManager(Grid gridMain, Frame frame)
        {
            this.gridMain = gridMain;
            this.frame = frame;

            HealthBar = new HealthBarManager(gridMain);
            Player = new PlayerManager(gridMain);
            Enemies = new EnemyManager(gridMain, Player);
            Projectiles = new ProjectileManager(gridMain, Player, Enemies);
            Collectibles = new CollectibleManager(gridMain, Player, HealthBar, this);

            scoreManager = new ScoreManager(10, 5, 2000);

            Enemies.EnemyKilled += () => scoreManager.RegisterKill();

            Player.Damaged += () => scoreManager.ResetCombo();
            //UpdateScoreText();

            SetupTimers();
        }

        // Allow the page to provide the TextBlock that will display the score.
        public void RegisterScoreTextBlock(TextBlock tb)
        {
            if (tb == null) throw new ArgumentNullException(nameof(tb));
            scoreManager.RegisterScoreTextBlock(tb);
        }

        public void RegisterComboTextBlock(TextBlock tb)
        {
            if (tb == null) throw new ArgumentNullException(nameof(tb));
            scoreManager.RegisterComboTextBlock(tb);
        }

        // Timer setup method
        private void SetupTimers()
        {
            // Game loop
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; // 60 FPS
            gameTimer.Tick += (s, e) => GameLoop();
            gameTimer.Start();

            // Enemy spawning
            enemySpawnTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(enemySpawnInterval) };
            enemySpawnTimer.Tick += (s, e) => Enemies.SpawnEnemy();
            enemySpawnTimer.Start();

            // Enemy spawn acceleration
            enemySpawnAccelerateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            enemySpawnAccelerateTimer.Tick += (s, e) =>
            {
                enemySpawnInterval = Math.Max(MinEnemySpawnInterval, enemySpawnInterval - EnemySpawnDecrement);
                enemySpawnTimer.Stop();
                enemySpawnTimer.Interval = TimeSpan.FromSeconds(enemySpawnInterval);
                enemySpawnTimer.Start();
            };
            enemySpawnAccelerateTimer.Start();

            // Projectile firing
            projectileTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            projectileTimer.Tick += (s, e) => Projectiles.FireProjectile();
            projectileTimer.Start();

            // Collectible spawning
            collectibleTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            collectibleTimer.Tick += (s, e) => Collectibles.SpawnCollectible();
            collectibleTimer.Start();
        }

        // Method to stop all timers
        private void StopAllTimers()
        {
            gameTimer.Stop();
            enemySpawnTimer.Stop();
            enemySpawnAccelerateTimer.Stop();
            projectileTimer.Stop();
            collectibleTimer.Stop();
        }


        // Main game loop method
        public event Action<int> GameOver;


        // Main game loop method
        private void GameLoop()
        {
            Projectiles.UpdateProjectiles();
            Player.UpdatePlayerMovement();
            Enemies.UpdateEnemies();
            Collectibles.UpdateCollectibles();

            HealthBar.UpdateHealthBar(Player.Health, Player.MaxHealth);

            if (Player.Health <= 0)
            {
                StopAllTimers();
                GameOver?.Invoke(scoreManager.Score);
            }
        }

        // Input handling methods
        public void OnKeyDown(Windows.System.VirtualKey key) => Player.OnKeyDown(key);
        public void OnKeyUp(Windows.System.VirtualKey key) => Player.OnKeyUp(key);
        public void OnPointerMoved(Point mousePos) => Projectiles.SetMousePosition(mousePos);

        // Method to adjust projectile firing interval
        public void SetProjectileFireInterval(TimeSpan interval)
        {
            projectileTimer.Stop();
            projectileTimer.Interval = interval;
            projectileTimer.Start();
        }
    }
}
