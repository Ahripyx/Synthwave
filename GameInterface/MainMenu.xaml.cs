using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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

        private Grid grid;

        public MainMenu()
        {
            this.InitializeComponent();

            // Creating the main grid layout
            grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            grid.Width = 1152;
            grid.Height = 648;

            double extraPadding = 80; // increase if still clipped, decrease if too large
            var preferredWidth = grid.Width + extraPadding;
            var preferredHeight = grid.Height + extraPadding;

            ApplicationView.PreferredLaunchViewSize = new Size(preferredWidth, preferredHeight);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // Defining rows and columns
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            var headingBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 46, 204, 113)); // mint/green
            var buttonBg = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 45, 125, 230));     // blue
            var buttonFg = new SolidColorBrush(Windows.UI.Colors.White);
            var buttonBorder = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 30, 30));

            // Creating title and buttons
            var title = new TextBlock
            {
                Text = "Synthwave",
                Foreground = headingBrush,
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
                Background = buttonBg,   // Neon Pink
                Foreground = buttonFg,  // Neon Cyan
                BorderBrush = buttonBorder, // Neon Yellow
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
                Background = buttonBg,   // Neon Pink
                Foreground = buttonFg,  // Neon Cyan
                BorderBrush = buttonBorder, // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            controlsButton.Click += ControlsButton_Click;
            Grid.SetRow(controlsButton, 2);
            Grid.SetColumn(controlsButton, 1);

            // Adding elements to the grid
            grid.Children.Add(title);
            grid.Children.Add(playButton);
            grid.Children.Add(controlsButton);

            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 20, 20)), // dark border
                BorderThickness = new Thickness(6),
                CornerRadius = new CornerRadius(4),
                Background = new SolidColorBrush(Windows.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = grid
            };

            this.Content = border;
        }


        // Button event handlers
        private void ControlsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Controls));
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the game page
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
