using System;
using System.Collections.Generic;
using System.Drawing;


namespace littleRunner.Drawing
{
    public enum dFontWeight
    {
        Normal,
        Bold
    }
    public class dFontStyle
    {
        dFontWeight weight;

        public dFontWeight Weight { get { return weight; } }

        public dFontStyle(dFontWeight weight)
        {
            this.weight = weight;
        }


        public System.Drawing.FontStyle ToGDI()
        {
            switch (weight)
            {
                case dFontWeight.Normal:
                    return System.Drawing.FontStyle.Regular;
                case dFontWeight.Bold:
                    return System.Drawing.FontStyle.Bold;

                default: return System.Drawing.FontStyle.Regular;
            }
        }
    }
    public enum dFontAligment
    {
        Left,
        Center,
        Right
    }
    public class dFontFormat
    {
        dFontAligment aligment;

        public dFontFormat(dFontAligment aligment)
        {
            this.aligment = aligment;
        }


        public System.Drawing.StringFormat ToGDI()
        {
            StringFormat format = new StringFormat();
            switch (aligment)
            {
                case dFontAligment.Left: format.Alignment = StringAlignment.Near; break;
                case dFontAligment.Center: format.Alignment = StringAlignment.Center; break;
                case dFontAligment.Right: format.Alignment = StringAlignment.Far; break;
            }
            return format;
        }
    }


    public class dFont
    {
        string family;
        float size;
        dFontStyle style;
        dFontFormat format;

        public string Family
        {
            get { return family; }
        }
        public float Size
        {
            get { return size; }
        }
        public dFontStyle Style
        {
            get { return style; }
        }
        public dFontFormat Format
        {
            get { return format; }
        }


        public dFont(string family, float size, dFontStyle style, dFontFormat format)
        {
            this.family = family;
            this.size = size;
            this.style = style;
            this.format = format;
        }
        public dFont(string family, float size)
            : this(family, size, new dFontStyle(dFontWeight.Normal), new dFontFormat(dFontAligment.Left))
        {
        }
    }
}
