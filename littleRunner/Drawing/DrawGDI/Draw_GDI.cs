using System;
using System.Windows.Forms;
using littleRunner.Drawing;


namespace littleRunner.Drawing.GDI
{
    class Draw_GDI : Draw
    {
        System.Drawing.Graphics g;

        public Draw_GDI(System.Drawing.Graphics g)
        {
            this.g = g;
        }



        public override void DrawImage(dImage image, float x, float y, int width, int height)
        {
            dImage_GDI gdiImage = (dImage_GDI)image;
            g.DrawImage(gdiImage.ToGDI(), (int)x, (int)y, width, height);
        }
        public override void DrawRectangle(dPen pen, float x, float y, int width, int height)
        {
            g.DrawRectangle(pen.ToGDIPen(), (int)x, (int)y, width, height);
        }
        public override void FillRectangle(dPen pen, float x, float y, int width, int height)
        {
            g.FillRectangle(pen.ToGDIBrush(), (int)x, (int)y, width, height);
        }
        public override void DrawString(string text, dFont font, dColor color, float x, float y)
        {
            System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(color.ToGDI());
            System.Drawing.Font f = new System.Drawing.Font(font.Family, font.Size, font.Style.ToGDI());
            g.DrawString(text, f, b, (int)x, (int)y, font.Format.ToGDI());
        }


        public override void MoveCoords(float x, float y)
        {
            g.TranslateTransform((int)x, (int)y);
        }
    }
}