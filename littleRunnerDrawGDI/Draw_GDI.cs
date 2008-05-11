using System;
using System.Windows.Forms;
using littleRunner.Drawing;


namespace littleRunner.Drawing.GDI
{
    class DrawGDI : Draw
    {
        System.Drawing.Graphics g;

        public DrawGDI(System.Drawing.Graphics g)
        {
            this.g = g;
        }



        public override void DrawImage(dImage image, int x, int y, int width, int height)
        {
            g.DrawImage(((dImage_GDI)image).ToGDI(), x, y, width, height);
        }
        public override void DrawRectangle(dPen pen, int x, int y, int width, int height)
        {
            g.DrawRectangle(pen.ToGDIPen(), x, y, width, height);
        }
        public override void FillRectangle(dPen pen, int x, int y, int width, int height)
        {
            g.FillRectangle(pen.ToGDIBrush(), x, y, width, height);
        }
        public override void DrawString(string text, dFont font, dColor color, int x, int y)
        {
            System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(color.ToGDI());
            System.Drawing.Font f = new System.Drawing.Font(font.Family, font.Size, font.Style.ToGDI());
            g.DrawString(text, f, b, x, y, font.Format.ToGDI());
        }


        public override void MoveCoords(int x, int y)
        {
            g.TranslateTransform(x, y);
        }
    }
}