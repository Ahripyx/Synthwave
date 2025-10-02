using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GameLibrary
{
    public class ProjectileManager
    {
        // References to other managers
        private readonly Grid gridMain;
        private readonly PlayerManager playerManager;
        private readonly EnemyManager enemyManager;

        // Projectile list and mouse position
        private readonly List<Projectile> projectiles = new List<Projectile>();
        private Point mousePosition;

        // Constructor
        public ProjectileManager(Grid gridMain, PlayerManager playerManager, EnemyManager enemyManager)
        {
            this.gridMain = gridMain;
            this.playerManager = playerManager;
            this.enemyManager = enemyManager;
            mousePosition = new Point(playerManager.Player.Left, playerManager.Player.Top);
        }


        // Method to set mouse position
        public void SetMousePosition(Point pos) => mousePosition = pos;

        // Method to fire a projectile
        public void FireProjectile()
        {
            if (mousePosition == null || playerManager.Player == null) return;

            var sfx = new MediaPlayer();
            sfx.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/retrolaser.wav"));
            sfx.Volume = 0.1;
            sfx.Play();
            sfx.MediaEnded += (snd, args) => sfx.Dispose();

            Image projImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/projectile.png")),
                Width = 24,
                Height = 32
            };

            double playerX = playerManager.Player.Left + playerManager.Player.Width / 2 - projImage.Width / 2;
            double playerY = playerManager.Player.Top + playerManager.Player.Height / 2 - projImage.Height / 2;

            double mouseX = mousePosition.X - playerX;
            double mouseY = mousePosition.Y - playerY;
            double length = Math.Sqrt(mouseX * mouseX + mouseY * mouseY);
            if (length < 1) return;

            double speed = 60;
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

            projImage.Margin = new Windows.UI.Xaml.Thickness(playerX, playerY, 0, 0);
            projImage.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            projImage.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            gridMain.Children.Add(projImage);

            projectiles.Add(new Projectile { Image = projImage, X = playerX, Y = playerY, VelocityX = velocityX, VelocityY = velocityY });
        }


        // Method to update projectile positions and check for collisions
        public void UpdateProjectiles()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var proj = projectiles[i];
                proj.X += proj.VelocityX;
                proj.Y += proj.VelocityY;
                proj.Image.Margin = new Windows.UI.Xaml.Thickness(proj.X, proj.Y, 0, 0);

                if (proj.X < -proj.Width || proj.X > gridMain.ActualWidth || proj.Y < -proj.Height || proj.Y > gridMain.ActualHeight)
                {
                    gridMain.Children.Remove(proj.Image);
                    projectiles.RemoveAt(i);
                    continue;
                }
                bool hitEnemy = enemyManager.CheckProjectileHit(proj);
                if (hitEnemy)
                {
                    gridMain.Children.Remove(proj.Image);
                    projectiles.RemoveAt(i);
                }
            }
        }
    }
}
