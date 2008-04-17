using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.GameObjects;
using System.Diagnostics;

namespace littleRunner
{
    static class GamePhysics
    {
        public static class FallingClass<T> where T : StickyElement
        {
            public static bool CheckFalling(List<T> list, GameObject go,
                int newtop, int newleft)
            {
                bool falling = true;

                foreach (T el in list)
                {
                    if (el.canStandOn)
                    {
                        if (go.Right-newleft > el.Left && go.Left+newleft < el.Right && // left+right ok?
                            go.Bottom-newtop == el.Top)
                        {
                            falling = false;
                            break;
                        }
                    }
                }

                return falling;
            }
        }

        static public bool Falling(List<StickyElement> stickyelements,
            List<MovingElement> movingelements,
            int newtop,
            int newleft,
            GameObject go)
        {
            bool falling = true;

            falling = FallingClass<StickyElement>.CheckFalling(stickyelements, go,newtop, newleft);
            if (falling)
                falling = FallingClass<MovingElement>.CheckFalling(movingelements, go, newtop, newleft);

            return falling;
        }



        static public void Jumping(ref int jumping, ref int newtop, ref int newleft)
        {
            // jump left
            if (jumping >= 0 && jumping < 20)
            {
                newleft -= 5;
                newtop -= 10;
            }
            else if (jumping >= 20 && jumping < 40)
            {
                newleft -= 5;
                newtop += 10;
            }

            // jump top
            else if (jumping >= 100 && jumping < 120)
            {
                newtop -= 10;
            }
            else if (jumping >= 120 && jumping < 140)
            {
                newtop += 10;
            }

            // jump right
            else if (jumping >= 200 && jumping < 220)
            {
                newleft += 5;
                newtop -= 10;
            }
            else if (jumping >= 220 && jumping < 240)
            {
                newleft += 5;
                newtop += 10;
            }


            if (jumping != -1)
                jumping++;
            if (jumping == 40 || jumping == 140 || jumping == 240)
                jumping = -1;
        }


        static public bool SingleCrashDetection(GameObject go, GameObject go2, out GameDirection direction, ref int newtop, ref int newleft, bool change)
        {
            bool crashedIn = false;
            direction = GameDirection.None;

            if (go.Right + newleft > go2.Left && go.Left + newleft < go2.Right && // left+right ok?
                go.Bottom + newtop > go2.Top && go.Top + newtop < go2.Bottom)
            {
                if (go.Right <= go2.Left) // crash into right
                {
                    if (change)
                        newleft = go2.Left - go.Right;
                    direction = GameDirection.Left;
                }
                else if (go.Left >= go2.Right)
                {
                    if (change)
                        newleft = -(go.Left - go2.Right);
                    direction = GameDirection.Right;
                }

                crashedIn = true;
            }

            // newleft may be changed, recheck!
            if (go.Right + newleft > go2.Left && go.Left + newleft < go2.Right && // left+right ok?
                go.Bottom + newtop > go2.Top && go.Top + newtop < go2.Bottom)
            {
                if (go.Bottom - 5 < go2.Top)
                {
                    if (change)
                        newtop = go2.Top - go.Bottom;
                    direction = GameDirection.Top;
                }
                else if (go.Top + 5 > go2.Bottom)
                {
                    if (change)
                        newtop = -(go.Top - go2.Bottom);
                    direction = GameDirection.Bottom;
                }

                crashedIn = true;
            }

            return crashedIn;
        }


        static public void CrashDetection(GameObject go, List<StickyElement> stickyelements, List<MovingElement> movingelements, GameEventHandler geventhandler, ref int newtop, ref int newleft)
        {
            for (int i = 0; i < stickyelements.Count; i++)
            {
                StickyElement se = stickyelements[i];
                if (se != go)
                {
                    GameDirection direction = GameDirection.None;
                    if (SingleCrashDetection(go, se, out direction, ref newtop, ref newleft, se.canStandOn))
                    {
                        se.onOver(geventhandler, GameAI.WhoIsIt(go), direction);
                    }
                }
            }

            for (int i = 0; i < movingelements.Count; i++)
            {
                MovingElement se = movingelements[i];
                if (se != go)
                {
                    GameDirection direction = GameDirection.None;
                    if (SingleCrashDetection(go, se, out direction, ref newtop, ref newleft, se.canStandOn))
                    {
                        se.onOver(geventhandler, GameAI.WhoIsIt(go), direction);
                    }
                }
            }
        }


        static public Enemy CrashEnemy(GameObject go, List<Enemy> enemies, GameEventHandler geventhandler, ref int newtop, ref int newleft)
        {
            Enemy crashedIn = null;

            foreach (Enemy enemie in enemies)
            {
                if (enemie != go)
                {
                    GameDirection direction;
                    if (SingleCrashDetection(go, enemie, out direction, ref newtop, ref newleft, true))
                    {
                        if (!enemie.getCrashEvent(go, direction))
                            geventhandler(GameEvent.crashInEnemy, new Dictionary<GameEventArg, object>());
                        crashedIn = enemie;
                        break;
                    }
                }
            }

            return crashedIn;
        }


        static public bool SimpleCrashDetection(GameObject my, GameObject box, int newtop, int newleft)
        {
            return my.Left + newleft < box.Right && my.Right + newleft > box.Left &&
                   my.Top + newtop < box.Bottom && my.Bottom + newtop > box.Top;
        }

        static public bool SimpleCrashDetections(GameObject my, List<StickyElement> stickyelements, bool onlyWhenCanStandOn, int newtop, int newleft)
        {
            foreach (StickyElement se in stickyelements)
            {
                if (onlyWhenCanStandOn)
                {
                    if (se.canStandOn && SimpleCrashDetection(my, se, newtop, newleft))
                        return true;
                }
                else
                    SimpleCrashDetection(my, se, newtop, newleft);
            }
            return false;
        }
    }
}
