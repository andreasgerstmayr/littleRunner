using System;
using System.Collections.Generic;
using System.Drawing;


namespace littleRunner.Drawing
{
    public enum dPenStyle
    {
        Dashed,
        Solid
    }
    public class dPen
    {
        public dColor Color;
        public dPenStyle Style;

        public dPen(dColor color, dPenStyle style)
        {
            this.Color = color;
            this.Style = style;
        }
        public dPen(dColor color)
            : this(color, dPenStyle.Solid)
        {
        }


        public static dPen FromGDI(System.Drawing.Pen gdiPen)
        {
            dPenStyle style = dPenStyle.Solid;
            switch (gdiPen.DashStyle)
            {
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    style = dPenStyle.Solid; break;
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    style = dPenStyle.Dashed; break;
            }

            return new dPen(new dColor(gdiPen.Color), style);
        }

        public System.Drawing.Pen ToGDIPen()
        {
            System.Drawing.Pen p = new System.Drawing.Pen(Color.ToGDI());
            switch (Style)
            {
                case dPenStyle.Solid:
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case dPenStyle.Dashed:
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
            }
            return p;
        }
        public System.Drawing.Brush ToGDIBrush()
        {
            return new System.Drawing.SolidBrush(Color.ToGDI());
        }

        public static dPen Black
        {
            get { return new dPen(new dColor(System.Drawing.Color.Black), dPenStyle.Solid); }
        }
    }
}
