using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth.Background;
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

        public Image Image => onScreen; // provide access to the image for adding and removing to the UI

        // Player movement function
        public bool Move(HashSet<VirtualKey> directions, double containerWidth, double containerHeight)
        {
            bool moved = false;
            double step = 10;

            double newLeft = objectMargins.Left;
            double newTop = objectMargins.Top;

            if (directions.Contains(VirtualKey.W) || directions.Contains(VirtualKey.Up)) { newTop -= step; moved = true; }
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


        // Getting positionings
        public double Left
        {
            get => objectMargins.Left;
            set
            {
                objectMargins.Left = value;
                onScreen.Margin = objectMargins;
            }
        }

        public double Top
        {
            get => objectMargins.Top;
            set
            {
                objectMargins.Top = value;
                onScreen.Margin = objectMargins;
            }
        }

        public void SetPosition(double left, double top)
        {
            objectMargins.Left = left;
            objectMargins.Top = top;
            onScreen.Margin = objectMargins;
        }

        // Enemy movement function
        public void MoveTowards(double targetX, double targetY, double speed)
        {
            double dx = targetX - this.Left;
            double dy = targetY - this.Top;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance < 1.0) return; // Already close enough

            double stepX = dx / distance * speed;
            double stepY = dy / distance * speed;

            SetPosition(this.Left + stepX, this.Top + stepY);
        }
    }
}
