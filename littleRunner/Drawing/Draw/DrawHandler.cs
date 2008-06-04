using System;
using System.Windows.Forms;

using System.Drawing;


namespace littleRunner.Drawing
{
    public delegate void UpdateHandler(Draw d);

    public abstract class DrawHandler
    {
        protected Control c;
        protected UpdateHandler updateHandler;

        public DrawHandler(Control c, UpdateHandler updateHandler)
        {
            this.c = c;
            this.updateHandler = updateHandler;
        }

        abstract public void Update();
    }
}
