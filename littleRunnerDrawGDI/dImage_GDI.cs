using System;
using littleRunner.Drawing;


namespace littleRunner.Drawing.GDI
{
    public class dImage_GDI : dImage
    {
        System.Drawing.Image img;

        public dImage_GDI()
            : base()
        {
        }
        public dImage_GDI(string filename)
            : base()
        {
            img = System.Drawing.Image.FromFile(filename);
        }

        public override int Width { get { return img.Width; } }
        public override int Height { get { return img.Height; } }


        private bool ThumbnailCallback()
        {
            return false;
        }
        public override dImage GetThumbnail(int width, int height)
        {
            img = img.GetThumbnailImage(width, height, ThumbnailCallback, IntPtr.Zero);
            return this;
        }
        public override void Rotate(RotateDirection direction)
        {
            switch (direction)
            {
                case RotateDirection.Horizontal:
                    img.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
                    break;
                case RotateDirection.Vertical:
                    img.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
                    break;
            }
        }

        public override System.Drawing.Image ToGDI()
        {
            return img;
        }
    }
}
