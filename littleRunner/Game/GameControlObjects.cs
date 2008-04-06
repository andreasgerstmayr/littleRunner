using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Runtime.InteropServices;

namespace littleRunner
{
    class GameControl_Points : GameObject
    {
        int points;
        string font;
        float size;

        public override void Draw(Graphics g)
        {
            g.DrawString("Points: " + points.ToString().PadLeft(7), new Font(font, size, FontStyle.Bold), Brushes.Black, Left, Top);
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
        }
    }
    class GameControl_Lives : GameObject
    {
        int lives;
        string font;
        float size;

        public override void Draw(Graphics g)
        {
            g.DrawString("Lives: " + lives.ToString().PadLeft(9), new Font(font, size, FontStyle.Bold), Brushes.Black, Left, Top);
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public GameControl_Lives(int lives, int top, int left, string font, float size)
        {
            this.lives = lives;
            Top = top;
            Left = left;
            this.font = font;
            this.size = size;
        }
    }
    class GameControl_Sound
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



    class GameControlObjects
    {
        private GameControl_Points points;
        private GameControl_Lives lives;
        private GameControl_Sound sound;

        public void Draw(Graphics g)
        {
            points.Draw(g);
            lives.Draw(g);
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
        public GameControl_Sound Sound
        {
            get { return sound; }
        }


        public GameControlObjects(GameControl_Points points, GameControl_Lives lives, GameControl_Sound sound)
        {
            this.points = points;
            this.lives = lives;
            this.sound = sound;
        }


        public void OnKeyPress(char c)
        {
            if (c == 'o')
                sound.Stop();
            else if (c == 'p')
                sound.Start();
        }
    }
}
