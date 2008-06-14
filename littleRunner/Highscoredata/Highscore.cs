using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;


namespace littleRunner
{
    class Highscore
    {
        public static string FileName;
        private static HighscoreSort HighscoreSorter = new HighscoreSort();
        private static char[] GUID = GetGuid().ToCharArray();


        public struct Data
        {
            public string Name;
            public int Points;

            public static Data New(string name, int points)
            {
                Data d = new Data();
                d.Name = name;
                d.Points = points;
                return d;
            }
        }

        class HighscoreSort : IComparer<Data>
        {
            public int Compare(Data a, Data b)
            {
                if (a.Points > b.Points)
                    return -1;
                else if (a.Points < b.Points)
                    return 1;
                else
                    return 0;
            }
        }

        static string GetGuid()
        {
            object[] Attributes;
            Attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false);

            return ((GuidAttribute)Attributes[0]).Value;
        }

        #region file read & write
        static string ReadFile()
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
            BinaryReader br = new BinaryReader(fs);

            byte[] decrypted = new byte[fs.Length];
            for (long i = 0; i < fs.Length; i++)
            {
                decrypted[i] = (byte)(br.ReadByte() ^ GUID[i % GUID.Length]);
            }
            br.Close();

            return new String(Encoding.UTF8.GetChars(decrypted));
        }
        static void WriteFile(string text)
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
            BinaryWriter bw = new BinaryWriter(fs);


            byte[] crypted = Encoding.UTF8.GetBytes(text.ToCharArray());

            for (int i = 0; i < crypted.Length; i++)
            {
                bw.Write((byte)(crypted[i] ^ GUID[i % GUID.Length]));
            }

            bw.Close();
        }
        #endregion

        #region read
        public static List<Highscore.Data> Read(int count)
        {
            List<Highscore.Data> list = new List<Highscore.Data>();

            string[] lines = ReadFile().Split('\n');
            for (int i = 0; (i < count || count == -1) && i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.Length > 0)
                {
                    int sep = line.IndexOf(' ');

                    Data data = new Data();
                    data.Points = Convert.ToInt32(line.Substring(0, sep));
                    data.Name = line.Substring(sep + 1, line.Length - sep - 1);

                    list.Add(data);
                }
            }

            return list;
        }
        public static List<Highscore.Data> ReadTop10()
        {
            return Read(10);
        }
        public static List<Highscore.Data> Read()
        {
            return Read(-1);
        }
        #endregion

        #region write
        public static void Write(string name, int points)
        {
            Write(Data.New(name, points));
        }
        public static void Write(Data data)
        {
            List<Highscore.Data> list = new List<Highscore.Data>(Read());
            list.Add(data);

            list.Sort(HighscoreSorter);


            string text = "";
            foreach (Data d in list)
            {
                text += d.Points.ToString() + " " + d.Name + "\n";
            }

            WriteFile(text);
        }
        #endregion
    }
}
