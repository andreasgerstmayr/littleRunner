using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects.MainGameObjects
{
    class NullMGO : MainGameObject
    {
        public override void Move(MoveType mtype, int length)
        {
            throw new NotImplementedException();
        }
        public override MainGameObjectMode currentMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
