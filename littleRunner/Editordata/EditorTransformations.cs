using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Gamedata.Worlddata;
using littleRunner.GameObjects;


namespace littleRunner
{
    class EditorTransformations
    {

        static public void Move(int offset, ref World world)
        {
            foreach (GameObject go in world.AllElements)
            {
                go.Top += offset;
            }
        }

    }
}
