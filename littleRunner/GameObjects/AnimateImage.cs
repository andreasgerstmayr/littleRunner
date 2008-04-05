using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    
    class AnimateImage
    {
        Image[] images;
        int milliSecPerFrame;
        int cur;
        DateTime last;

        public Image CurImage
        {
            get { return images[cur]; }
        }

        public void Draw(Graphics g, int left, int top, int width, int height)
        {
            g.DrawImage(images[cur], left, top, width, height);
            if ((DateTime.Now - last).TotalMilliseconds >= milliSecPerFrame)
            {
                cur++;
                last = DateTime.Now;
            }
            if (cur >= images.Length)
                cur = 0;
        }

        public static GameDirection getDirection(string s)
        {
            switch (s)
            {
                case "L": return GameDirection.Left;
                case "R": return GameDirection.Right;
            }
            return GameDirection.None;
        }
        public AnimateImage(string imagesFn, int milliSecPerFrame, GameDirection direction)
        {
            if (direction == GameDirection.None)
            {
                images = new Image[1] { Image.FromFile(imagesFn) };
            }
            else
            {
                List<string> files = AnimateImage.getFiles(imagesFn);
                images = new Image[files.Count];
                for (int i = 0; i < files.Count; i++)
                {
                    string dir = imagesFn.Substring(imagesFn.IndexOf("#") + 1, 1);
                    GameDirection imgDir = AnimateImage.getDirection(dir);

                    Image img = Image.FromFile(files[i]);
                    if (direction == imgDir)
                    { } // do nothing
                    else if ((direction == GameDirection.Left && imgDir == GameDirection.Right) ||
                             (direction == GameDirection.Right && imgDir == GameDirection.Left)) // flip
                    {
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    images[i] = img;
                }
            }

            this.milliSecPerFrame = milliSecPerFrame;
            cur = 0;
            last = DateTime.Now;
        }


        public static List<string> getFiles(string filename)
        {
            List<string> files = new List<string>();

            int start = filename.IndexOf("#");
            if (start != -1)
            {
                int dash = filename.LastIndexOf("-");
                int lastBracket = filename.LastIndexOf("]");

                int from = Convert.ToInt32(filename.Substring(start + 3, dash - (start + 3)));
                int to = Convert.ToInt32(filename.Substring(dash + 1, lastBracket - (dash + 1)));

                filename = filename.Substring(0, start + 1) + filename.Substring(lastBracket + 1);
                for (int i = from; i <= to; i++)
                {
                    files.Add(filename.Replace("#", i.ToString()));
                }
            }
            else
                files.Add(filename);

            return files;
        }
        public static Image FirstImage(string filename)
        {
            return Image.FromFile(getFiles(filename)[0]);
        }
    }
}
