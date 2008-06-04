using System;
using System.Windows.Forms;

using littleRunner.Drawing;


namespace littleRunner.Drawing.GDI
{
    public class DrawHandler_GDI : DrawHandler
    {
        public DrawHandler_GDI(Control c, UpdateHandler updateHandler)
            : base(c, updateHandler)
        {
            this.c.Paint += new PaintEventHandler(c_Paint);
        }
        public override void Update()
        {
            c.Invalidate();
        }


        void c_Paint(object sender, PaintEventArgs e)
        {
            updateHandler(new Draw_GDI(e.Graphics));
        }
    }
}
