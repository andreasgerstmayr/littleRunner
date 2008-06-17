using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace littleRunner.Editordata
{
    class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
            : base()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
