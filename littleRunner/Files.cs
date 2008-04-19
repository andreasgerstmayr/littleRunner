using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace littleRunner
{
    class Files
    {
        public static string background_blue_hills
        {
            get { return "Data/Images/Background/blue_hills.png"; }
        }
        public static string background_green_hills
        {
            get { return "Data/Images/Background/green_hills.png"; }
        }
        public static string box1
        {
            get { return "Data/Images/Box/box1.png"; }
        }
        public static string brick_blue
        {
            get { return "Data/Images/Brick/brick_blue.png"; }
        }
        public static string brick_ice
        {
            get { return "Data/Images/Brick/brick_ice.png"; }
        }
        public static string brick_red
        {
            get { return "Data/Images/Brick/brick_red.png"; }
        }
        public static string brick_yellow
        {
            get { return "Data/Images/Brick/brick_yellow.png"; }
        }
        public static string brick_invisible
        {
            get { return "Data/Images/Brick/brick_invisible.png"; }
        }
        public static string fire
        {
            get { return "Data/Images/GameElement/fire.png"; }
        }
        public static string fire_flower
        {
            get { return "Data/Images/GameItem/fire_flower.png"; }
        }
        public static string floor_left
        {
            get { return "Data/Images/Floor/floor_left.png"; }
        }
        public static string floor_middle
        {
            get { return "Data/Images/Floor/floor.png"; }
        }
        public static string floor_right
        {
            get { return "Data/Images/Floor/floor_right.png"; }
        }
        public static string gumba_brown
        {
            get { return "Data/Images/Enemie/Gumba/Brown/walk_#N[1-10].png"; }
        }
        public static string icon_png
        {
            get { return "Data/Images/Main/TuxIcon.png"; }
        }
        public static string levelend_house
        {
            get { return "Data/Images/LevelEnd/house.png"; }
        }
        public static string mushroom_green
        {
            get { return "Data/Images/GameItem/mushroom_green.png"; }
        }
        public static string mushroom_poison
        {
            get { return "Data/Images/GameItem/mushroom_poison.png"; }
        }
        public static string pipe_green_main
        {
            get { return "Data/Images/Pipe/pipe_green_main.png"; }
        }
        public static string pipe_green_up
        {
            get { return "Data/Images/Pipe/pipe_green_up.png"; }
        }
        public static string spika_green
        {
            get { return "Data/Images/Enemie/Spika/green.png"; }
        }
        public static string spika_grey
        {
            get { return "Data/Images/Enemie/Spika/grey.png"; }
        }
        public static string spika_orange
        {
            get { return "Data/Images/Enemie/Spika/orange.png"; }
        }
        public static string star
        {
            get { return "Data/Images/GameItem/star.png"; }
        }
        public static string tree
        {
            get { return "Data/Images/GameElement/tree.png"; }
        }
        public static string turtle_green_down
        {
            get { return "Data/Images/Enemie/Turtle/Green/turtle_down.png"; }
        }
        public static string turtle_green
        {
            get { return "Data/Images/Enemie/Turtle/Green/walk_#L[0-2].png"; }
        }
        public static string tux_normal
        {
            get { return "Data/Images/MainGameObject/Tux/Normal/tux_#R[0-5].png"; }
        }
        public static string tux_small
        {
            get { return "Data/Images/MainGameObject/Tux/Small/tux_small_#R[0-7].png"; }
        }


        public static string[] All()
        {
            PropertyInfo[] infos = typeof(Files).GetProperties();
            string[] ret = new string[infos.Length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (string)infos[i].GetGetMethod().Invoke(new object(), new object[0]);
            }

            return ret;
        }
    }
}
