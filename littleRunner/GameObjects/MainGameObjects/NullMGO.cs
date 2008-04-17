using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects.MainGameObjects
{
    class NullMGO : MainGameObject
    {
        public override GameDirection Direction
        {
            get { return GameDirection.None; }
        }
        public override void Move(MoveType mtype, int value, GameInstruction instruction)
        {
            throw new NotImplementedException();
        }
        public override MainGameObjectMode Mode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        } 
        public override void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
