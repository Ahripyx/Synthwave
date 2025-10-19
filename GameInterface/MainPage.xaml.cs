using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.Media.Playback;
using Windows.Media.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//Project: Lab 1A - UWP Game
//Student Name:
//Date:

namespace GameInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GameManager gameManager;

        public MainPage()
        {
            this.InitializeComponent();

            // Set grid size and preferred launch size
            gridMain.Width = 1152;
            gridMain.Height = 648;
            ApplicationView.PreferredLaunchViewSize = new Size(1152, 648);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // Handle window size changes to keep gridMain centered
            Window.Current.SizeChanged += (s, e) =>
            {
                gridMain.HorizontalAlignment = HorizontalAlignment.Center;
                gridMain.VerticalAlignment = VerticalAlignment.Center;
            };

            // Initialize game manager
            gameManager = new GameManager(gridMain, Frame);

            var hud = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                IsHitTestVisible = false // don't block input
            };

            var scoreText = new TextBlock
            {
                Text = "Score: 0",
                FontSize = 20,
                Foreground = new SolidColorBrush(Windows.UI.Colors.Black),
                Margin = new Thickness(10, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                IsHitTestVisible = false
            };
            hud.Children.Add(scoreText);

            gridMain.Children.Add(hud);

            gameManager.RegisterScoreTextBlock(scoreText);

            gameManager.GameOver += (finalScore) =>
            {
                Frame.Navigate(typeof(GameOver), finalScore);
            };

            // Event handlers
            Window.Current.CoreWindow.KeyDown += (s, e) => gameManager.OnKeyDown(e.VirtualKey);
            Window.Current.CoreWindow.KeyUp += (s, e) => gameManager.OnKeyUp(e.VirtualKey);
            gridMain.PointerMoved += (s, e) => gameManager.OnPointerMoved(e.GetCurrentPoint(gridMain).Position);
        }
    }
}
