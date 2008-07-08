using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;


namespace littleRunner.GameObjects.MainGameObjects
{
    class NullMGO : MainGameObject
    {
        public override GameDirection Direction
        {
            get { return GameDirection.None; }
        }
        public override void Move(MoveType mtype, float value, GameInstruction instruction)
        {
            throw new NotImplementedException();
        }
        public override MainGameObjectMode Mode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        } 
        public override void Update(Draw d)
        {
            throw new NotImplementedException();
        }
    }
}
