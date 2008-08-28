using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace littleRunner.Gamedata
{
    static class Cheat
    {
        static bool activated = false;
        static string curText = "";
        static GameAI ai;
        static Dictionary<GameEventArg, object> eventArgs;

        public static void Init(GameAI _ai)
        {
            ai = _ai;
            eventArgs = new Dictionary<GameEventArg, object>();
        }


        public static void Pressed(Keys key)
        {
            if (key != Keys.Escape && curText.Length < 20)
                curText += Char.ToLower((char)key); 
            else
                curText = "";

            CheckCheats();
        }

        private static void CheckCheats()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5hash = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(curText));

            string textInMD5 = BitConverter.ToString(md5hash).Replace("-", "").ToLower();



            if (textInMD5 == "0c7827f6a014148adc37144460f5257b") // activate
            {
                activated = true;
                curText = "";
            }

            if (activated)
            {
                bool success = true;

                switch (textInMD5)
                {
                    case "d0dbe915091d400bd8ee7f27f0791303": // live
                        ai.getEvent(GameEvent.gotLive, eventArgs);
                        break;

                    case "e372717f03c6bd1f41f1a4ca8433df81": // 5points
                        eventArgs[GameEventArg.points] = 5;
                        ai.getEvent(GameEvent.gotPoints, eventArgs);
                        eventArgs.Clear();
                        break;

                    case "79674f0c722eb54cedf3c1352f25954f": // 100points
                        eventArgs[GameEventArg.points] = 100;
                        ai.getEvent(GameEvent.gotPoints, eventArgs);
                        eventArgs.Clear();
                        break;

                    case "6e29d8c603a836413bcb0b3c0216a56a": // gimme good mushroom
                        ai.World.MGO.getEvent(GameEvent.gotGoodMushroom, eventArgs);
                        break;

                    case "7234df3c98e9ae4c50da56ff381caeff": // fireflower
                        ai.World.MGO.getEvent(GameEvent.gotFireFlower, eventArgs);
                        break;

                    case "01d960ae8b9808eebf171f826cc53a29": // gimme poison
                        ai.World.MGO.getEvent(GameEvent.gotPoisonMushroom, eventArgs);
                        break;

                    case "b87e925882615c980ffecd83aa06cd4b": // imbest
                        ai.World.MGO.getEvent(GameEvent.gotImmortialize, eventArgs);
                        break;

                    case "1c946fb776d20916f4bebe1dd06db424": // imdead
                        ai.World.MGO.getEvent(GameEvent.dead, eventArgs);
                        break;


                    default: success = false;
                        break;
                }

                if (success)
                    curText = "";
            }

        }
    }
}
