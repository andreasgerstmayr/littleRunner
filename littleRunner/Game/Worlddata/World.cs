using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using IronPython.Runtime.Exceptions;
using littleRunner.GameObjects;
using littleRunner.Worlddata;
using littleRunner.GameObjects.MainGameObjects;


namespace littleRunner
{
    public delegate void MyInvalidateEventHandler();

    public enum PlayMode
    {
        Game,
        GameInEditor,
        Editor
    }

    public class World
    {
        List<Enemy> enemies;
        List<StickyElement> stickyelements;
        List<MovingElement> movingelements;

        public string fileName;
        MainGameObject mainGameObject;
        public LevelSettings Settings;
        public MyInvalidateEventHandler Invalidate;
        private GameEventHandler aiEventHandler;
        public PlayMode PlayMode;
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
        private World(PlayMode playMode)
        {
            this.PlayMode = playMode;

            if (playMode == PlayMode.Editor)
                mainGameObject = new NullMGO();
        }
        public World(int width, int height, MyInvalidateEventHandler invalidate, PlayMode playMode)
            : this(playMode)
        {
            Settings = new LevelSettings();
            Settings.LevelWidth = width;
            Settings.GameWindowWidth = width;
            Settings.LevelHeight = height;
            this.Invalidate = invalidate;

            this.aiEventHandler = GameAI.NullAiEventHandlerMethod;
            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
        }
        // new world with the game
        public World(string filename, MyInvalidateEventHandler invalidate, GameEventHandler aiEventHandler, PlayMode playMode)
            : this(playMode)
        {
            this.Invalidate = invalidate;
            this.fileName = filename;
            this.aiEventHandler = aiEventHandler;
            Deserialize();
        }
        public World(string filename, MyInvalidateEventHandler invalidate, PlayMode playMode)
            : this(filename, invalidate, GameAI.NullAiEventHandlerMethod, playMode)
        {
        }

        public void Init(MainGameObject mainGameObject) // only called when starting the game
        {
            this.mainGameObject = mainGameObject;
            string msg = InitScript();

            if (msg != "")
            {
                if (PlayMode == PlayMode.Game)
                    throw new littleRunnerScriptException(msg);
                else
                    MessageBox.Show("Can't load script.", "Script error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public string InitScript()
        {
            if (Settings.Script.Length > 0)
            {
                Script = new Script(this);

                try
                {
                    foreach (GameObject go in this.AllElements)
                    {
                        if (go.Name != null && go.Name != "")
                        {
                            Script.GlobalsAdd(go.Name, go);
                            Script.Execute("handler." + go.Name + " = AttrDict()");
                        }
                    }

                    Script.GlobalsAdd("MGO", MGO);
                    Script.GlobalsAdd("World", this);
                    Script.Execute(Settings.Script);
                }
                catch (Exception e)
                {
                    return e.GetType().FullName + ":\n" + e.Message;
                }
            }

            return "";
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
        public void Draw(Graphics g, bool drawBackground, object[] selected)
        {
            if (drawBackground && Settings.BackgroundImg != null)
                g.DrawImage(Settings.BackgroundImg, 0, 0, Settings.GameWindowWidth, Settings.LevelHeight);

            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            foreach (GameObject go in AllElements)
            {
                if (Array.IndexOf<object>(selected, go) != -1)
                    g.DrawRectangle(pen, go.Left-2, go.Top-2, go.Width+3, go.Height+3);

                go.Draw(g);
            }
        }


        public void Add(GameObject go)
        {
            if (go is MovingElement) // check first, because MovingElement inherits from StickyElement!
                movingelements.Add((MovingElement)go);
            else if (go is StickyElement)
                stickyelements.Add((StickyElement)go);
            else if (go is Enemy)
                enemies.Add((Enemy)go);
        }
        public void Remove(GameObject go)
        {
            if (go is MovingElement) // check first, because MovingElement inherits from StickyElement!
                movingelements.Remove((MovingElement)go);
            else if (go is StickyElement)
                stickyelements.Remove((StickyElement)go);
            else if (go is Enemy)
                enemies.Remove((Enemy)go);
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
                foreach (MovingElement me in movingelements)
                {
                    ret.Add(me);
                }
                foreach (Enemy e in enemies)
                {
                    ret.Add(e);
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
            WorldSerialization.Serialize(filename, stickyelements, movingelements, enemies, Settings);
        }

        public void Deserialize()
        {
            WorldSerialization.Deserialize(fileName,
                out Settings,
                out stickyelements,
                out movingelements,
                out enemies,
                this,
                aiEventHandler);
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
        private string script;

        public Changed_Setting cLevelWidth;
        public Changed_Setting cLevelHeight;
        public Changed_Setting cGameWindowWidth;

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
                    case Backgrounds.Green_Hills:
                        BackgroundImg = Image.FromFile(Files.background_green_hills);
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
            script = (string)ser["Script"];
        }
    }
}
