using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using littleRunner.GameObjects.MovingElements;


namespace littleRunner.GameObjects.Objects
{
    enum BoxType
    {
        GoodMushroom,
        PoisonMushroom,
        FireFlower,
        ModeDependent
    }
    enum BoxStyle
    {
        Yellow
    }
    class Box : StickyImageElement
    {
        BoxStyle style;
        BoxType btype;
        bool got;

        override public bool canStandOn
        {
            get { return true; }
        }
        [Category("Box")]
        public BoxStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                switch (style)
                {
                    case BoxStyle.Yellow:
                        CurImgFilename = Files.box1;
                        break;
                }
            }
        }
        [Category("Box")]
        public BoxType BoxType
        {
            get { return btype; }
            set { btype = value; }
        }

        private void getGoodie(BoxType type)
        {
            Mushroom m;
            switch (type)
            {
                case BoxType.GoodMushroom:
                    m = new Mushroom(MushroomType.Good, Top, Left);
                    m.Init(World, AiEventHandler);
                    World.MovingElements.Add(m);
                    break;

                case BoxType.PoisonMushroom:
                    m = new Mushroom(MushroomType.Poison, Top, Left);
                    m.Init(World, AiEventHandler);
                    World.MovingElements.Add(m);
                    break;

                case BoxType.FireFlower:
                    FireFlower f = new FireFlower(Top, Left);
                    f.Init(World, AiEventHandler);
                    World.StickyElements.Add(f);
                    break;
            }
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (!got && direction == GameDirection.Bottom && who == GameElement.MGO)
            {
                switch (btype)
                {
                    case BoxType.GoodMushroom:
                        getGoodie(BoxType.GoodMushroom);
                        break;
                    case BoxType.PoisonMushroom:
                        getGoodie(BoxType.PoisonMushroom);
                        break;
                    case BoxType.FireFlower:
                        getGoodie(BoxType.FireFlower);
                        break;
                    case BoxType.ModeDependent:
                        switch(World.MGO.Mode)
                        {
                            case MainGameObjectMode.Small:
                                getGoodie(BoxType.GoodMushroom); break;
                            case MainGameObjectMode.Normal:
                            case MainGameObjectMode.NormalFire:
                                getGoodie(BoxType.FireFlower); break;
                        }
                        break;
                }

                got = true;
            }
        }

        public Box()
            : base()
        {
            got = false;
        }
        public Box(int top, int left, BoxStyle style)
            : base(top, left)
        {
            got = false;
            Style = style;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["BoxType"] = btype;
            ser["BoxStyle"] = style;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            btype = (BoxType)ser["BoxType"];
            Style = (BoxStyle)ser["BoxStyle"];
        }
    }
}
