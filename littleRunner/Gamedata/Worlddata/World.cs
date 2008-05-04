using System;
using System.Collections.Generic;

using System.Drawing;

using IronPython.Runtime.Exceptions;
using littleRunner.GameObjects;
using littleRunner.GameObjects.MainGameObjects;


namespace littleRunner.Gamedata.Worlddata
{
    public delegate void InvalidateHandler();

    public class World
    {
        List<Enemy> enemies;
        List<StickyElement> stickyelements;
        List<MovingElement> movingelements;

        public string fileName;
        MainGameObject mainGameObject;
        GameSession session;
        public LevelSettings Settings;
        public InvalidateHandler Invalidate;
        private GameEventHandler aiEventHandler;
        public PlayMode PlayMode;
        public Script Script;
        private GamePoint viewport;

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
        public GamePoint Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        // new world with the editor
        private World(PlayMode playMode)
        {
            this.PlayMode = playMode;
            this.viewport = new GamePoint(0, 0);

            if (playMode == PlayMode.Editor)
                mainGameObject = new NullMGO();
        }
        public World(int gameWindowWidth, int gameWindowHeight,
            int levelWidth, int levelHeight,
            InvalidateHandler invalidate, PlayMode playMode)
            : this(playMode)
        {
            Settings = new LevelSettings();
            Settings.GameWindowWidth = gameWindowWidth;
            Settings.GameWindowHeight = gameWindowHeight;
            Settings.LevelWidth = levelWidth;
            Settings.LevelHeight = levelHeight;
            this.Invalidate = invalidate;

            this.aiEventHandler = GameAI.NullAiEventHandlerMethod;
            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
        }
        // new world with the game
        public World(string filename, InvalidateHandler invalidate, GameEventHandler aiEventHandler, GameSession session, PlayMode playMode)
            : this(playMode)
        {
            this.Invalidate = invalidate;
            this.fileName = filename;
            this.aiEventHandler = aiEventHandler;
            this.session = session;
            Deserialize();
        }
        public World(string filename, InvalidateHandler invalidate, GameSession session, PlayMode playMode)
            : this(filename, invalidate, GameAI.NullAiEventHandlerMethod, session, playMode)
        {
        }

        public void Init(MainGameObject mainGameObject) // only called when starting the game
        {
            this.mainGameObject = mainGameObject;
        }


        public string InitScript()
        {
            if (this.fileName == "Data/Levels/bonuslevel1.lrl")
            {
            }
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
                            Script.Execute("handler." + go.Name + " = EventAttrDict()");
                        }
                    }


                    Script.GlobalsAdd("MGO", MGO);
                    Script.GlobalsAdd("World", this);
                    Script.GlobalsAdd("Session", session);
                    Script.GlobalsAdd("AiEventHandler", aiEventHandler);

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

            g.TranslateTransform(viewport.X, viewport.Y);
            foreach (GameObject go in AllElements)
            {
                go.Draw(g);
            }
            g.TranslateTransform(-viewport.X, -viewport.Y);
        }
        public void Draw(Graphics g, bool drawBackground, object[] selected)
        {
            Draw(g, drawBackground);

            g.TranslateTransform(viewport.X, viewport.Y);
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            foreach (GameObject go in AllElements)
            {
                if (Array.IndexOf<object>(selected, go) != -1)
                    g.DrawRectangle(pen, go.Left-2, go.Top-2, go.Width+3, go.Height+3);
            }
            g.TranslateTransform(-viewport.X, -viewport.Y);
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
}