using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;


namespace littleRunner.GameObjects
{

    class AnimateImage
    {
        static public bool Refresh;
        dImage[,] images;
        int milliSecPerFrame;
        int cur;
        DateTime last;

        public dImage CurImage(GameDirection direction)
        {
            return images[(int)direction, cur];
        }

        public void Update(Draw d, GameDirection direction, int left, int top, int width, int height)
        {
            d.DrawImage(images[(int)direction, cur], left, top, width, height);
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
            images = new dImage[Enum.GetNames(typeof(GameDirection)).Length, files.Count];

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
                    dImage img = GetDraw.Image(files[i]);

                    if (imgDir == dir)
                        images[(int)imgDir, i] = img;
                    else
                    {
                        switch (dir)
                        {
                            case GameDirection.Left:
                                if (imgDir == GameDirection.Right)
                                    img.Rotate(dImage.RotateDirection.Horizontal);
                                break;
                            case GameDirection.Right:
                                if (imgDir == GameDirection.Left)
                                    img.Rotate(dImage.RotateDirection.Horizontal);
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
        public static dImage FirstImage(string filename)
        {
            return GetDraw.Image(getFiles(filename)[0]);
        }
    }
}
