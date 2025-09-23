using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();

            var grid = new Grid
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x18, 0x1A, 0x20)),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            var title = new TextBlock
            {
                Text = "Synthwave",
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x70, 0xFF, 0xAF)),
                FontSize = 60,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 30)
            };
            Grid.SetRow(title, 0);
            Grid.SetColumn(title, 0);
            Grid.SetColumnSpan(title, 3);

            var playButton = new Button
            {
                Content = "Play",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),   // Neon Pink
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),  // Neon Cyan
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)), // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            playButton.Click += PlayButton_Click;
            Grid.SetRow(playButton, 1);
            Grid.SetColumn(playButton, 1);
            
            var controlsButton = new Button
            {
                Content = "Controls",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),   // Neon Pink
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),  // Neon Cyan
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)), // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            controlsButton.Click += ControlsButton_Click;
            Grid.SetRow(controlsButton, 2);
            Grid.SetColumn(controlsButton, 1);

            grid.Children.Add(title);
            grid.Children.Add(playButton);
            grid.Children.Add(controlsButton);

            this.Content = grid;
        }

        private void ControlsButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the game page
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
