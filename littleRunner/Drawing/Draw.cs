using System;
using System.Windows.Forms;

using System.Drawing;


namespace littleRunner.Drawing
{
    abstract public class Draw
    {
        public delegate void UpdateHandler(Draw d);

        public abstract class DrawHandler
        {
            protected Control c;
            protected UpdateHandler updateHandler;

            public DrawHandler(Control c, UpdateHandler updateHandler)
            {
                this.c = c;
                this.updateHandler = updateHandler;
            }

            abstract public void Update();


            public static DrawHandler Create(Control c, UpdateHandler updateHandler)
            {
                if (Globals.VideoRenderMode == VideoRenderMode.GDI)
                    return new DrawGDI.DrawHandler(c, updateHandler);
                else
                    return new DrawSDL.DrawHandler(c, updateHandler);
            }
        }


        public abstract void DrawImage(Draw.Image image, int x, int y, int width, int height);
        public abstract void DrawRectangle(Draw.Pen pen, int x, int y, int width, int height);
        public abstract void FillRectangle(Draw.Pen pen, int x, int y, int width, int height);
        public abstract void DrawString(string text, Draw.Font font, Draw.Color color, int x, int y);

        public abstract void MoveCoords(int x, int y);


        public abstract class Image
        {
            public enum RotateDirection
            {
                Horizontal,
                Vertical
            }

            public Image()
            {
            }
            public Image(string filename)
            {
            }
            public abstract int Width { get; }
            public abstract int Height { get; }

            public abstract Draw.Image GetThumbnail(int width, int height);
            public abstract void Rotate(RotateDirection direction);

            public abstract System.Drawing.Image ToGDI();
            public abstract System.Drawing.Bitmap ToBitmap();


            // create correct instances
            public static Image OpenEmpty()
            {
                if (Globals.VideoRenderMode == VideoRenderMode.GDI)
                    return new DrawGDI.Image();
                else
                    return new DrawSDL.Image();
            }
            public static Image Open(string filename)
            {
                if (Globals.VideoRenderMode == VideoRenderMode.GDI)
                    return new DrawGDI.Image(filename);
                else
                    return new DrawSDL.Image(filename);
            }
        }

        public class Color
        {
            public int A, R, G, B;
            public Color(int a, int r, int g, int b)
            {
                this.A = a;
                this.R = r;
                this.G = g;
                this.B = b;
            }
            public Color(int r, int g, int b)
                : this(255, r, g, b)
            {
            }
            public Color(System.Drawing.Color c)
            {
                A = c.A;
                R = c.R;
                G = c.G;
                B = c.B;
            }

            public System.Drawing.Color ToGDI()
            {
                return System.Drawing.Color.FromArgb(A, R, G, B);
            }
        }
        public class Pen
        {
            public enum PenStyle
            {
                Dashed,
                Solid
            }
            public Draw.Color Color;
            public PenStyle Style;

            public Pen(Draw.Color color, PenStyle style)
            {
                this.Color = color;
                this.Style = style;
            }
            public Pen(Color color)
                : this(color, PenStyle.Solid)
            {
            }


            public static Pen FromGDI(System.Drawing.Pen gdiPen)
            {
                PenStyle style = PenStyle.Solid;
                switch (gdiPen.DashStyle)
                {
                    case System.Drawing.Drawing2D.DashStyle.Solid:
                        style = PenStyle.Solid; break;
                    case System.Drawing.Drawing2D.DashStyle.Dash:
                        style = PenStyle.Dashed; break;
                }

                return new Pen(new Color(gdiPen.Color), style);
            }

            public System.Drawing.Pen ToGDIPen()
            {
                System.Drawing.Pen p = new System.Drawing.Pen(Color.ToGDI());
                switch (Style)
                {
                    case PenStyle.Solid:
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                    case PenStyle.Dashed:
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                }
                return p;
            }
            public System.Drawing.Brush ToGDIBrush()
            {
                return new System.Drawing.SolidBrush(Color.ToGDI());
            }

            public static Draw.Pen Black
            {
                get { return new Draw.Pen(new Color(System.Drawing.Color.Black), PenStyle.Solid); }
            }
        }
        public enum FontWeight
        {
            Normal,
            Bold
        }
        public class FontStyle
        {
            FontWeight weight;

            public FontWeight Weight { get { return weight; } }

            public FontStyle(FontWeight weight)
            {
                this.weight = weight;
            }


            public System.Drawing.FontStyle ToGDI()
            {
                switch (weight)
                {
                    case FontWeight.Normal:
                        return System.Drawing.FontStyle.Regular;
                    case FontWeight.Bold:
                        return System.Drawing.FontStyle.Bold;

                    default: return System.Drawing.FontStyle.Regular;
                }
            }
        }
        public enum FontAligment
        {
            Left,
            Center,
            Right
        }
        public class FontFormat
        {
            FontAligment aligment;

            public FontFormat(FontAligment aligment)
            {
                this.aligment = aligment;
            }


            public System.Drawing.StringFormat ToGDI()
            {
                StringFormat format = new StringFormat();
                switch (aligment)
                {
                    case FontAligment.Left: format.Alignment = StringAlignment.Near; break;
                    case FontAligment.Center: format.Alignment = StringAlignment.Center; break;
                    case FontAligment.Right: format.Alignment = StringAlignment.Far; break;
                }
                return format;
            }
        }
        public class Font
        {
            string family;
            float size;
            FontStyle style;
            FontFormat format;

            public string Family
            {
                get { return family; }
            }
            public float Size
            {
                get { return size; }
            }
            public FontStyle Style
            {
                get { return style; }
            }
            public FontFormat Format
            {
                get { return format; }
            }


            public Font(string family, float size, FontStyle style, FontFormat format)
            {
                this.family = family;
                this.size = size;
                this.style = style;
                this.format = format;
            }
            public Font(string family, float size)
                : this(family, size, new FontStyle(FontWeight.Normal), new FontFormat(FontAligment.Left))
            {
            }
        }
    }
}