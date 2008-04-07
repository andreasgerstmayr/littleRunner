using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using System.Drawing;


namespace littleRunner
{
    abstract class GameObject
    {
        public abstract void Draw(Graphics g);

        public virtual void onKeyPress(GameKey gkey)
        {
        }

        private World world;
        private int top;
        private int left;
        private int width;
        private int height;

        [Browsable(false)]
        public World World
        {
            get { return world; }
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

        public virtual void Init(World world)
        {
            this.world = world;
        }


        public virtual Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string,object>();
            ser["Top"] = top;
            ser["Left"] = left;
            ser["Width"] = width;
            ser["Height"] = height;
            return ser;
        }
        public virtual void Deserialize(Dictionary<string, object> ser)
        {
            top = (int)ser["Top"];
            left = (int)ser["Left"];
            width = (int)ser["Width"];
            height = (int)ser["Height"];
        }
    }
}
