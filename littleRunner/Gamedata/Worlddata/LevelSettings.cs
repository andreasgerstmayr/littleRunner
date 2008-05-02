using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;


namespace littleRunner.Gamedata.Worlddata
{
    public enum Backgrounds
    {
        Blue_Hills,
        Blue_Mountains,
        Blue_Waterhills,
        Green_Hills1,
        Green_Hills2,
        Green_Junglehills,
        None
    }

    public delegate void Changed_Setting();
    public class LevelSettings
    {
        private int gameWindowWidth, gameWindowHeight;
        private int levelWidth, levelHeight;
        private Backgrounds background;
        private Image backgroundImg;
        private string script;

        public Changed_Setting cLevelWidth, cLevelHeight;
        public Changed_Setting cGameWindowWidth, cGameWindowHeight;

        public LevelSettings()
        {
            Background = Backgrounds.None;
            script = "";
        }


        [Category("Level settings")]
        public int GameWindowWidth
        {
            get { return gameWindowWidth; }
            set
            {
                gameWindowWidth = value;
                if (cGameWindowWidth != null) cGameWindowWidth();
            }
        }
        [Category("Level settings")]
        public int GameWindowHeight
        {
            get { return gameWindowHeight; }
            set
            {
                gameWindowHeight = value;
                if (cGameWindowHeight != null) cGameWindowHeight();
            }
        }
        [Category("Level settings")]
        public int LevelWidth
        {
            get { return levelWidth; }
            set
            {
                levelWidth = value;
                if (cLevelWidth != null) cLevelWidth();
            }
        }
        [Category("Level settings")]
        public int LevelHeight
        {
            get { return levelHeight; }
            set
            {
                levelHeight = value;
                if (cLevelHeight != null) cLevelHeight();
            }
        }
        [Category("Level settings")]
        public Backgrounds Background
        {
            get { return background; }
            set
            {
                background = value;
                switch (background)
                {
                    case Backgrounds.None:
                        BackgroundImg = null;
                        break;
                    case Backgrounds.Blue_Hills:
                        BackgroundImg = Image.FromFile(Files.background_blue_hills);
                        break;
                    case Backgrounds.Blue_Mountains:
                        BackgroundImg = Image.FromFile(Files.background_blue_mountains);
                        break;
                    case Backgrounds.Blue_Waterhills:
                        BackgroundImg = Image.FromFile(Files.background_blue_waterhills);
                        break;
                    case Backgrounds.Green_Hills1:
                        BackgroundImg = Image.FromFile(Files.background_green_hills1);
                        break;
                    case Backgrounds.Green_Hills2:
                        BackgroundImg = Image.FromFile(Files.background_green_hills2);
                        break;
                    case Backgrounds.Green_Junglehills:
                        BackgroundImg = Image.FromFile(Files.background_green_junglehills);
                        break;
                }
            }
        }

        [Browsable(false), Category("Script")]
        public string Script
        {
            get { return script; }
            set { script = value; }
        }


        private bool ThumbnailCallback()
        {
            return false;
        }
        [Browsable(false)]
        public Image BackgroundImg
        {
            get { return backgroundImg; }
            set
            {
                if (value == null)
                {
                    backgroundImg = null;
                }
                else
                {
                    // create Thumbnail & keep in Cache (RAM)
                    backgroundImg = value.GetThumbnailImage(gameWindowWidth, LevelHeight, ThumbnailCallback, IntPtr.Zero);
                }
            }
        }


        public Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>();
            ser["gameWindowWidth"] = gameWindowWidth;
            ser["gameWindowHeight"] = gameWindowHeight;
            ser["levelWidth"] = levelWidth;
            ser["levelHeight"] = levelHeight;
            ser["Background"] = background;
            ser["Script"] = script;
            return ser;
        }
        public void Deserialize(Dictionary<string, object> ser)
        {
            gameWindowWidth = (int)ser["gameWindowWidth"];
            gameWindowHeight = (int)ser["gameWindowHeight"];
            levelWidth = (int)ser["levelWidth"];
            levelHeight = (int)ser["levelHeight"];
            Background = (Backgrounds)ser["Background"];
            script = (string)ser["Script"];
        }
    }
}
