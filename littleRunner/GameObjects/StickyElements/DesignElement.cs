using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner.GameObjects.StickyElements
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
        [Category("Design element")]
        public DesignElement Element
        {
            get { return element; }
            set
            {
                element = value;
                switch (element)
                {
                    case DesignElement.Tree:
                        CurImgFilename = Files.tree;
                        break;
                }
            }
        }

        public DesignElements()
            : base()
        {
        }
        public DesignElements(float top, float left, DesignElement element)
            : base(top, left)
        {
            Element = element;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["Element"] = element;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Element = (DesignElement)ser["Element"];
        }
    }
}
