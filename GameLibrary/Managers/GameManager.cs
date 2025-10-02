using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.PlayTo;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameLibrary
{
    public class GameManager
    {
        // Referencing Managers
        public PlayerManager Player { get; }
        public EnemyManager Enemies { get; }
        public ProjectileManager Projectiles { get; }
        public HealthBarManager HealthBar { get; }


        // References to UI elements
        private readonly Grid gridMain;
        private readonly Frame frame;

        // Timers
        private DispatcherTimer gameTimer;
        private DispatcherTimer enemySpawnTimer;
        private DispatcherTimer projectileTimer;

        // Constructor
        public GameManager(Grid gridMain, Frame frame)
        {
            this.gridMain = gridMain;
            this.frame = frame;

            HealthBar = new HealthBarManager(gridMain);
            Player = new PlayerManager(gridMain);
            Enemies = new EnemyManager(gridMain, Player);
            Projectiles = new ProjectileManager(gridMain, Player, Enemies);

            SetupTimers();
        }

        // Timer setup method
        private void SetupTimers()
        {
            // Game loop
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; // 60 FPS
            gameTimer.Tick += (s, e) => GameLoop();
            gameTimer.Start();

            // Enemy spawning
            enemySpawnTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            enemySpawnTimer.Tick += (s, e) => Enemies.SpawnEnemy();
            enemySpawnTimer.Start();

            // Projectile firing
            projectileTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            projectileTimer.Tick += (s, e) => Projectiles.FireProjectile();
            projectileTimer.Start();
        }

        // Method to stop all timers
        private void StopAllTimers()
        {
            gameTimer.Stop();
            enemySpawnTimer.Stop();
            projectileTimer.Stop();
        }


        // Main game loop method
        public event Action GameOver;


        // Main game loop method
        private void GameLoop()
        {
            Projectiles.UpdateProjectiles();
            Player.UpdatePlayerMovement();
            Enemies.UpdateEnemies();

            HealthBar.UpdateHealthBar(Player.Health, Player.MaxHealth);

            if (Player.Health <= 0)
            {
                StopAllTimers();
                GameOver?.Invoke();
            }
        }

        // Input handling methods
        public void OnKeyDown(Windows.System.VirtualKey key) => Player.OnKeyDown(key);
        public void OnKeyUp(Windows.System.VirtualKey key) => Player.OnKeyUp(key);
        public void OnPointerMoved(Point mousePos) => Projectiles.SetMousePosition(mousePos);
    }
}
