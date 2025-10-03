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
    public sealed partial class Controls : Page
    {
        public Controls()
        {
            this.InitializeComponent();

            var grid = new Grid
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x18, 0x1A, 0x20)),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }); // Back button
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Content

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var backButton = new Button
            {
                Content = "<- Back",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 20, 0, 0),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)),
                BorderThickness = new Thickness(2)
            };
            backButton.Click += BackButton_Click;
            Grid.SetRow(backButton, 0);
            Grid.SetColumn(backButton, 0);

            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 18,
                Margin = new Thickness(0, 40, 0, 0)
            };

            var title = new TextBlock
            {
                Text = "Game Controls",
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x70, 0xFF, 0xAF)),
                FontSize = 44,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };

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

            this.Content = grid;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private StackPanel CreateControlText(string action, string keys)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
            panel.Children.Add(new TextBlock
            {
                Text = action,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)),
                FontSize = 26,
                FontWeight = Windows.UI.Text.FontWeights.SemiBold
            });
            panel.Children.Add(new TextBlock
            {
                Text = keys,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),
                FontSize = 26
            });
            return panel;
        }
    }
}
