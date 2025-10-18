using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GameLibrary
{
    public class HealthBarManager
    {
        // UI Elements
        private readonly Rectangle healthBarBackground;
        private readonly Rectangle healthBar;
        private readonly TextBlock healthText;

        private const double BarWidth = 400;
        private const double BarHeight = 40;

        // Constructor
        public HealthBarManager(Grid gridMain)
        {
            healthBarBackground = new Rectangle
            {
                Width = 400,
                Height = 40,
                Fill = new SolidColorBrush(Windows.UI.Colors.Gray),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 10, 0, 0)
            };

            healthBar = new Rectangle
            {
                Width = 400,
                Height = 40,
                Fill = new SolidColorBrush(Windows.UI.Colors.Green),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 10, 0, 0)
            };

            healthText = new TextBlock
            {
                Width = BarWidth,
                Height = BarHeight,
                Text = "100%",
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                FontSize = 24,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(30, 10, 0, 0),
                IsHitTestVisible = false
            };

            gridMain.Children.Add(healthBarBackground);
            gridMain.Children.Add(healthBar);
            gridMain.Children.Add(healthText);
        }

        // Method to update health bar based on player's health
        public void UpdateHealthBar(int playerHealth, int maxHealth)
        {
            if (maxHealth <= 0) maxHealth = 1;
            double percent = Math.Max(0.0, Math.Min(1.0, (double)playerHealth / maxHealth));
            healthBar.Width = BarWidth * percent;
            healthText.Text = $"{(int)Math.Round(percent * 100)}%";
        }
    }
}
