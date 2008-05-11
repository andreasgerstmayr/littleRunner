using System;
using System.Collections.Generic;
using System.Drawing;


namespace littleRunner.Drawing
{
    public abstract class dImage
    {
        public enum RotateDirection
        {
            Horizontal,
            Vertical
        }

        public dImage()
        {
        }
        public dImage(string filename)
        {
        }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract dImage GetThumbnail(int width, int height);
        public abstract void Rotate(RotateDirection direction);


        public abstract System.Drawing.Image ToGDI();
    }
}
