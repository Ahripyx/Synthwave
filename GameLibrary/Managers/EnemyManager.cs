using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameLibrary
{
    public class EnemyManager
    {
        // References
        private readonly Grid gridMain;
        private readonly PlayerManager playerManager;
        private readonly Random rng = new Random();

        // Enemy list

        public List<GamePiece> Enemies { get; } = new List<GamePiece>();


        // Constructor
        public EnemyManager(Grid gridMain, PlayerManager playerManager)
        {
            this.gridMain = gridMain;
            this.playerManager = playerManager;
        }


        // Method for spawning enemies
        public void SpawnEnemy()
        {
            int enemySize = 48;
            int maxTries = 5;
            double minDistance = 100;
            double playerX = playerManager.Player.Left + playerManager.Player.Width / 2;
            double playerY = playerManager.Player.Top + playerManager.Player.Height / 2;

            int enemyX, enemyY, tries = 0;
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

            var enemy = CreatePiece("enemy", enemySize, enemyX, enemyY);
            Enemies.Add(enemy);

            // Remove after 10 sec
            var removalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            removalTimer.Tick += (s2, e2) =>
            {
                removalTimer.Stop();
                gridMain.Children.Remove(enemy.Image);
                Enemies.Remove(enemy);
            };
            removalTimer.Start();
        }

        // Method for updating enemy positions and checking collisions
        public void UpdateEnemies()
        {
            foreach (var enemy in Enemies.ToList())
            {
                enemy.MoveTowards(playerManager.Player.Left, playerManager.Player.Top, 3);

                bool isColliding = RectsOverlap(
                    playerManager.Player.Left, playerManager.Player.Top, playerManager.Player.Width, playerManager.Player.Height,
                    enemy.Left, enemy.Top, enemy.Width, enemy.Height);

                if (isColliding)
                {
                    playerManager.TakeDamage(10);
                    gridMain.Children.Remove(enemy.Image);
                    Enemies.Remove(enemy);
                }
            }
        }

        // Method to check if a projectile hits any enemy
        public bool CheckProjectileHit(GameLibrary.Projectile proj)
        {
            for (int j = Enemies.Count - 1; j >= 0; j--)
            {
                var enemy = Enemies[j];
                if (RectsOverlap(proj.X, proj.Y, proj.Width, proj.Height,
                                 enemy.Left, enemy.Top, enemy.Width, enemy.Height))
                {
                    gridMain.Children.Remove(enemy.Image);
                    Enemies.RemoveAt(j);
                    return true;
                }
            }
            return false;
        }


        // Helper method to create a game piece
        private GamePiece CreatePiece(string imgSrc, int size, int left, int top)
        {
            Image img = new Image
            {
                Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri($"ms-appx:///Assets/{imgSrc}.png")),
                Width = size,
                Height = size,
                Margin = new Windows.UI.Xaml.Thickness(left, top, 0, 0),
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left
            };
            gridMain.Children.Add(img);
            return new GamePiece(img);
        }


        // Helper method to check rectangle overlap
        private bool RectsOverlap(double x1, double y1, double w1, double h1,
                                  double x2, double y2, double w2, double h2)
        {
            return x1 < x2 + w2 && x1 + w1 > x2 &&
                   y1 < y2 + h2 && y1 + h1 > y2;
        }
    }
}
