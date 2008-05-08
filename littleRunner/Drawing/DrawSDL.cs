using System;
using System.Drawing;
using System.Windows.Forms;

using SdlDotNet.Windows;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;


namespace littleRunner.Drawing
{
    class DrawSDL : Draw
    {
        public new class DrawHandler : Draw.DrawHandler
        {
            SurfaceControl surfaceControl;
            Surface surf;

            public DrawHandler(Control c, UpdateHandler updateHandler)
                : base(c, updateHandler)
            {
                surfaceControl = new SurfaceControl();
                surfaceControl.Location = c.Location;
                surfaceControl.Size = c.Size;

                surf = new Surface(surfaceControl.Size);
            }


            public override void Update()
            {
                updateHandler(new DrawSDL(surf));

                surfaceControl.Blit(surf);
            }
        }


        Surface master;

        public DrawSDL(Surface master)
        {
            this.master = master;
        }



        public override void DrawImage(Draw.Image image, int x, int y, int width, int height)
        {
            Surface s = new Surface(image.ToBitmap());
            if (s.Width != width || s.Height != height)
                s = s.CreateResizedSurface(new Size(width, height));

            master.Blit(s, new Point(x, y));
        }
        public override void DrawRectangle(Draw.Pen pen, int x, int y, int width, int height)
        {
            //g.DrawRectangle(pen.ToGDIPen(), x, y, width, height);
        }
        public override void FillRectangle(Draw.Pen pen, int x, int y, int width, int height)
        {
            Surface s = new Surface(width, height);
            s.Fill(pen.Color.ToGDI());

            master.Blit(s, new Point(x, y));
        }
        public override void DrawString(string text, Draw.Font font, Draw.Color color, int x, int y)
        {
            SdlDotNet.Graphics.Font f = new SdlDotNet.Graphics.Font("C:\\Windows\\Fonts\\"+font.Family+".ttf", (int)font.Size);

            switch (font.Style.Weight)
            {
                case FontWeight.Bold:
                    f.Bold = true; break;
            }

            TextSprite s = new TextSprite(text, f);

            master.Blit(s, new Point(x, y));
        }


        public override void MoveCoords(int x, int y)
        {
            //master.CreateSurfaceFromClipRectangle(new Rectangle
            //g.TranslateTransform(x, y);
        }


        public new class Image : Draw.Image
        {
            Surface img;

            public Image()
                : base()
            {
            }
            public Image(string filename)
                : base()
            {
                img = new Surface(filename);
            }

            public override int Width { get { return img.Width; } }
            public override int Height { get { return img.Height; } }


            public override Draw.Image GetThumbnail(int width, int height)
            {
                img = img.CreateResizedSurface(new Size(width, height));
                return this;
            }
            public override void Rotate(RotateDirection direction)
            {
                switch (direction)
                {
                    case RotateDirection.Horizontal:
                        img = img.CreateFlippedHorizontalSurface();
                        break;
                    case RotateDirection.Vertical:
                        img = img.CreateFlippedVerticalSurface();
                        break;
                }
            }

            public override System.Drawing.Image ToGDI()
            {
                return System.Drawing.Image.FromHbitmap(this.ToBitmap().GetHbitmap());
            }
            public override Bitmap ToBitmap()
            {
                return img.Bitmap;
            }
        }
    }
}