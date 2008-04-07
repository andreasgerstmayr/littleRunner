using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using littleRunner.GameObjects.Objects;


namespace littleRunner
{
    public partial class Editor : Form
    {
        ProgramSwitcher programSwitcher;
        Game g;
        GameObject focus;
        bool moving;
        int mouseX, mouseY;
        int scrolled;

        World world;
        string curfile;

        public Editor(ProgramSwitcher programSwitcher)
        {
            InitializeComponent();

            this.programSwitcher = programSwitcher;
            focus = null;
            moving = false;
            mouseX = 0;
            mouseY = 0;
            scrolled = 0;


            // Images
            floorToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.floor_middle]);

            designElementToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.tree]);
            treeToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.tree]);

            brickToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.brick_blue]);

            boxToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.box1]);

            pipeToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.pipe_green_up]);

            pointStarToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.star]);

            enemyToolStripMenuItem.Image = AnimateImage.FirstImage(Files.f[gFile.turtle_green]);
            turtleToolStripMenuItem.Image = AnimateImage.FirstImage(Files.f[gFile.turtle_green]);

            spikaToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.spika_green]);

            gumbaToolStripMenuItem.Image = AnimateImage.FirstImage(Files.f[gFile.gumba_brown]);

            levelEndToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.levelend_house]);
            houseToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.levelend_house]);

            startGameToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.icon_png]);
            startGamecurrentToolStripMenuItem.Image = Image.FromFile(Files.f[gFile.icon_png]);

            curfile = "";
            newToolStripMenuItem_Click(new object(), new EventArgs());
        }


        private void level_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            foreach (GameObject go in world.AllElements)
            {
                if (go.Hit(e.Y, e.X))
                {
                    focus = go;
                    actualFocus.Text = "Focus: " + focus.GetType().Name;
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
            level.Invalidate(); // paint again, with background
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


        private void showlevelSettings_Click(object sender, EventArgs e)
        {
            propertys.SelectedObject = world.Settings;
            actualFocus.Text = "Focus: " + world.Settings.GetType().Name;
        }

        private void setDelegateHandlers()
        {
            world.Settings.cGameWindowWidth = changedGameWindowWidth;
            world.Settings.cLevelWidth = changedLevelWidth;
            world.Settings.cLevelHeight = changedLevelHeight;
            changedGameWindowWidth();
            changedLevelWidth();
            changedLevelHeight();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curfile = "";
            this.Text = "littleRunner Game Editor";
            world = new World(700, 550, level.Invalidate, false);
            setDelegateHandlers();

            showlevelSettings_Click(sender, e);
            level.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
            trackBar.Value = 0;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                curfile = openFile.FileName;
                this.Text = "littleRunner Game Editor - " + curfile;
                world = new World(curfile, level.Invalidate, false);
                setDelegateHandlers();

                showlevelSettings_Click(sender, e);
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
                scrollAll(scrolled);

                world.Serialize(curfile);

                scrollAll(-scrolled);
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
                this.Text = "littleRunner Game Editor - " + curfile;
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
                g = new Game(programSwitcher, curfile);
                g.ShowDialog();
                g = null;
            }
        }
        private void startGamecurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (save())
            {
                g = new Game(programSwitcher, curfile);
                g.AI.Scroll(-trackBar.Value, false);

                g.ShowDialog();
                g = null;
            }
        }



        private void addElement(GameObject go)
        {
            go.Init(world);
            world.Add(go);
            level.Invalidate();
        }


        private void floorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Floor f = new Floor(0, 0, FloorColor.Green);
            addElement(f);
        }


        private void treeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesignElements tree = new DesignElements(0, 0, DesignElement.Tree);
            addElement(tree);
        }

        private void boxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Box b = new Box(0, 0, BoxStyle.Yellow);
            addElement(b);
        }

        private void brickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brick b = new Brick(0, 0, BrickColor.Blue);
            addElement(b);
        }

        private void pipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pipe p = new Pipe(0, 0, PipeColor.Green);
            addElement(p);
        }

        private void pointStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Star p = new Star(0, 0);
            addElement(p);
        }

        private void turtleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Turtle t = new Turtle(0, 0);
            addElement(t);
        }


        private void spikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Spika s = new Spika(0, 0);
            addElement(s);
        }

        private void gumbaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gumba g = new Gumba(0, 0);
            addElement(g);
        }

        private void houseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LevelEnd l = new LevelEnd(0, 0, LevelEndImg.House);
            addElement(l);
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

        private void level_Paint(object sender, PaintEventArgs e)
        {
            world.Draw(e.Graphics, !moving);
        }

        private void propertys_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            level.Invalidate();
        }


        private void changedGameWindowWidth()
        {
            this.Width = 44 + world.Settings.GameWindowWidth + propertys.Width;
        }
        private void changedLevelWidth()
        {
            trackBar.Maximum = world.Settings.LevelWidth;
            trackBar_ValueChanged(new object(), new EventArgs());
        }
        private void changedLevelHeight()
        {
            this.Height = 148 + world.Settings.LevelHeight;
        }


        private void scrollAll(int val)
        {
            foreach (GameObject go in world.AllElements)
            {
                go.Left += val;
            }
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            scrollAll(-(trackBar.Value - scrolled));
            scrolled = trackBar.Value;
            curScrolling.Text = trackBar.Value.ToString();

            level.Invalidate();
        }
    }
}