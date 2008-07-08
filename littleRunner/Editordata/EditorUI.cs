using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

using littleRunner.Drawing;
using littleRunner.Gamedata.Worlddata;
using littleRunner.GameObjects;


namespace littleRunner.Editordata
{
    class EditorUI
    {
        static public Drawing.DrawHandler drawHandler;
        static public DoubleBufferPanel level;
        static public PropertyGrid properties;


        public static bool HasProperty(object obj, string name, Type propertyType, out string value)
        {
            PropertyInfo i = obj.GetType().GetProperty(name); // null --> not found

            if (i != null && i.PropertyType == propertyType)
            {
                bool browseable = true;
                foreach (object attr in i.GetCustomAttributes(true))
                {
                    if (attr.GetType() == typeof(BrowsableAttribute))
                        browseable = ((BrowsableAttribute)attr).Browsable;
                }

                if (browseable)
                {
                    value = Enum.GetName(i.PropertyType, i.GetGetMethod().Invoke(obj, new object[] { }));
                    return true;
                }
            }

            value = "";
            return false;
        }


        public static bool CheckOtherObjects(string name, Type propertyType, string startValue, out bool valueEqual)
        {
            valueEqual = true;

            for (int i = 1; i < properties.SelectedObjects.Length; i++)
            {
                string value;

                if (!HasProperty(properties.SelectedObjects[i], name, propertyType, out value))
                    return false;

                if (valueEqual && value != startValue)
                    valueEqual = false;
            }

            return true;
        }


        public static List<ToolStripItem> FirstObject()
        {
            List<ToolStripItem> newitems = new List<ToolStripItem>();

            foreach (PropertyInfo i in properties.SelectedObjects[0].GetType().GetProperties())
            {
                if (!i.PropertyType.IsEnum)
                    continue;

                bool browseable = true;
                foreach (object attr in i.GetCustomAttributes(true))
                {
                    if (attr.GetType() == typeof(BrowsableAttribute))
                        browseable = ((BrowsableAttribute)attr).Browsable;
                }

                if (!browseable || !i.PropertyType.IsEnum)
                    continue;

                string selected = Enum.GetName(i.PropertyType, i.GetGetMethod().Invoke(properties.SelectedObject, new object[] { }));
                bool valuesEqual;
                if (CheckOtherObjects(i.Name, i.PropertyType, selected, out valuesEqual))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(i.Name);

                    List<ToolStripMenuItem> itemDDItems = new List<ToolStripMenuItem>();
                    bool canWrite = i.CanWrite;
                    foreach (string value in Enum.GetNames(i.PropertyType))
                    {
                        ToolStripMenuItem valueItem = new ToolStripMenuItem(value);
                        if (valuesEqual && value == selected)
                            valueItem.Checked = true;
                        valueItem.CheckOnClick = true;

                        if (!canWrite) // can't write to it, disable it
                            valueItem.Enabled = false;

                        valueItem.CheckStateChanged += new EventHandler(objectContextItem_CheckStateChanged);

                        itemDDItems.Add(valueItem);
                    }

                    if (!canWrite)
                        item.Enabled = false;


                    item.DropDownItems.AddRange(itemDDItems.ToArray());
                    newitems.Add(item);
                }
            }

            return newitems;
        }


        public static List<ToolStripItem> GenerateProperties()
        {
            List<ToolStripItem> newitems;


            newitems = FirstObject();

            if (newitems.Count > 0)
            {
                ToolStripSeparator seperator = new ToolStripSeparator();
                newitems.Insert(0, seperator);
            }

            return newitems;
        }


        static void objectContextItem_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = (ToolStripMenuItem)sender;
            ToolStripItem owner = selected.OwnerItem;

            foreach (object obj in properties.SelectedObjects)
            {
                PropertyInfo i = obj.GetType().GetProperty(owner.Text);
                object enumValue = Enum.Parse(i.PropertyType, selected.Text);

                if (i.CanWrite)
                    i.GetSetMethod().Invoke(obj, new object[] { enumValue });
            }

