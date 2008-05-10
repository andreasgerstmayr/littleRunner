using System;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;
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
            Surface master;

            public DrawHandler(Control c, UpdateHandler updateHandler)
                : base(c, updateHandler)
            {
                c.Resize += new EventHandler(c_Resize);
               
                surfaceControl = new SurfaceControl();
                surfaceControl.Location = c.Location;
                surfaceControl.Size = c.Size;

                c.Controls.Add(surfaceControl);

                //InitGL();
                createMasterSurface();

                //ReshapeGL();
            }

            void createMasterSurface()
            {
                master = Video.SetVideoMode(c.Width, c.Height, true, true);
                master.Fill(System.Drawing.Color.White);
              
            }

            void ReshapeGL() // reshape the window when it's moved or resized
            {
                int width = c.Width;
                int height = c.Height;
                int distance = 0;


                // Reset The Current Viewport
                Gl.glViewport(0, 0, width, height); // reset the current viewport

                // Select The Projection Matrix
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                // Reset The Projection Matrix
                Gl.glLoadIdentity();
                // Calculate The Aspect Ratio Of The Window
                Glu.gluPerspective(45.0F, (width / (float)height), 0.1F, distance);
                // Select The Modelview Matrix
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                // Reset The Modelview Matrix
                Gl.glLoadIdentity();
            }

            void InitGL()
            {
                Gl.glShadeModel(Gl.GL_SMOOTH);
                Gl.glClearColor(0.0F, 0.0F, 0.0F, 0.5F);

                // Depth Buffer Setup
                Gl.glClearDepth(1.0F);
                // Enables Depth Testing
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                // The Type Of Depth Testing To Do
                Gl.glDepthFunc(Gl.GL_LEQUAL);

                Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            }

            void c_Resize(object sender, EventArgs e)
            {
                surfaceControl.Size = c.Size;

                createMasterSurface();

                //Video.SetVideoMode(c.Width, c.Height, true, true);
                //ReshapeGL();
            }


            public override void Update()
            {
                 Gl.glClearColor(0, 0, 0, 0);
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glColor3f(1, 1, 1);
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Gl.glOrtho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
                Gl.glBegin(Gl.GL_POLYGON);
                Gl.glVertex2f(-0.5F, -0.5F);
                Gl.glVertex2f(-0.5F, 0.5F);
                Gl.glVertex2f(0.5F, 0.5F);
                Gl.glVertex2f(0.5F, -0.5F);
                Gl.glEnd();
                Gl.glFlush();

                Video.GLSwapBuffers();

                //updateHandler(new DrawSDL(master));


                //Video.GLSwapBuffers();

                //surfaceControl.Blit(master);
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
            SdlDotNet.Graphics.Font f = new SdlDotNet.Graphics.Font("C:\\Windows\\Fonts\\" + font.Family + ".ttf", (int)font.Size);

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
                img = img.CreateResizedSurface(new Size(width, height)); // don't work correctly
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