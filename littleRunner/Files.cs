using System;
using System.Collections.Generic;
using System.Text;


namespace littleRunner
{
    enum gFile
    {
        background_blue_hills,
        background_green_hills,
        box1,
        brick_blue,
        brick_ice,
        brick_red,
        brick_yellow,
        brick_invisible,
        fire,
        fire_flower,
        floor_left,
        floor_middle,
        floor_right,
        gumba_brown,
        icon_png,
        levelend_house,
        mushroom_green,
        mushroom_poison,
        pipe_green_main,
        pipe_green_up,
        spika_green,
        spika_grey,
        spika_orange,
        star,
        tree,
        turtle_green_down,
        turtle_green,
        tux,
        tux_right
    }


    class Files
    {
        public static Dictionary<gFile, string> f = new Dictionary<gFile, string>();

        public static void fill()
        {
            f[gFile.background_blue_hills] = "Data/Images/Background/blue_hills.png";
            f[gFile.background_green_hills] = "Data/Images/Background/green_hills.png";
            f[gFile.box1] = "Data/Images/Box/box1.png";
            f[gFile.brick_blue] = "Data/Images/Brick/brick_blue.png";
            f[gFile.brick_ice] = "Data/Images/Brick/brick_ice.png";
            f[gFile.brick_red] = "Data/Images/Brick/brick_red.png";
            f[gFile.brick_yellow] = "Data/Images/Brick/brick_yellow.png";
            f[gFile.brick_invisible] = "Data/Images/Brick/brick_invisible.png";
            f[gFile.fire] = "Data/Images/GameElement/fire.png";
            f[gFile.fire_flower] = "Data/Images/GameItem/fire_flower.png";
            f[gFile.floor_left] = "Data/Images/Floor/floor_left.png";
            f[gFile.floor_middle] = "Data/Images/Floor/floor.png";
            f[gFile.floor_right] = "Data/Images/Floor/floor_right.png";
            f[gFile.gumba_brown] = "Data/Images/Enemie/Gumba/Brown/walk_#L[1-10].png";
            f[gFile.icon_png] = "Data/Images/Main/TuxIcon.png";
            f[gFile.levelend_house] = "Data/Images/LevelEnd/house.png";
            f[gFile.mushroom_green] = "Data/Images/GameItem/mushroom_green.png";
            f[gFile.mushroom_poison] = "Data/Images/GameItem/mushroom_poison.png";
            f[gFile.pipe_green_main] = "Data/Images/Pipe/pipe_green_main.png";
            f[gFile.pipe_green_up] = "Data/Images/Pipe/pipe_green_up.png";
            f[gFile.spika_green] = "Data/Images/Enemie/Spika/green.png";
            f[gFile.spika_grey] = "Data/Images/Enemie/Spika/grey.png";
            f[gFile.spika_orange] = "Data/Images/Enemie/Spika/orange.png";
            f[gFile.star] = "Data/Images/GameItem/star.png";
            f[gFile.tree] = "Data/Images/GameElement/tree.png";
            f[gFile.turtle_green_down] = "Data/Images/Enemie/Turtle/Green/turtle_down.png";
            f[gFile.turtle_green] = "Data/Images/Enemie/Turtle/Green/walk_#L[0-2].png";
            f[gFile.tux] = "Data/Images/MainGameObject/Tux/tux_#R[0-5].png";
        }
    }
}
