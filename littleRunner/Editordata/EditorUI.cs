using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

using littleRunner.GameObjects;


namespace littleRunner
{
    class EditorUI
    {
        static GameObject focus;
        static Panel level;
        static PropertyGrid properties;

        public static List<ToolStripItem> GenerateProperties(ref GameObject focus,
            ref Panel level,
            ref PropertyGrid properties)
        {
            EditorUI.focus = focus;
            EditorUI.level = level;
            EditorUI.properties = properties;
            List<ToolStripItem> newitems = new List<ToolStripItem>();


            foreach (PropertyInfo i in focus.GetType().GetProperties())
            {
                if (i.PropertyType.IsEnum)
                {
                    bool browseable = true;
                    foreach (object attr in i.GetCustomAttributes(true))
                    {
                        if (attr.GetType() == typeof(BrowsableAttribute))
                            browseable = ((BrowsableAttribute)attr).Browsable;
                    }

                    if (browseable)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(i.Name);
                        string selected = Enum.GetName(i.PropertyType, i.GetGetMethod().Invoke(focus, new object[] { }));

                        List<ToolStripMenuItem> itemDDItems = new List<ToolStripMenuItem>();
                        bool canWrite = i.CanWrite;
                        foreach (string value in Enum.GetNames(i.PropertyType))
                        {
                            ToolStripMenuItem valueItem = new ToolStripMenuItem(value);
                            if (value == selected)
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
            }


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

            PropertyInfo i = focus.GetType().GetProperty(owner.Text);
            object enumValue = Enum.Parse(i.PropertyType, selected.Text);


            if (i.CanWrite)
            {
                i.GetSetMethod().Invoke(focus, new object[] { enumValue });
                properties.Refresh();
                level.Invalidate();
            }
        }
    }
}
