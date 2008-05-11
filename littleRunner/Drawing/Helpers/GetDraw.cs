using System;
using System.Collections.Generic;
using System.Windows.Forms;

using littleRunner.Drawing;
using littleRunner.Drawing.GDI;


namespace littleRunner.Drawing.Helpers
{
    static class GetDraw
    {
        static object getInstance(string className, params object[] param)
        {
            return Activator.CreateInstance(Type.GetType(className), param);
        }



        public static dImage Image()
        {
            string mode = Enum.GetName(typeof(VideoRenderMode), Globals.VideoRenderMode);
            string type = "littleRunner.Drawing." + mode + ".dImage_" + mode + ", littleRunnerDraw" + mode;

            return (dImage)getInstance(type);
        }
        public static dImage Image(string filename)
        {
            string mode = Enum.GetName(typeof(VideoRenderMode), Globals.VideoRenderMode);
            string type = "littleRunner.Drawing." + mode + ".dImage_" + mode + ", littleRunnerDraw" + mode;

            return (dImage)getInstance(type, filename);
        }

        public static DrawHandler DrawHandler(Control c, UpdateHandler updateHandler)
        {
            string mode = Enum.GetName(typeof(VideoRenderMode), Globals.VideoRenderMode);
            string type = "littleRunner.Drawing." + mode + ".DrawHandler_" + mode + ", littleRunnerDraw" + mode;

            return (DrawHandler)getInstance(type, c, updateHandler);
        }
    }
}