            properties.Refresh();
            drawHandler.Update();
        }


        static public void FetchElementsInRectangle(Rectangle curRectangle, ref World world, ref PropertyGrid properties)
        {
            List<object> selected = new List<object>();

            foreach (GameObject go in world.AllElements)
            {
                if (go.InRectangle(curRectangle))
                    selected.Add(go);
            }

            properties.SelectedObjects = selected.ToArray();
        }



        public static void horizAlign()
        {
            if (properties.SelectedObject is LevelSettings || properties.SelectedObjects.Length < 2)
                return;

            float most_top = ((GameObject)properties.SelectedObjects[0]).Top;

            foreach (object o in properties.SelectedObjects)
            {
                GameObject gameObject = (GameObject)o;
                if (gameObject.Top < most_top)
                    most_top = gameObject.Top;
            }
            foreach (object o in properties.SelectedObjects)
            {
                GameObject gameObject = (GameObject)o;
                gameObject.Top = most_top;
            }

            level.Invalidate();
            properties.Refresh();
        }
        public static void vertAlign()
        {
            if (properties.SelectedObject is LevelSettings || properties.SelectedObjects.Length < 2)
                return;

            float most_left = ((GameObject)properties.SelectedObjects[0]).Left;

            foreach (object o in properties.SelectedObjects)
            {
                GameObject gameObject = (GameObject)o;
                if (gameObject.Left < most_left)
                    most_left = gameObject.Left;
            }
            foreach (object o in properties.SelectedObjects)
            {
                GameObject gameObject = (GameObject)o;
                gameObject.Left = most_left;
            }

            level.Invalidate();
            properties.Refresh();
        }


        class GameObjectHorizSort : IComparer<GameObject>
        {
            public int Compare(GameObject a, GameObject b)
            {
                if (a.Left > b.Left)
                    return 1;
                else if (a.Left < b.Left)
                    return -1;
                else
                    return 0;
            }
        }
        public static void horizSpaceAdjust()
        {
            if (properties.SelectedObject is LevelSettings || properties.SelectedObjects.Length < 2)
                return;

            List<GameObject> sortedSelectedObjs = new List<GameObject>(properties.SelectedObjects.Length);
            int fullObjsWidth = 0;

            foreach (object o in properties.SelectedObjects)
            {
                GameObject go = (GameObject)o;
                sortedSelectedObjs.Add(go);
                fullObjsWidth += go.Width;
            }
            sortedSelectedObjs.Sort(new GameObjectHorizSort());


            float allSpacesLength = sortedSelectedObjs[sortedSelectedObjs.Count - 1].Right - sortedSelectedObjs[0].Left - fullObjsWidth;
            float spaceLenPerObj = allSpacesLength / (properties.SelectedObjects.Length - 1);

            float curleft = sortedSelectedObjs[0].Left;
            foreach (GameObject go in sortedSelectedObjs)
            {
                go.Left = curleft;
                curleft += go.Width + spaceLenPerObj;
            }

            level.Invalidate();
            properties.Refresh();
        }

        class GameObjectVertSort : IComparer<GameObject>
        {
            public int Compare(GameObject a, GameObject b)
            {
                if (a.Top > b.Top)
                    return 1;
                else if (a.Top < b.Top)
                    return -1;
                else
                    return 0;
            }
        }
        public static void vertSpaceAdjust()
        {
            if (properties.SelectedObject is LevelSettings || properties.SelectedObjects.Length < 2)
                return;

            List<GameObject> sortedSelectedObjs = new List<GameObject>(properties.SelectedObjects.Length);
            int fullObjsHeight = 0;

            foreach (object o in properties.SelectedObjects)
            {
                GameObject go = (GameObject)o;
                sortedSelectedObjs.Add(go);
                fullObjsHeight += go.Height;
            }
            sortedSelectedObjs.Sort(new GameObjectVertSort());


            float allSpacesLength = sortedSelectedObjs[sortedSelectedObjs.Count - 1].Bottom - sortedSelectedObjs[0].Top - fullObjsHeight;
            float spaceLenPerObj = allSpacesLength / (properties.SelectedObjects.Length - 1);

            float curtop = sortedSelectedObjs[0].Top;
            foreach (GameObject go in sortedSelectedObjs)
            {
                go.Top = curtop;
                curtop += go.Height + spaceLenPerObj;
            }

            level.Invalidate();
            properties.Refresh();
        }
    }
}
