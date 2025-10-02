using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameLibrary
{
    public class PlayerManager
    {
        // Player properties
        public GamePiece Player { get; }
        public int Health { get; private set; } = 3;
        public int MaxHealth { get; } = 3;

        // References
        private readonly Grid gridMain;
        private readonly HashSet<VirtualKey> pressedKeys = new HashSet<VirtualKey>();


        // Constructor
        public PlayerManager(Grid gridMain)
        {
            this.gridMain = gridMain;
            Player = CreatePiece("player", 48, 50, 50);
        }

        // Key event handlers
        public void OnKeyDown(VirtualKey key) => pressedKeys.Add(key);
        public void OnKeyUp(VirtualKey key) => pressedKeys.Remove(key);

        // Method to update player movement
        public void UpdatePlayerMovement()
        {
            Player.Move(pressedKeys, gridMain.ActualWidth, gridMain.ActualHeight);
        }


        // Method to apply damage to the player
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        // Method to heal the player
        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
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
    }
}
