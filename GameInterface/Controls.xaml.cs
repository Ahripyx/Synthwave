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
    public sealed partial class Controls : Page
    {

        private Grid grid;
        public Controls()
        {
            this.InitializeComponent();

            // Creating new grid
            grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            /// Setting grid size
            grid.Width = 1152;
            grid.Height = 648;

            // Setting preferred window size
            double extraPadding = 80; 
            var preferredWidth = grid.Width + extraPadding;
            var preferredHeight = grid.Height + extraPadding;

            ApplicationView.PreferredLaunchViewSize = new Size(preferredWidth, preferredHeight);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // Defining brushes
            var headingBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 46, 204, 113)); // mint
            var backBg = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 60, 60, 60)); // dark grey
            var backFg = new SolidColorBrush(Windows.UI.Colors.White);
            var buttonBorder = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 30, 30)); // dark border

            // Defining rows and columns
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }); 
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); 
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            // Creating Back button
            var backButton = new Button
            {
                Content = "<- Back",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 20, 0, 0),
                Background = backBg,
                Foreground = backFg,
                BorderBrush = buttonBorder,
                BorderThickness = new Thickness(2)
            };
            backButton.Click += BackButton_Click;
            Grid.SetRow(backButton, 0);
            Grid.SetColumn(backButton, 0);


            // Creating StackPanel for content
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 18,
                Margin = new Thickness(0, 40, 0, 0)
            };

            // Title
            var title = new TextBlock
            {
                Text = "Game Controls",
                Foreground = headingBrush,
                FontSize = 44,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };

            // Adding title and controls to StackPanel
            stackPanel.Children.Add(title);

            stackPanel.Children.Add(CreateControlText("Move Up:", "W or Up-Arrow"));
            stackPanel.Children.Add(CreateControlText("Move Down:", "S or Down-Arrow"));
            stackPanel.Children.Add(CreateControlText("Move Left:", "A or Left-Arrow"));
            stackPanel.Children.Add(CreateControlText("Move Right:", "D or Right-Arrow"));
            stackPanel.Children.Add(CreateControlText("Aim:", "Mouse"));

            Grid.SetRow(stackPanel, 1);
            Grid.SetColumn(stackPanel, 0);

            grid.Children.Add(backButton);
            grid.Children.Add(stackPanel);

            // Creating border around grid
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 20, 20)),
                BorderThickness = new Thickness(6),
                CornerRadius = new CornerRadius(4),
                Background = new SolidColorBrush(Windows.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = grid
            };

            this.Content = border;
        }

        // Event handler for Back button
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        // Helper method to create control text blocks
        private StackPanel CreateControlText(string action, string keys)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
            panel.Children.Add(new TextBlock
            {
                Text = action,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 68, 68, 68)),
                FontSize = 26,
                FontWeight = Windows.UI.Text.FontWeights.SemiBold
            });
            panel.Children.Add(new TextBlock
            {
                Text = keys,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 160, 195)),
                FontSize = 26
            });
            return panel;
        }
    }
}
