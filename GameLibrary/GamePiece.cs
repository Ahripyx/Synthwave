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

        public GamePiece(Image img)                 //constructor creates a piece and a reference to its associated image
        {                                           //use this to set up other GamePiece properties
            onScreen = img;
            objectMargins = img.Margin;
        }

        public bool Move(HashSet<VirtualKey> directions)   //calculate a new location for the piece, based on a key press
        {
            bool moved = false;

            if (directions.Contains(VirtualKey.W))
            {
                objectMargins.Top -= 10;
                moved = true;
            }
            if (directions.Contains(VirtualKey.S))
            {
                objectMargins.Top += 10;
                moved = true;
            }
            if (directions.Contains(VirtualKey.A))
            {
                objectMargins.Left -= 10;
                moved = true;
            }
            if (directions.Contains(VirtualKey.D))
            {
                objectMargins.Left += 10;
                moved = true;
            }
            if (moved) onScreen.Margin = objectMargins;  //update the image location on screen
            return moved;
        }
    }
}
