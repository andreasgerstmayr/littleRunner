using System;
using System.Windows.Forms;

using System.Drawing;


namespace littleRunner.Drawing
{
    abstract public class Draw
    {
        public abstract void DrawImage(dImage image, float x, float y, int width, int height);
        public abstract void DrawRectangle(dPen pen, float x, float y, int width, int height);
        public abstract void FillRectangle(dPen pen, float x, float y, int width, int height);
        public abstract void DrawString(string text, dFont font, dColor color, float x, float y);
        public abstract void MoveCoords(float x, float y);
    }
}