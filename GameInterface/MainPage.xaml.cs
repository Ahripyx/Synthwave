using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//Project: Lab 1A - UWP Game
//Student Name:
//Date:

namespace GameInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static GamePiece player;
        private static GamePiece collectible;
        private static Random rng = new Random();
        private HashSet<Windows.System.VirtualKey> pressedKeys = new HashSet<Windows.System.VirtualKey>();

        //Timers
        private DispatcherTimer gameTimer;
        private DispatcherTimer enemySpawnTimer;

        private List<GamePiece> enemies = new List<GamePiece>();

        private Rectangle healthBarBackground;
        private Rectangle healthBar;
        private int playerHealth = 100;
        private const int maxHealth = 100;

        private List<Projectile> projectiles = new List<Projectile>();
        private DispatcherTimer projectileTimer;
        private Point mousePosition;

        public MainPage()
        {
            this.InitializeComponent();

            // Create the gray health bar background
            healthBarBackground = new Rectangle
            {
                Width = 400,
                Height = 40,
                Fill = new SolidColorBrush(Windows.UI.Colors.Gray),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 10, 0, 0) 
            };

            // Create the green health bar
            healthBar = new Rectangle
            {
                Width = 400,
                Height = 40,
                Fill = new SolidColorBrush(Windows.UI.Colors.Green),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 10, 0, 0)
            };

            // Adding health bar to grid
            gridMain.Children.Add(healthBarBackground);
            gridMain.Children.Add(healthBar);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            player = CreatePiece("player", 48, 50, 50);

            gridMain.Width = 1152;
            gridMain.Height = 648;

            //Setting preffered launch size and forcing it on user wehn game launches
            ApplicationView.PreferredLaunchViewSize = new Size(1152, 648); // avg retro game size
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            gridMain.PointerMoved += GridMain_PointerMoved;

            projectileTimer = new DispatcherTimer();
            projectileTimer.Interval = TimeSpan.FromSeconds(0.5);
            projectileTimer.Tick += ProjectileTimer_Tick;
            projectileTimer.Start();

            gridMain.Loaded += (s, e) =>
            {
                // Setting game timer so player movement is smooth
                gameTimer = new DispatcherTimer();
                gameTimer.Interval = TimeSpan.FromMilliseconds(16); // 60 FPS
                gameTimer.Tick += GameTimer_Tick;
                gameTimer.Start();

                // Setting enemy spawn timer
                enemySpawnTimer = new DispatcherTimer();
                enemySpawnTimer.Interval = TimeSpan.FromSeconds(2);
                enemySpawnTimer.Tick += EnemySpawnTimer_Tick;
                enemySpawnTimer.Start();
            };
        }
        private void ProjectileTimer_Tick(object sender, object e)
        {
            if (mousePosition == null || player == null) return;

            Image projImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/projectile.png")),
                Width = 24,
                Height = 32
            };

            double playerX = player.Left + player.Width / 2 - projImage.Width / 2;
            double playerY = player.Top + player.Height / 2 - projImage.Height / 2;

            double mouseX = mousePosition.X - playerX;
            double mouseY = mousePosition.Y - playerY;
            double length = Math.Sqrt(mouseX * mouseX + mouseY * mouseY);
            if (length < 1) return; // Prevent division by zero

            double speed = 60; // Projectile speed
            double velocityX = (mouseX / length) * speed;
            double velocityY = (mouseY / length) * speed;

            double angle = Math.Atan2(mouseY, mouseX) * 180.0 / Math.PI - 90.0;
            var rotate = new RotateTransform
            {
                Angle = angle,
                CenterX = projImage.Width / 2,
                CenterY = projImage.Height / 2
            };
            projImage.RenderTransform = rotate;

            projImage.Margin = new Thickness(playerX, playerY, 0, 0);
            projImage.VerticalAlignment = VerticalAlignment.Top;
            projImage.HorizontalAlignment = HorizontalAlignment.Left;
            gridMain.Children.Add(projImage);

            projectiles.Add(new Projectile { Image = projImage, X = playerX, Y = playerY, VelocityX = velocityX, VelocityY = velocityY });
        }

        private void EnemySpawnTimer_Tick(object sender, object e)
        {

            int enemySize = 48;
            int maxTries = 5;
            int tries = 0;
            double minDistance = 100; // Minimum distance from player

            double playerX = player.Left + player.Width / 2;
            double playerY = player.Top + player.Height / 2;

            int enemyX, enemyY;
            double enemyCenterX, enemyCenterY, distance;

            do
            {
                enemyX = rng.Next(0, (int)(gridMain.Width - enemySize));
                enemyY = rng.Next(0, (int)(gridMain.Height - enemySize));
                enemyCenterX = enemyX + enemySize / 2.0;
                enemyCenterY = enemyY + enemySize / 2.0;

                double dx = playerX - enemyCenterX;
                double dy = playerY - enemyCenterY;
                distance = Math.Sqrt(dx * dx + dy * dy);

                tries++;
            }
            while (distance < minDistance && tries < maxTries);
            var newEnemy = CreatePiece("enemy", 48, enemyX, enemyY);
            enemies.Add(newEnemy);

            // Removing enemy after 10 seconds: Using for testing purposes
            var removalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            removalTimer.Tick += (s2, e2) =>
            {
                removalTimer.Stop();
                gridMain.Children.Remove(newEnemy.Image);
                enemies.Remove(newEnemy);
            };
            removalTimer.Start();
        }

        // Game loop tick event
        private void GameTimer_Tick(object sender, object e)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var proj = projectiles[i];
                proj.X += proj.VelocityX;
                proj.Y += proj.VelocityY;
                // Update projectile position
                proj.Image.Margin = new Thickness(proj.X, proj.Y, 0, 0);
                // Remove projectile if it goes off-screen
                if (proj.X < -proj.Width || proj.X > gridMain.ActualWidth || proj.Y < -proj.Height || proj.Y > gridMain.ActualHeight)
                {
                    gridMain.Children.Remove(proj.Image);
                    projectiles.RemoveAt(i);
                    continue;
                }
                // Check collision with enemies
                bool hitEnemy = false;
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    var enemy = enemies[j];
                    if (RectsOverlap(proj.X, proj.Y, proj.Width, proj.Height,
                                     enemy.Left, enemy.Top, enemy.Width, enemy.Height))
                    {
                        hitEnemy = true;
                        gridMain.Children.Remove(enemy.Image);
                        enemies.RemoveAt(j);
                        break;
                    }
                }
                if (hitEnemy)
                {
                    gridMain.Children.Remove(proj.Image);
                    projectiles.RemoveAt(i);
                }
            }

            double gridWidth = gridMain.ActualWidth;
            double gridHeight = gridMain.ActualHeight;

            if (gridWidth <= 0 || gridHeight <= 0) return;

            player.Move(pressedKeys, gridWidth, gridHeight);

            // Move all active enemies toward player
            foreach (var enemy in enemies.ToList())
            {
                enemy.MoveTowards(player.Left, player.Top, 3);

                bool isColliding = RectsOverlap(player.Left, player.Top, player.Width, player.Height,
                                            enemy.Left, enemy.Top, enemy.Width, enemy.Height);

                if (isColliding)
                {
                    playerHealth -= 10;
                    UpdateHealthBar();
                    gridMain.Children.Remove(enemy.Image);
                    enemies.Remove(enemy);
                }
            }
        }

        /// This method creates the Image object (to display the picture) and sets its properties.
        private GamePiece CreatePiece(string imgSrc, int size, int left, int top)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imgSrc}.png"));
            img.Width = size;
            img.Height = size;
            img.Name = $"img{imgSrc}" + Guid.NewGuid().ToString("N");
            img.Margin = new Thickness(left, top, 0, 0);
            img.VerticalAlignment = VerticalAlignment.Top;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            gridMain.Children.Add(img);

            return new GamePiece(img);
        }

        // Event handlers for key presses
        private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Add(e.VirtualKey);
        }
        private void CoreWindow_KeyUp(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Remove(e.VirtualKey);
        }

        // This method checks whether two rectangles overlap (collide) in 2D space.
        // (x1, y1, w1, h1): position and size of the first rectangle
        // (x2, y2, w2, h2): position and size of the second rectangle
        // The logic: Two rectangles overlap if their projections on both the X and Y axes overlap.
        // Returns true if the rectangles intersect, false otherwise.
        private bool RectsOverlap(double x1, double y1, double w1, double h1,
                                 double x2, double y2, double w2, double h2)
        {
            return x1 < x2 + w2 && x1 + w1 > x2 &&   // X-axis overlap
                   y1 < y2 + h2 && y1 + h1 > y2;     // Y-axis overlap
        }

        private void UpdateHealthBar()
        {
            double percent = Math.Max(0, (double)playerHealth / maxHealth);
            healthBar.Width = 400 * percent;
        }

        private void GridMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            mousePosition = e.GetCurrentPoint(gridMain).Position;
        }
    }
}
