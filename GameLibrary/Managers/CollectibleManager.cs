using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary.Entities;
using Windows.Gaming.UI;
using Windows.UI.Xaml.Controls;

namespace GameLibrary.Managers
{
    public class CollectibleManager
    {
        private readonly Grid gridMain;
        private readonly PlayerManager playerManager;
        private readonly Random rng = new Random();
        private readonly string[] collectibleTypes = { "powerup", "healthup" };
        private readonly List<Collectible> collectibles = new List<Collectible>();

        private readonly HealthBarManager healthBarManager;

        public CollectibleManager(Grid gridMain, PlayerManager playerManager, HealthBarManager healthBarManager)
        {
            this.gridMain = gridMain;
            this.playerManager = playerManager;
            this.healthBarManager = healthBarManager;
        }

        public void SpawnCollectible()
        {
            string type = collectibleTypes[rng.Next(collectibleTypes.Length)];
            int size = 40;
            double x = rng.Next(0, (int)(gridMain.ActualWidth - size));
            double y = rng.Next(0, (int)(gridMain.ActualHeight - size));

            Image img = new Image
            {
                Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri($"ms-appx:///Assets/{type}.png")),
                Width = size,
                Height = size,
                Margin = new Windows.UI.Xaml.Thickness(x, y, 0, 0),
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left
            };
            gridMain.Children.Add(img);
            var piece = new GamePiece(img);
            collectibles.Add(new Collectible(piece, type));
        }

        public void UpdateCollectibles()
        {
            for (int i = collectibles.Count - 1; i >= 0; i--)
            {
                var collectible = collectibles[i];
                if (RectsOverlap(playerManager.Player.Left, playerManager.Player.Top,
                                 playerManager.Player.Width, playerManager.Player.Height,
                                 collectible.Piece.Left, collectible.Piece.Top, collectible.Piece.Width, collectible.Piece.Height))
                {
                    OnCollectiblePickedUp(collectible);
                    gridMain.Children.Remove(collectible.Piece.Image);
                    collectibles.RemoveAt(i);
                }
            }
        }

        private void OnCollectiblePickedUp(Collectible collectible)
        {


            if (collectible.Type == "healthup")
            {
                if (playerManager.Health < playerManager.MaxHealth)
                {
                    playerManager.Heal(20);
                    healthBarManager.UpdateHealthBar(playerManager.Health, playerManager.MaxHealth);
                }
            }
        }

        private bool RectsOverlap(double x1, double y1, double w1, double h1,
                                  double x2, double y2, double w2, double h2)
        {
            return x1 < x2 + w2 && x1 + w1 > x2 &&
                   y1 < y2 + h2 && y1 + h1 > y2;
        }
    }
}
