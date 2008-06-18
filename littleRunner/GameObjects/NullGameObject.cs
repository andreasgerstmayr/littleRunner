using System;


namespace littleRunner.GameObjects
{
    class NullGameObject : GameObject
    {
        public override void Update(littleRunner.Drawing.Draw d)
        {
        }

        public NullGameObject(int top, int left, int width, int height)
            : base()
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        public NullGameObject()
            : this(0, 0, 0, 0)
        {
        }
    }
}
