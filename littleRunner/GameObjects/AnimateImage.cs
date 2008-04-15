using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects
{

    class AnimateImage
    {
        Image[,] images;
        int milliSecPerFrame;
        int cur;
        bool dirRelevant;
        DateTime last;

        public Image CurImage(GameDirection direction)
        {
            return images[dirRelevant ? (int)direction : 0, cur];
        }

        public void Draw(Graphics g, GameDirection direction, int left, int top, int width, int height)
        {
            g.DrawImage(images[dirRelevant ? (int)direction : 0, cur], left, top, width, height);
            if ((DateTime.Now - last).TotalMilliseconds >= milliSecPerFrame)
            {
                cur++;
                last = DateTime.Now;
            }
            if (cur >= images.GetLength(1))
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
        public AnimateImage(string imagesFn, int milliSecPerFrame)
        {
            List<string> files = AnimateImage.getFiles(imagesFn);
            images = new Image[Enum.GetNames(typeof(GameDirection)).Length, files.Count];

            for (int i = 0; i < files.Count; i++)
            {
                int indexOfSharp = imagesFn.IndexOf("#");
                dirRelevant = indexOfSharp != -1;

                if (dirRelevant)
                {
                    string dir = imagesFn.Substring(indexOfSharp + 1, 1);
                    GameDirection imgDir = AnimateImage.getDirection(dir);

                    Image original = Image.FromFile(files[i]);
                    // write with original direction
                    images[(int)imgDir, i] = original;

                    Image flip = (Image)original.Clone();

                    // change direction & write
                    if (imgDir == GameDirection.Left)
                        imgDir = GameDirection.Right;
                    else if (imgDir == GameDirection.Right)
                        imgDir = GameDirection.Left;

                    flip.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    images[(int)imgDir, i] = flip;
                }
                else
                {
                    Image original = Image.FromFile(files[i]);
                    images[0, i] = original;
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
