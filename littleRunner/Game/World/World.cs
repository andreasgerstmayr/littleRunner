using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace littleRunner
{
    public delegate void MyInvalidateEventHandler();

    public class World
    {
        List<Enemy> enemies;
        List<StickyElement> stickyelements;
        List<MovingElement> movingelements;

        public string fileName;
        MainGameObject mainGameObject;
        public LevelSettings Settings;
        public MyInvalidateEventHandler Invalidate;
        public bool playMode;
        public Script Script;

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }
        public List<StickyElement> StickyElements
        {
            get { return stickyelements; }
        }
        public List<MovingElement> MovingElements
        {
            get { return movingelements; }
        }
        public MainGameObject MGO
        {
            get { return mainGameObject; }
        }


        // new world with the editor
        public World(int width, int height, MyInvalidateEventHandler invalidate, bool playMode)
        {
            Settings = new LevelSettings();
            Settings.LevelWidth = width;
            Settings.GameWindowWidth = width;
            Settings.LevelHeight = height;
            this.Invalidate = invalidate;

            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
            this.playMode = playMode;
        }
        // new world with the game
        public World(string filename, MyInvalidateEventHandler invalidate, bool playMode)
        {
            this.Invalidate = invalidate;
            this.playMode = playMode;
            this.fileName = filename;
            Deserialize();
        }
        public void Init(MainGameObject mainGameObject)
        {
            this.mainGameObject = mainGameObject;

            // Script
            if (Settings.Script.Length > 0)
            {
                Script = new Script(this);

                foreach (GameObject go in this.AllElements)
                {
                    if (go.Name != null && go.Name != "")
                    {
                        Script.GlobalsAdd(go.Name, go);
                        Script.Execute("handler." + go.Name + " = AttrDict()");
                    }
                }

                string code = "";
                for (int i = 0; i < Settings.Script.Length; i++)
                {
                    code += Settings.Script[i] + (i + 1 == Settings.Script.Length ? "" : "\n");
                }

                Script.Execute(code);
            }
        }

        public void Draw(Graphics g, bool drawBackground)
        {
            if (drawBackground && Settings.BackgroundImg != null)
                g.DrawImage(Settings.BackgroundImg, 0, 0, Settings.GameWindowWidth, Settings.LevelHeight);

            foreach (GameObject go in AllElements)
            {
                go.Draw(g);
            }
        }

        public void Add(GameObject go)
        {
            if (go is Enemy)
                enemies.Add((Enemy)go);
            else if (go is StickyElement)
                stickyelements.Add((StickyElement)go);
        }
        public void Remove(GameObject go)
        {
            if (go is Enemy)
                enemies.Remove((Enemy)go);
            else if (go is StickyElement)
                stickyelements.Remove((StickyElement)go);
        }
        public List<GameObject> AllElements
        {
            get
            {
                List<GameObject> ret = new List<GameObject>();

                foreach (StickyElement se in stickyelements)
                {
                    ret.Add(se);
                }
                foreach (Enemy e in enemies)
                {
                    ret.Add(e);
                }
                foreach (MovingElement me in movingelements)
                {
                    ret.Add(me);
                }
                return ret;
            }
        }

        public void SetFirst(GameObject go)
        {
            if (go is Enemy)
            {
                enemies.Remove((Enemy)go);
                enemies.Insert(0, (Enemy)go);
            }
            else if (go is StickyElement)
            {
                stickyelements.Remove((StickyElement)go).ToString();
                stickyelements.Insert(0, (StickyElement)go);
            }
        }
        public void SetLast(GameObject go)
        {
            if (go is Enemy)
            {
                enemies.Remove((Enemy)go);
                enemies.Insert(enemies.Count, (Enemy)go);
            }
            else if (go is StickyElement)
            {
                stickyelements.Remove((StickyElement)go);
                stickyelements.Insert(stickyelements.Count, (StickyElement)go);
            }
        }

        public void Serialize(string filename)
        {
            WorldSerialization.Serialize(filename, stickyelements, enemies, Settings);
        }

        public void Deserialize()
        {
            WorldSerialization.Deserialize(fileName, out Settings, out enemies, out stickyelements, this);
            movingelements = new List<MovingElement>();
        }
    }

    public enum Backgrounds
    {
        Blue_Hills,
        Green_Hills,
        None
    }

    public delegate void Changed_Setting();
    public class LevelSettings
    {
        private int gameWindowWidth;
        private int levelWidth;
        private int levelHeight;
        private Backgrounds background;
        private Image backgroundImg;
        private string[] script = new string[0];

        public Changed_Setting cLevelWidth;
        public Changed_Setting cLevelHeight;
        public Changed_Setting cGameWindowWidth;

        public LevelSettings()
        {
            Background = Backgrounds.None;
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
                        BackgroundImg = Image.FromFile(Files.f[gFile.background_blue_hills]);
                        break;
                    case Backgrounds.Green_Hills:
                        BackgroundImg = Image.FromFile(Files.f[gFile.background_green_hills]);
                        break;
                }
            }
        }

        [Category("Script")]
        public string[] Script
        {
            get { return script; }
            set { script = value; }
        }


        private bool ThumbnailCallback()
        {
            return false;
        }
        internal Image BackgroundImg
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
            ser["levelWidth"] = levelWidth;
            ser["levelHeight"] = levelHeight;
            ser["Background"] = background;
            ser["Script"] = script;
            return ser;
        }
        public void Deserialize(Dictionary<string, object> ser)
        {
            gameWindowWidth = (int)ser["gameWindowWidth"];
            levelWidth = (int)ser["levelWidth"];
            levelHeight = (int)ser["levelHeight"];
            Background = (Backgrounds)ser["Background"];
            script = (string[])ser["Script"];
        }
    }
}
