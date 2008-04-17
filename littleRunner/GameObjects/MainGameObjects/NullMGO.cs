using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects.MainGameObjects
{
    class NullMGO : MainGameObject
    {
        public override void Move(MoveType mtype, int length, GameInstruction doThen)
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
