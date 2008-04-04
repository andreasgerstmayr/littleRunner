using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace littleRunner
{
    delegate void MyInvalidateEventHandler();

    class World
    {
        List<Enemy> enemies;
        List<StickyElement> stickyelements;
        List<MovingElement> movingelements;
 
        public string fileName;
        MainGameObject mainGameObject;
        public int Width;
        public int Height;
        public MyInvalidateEventHandler Invalidate;
        public bool playMode;

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
            this.Width = width;
            this.Height = height;
            this.Invalidate = invalidate;

            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
            this.playMode = playMode;
        }
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
        }

        public void Draw(Graphics g)
        {
            if (playMode)
                ImageAnimator.UpdateFrames();
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
            fileName = filename;
            object[] save = new object[stickyelements.Count+enemies.Count+1];
            save[0] = Width.ToString() + "x" + Height.ToString();

            for (int i = 0; i < enemies.Count; i++)
            {
                object[] arr = new object[2];
                arr[0] = enemies[i].GetType();
                arr[1] = enemies[i].Serialize();

                save[i+1] = arr;
            }
            for (int i = 0; i < stickyelements.Count; i++)
            {
                object[] arr = new object[2];
                arr[0] = stickyelements[i].GetType();
                arr[1] = stickyelements[i].Serialize();

                save[i+enemies.Count+1] = arr;
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, save);
            fs.Close();
        }

        public void Deserialize()//string filename)
        {
            enemies = new List<Enemy>();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();

            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            object[] elements = (object[])bf.Deserialize(fs);
            fs.Close();

            string[] dimension = elements[0].ToString().Split(new char[] { 'x' });
            Width = Convert.ToInt32(dimension[0]);
            Height = Convert.ToInt32(dimension[1]);

            for (int i = 1; i < elements.Length; i++)
            {
                object[] arr = (object[])elements[i];

                Type objtype = (Type)arr[0];
                GameObject go = (GameObject)Activator.CreateInstance(objtype);
                go.Deserialize((Dictionary<string, object>)arr[1]);

                go.Init(this);
                if (typeof(Enemy).IsAssignableFrom(objtype))
                    enemies.Add((Enemy)go);
                else if (typeof(StickyElement).IsAssignableFrom(objtype))
                    stickyelements.Add((StickyElement)go);
            }
        }
    }
}
