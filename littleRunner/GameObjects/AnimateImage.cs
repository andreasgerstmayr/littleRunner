using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects
{

    class AnimateImage
    {
        static public bool Refresh;
        Image[,] images;
        int milliSecPerFrame;
        int cur;
        DateTime last;

        public Image CurImage(GameDirection direction)
        {
            return images[(int)direction, cur];
        }

        public void Draw(Graphics g, GameDirection direction, int left, int top, int width, int height)
        {
            g.DrawImage(images[(int)direction, cur], left, top, width, height);
            if ((DateTime.Now - last).TotalMilliseconds >= milliSecPerFrame && Refresh)
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
        public AnimateImage(string imagesFn, int milliSecPerFrame, params GameDirection[] needDirections)
        {
            List<string> files = AnimateImage.getFiles(imagesFn);
            images = new Image[Enum.GetNames(typeof(GameDirection)).Length, files.Count];

            for (int i = 0; i < files.Count; i++)
            {
                GameDirection imgDir;
                int indexOfSharp = imagesFn.IndexOf("#");

                if (indexOfSharp == -1)
                    imgDir = GameDirection.None;
                else
                {
                    string sDirection = imagesFn.Substring(indexOfSharp + 1, 1);
                    imgDir = AnimateImage.getDirection(sDirection);
                }

                foreach (GameDirection dir in needDirections)
                {
                    Image img = Image.FromFile(files[i]);

                    if (imgDir == dir)
                        images[(int)imgDir, i] = img;
                    else
                    {
                        switch (dir)
                        {
                            case GameDirection.Left:
                                if (imgDir == GameDirection.Right)
                                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case GameDirection.Right:
                                if (imgDir == GameDirection.Left)
                                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                        }
                        images[(int)dir, i] = img;
                    }
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
