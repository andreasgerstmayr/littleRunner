using System;
using System.Collections.Generic;
using System.Drawing;
using IronPython.Runtime.Exceptions;

using littleRunner.Drawing;
using littleRunner.GameObjects;
using littleRunner.GameObjects.MainGameObjects;


namespace littleRunner.Gamedata.Worlddata
{
    public class World
    {
        List<Enemy> enemies;
        List<StickyElement> stickyelements;
        List<MovingElement> movingelements;

        public string fileName;
        MainGameObject mainGameObject;
        GameSession session;
        public LevelSettings Settings;
        public Drawing.DrawHandler DrawHandler;
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
        public Script pScript
        {
            get { return Script; }
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
            Drawing.DrawHandler drawHandler, PlayMode playMode)
            : this(playMode)
        {
            Settings = new LevelSettings();
            Settings.GameWindowWidth = gameWindowWidth;
            Settings.GameWindowHeight = gameWindowHeight;
            Settings.LevelWidth = levelWidth;
            Settings.LevelHeight = levelHeight;
            this.DrawHandler = drawHandler;

            this.aiEventHandler = GameAI.NullAiEventHandlerMethod;
            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
        }
        private void EmptyDraw(Draw d)
        {
        }
        public World() // get default world
            : this(700, 550, 1000, 550, null, PlayMode.Editor)
        {
        }

        // new world with the game
        public World(string filename, Drawing.DrawHandler drawHandler, GameEventHandler aiEventHandler, GameSession session, PlayMode playMode)
            : this(playMode)
        {
            this.DrawHandler = drawHandler;
            this.fileName = filename;
            this.aiEventHandler = aiEventHandler;
            this.session = session;
            Deserialize();
        }
        public World(string filename, Drawing.DrawHandler drawHandler, GameSession session, PlayMode playMode)
            : this(filename, drawHandler, GameAI.NullAiEventHandlerMethod, session, playMode)
        {
        }

        public void Init(MainGameObject mainGameObject) // only called when starting the game
        {
            this.mainGameObject = mainGameObject;
        }


        public string InitScript()
        {
            Script = new Script(this);

            if (Settings.Script.Length > 0)
            {
                try
                {
                    Script.GlobalsAdd("MGO", MGO);
                    Script.GlobalsAdd("World", this);
                    Script.GlobalsAdd("Session", session);
                    Script.GlobalsAdd("AiEventHandler", aiEventHandler);
                    Script.GlobalsAdd("GetFrameFactor", new GameAI.GetFrameFactorDelegate(GameAI.GetFrameFactor));

                    Script.Execute("lr = littleRunner(MGO, World, Session, AiEventHandler, GetFrameFactor)");


                    foreach (GameObject go in this.AllElements)
                    {
                        if (go.Name != null && go.Name != "")
                        {
                            Script.GlobalsAdd(go.Name, go);
                            Script.Execute("lr.Handler." + go.Name + " = EventAttrDict()");
                        }
                    }

                    Script.Execute("handler = lr.Handler"); // very important! because Script.cs needs access to Globals["handler"].
                    Script.Execute(Settings.Script);
                    Script.Init = false;
                }
                catch (Exception e)
                {
                    return e.GetType().FullName + ":\n" + e.Message;
                }
            }

            return "";
        }

        public void Update(Draw d)
        {
            if (Settings.BackgroundImg != null)
                d.DrawImage(Settings.BackgroundImg, 0, 0, Settings.GameWindowWidth, Settings.LevelHeight);

            d.MoveCoords(viewport.X, viewport.Y);
            foreach (GameObject go in AllElements)
            {
                go.Update(d);
            }
            d.MoveCoords(-viewport.X, -viewport.Y);
        }
        public void Update(Draw d, object[] selected)
        {
            Update(d);

            d.MoveCoords(viewport.X, viewport.Y);
            dPen pen = new dPen(new dColor(Color.Black.R, Color.Black.G, Color.Black.B), dPenStyle.Dashed);

            foreach (GameObject go in AllElements)
            {
                if (Array.IndexOf<object>(selected, go) != -1)
                    d.DrawRectangle(pen, go.Left-2, go.Top-2, go.Width+3, go.Height+3);
            }
            d.MoveCoords(-viewport.X, -viewport.Y);
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
