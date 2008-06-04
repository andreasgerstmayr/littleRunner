using System;
using System.Collections.Generic;
using System.Windows.Forms;

using littleRunner.Drawing;
using System.Reflection;


namespace littleRunner.Drawing.Helpers
{
    static class GetDraw
    {
        static object getInstance(string className, params object[] param)
        {
            return Activator.CreateInstance(Type.GetType(className), param);
        }


        static string GetSmallModeName()
        {
            switch (Globals.VideoRenderMode)
            {
                case VideoRenderMode.GDI: return "GDI";
                default: return null;
            }
        }

        public static dImage Image(string filename)
        {
            string mode = GetSmallModeName();
            string type = "littleRunner.Drawing." + mode + ".dImage_" + mode;

            return (dImage)getInstance(type, filename);
        }

        public static DrawHandler DrawHandler(Control c, UpdateHandler updateHandler)
        {
            string mode = GetSmallModeName();
            string type = "littleRunner.Drawing." + mode + ".DrawHandler_" + mode;

            return (DrawHandler)getInstance(type, c, updateHandler);
        }
    }
}
