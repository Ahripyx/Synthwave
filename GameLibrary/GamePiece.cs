using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Project: Lab 1A - UWP Game
//Student Name:
//Date:

namespace GameLibrary
{
    public class GamePiece
    {
        private Thickness objectMargins;            //represents the location of the piece on the game board
        private Image onScreen;                     //the image that is displayed on screen
        public Thickness Location                   //get access only - can not directly modify the location of the piece
        {
            get { return onScreen.Margin; }
        }

        public double Width => onScreen.Width;
        public double Height => onScreen.Height;

        public GamePiece(Image img)                 //constructor creates a piece and a reference to its associated image
        {                                           //use this to set up other GamePiece properties
            onScreen = img;
            objectMargins = img.Margin;
        }

        public bool Move(HashSet<VirtualKey> directions, double containerWidth, double containerHeight)
        {
            bool moved = false;
            double step = 10;

            double newLeft = objectMargins.Left;
            double newTop = objectMargins.Top;

            if (directions.Contains(VirtualKey.W)) { newTop -= step; moved = true; }
            if (directions.Contains(VirtualKey.S)) { newTop += step; moved = true; }
            if (directions.Contains(VirtualKey.A)) { newLeft -= step; moved = true; }
            if (directions.Contains(VirtualKey.D)) { newLeft += step; moved = true; }

            // Clamp so the ENTIRE image stays onscreen
            double maxLeft = containerWidth - onScreen.Width;
            double maxTop = containerHeight - onScreen.Height;

            // Prevent negative max values (if image is bigger than container, just stick at 0)
            if (maxLeft < 0) maxLeft = 0;
            if (maxTop < 0) maxTop = 0;

            newLeft = Math.Max(0, Math.Min(newLeft, maxLeft));
            newTop = Math.Max(0, Math.Min(newTop, maxTop));

            objectMargins.Left = newLeft;
            objectMargins.Top = newTop;
            onScreen.Margin = objectMargins;

            return moved;
        }
    }
}
