using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using littleRunner.GameObjects;


namespace littleRunner
{
    public partial class Editor : Form
    {
        ProgramSwitcher programSwitcher;
        GameObject focus;
        bool moving;
        int mouseX, mouseY;
        int scrolled;

        World world;
        string curfile;

        private int[] getValues()
        {
            int[] values = new int[2];

            if (!Int32.TryParse(txtRealWidth.Text, out values[0]))
                values[0] = 0;
            if (!Int32.TryParse(txtHeight.Text, out values[1]))
                values[1] = 0;

            return values;
        }

        public Editor(ProgramSwitcher programSwitcher)
        {
            InitializeComponent();

            this.programSwitcher = programSwitcher;
            focus = null;
            moving = false;
            mouseX = 0;
            mouseY = 0;
            scrolled = 0;

            int[] dimension = getValues();
            world = new World(dimension[0], dimension[1], level.Invalidate, false);
            curfile = "";

            // Images
            floorToolStripMenuItem.Image = Image.FromFile(Properties.Resources.floor_middle);
            floor2ToolStripMenuItem.Image = Image.FromFile(Properties.Resources.floor_middle);

            designElementToolStripMenuItem.Image = Image.FromFile(Properties.Resources.tree);
            treeToolStripMenuItem.Image = Image.FromFile(Properties.Resources.tree);

            brickToolStripMenuItem.Image = Image.FromFile(Properties.Resources.brick_blue);
            yellowToolStripMenuItem.Image = Image.FromFile(Properties.Resources.brick_yellow);
            blueToolStripMenuItem.Image = Image.FromFile(Properties.Resources.brick_blue);
            redToolStripMenuItem.Image = Image.FromFile(Properties.Resources.brick_red);
            iceToolStripMenuItem.Image = Image.FromFile(Properties.Resources.brick_ice);

            boxToolStripMenuItem.Image = Image.FromFile(Properties.Resources.box1);
            boxToolStripMenuItem1.Image = Image.FromFile(Properties.Resources.box1);

            pipeToolStripMenuItem.Image = Image.FromFile(Properties.Resources.pipe_green_up);
            upToolStripMenuItem.Image = Image.FromFile(Properties.Resources.pipe_green_up);
            mainToolStripMenuItem.Image = Image.FromFile(Properties.Resources.pipe_green_main);

            pointStarToolStripMenuItem.Image = Image.FromFile(Properties.Resources.star);

            enemyToolStripMenuItem.Image = Image.FromFile(Properties.Resources.turtle_green_right);
            turtleToolStripMenuItem.Image = Image.FromFile(Properties.Resources.turtle_green_right);

            spikaToolStripMenuItem.Image = Image.FromFile(Properties.Resources.spika_green);
            greenToolStripMenuItem.Image = Image.FromFile(Properties.Resources.spika_green);
            orangeToolStripMenuItem.Image = Image.FromFile(Properties.Resources.spika_orange);
            greyToolStripMenuItem.Image = Image.FromFile(Properties.Resources.spika_grey);

            gumbaToolStripMenuItem.Image = Image.FromFile(Properties.Resources.gumba_brown);
            brownToolStripMenuItem.Image = Image.FromFile(Properties.Resources.gumba_brown);

            levelEndToolStripMenuItem.Image = Image.FromFile(Properties.Resources.levelend_house);
            houseToolStripMenuItem.Image = Image.FromFile(Properties.Resources.levelend_house);

            startGameToolStripMenuItem.Image = Image.FromFile(Properties.Resources.icon_png);
        }


        private void level_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            foreach (GameObject go in world.AllElements)
            {
                if (go.Hit(e.Y, e.X))
                {
                    focus = go;
                    actualFocus.Text = "Focus: "+focus.GetType().Name;
                    moving = true;
                    mouseX = e.X - go.Left;
                    mouseY = e.Y - go.Top;
                    break;
                }
            }
        }

