using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

using littleRunner.Drawing;
using littleRunner.Gamedata;
using littleRunner.GameObjects;


namespace littleRunner
{
    public class GameControl_Score : GameObject
    {
        dFont f1;
        dFont f2;
        dColor color = new dColor(System.Drawing.Color.Black);

        int score;
        string font;
        float size;


        public override void Update(Draw d)
        {
            d.DrawString("Score:", f1, color, Left, Top);
            d.DrawString(score.ToString(), f2, color, Left+115, Top);
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public GameControl_Score(int top, int left, string font, float size)
        {
            score = 0;
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;

            f1 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Left));
            f2 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Right));
        }
    }
    public class GameControl_Points : GameObject
    {
        dFont f1;
        dFont f2;
        dColor color = new dColor(System.Drawing.Color.Black);

        int points;
        string font;
        float size;


        public override void Update(Draw d)
        {
            d.DrawString("Points:", f1, color, Left, Top);
            d.DrawString(points.ToString(), f2, color, Left + 115, Top);
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public GameControl_Points(int top, int left, string font, float size)
        {
            points = 0;
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;

            f1 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Left));
            f2 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Right));
        }
    }
    public class GameControl_Lives : GameObject
    {
        dFont f1;
        dFont f2;
        dColor color = new dColor(System.Drawing.Color.Black);

        int lives;
        string font;
        float size;


        public override void Update(Draw d)
        {
            d.DrawString("Lives:", f1, color, Left, Top);
            d.DrawString((Cheat.Activated?"-":"") + lives.ToString(), f2, color, Left + 115, Top);
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public GameControl_Lives(int top, int left, int lives, string font, float size)
        {
            this.lives = lives;
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;

            f1 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Left));
            f2 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Right));
        }
    }
    public class GameControl_FPS : GameObject
    {
        dFont f1;
        dFont f2;
        dColor color = new dColor(System.Drawing.Color.Black);

        public bool Visible;
        List<int> fps;
        int average;
        string font;
        float size;


        public override void Update(Draw d)
        {
            if (Visible)
            {
                d.DrawString("FPS:", f1, color, Left, Top);
                d.DrawString(average.ToString(), f2, color, Left + 115, Top);
            }
        }

        public int FPS
        {
            get { return average; }
            set
            {
                if (fps.Count > 10)
                    fps.RemoveAt(0);

                fps.Add(value);

                int tmp = 0;
                foreach (int cFps in fps)
                {
                    tmp += cFps;
                }
                average = Convert.ToInt32(Convert.ToSingle(tmp) / fps.Count);
            }
        }

        public GameControl_FPS(int top, int left, string font, float size)
        {
            Visible = false;
            this.fps = new List<int>();
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;

            f1 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Left));
            f2 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Right));
        }
    }
    public class GameControl_Debug : GameObject
    {
        dFont f1;
        dColor color = new dColor(System.Drawing.Color.Black);

        public bool Visible;
        string font;
        float size;


        public override void Update(Draw d)
        {
            if (Visible)
                d.DrawString(".NET v"+Environment.Version.ToString(), f1, color, Left, Top);
        }

        public GameControl_Debug(int top, int left, string font, float size)
        {
            Visible = false;
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;

            f1 = new dFont(font, size, new dFontStyle(dFontWeight.Bold), new dFontFormat(dFontAligment.Right));
        }
    }
    public class GameControl_Sound
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string cmd, StringBuilder ret, int retLen, IntPtr hwnd);

        string fileName;
        bool soundPlaying;

        public GameControl_Sound()
        {
            this.fileName = "";
            soundPlaying = false;
        }
        public GameControl_Sound(string fileName)
        {
            this.fileName = fileName;
            soundPlaying = false;
        }
        

        public void Start()
        {
            if (!soundPlaying && fileName != "")
            {
                string cmd = "open \"" + fileName + "\" type MPEGVideo alias MediaFile";
                mciSendString(cmd, null, 0, IntPtr.Zero);
                cmd = "play MediaFile from 0";
                mciSendString(cmd, null, 0, IntPtr.Zero);

                soundPlaying = true;
            }
        }
        public void Stop()
        {
            if (soundPlaying && fileName != "")
            {
                string cmd = "stop MediaFile";
                mciSendString(cmd, null, 0, IntPtr.Zero);
                cmd = "close MediaFile";
                mciSendString(cmd, null, 0, IntPtr.Zero);

                soundPlaying = false;
            }
        }
        public void Volume(int value)
        {
            mciSendString("setaudio MediaFile volume to " + value.ToString(), null, 0, IntPtr.Zero);
        }
    }



    public class GameControlObjects
    {
        private GameControl_Score score;
        private GameControl_Points points;
        private GameControl_Lives lives;
        private GameControl_FPS fps;
        private GameControl_Debug debug;
        private GameControl_Sound sound;

        public void Update(Draw d)
        {
            score.Update(d);
            points.Update(d);
            lives.Update(d);
            fps.Update(d);
            debug.Update(d);
        }

        public int Score
        {
            get { return score.Score; }
            set { score.Score = value; }
        }
        public int Points
        {
            get { return points.Points; }
            set { points.Points = value; }
        }
        public int Lives
        {
            get { return lives.Lives; }
            set { lives.Lives = value; }
        }
        public int FPS
        {
            get { return fps.FPS; }
            set { fps.FPS = value; }
        }
        public GameControl_Sound Sound
        {
            get { return sound; }
        }


        public GameControlObjects(GameControl_Score score,
            GameControl_Points points,
            GameControl_Lives lives,
            GameControl_FPS fps,
            GameControl_Debug debug,
            GameControl_Sound sound)
        {
            this.score = score;
            this.points = points;
            this.lives = lives;
            this.fps = fps;
            this.debug = debug;
            this.sound = sound;
        }


        public void OnKeyPress(char c)
        {
            if (c == 'o')
                sound.Stop();
            else if (c == 'p')
                sound.Start();
            else if (c == 'f')
                fps.Visible = !fps.Visible;
            else if (c == 'l')
                debug.Visible = !debug.Visible;
        }
    }
}
