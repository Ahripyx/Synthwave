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
        private HashSet<Windows.System.VirtualKey> pressedKeys = new HashSet<Windows.System.VirtualKey>();
        private DispatcherTimer gameTimer;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            player = CreatePiece("player", 48, 50, 50);                      //create a GamePiece object associated with the pac-man image
            collectible = CreatePiece("collectible", 50, 150, 150);


            //Setting preffered launch size and forcing it on user wehn game launches
            ApplicationView.PreferredLaunchViewSize = new Size(1152, 648); // avg retro game size
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // Start the game loop
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        /// <summary>
        /// This method creates the Image object (to display the picture) and sets its properties.
        /// It adds the image to the screen.
        /// Then it calls the GamePiece constructor, passing the Image object as a parameter.
        /// </summary>
        /// <param name="imgSrc">Name of the image file</param>
        /// <param name="size">Size in pixels (used for both dimensions, the images are square)</param>
        /// <param name="left">Left location relative to parent</param>
        /// <param name="top">Top location relative to parent</param>
        /// <returns></returns>
        private GamePiece CreatePiece(string imgSrc, int size, int left, int top)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imgSrc}.png"));
            img.Width = size;
            img.Height = size;
            img.Name = $"img{imgSrc}";
            img.Margin = new Thickness(left, top, 0, 0);
            img.VerticalAlignment = VerticalAlignment.Top;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            gridMain.Children.Add(img);

            return new GamePiece(img);
        }



        private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Add(e.VirtualKey);
        }
        private void CoreWindow_KeyUp(object sender, Windows.UI.Core.KeyEventArgs e)
        {
            pressedKeys.Remove(e.VirtualKey);
        }
        private async void GameTimer_Tick(object sender, object e)
        {
            // Move player based on currently pressed keys
            player.Move(pressedKeys);

            // Check for collision
            if (player.Location == collectible.Location)
            {
                await new MessageDialog("Collision Detected").ShowAsync();
            }
        }
    }
}
