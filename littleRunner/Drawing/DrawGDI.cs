using System;
using System.Windows.Forms;
using System.Drawing;


namespace littleRunner.Drawing
{
    class DrawGDI : Draw
    {
        public new class DrawHandler : Draw.DrawHandler
        {
            public DrawHandler(Control c, UpdateHandler updateHandler)
                : base(c, updateHandler)
            {
                this.c.Paint += new PaintEventHandler(c_Paint);
            }
            public override void Update()
            {
                c.Invalidate();
            }


            void c_Paint(object sender, PaintEventArgs e)
            {
                updateHandler(new DrawGDI(e.Graphics));
            }
        }

        Graphics g;

        public DrawGDI(Graphics g)
        {
            this.g = g;
        }



        public override void DrawImage(Draw.Image image, int x, int y, int width, int height)
        {
            g.DrawImage(image.ToGDI(), x, y, width, height);
        }
        public override void DrawRectangle(Draw.Pen pen, int x, int y, int width, int height)
        {
            g.DrawRectangle(pen.ToGDIPen(), x, y, width, height); 
        }
        public override void FillRectangle(Draw.Pen pen, int x, int y, int width, int height)
        {
            g.FillRectangle(pen.ToGDIBrush(), x, y, width, height); 
        }
        public override void DrawString(string text, Draw.Font font, Draw.Color color, int x, int y)
        {
            SolidBrush b = new SolidBrush(color.ToGDI());
            System.Drawing.Font f = new System.Drawing.Font(font.Family, font.Size, font.Style.ToGDI());
            g.DrawString(text, f, b, x, y, font.Format.ToGDI());
        }


        public override void MoveCoords(int x, int y)
        {
            g.TranslateTransform(x, y);
        }


        public new class Image : Draw.Image
        {
            System.Drawing.Image img;

            public Image() 
                : base()
            {
            }
            public Image(string filename) 
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
            public override Draw.Image GetThumbnail(int width, int height)
            {
                img = img.GetThumbnailImage(width, height, ThumbnailCallback, IntPtr.Zero);
                return this;
            }
            public override void Rotate(RotateDirection direction)
            {
                switch (direction)
                {
                    case RotateDirection.Horizontal:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case RotateDirection.Vertical:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                }
            }

            public override System.Drawing.Image ToGDI()
            {
                return img;
            }
            public override Bitmap ToBitmap()
            {
                return new Bitmap(img);
            }
        }
    }
}