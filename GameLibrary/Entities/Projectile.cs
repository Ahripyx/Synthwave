using Windows.UI.Xaml.Controls; // Add this for Image

namespace GameLibrary
{
    public class Projectile
    {
        public Image Image;
        public double X, Y;
        public double VelocityX, VelocityY;
        public double Width => Image.Width;
        public double Height => Image.Height;
    }
}
