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
        private Thickness objectMargins;
        private Image onScreen;
        public Thickness Location => onScreen.Margin;
        public double Width => onScreen.Width;
        public double Height => onScreen.Height;

        public GamePiece(Image img)
        {
            onScreen = img;
            objectMargins = img.Margin;
        }

        public Image Image => onScreen;

        public bool Move(HashSet<VirtualKey> directions, double containerWidth, double containerHeight)
        {
            bool moved = false;
            double step = 10;
            double newLeft = objectMargins.Left, newTop = objectMargins.Top;

            if (directions.Contains(VirtualKey.W) || directions.Contains(VirtualKey.Up)) { newTop -= step; moved = true; }
            if (directions.Contains(VirtualKey.S)) { newTop += step; moved = true; }
            if (directions.Contains(VirtualKey.A)) { newLeft -= step; moved = true; }
            if (directions.Contains(VirtualKey.D)) { newLeft += step; moved = true; }

            double maxLeft = containerWidth - onScreen.Width, maxTop = containerHeight - onScreen.Height;
            if (maxLeft < 0) maxLeft = 0;
            if (maxTop < 0) maxTop = 0;

            newLeft = System.Math.Max(0, System.Math.Min(newLeft, maxLeft));
            newTop = System.Math.Max(0, System.Math.Min(newTop, maxTop));

            objectMargins.Left = newLeft;
            objectMargins.Top = newTop;
            onScreen.Margin = objectMargins;

            return moved;
        }

        public double Left
        {
            get => objectMargins.Left;
            set { objectMargins.Left = value; onScreen.Margin = objectMargins; }
        }

        public double Top
        {
            get => objectMargins.Top;
            set { objectMargins.Top = value; onScreen.Margin = objectMargins; }
        }

        public void SetPosition(double left, double top)
        {
            objectMargins.Left = left;
            objectMargins.Top = top;
            onScreen.Margin = objectMargins;
        }

        public void MoveTowards(double targetX, double targetY, double speed)
        {
            double dx = targetX - this.Left;
            double dy = targetY - this.Top;
            double distance = System.Math.Sqrt(dx * dx + dy * dy);
            if (distance < 1.0) return;

            double stepX = dx / distance * speed;
            double stepY = dy / distance * speed;

            SetPosition(this.Left + stepX, this.Top + stepY);
        }
    }
}
