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
        private readonly Rectangle healthBarBackground;
        private readonly Rectangle healthBar;

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

            gridMain.Children.Add(healthBarBackground);
            gridMain.Children.Add(healthBar);
        }

        public void UpdateHealthBar(int playerHealth, int maxHealth)
        {
            double percent = System.Math.Max(0, (double)playerHealth / maxHealth);
            healthBar.Width = 400 * percent;
        }
    }
}
