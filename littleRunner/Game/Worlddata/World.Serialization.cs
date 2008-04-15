using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using littleRunner.GameObjects;


namespace littleRunner.Worlddata
{
    class WorldSerialization
    {
        #region Obj <--> String serialisation
        private static string ObjToStr(object o)
        {
            string s = null;

            if (o == null)
                s = "null";
            else if (o.GetType() == typeof(bool))
                s = o.ToString();
            else if (o.GetType() == typeof(string))
                s = (string)o;
            else if (o.GetType() == typeof(string[]))
            {
                s = "";
                string[] array = (string[])o;
                for(int i=0; i<array.Length; i++)
                {
                    s += array[i] + (i+1 == array.Length ? "" : "\n");
                }
            }
            else if (o.GetType() == typeof(int))
                s = o.ToString();
            else if (o.GetType().IsEnum)
                s = Enum.GetName(o.GetType(), o);

            return s;
        }
        private static object StrToObj(Type t, string s)
        {
            object o = null;
            
            if (t == null && s == "null")
                o = null;
            else if (t == typeof(bool))
                o = bool.Parse(s);
            else if (t == typeof(string))
                o = (string)s;
            else if (t == typeof(string[]))
                o = s.Trim('\n').Split(new char[] { '\n' });
            else if (t == typeof(int))
                o = Convert.ToInt32(s);
            else if (t.IsEnum)
                o = Enum.Parse(t, s);

            return o;
        }
        #endregion


