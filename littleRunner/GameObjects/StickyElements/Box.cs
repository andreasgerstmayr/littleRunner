using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using littleRunner.GameObjects.MovingElements;


namespace littleRunner.GameObjects.StickyElements
{
    enum BoxType
    {
        GoodMushroom,
        PoisonMushroom,
        LiveMushroom,
        FireFlower,
        ModeDependent,
        Star,
        ImmortializeStar
    }
    enum BoxStyle
    {
        Yellow
    }
    class Box : StickyImageElement
    {
        BoxStyle style;
        BoxType btype;
        int got;
        int canGet;
        DateTime canGetNext;

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
        [Category("Box")]
        public int Content
        {
            get { return canGet; }
            set { canGet = value; }
        }


        private void getGoodie(BoxType type)
        {
            Mushroom m;
            switch (type)
            {
                case BoxType.GoodMushroom:
                    m = new Mushroom(MushroomType.Good, GameDirection.Right, Top, Left);
                    m.Init(World, AiEventHandler);
                    World.MovingElements.Add(m);
                    break;

                case BoxType.PoisonMushroom:
                    m = new Mushroom(MushroomType.Poison, GameDirection.Right, Top, Left);
                    m.Init(World, AiEventHandler);
                    World.MovingElements.Add(m);
                    break;

                case BoxType.LiveMushroom:
                    m = new Mushroom(MushroomType.Live, GameDirection.Right, Top, Left);
                    m.Init(World, AiEventHandler);
                    World.MovingElements.Add(m);
                    break;

                case BoxType.FireFlower:
                    FireFlower f = new FireFlower(Top, Left);
                    f.Init(World, AiEventHandler);
                    World.StickyElements.Add(f);
                    break;

                case BoxType.Star:
                    JumpingStar js = new JumpingStar(Top, Left);
                    js.Init(World, AiEventHandler);
                    World.MovingElements.Add(js);
                    break;

                case BoxType.ImmortializeStar:
                    ImmortializeStar iStar = new ImmortializeStar(GameDirection.Right, Top, Left);
                    iStar.Init(World, AiEventHandler);
                    World.MovingElements.Add(iStar);
                    break;
            }
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (got < canGet && DateTime.Now > canGetNext &&
                direction == GameDirection.Bottom && who == GameElement.MGO)
            {
                switch (btype)
                {
                    case BoxType.GoodMushroom:      getGoodie(BoxType.GoodMushroom);    break;
                    case BoxType.PoisonMushroom:    getGoodie(BoxType.PoisonMushroom);  break;
                    case BoxType.LiveMushroom:      getGoodie(BoxType.LiveMushroom);    break;
                    case BoxType.FireFlower:        getGoodie(BoxType.FireFlower);      break;
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

                    case BoxType.Star:              getGoodie(BoxType.Star);             break;
                    case BoxType.ImmortializeStar:  getGoodie(BoxType.ImmortializeStar); break;
                }

                got++;
                canGetNext = DateTime.Now.AddMilliseconds(200);
            }
        }

        public Box()
            : base()
        {
            got = 0;
            canGet = 1;
            canGetNext = DateTime.Now;
        }
        public Box(int top, int left, BoxStyle style)
            : base(top, left)
        {
            got = 0;
            canGet = 1;
            canGetNext = DateTime.Now;

            Style = style;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["BoxType"] = btype;
            ser["BoxStyle"] = style;
            ser["Content"] = canGet;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            btype = (BoxType)ser["BoxType"];
            Style = (BoxStyle)ser["BoxStyle"];
            Content = (int)ser["Content"];
        }
    }
}
