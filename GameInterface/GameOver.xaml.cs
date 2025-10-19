using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameLibrary.Managers;
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
    public sealed partial class GameOver : Page
    {
        private readonly HighScoreManager highScoreManager = new HighScoreManager();
        private StackPanel scoresPanel;
        private int? pendingScore;

        public GameOver()
        {
            this.InitializeComponent();

            // Creating new grid
            var grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Defining rows and columns
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            // Creating title and buttons
            var title = new TextBlock
            {
                Text = "Game Over",
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xFF, 0x70, 0x70)),
                FontSize = 60,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 30)
            };
            Grid.SetRow(title, 0);
            Grid.SetColumn(title, 0);
            Grid.SetColumnSpan(title, 3);

            var retryButton = new Button
            {
                Content = "Retry",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),   // Neon Pink
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),  // Neon Cyan
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)), // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            retryButton.Click += RetryButton_Click;
            Grid.SetRow(retryButton, 1);
            Grid.SetColumn(retryButton, 1);

            var menuButton = new Button
            {
                Content = "Main Menu",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),   // Neon Pink
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),  // Neon Cyan
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)), // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            menuButton.Click += MenuButton_Click;
            Grid.SetRow(menuButton, 2);
            Grid.SetColumn(menuButton, 1);

            var exitButton = new Button
            {
                Content = "Quit Game",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 200)),   // Neon Pink
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 247)),  // Neon Cyan
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 247, 0)), // Neon Yellow
                BorderThickness = new Thickness(2)
            };
            exitButton.Click += ExitButton_Click;
            Grid.SetRow(exitButton, 3);
            Grid.SetColumn(exitButton, 1);

            // Adding elements to grid
            grid.Children.Add(title);
            grid.Children.Add(retryButton);
            grid.Children.Add(menuButton);
            grid.Children.Add(exitButton);

            //Scores panel
            scoresPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 8,
                Margin = new Thickness(10, 8, 10, 8)
            };
            Grid.SetRow(scoresPanel, 4);
            Grid.SetColumn(scoresPanel, 0);
            grid.Children.Add(scoresPanel);

            this.Content = grid;

            this.Loaded += async (s, e) =>
            {
                await highScoreManager.LoadAsync();
            };
        }

        private void RefreshScoresPanel()
        {
            scoresPanel.Children.Clear();

            scoresPanel.Children.Add(new TextBlock
            {
                Text = "High Scores",
                FontSize = 32,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x70, 0xFF, 0xAF)),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            if (highScoreManager.Entries.Count == 0)
            {
                scoresPanel.Children.Add(new TextBlock
                {
                    Text = "No high scores yet.",
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
            }
            else
            {
                int rank = 1;
                foreach(var e in highScoreManager.Entries)
                {
                    scoresPanel.Children.Add(new TextBlock
                    {
                        Text = $"{rank}. {e.Name} — {e.Score}",
                        FontSize = 20,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                    rank++;
                }
            }

            if (pendingScore.HasValue)
            {
                scoresPanel.Children.Add(new TextBlock
                {
                    Text = $"Your Score: {pendingScore.Value}",
                    FontSize = 20,
                    Margin = new Thickness(0, 8, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                var nameBox = new TextBox
                {
                    PlaceholderText = "Enter your name",
                    Text = "Player",
                    FontSize = 240,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                scoresPanel.Children.Add(nameBox);

                var saveButton = new Button
                {
                    Content = "Save Score",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 6, 0, 0)
                };
                scoresPanel.Children.Add(saveButton);

                var resultText = new TextBlock
                {
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 6, 0, 0)
                };
                scoresPanel.Children.Add(resultText);

                saveButton.Click += async (s, e) =>
                {
                    saveButton.IsEnabled = false;
                    string name = nameBox.Text;
                    int newScore = pendingScore.Value;
                    bool isTop = highScoreManager.AddScore(string.IsNullOrWhiteSpace(name) ? "Player" : name, newScore);
                    await highScoreManager.SaveAsync();
                    resultText.Text = isTop ? "Congratulations — New High Score!" : "Score saved.";
                    pendingScore = null;
                    RefreshScoresPanel();
                };
            }
        }
        // Button event handlers
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