        #region Serialize
        private static void Serialize(ref XmlTextWriter xmlWriter, Type type, Dictionary<string, object> serialized)
        {
            xmlWriter.WriteStartElement(type.FullName);
            foreach (KeyValuePair<string, object> keypair in serialized)
            {
                string xmlType = keypair.Value == null ? "NULL" : keypair.Value.GetType().FullName;
                string xmlValue = ObjToStr(keypair.Value);

                xmlWriter.WriteStartElement(keypair.Key);
                xmlWriter.WriteElementString("type", xmlType);
                if (xmlType.IndexOf("[]") != -1 || 
                    (xmlType == typeof(string).FullName && xmlValue.IndexOf("\n") != -1) )
                {
                    xmlWriter.WriteStartElement("value");
                    xmlWriter.WriteCData(xmlValue);
                    xmlWriter.WriteEndElement();
                }
                else
                {
                    xmlWriter.WriteElementString("value", xmlValue);
                }

                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        public static void Serialize(string filename,
            List<StickyElement> stickyelements,
            List<MovingElement> movingelements,
            List<Enemy> enemies,
            LevelSettings settings)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument(false);
            xmlWriter.WriteComment("littleRunner level file");

            xmlWriter.WriteStartElement("Level");

            xmlWriter.WriteStartElement("Settings");
            Serialize(ref xmlWriter, settings.GetType(), settings.Serialize());
            xmlWriter.WriteEndElement();


            xmlWriter.WriteStartElement("Data");


            xmlWriter.WriteStartElement("StickyElements");
            foreach (StickyElement se in stickyelements)
            {
                Serialize(ref xmlWriter, se.GetType(), se.Serialize());
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MovingElements");
            foreach (MovingElement me in movingelements)
            {
                Serialize(ref xmlWriter, me.GetType(), me.Serialize());
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Enemies");
            foreach (Enemy e in enemies)
            {
                Serialize(ref xmlWriter, e.GetType(), e.Serialize());
            }
            xmlWriter.WriteEndElement();


            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.Close();
        }
        #endregion


        #region Deserialization
        private static Dictionary<string, object> Deserialize(ref XmlTextReader xmlReader, string section)
        {
            Dictionary<string, object> serialized = new Dictionary<string, object>();

            xmlReader.ReadStartElement(section);
            while (xmlReader.Read() && xmlReader.Name != section)
            {
                string name = xmlReader.Name;
                xmlReader.ReadStartElement(name);

                string type = xmlReader.ReadElementString("type");
                string value = xmlReader.ReadElementString("value");
                xmlReader.ReadEndElement();


                object oValue = StrToObj(type=="NULL"?null:Type.GetType(type), value);
                serialized[name] = oValue;
            }
            xmlReader.ReadEndElement();

            return serialized;
        }
        private static List<StickyElement> createObjectsStickyElements(ref XmlTextReader xmlReader,
            World world,
            GameEventHandler aiEventHandler)
        {
            string section = "StickyElements";
            List<StickyElement> list = new List<StickyElement>();

            xmlReader.ReadStartElement(section);
            while (xmlReader.Depth > 2 && xmlReader.Read() && xmlReader.Name != section)
            {
                string type = xmlReader.Name;
                Dictionary<string, object> serialized = Deserialize(ref xmlReader, type);

                Type tType = Type.GetType(type);
                GameObject go = (GameObject)Activator.CreateInstance(tType);
                go.Deserialize(serialized);
                go.Init(world, aiEventHandler);

                list.Add((StickyElement)go);
            }

            if (xmlReader.Name != "")
                xmlReader.ReadEndElement();

            return list;
        }
        private static List<MovingElement> createObjectsMovingElements(ref XmlTextReader xmlReader,
            World world,
            GameEventHandler aiEventHandler)
        {
            string section = "MovingElements";
            List<MovingElement> list = new List<MovingElement>();

            xmlReader.ReadStartElement(section);
            while (xmlReader.Depth > 2 && xmlReader.Read() && xmlReader.Name != section)
            {
                string type = xmlReader.Name;
                Dictionary<string, object> serialized = Deserialize(ref xmlReader, type);

                Type tType = Type.GetType(type);
                GameObject go = (GameObject)Activator.CreateInstance(tType);
                go.Deserialize(serialized);
                go.Init(world, aiEventHandler);

                list.Add((MovingElement)go);
            }

            if (xmlReader.Name != "")
                xmlReader.ReadEndElement();

            return list;
        }
        private static List<Enemy> createObjectsEnemies(ref XmlTextReader xmlReader,
            World world,
            GameEventHandler aiEventHandler)
        {
            string section = "Enemies";
            List<Enemy> list = new List<Enemy>();
            
            xmlReader.ReadStartElement(section);
            
            while (xmlReader.Depth > 2 && xmlReader.Read() && xmlReader.Name != section)
            {
                string type = xmlReader.Name;
                Dictionary<string, object> serialized = Deserialize(ref xmlReader, type);

                Type tType = Type.GetType(type);
                GameObject go = (GameObject)Activator.CreateInstance(tType);
                go.Deserialize(serialized);
                go.Init(world, aiEventHandler);

                list.Add((Enemy)go);
            }

            if (xmlReader.Name != "")
                xmlReader.ReadEndElement();

            return list;
        }
        


        public static void Deserialize(string filename, out LevelSettings settings,
                                                        out List<StickyElement> stickyelements,
                                                        out List<MovingElement> movingelements,
                                                        out List<Enemy> enemies,
                                                        World world,
                                                        GameEventHandler aiEventHandler)
        {
            settings = new LevelSettings();
            stickyelements = new List<StickyElement>();
            movingelements = new List<MovingElement>();
            enemies = new List<Enemy>();
            

            XmlTextReader xmlReader = new XmlTextReader(filename);
            xmlReader.ReadStartElement("Level");
            xmlReader.ReadStartElement("Settings");
            settings.Deserialize(Deserialize(ref xmlReader, "littleRunner.LevelSettings"));
            xmlReader.ReadEndElement();

            
            xmlReader.ReadStartElement("Data");
            

            stickyelements = createObjectsStickyElements(ref xmlReader, world, aiEventHandler);
            movingelements = createObjectsMovingElements(ref xmlReader, world, aiEventHandler);
            enemies = createObjectsEnemies(ref xmlReader, world, aiEventHandler);


            xmlReader.ReadEndElement();

            xmlReader.Close();
        }
        #endregion
    }
}