        private void level_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (moving && focus != null)
            {
                focus.Top = e.Y - mouseY;
                focus.Left = e.X - mouseX;
                level.Invalidate();
            }
        }

        private void level_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
            moving = false;
        }

        private void level_MouseClick(object sender, MouseEventArgs e)
        {
            if (focus != null)
            {
                propertys.SelectedObject = focus;

                if (e.Button == MouseButtons.Right)
                {
                    objectContext.Show(Cursor.Position.X, Cursor.Position.Y);
                    mouseX = e.X - focus.Left;
                    mouseY = e.Y - focus.Top;
                }
            }
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] dimension = getValues();
            world = new World(dimension[0], dimension[1], level.Invalidate, false);

            level.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                curfile = openFile.FileName;
                world = new World(curfile, level.Invalidate, false);

                level.Invalidate();
            }
        }

        private bool save()
        {
            if (curfile == "")
            {
                return saveAs();
            }
            else
            {
                scrollAll(-scrolled);

                world.Serialize(curfile);

                scrollAll(scrolled);
                return true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }
        private bool saveAs()
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                curfile = saveFile.FileName;
                return save();
            }
            else
                return false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (save())
            {
                Game g = new Game(programSwitcher, curfile);
                g.Show();
            }
        }

        private void addElement(GameObject go)
        {
            go.Init(world);
            world.Add(go);
            level.Invalidate();
        }


        private void plainFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlainFloor f = new PlainFloor(Color.Black, 0, 0, 400, 20);
            addElement(f);
        }
        private void floor2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Floor f = new Floor(Image.FromFile(Properties.Resources.floor_left),
                                Image.FromFile(Properties.Resources.floor_middle),
                                Image.FromFile(Properties.Resources.floor_right),
                                0, 0);
            addElement(f);
        }


        private void treeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesignElement tree = new DesignElement(0, 0, Image.FromFile(Properties.Resources.tree));
            addElement(tree);
        }


        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brick b = new Brick(0, 0, Image.FromFile(Properties.Resources.brick_yellow));
            addElement(b);
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brick b = new Brick(0, 0, Image.FromFile(Properties.Resources.brick_blue));
            addElement(b);
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brick b = new Brick(0, 0, Image.FromFile(Properties.Resources.brick_red));
            addElement(b);
        }

        private void iceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brick b = new Brick(0, 0, Image.FromFile(Properties.Resources.brick_ice));
            addElement(b);
        }


        private void boxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Box b = new Box(0, 0, Image.FromFile(Properties.Resources.box1));
            addElement(b);
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pipe p = new Pipe(0, 0, Image.FromFile(Properties.Resources.pipe_green_up));
            addElement(p);
        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pipe p = new Pipe(0, 0, Image.FromFile(Properties.Resources.pipe_green_main));
            addElement(p);
        }

        private void pointStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Star p = new Star(0, 0, Image.FromFile(Properties.Resources.star));
            addElement(p);
        }

        private void turtleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Turtle t = new Turtle(0, 0, Image.FromFile(Properties.Resources.turtle_green_left),
                                        Image.FromFile(Properties.Resources.turtle_green_right));
            addElement(t);
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Spika s = new Spika(0, 0, Image.FromFile(Properties.Resources.spika_green));
            addElement(s);
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Spika s = new Spika(0, 0, Image.FromFile(Properties.Resources.spika_orange));
            addElement(s);
        }

        private void greyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Spika s = new Spika(0, 0, Image.FromFile(Properties.Resources.spika_grey));
            addElement(s);
        }

        private void brownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gumba g = new Gumba(0, 0, Image.FromFile(Properties.Resources.gumba_brown));
            addElement(g);
        }

        private void houseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            House h = new House(0, 0, Image.FromFile(Properties.Resources.levelend_house));
            addElement(h);
        }


        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------


        private void toForegroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focus != null)
            {
                world.SetLast(focus);
                level.Invalidate();
            }
        }

        private void toBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focus != null)
            {
                world.SetFirst(focus);
                level.Invalidate();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focus != null)
            {
                world.Remove(focus);
                level.Invalidate();
            }
        }

        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult res = MessageBox.Show("Do you want to save your changes?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (res == DialogResult.Yes)
                saveAs();
            programSwitcher.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void level_Paint(object sender, PaintEventArgs e)
        {
            world.Draw(e.Graphics);
        }

        private void propertys_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            level.Invalidate();
        }

        private void txtViewWidth_KeyUp(object sender, KeyEventArgs e)
        {
            int res;
            if (Int32.TryParse(txtViewWidth.Text, out res) && res >= 622)
            {
                world.Width = res;
            }
        }

        private void txtRealWidth_KeyUp(object sender, KeyEventArgs e)
        {
            int res;
            if (Int32.TryParse(txtRealWidth.Text, out res) && res >= 622)
            {
                scrollBar.Maximum = res;
            }
        }

        private void txtHeight_KeyUp(object sender, KeyEventArgs e)
        {
            int res;
            if (Int32.TryParse(txtHeight.Text, out res))
            {
                world.Height = res;

                level.Height = res;

                this.Height = level.Top + level.Height + scrollBar.Height + 44;
                propertys.Height = level.Height - 58;
            }
        }

        private void scrollAll(int val)
        {
            foreach (GameObject go in world.AllElements)
            {
                go.Left += val;
            }
        }

        private void scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            scrollAll(-(e.NewValue - e.OldValue) * 10);
            scrolled += -(e.NewValue - e.OldValue) * 10;

            level.Invalidate();
        }
    }
}