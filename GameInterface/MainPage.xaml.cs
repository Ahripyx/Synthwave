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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using GameLibrary;
using Windows.UI.ViewManagement;

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
        private static GamePiece player;
        private static GamePiece collectible;
        private static Random rng = new Random();
        private HashSet<Windows.System.VirtualKey> pressedKeys = new HashSet<Windows.System.VirtualKey>();

        //Timers
        private DispatcherTimer gameTimer;
        private DispatcherTimer enemySpawnTimer;

        private List<GamePiece> enemies = new List<GamePiece>();

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            player = CreatePiece("player", 48, 50, 50);                      //create a GamePiece object associated with the pac-man image
            collectible = CreatePiece("collectible", 50, 150, 150);

            gridMain.Width = 1152;
            gridMain.Height = 648;

            //Setting preffered launch size and forcing it on user wehn game launches
            ApplicationView.PreferredLaunchViewSize = new Size(1152, 648); // avg retro game size
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            gridMain.Loaded += (s, e) =>
            {
                // Setting game timer so player movement is smooth
                gameTimer = new DispatcherTimer();
                gameTimer.Interval = TimeSpan.FromMilliseconds(16); // 60 FPS
                gameTimer.Tick += GameTimer_Tick;
                gameTimer.Start();

                // Setting enemy spawn timer
                enemySpawnTimer = new DispatcherTimer();
                enemySpawnTimer.Interval = TimeSpan.FromSeconds(2);
                enemySpawnTimer.Tick += EnemySpawnTimer_Tick;
                enemySpawnTimer.Start();
            };
        }

        private void EnemySpawnTimer_Tick(object sender, object e)
        {
            // Spawn enemy at random location
            int enemyX = rng.Next(0, (int)(gridMain.Width - 48));
            int enemyY = rng.Next(0, (int)(gridMain.Height - 48));
            var newEnemy = CreatePiece("enemy", 48, enemyX, enemyY);
            enemies.Add(newEnemy);

            // Removing enemy after 3 seconds: Using for testing purposes
            var removalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            removalTimer.Tick += (s2, e2) =>
            {
                removalTimer.Stop();
                gridMain.Children.Remove(newEnemy.Image);
                enemies.Remove(newEnemy);
            };
            removalTimer.Start();
        }

        /// This method creates the Image object (to display the picture) and sets its properties.
        private GamePiece CreatePiece(string imgSrc, int size, int left, int top)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imgSrc}.png"));
            img.Width = size;
            img.Height = size;
            img.Name = $"img{imgSrc}" + Guid.NewGuid().ToString("N");
            img.Margin = new Thickness(left, top, 0, 0);
            img.VerticalAlignment = VerticalAlignment.Top;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            gridMain.Children.Add(img);

            return new GamePiece(img);
        }


        // Event handlers for key presses
        private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Add(e.VirtualKey);
        }
        private void CoreWindow_KeyUp(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Remove(e.VirtualKey);
        }

        // Game loop tick event
        private void GameTimer_Tick(object sender, object e)
        {
            double gridWidth = gridMain.ActualWidth;
            double gridHeight = gridMain.ActualHeight;

            if (gridWidth <= 0 || gridHeight <= 0) return;

            player.Move(pressedKeys, gridWidth, gridHeight);

            // Move all active enemies toward player
            foreach (var enemy in enemies.ToList())
            {
                enemy.MoveTowards(player.Left, player.Top, 1.5);
            }
        }
    }
}
