using System;
using System.Windows.Forms;

using System.Drawing;


namespace littleRunner.Drawing
{
    abstract public class Draw
    {
        public abstract void DrawImage(dImage image, int x, int y, int width, int height);
        public abstract void DrawRectangle(dPen pen, int x, int y, int width, int height);
        public abstract void FillRectangle(dPen pen, int x, int y, int width, int height);
        public abstract void DrawString(string text, dFont font, dColor color, int x, int y);
        public abstract void MoveCoords(int x, int y);
    }
}