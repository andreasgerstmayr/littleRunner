using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using littleRunner.Drawing;
using littleRunner.Gamedata.Worlddata;


namespace littleRunner.GameObjects
{
    public abstract class GameObject
    {
        private World world;
        private GameEventHandler aiEventHandler;
        private int top;
        private int left;
        private int width;
        private int height;
        private string name;


        public abstract void Update(Draw d);
        public virtual void onKeyPress(GameKey gkey)
        {
        }
        [Browsable(false), Category("Object")]
        public virtual bool canStandOn
        {
            get { return false; }
        }


        [Browsable(false)]
        protected World World
        {
            get { return world; }
        }
        [Browsable(false)]
        protected GameEventHandler AiEventHandler
        {
            get { return aiEventHandler; }
        }
        [Category("Script")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Category("Position")]
        public int Top
        {
            get { return top; }
            set { top = value; }
        }
        [Category("Position")]
        public int Bottom
        {
            get { return top + height; }
            set { top = value - height; }
        }
        [Category("Position")]
        public int Left
        {
            get { return left; }
            set { left = value; }
        }
        [Category("Position")]
        public int Right
        {
            get { return left + width; }
            set { left = value - width; }
        }
        [Category("Size")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        [Category("Size")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool Hit(int top, int left)
        {
            if (top >= Top && top <= Bottom && left >= Left && left <= Right)
                return true;
            return false;
        }
        public bool InRectangle(Rectangle rect)
        {
            if (Left >= rect.X && Right <= rect.X + rect.Width &&
                Top >= rect.Y && Bottom <= rect.Y + rect.Height)
                return true;
            return false;
        }


        public virtual void Init(World world, GameEventHandler aiEventHandler)
        {
            this.world = world;
            this.aiEventHandler = aiEventHandler;
        }


        public virtual Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string,object>();
            ser["Name"] = name; 
            ser["Top"] = top;
            ser["Left"] = left;
            ser["Width"] = width;
            ser["Height"] = height;
            return ser;
        }
        public virtual void Deserialize(Dictionary<string, object> ser)
        {
            name = (string)ser["Name"];
            top = (int)ser["Top"];
            left = (int)ser["Left"];
            width = (int)ser["Width"];
            height = (int)ser["Height"];
        }
    }
}
