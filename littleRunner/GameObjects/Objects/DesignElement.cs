using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects.Objects
{
    enum DesignElement
    {
        Tree
    }
    class DesignElements : StickyImageElement
    {
        DesignElement element;
        override public bool canStandOn
        {
            get { return false; }
        }
        public DesignElement Element
        {
            get { return element; }
            set
            {
                element = value;
                switch (element)
                {
                    case DesignElement.Tree:
                        CurImgFilename = Files.f[gFile.tree];
                        break;
                }
            }
        }

        public DesignElements()
            : base()
        {
        }
        public DesignElements(int top, int left, DesignElement element)
            : base(top, left)
        {
            Element = element;
        }
    }
}
