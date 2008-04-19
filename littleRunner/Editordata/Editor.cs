using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

using littleRunner.GameObjects;
using littleRunner.GameObjects.Enemies;
using littleRunner.GameObjects.MovingElements;
using littleRunner.GameObjects.Objects;


namespace littleRunner
{
    public partial class Editor : Form
    {
        ProgramSwitcher programSwitcher;
        Game g;
        TmpFileHandler tmpHandler;

        World world;
        GameObject focus;
        bool moving;
        bool enableBG;
        int mouseX, mouseY;
        int scrolled;
        List<Keys> pressedKeys;


        public Editor(ProgramSwitcher programSwitcher)
        {
            InitializeComponent();

            this.programSwitcher = programSwitcher;
            World defaultWorld = getDefaultWorld();
            tmpHandler = new TmpFileHandler(openFile, saveFile, defaultWorld.Serialize, 5);
            defaultWorld = null;


            focus = null;
            moving = false;
            mouseX = 0;
            mouseY = 0;
            scrolled = 0;
            enableBG = true;
            pressedKeys = new List<Keys>();


            #region Images loading
            floorToolStripMenuItem.Image = Image.FromFile(Files.floor_middle);
            floorToolStripButton.Image = Image.FromFile(Files.floor_middle);

            designElementToolStripMenuItem.Image = Image.FromFile(Files.tree);
            treeToolStripMenuItem.Image = Image.FromFile(Files.tree);
            treeToolStripButton.Image = Image.FromFile(Files.tree);

            brickToolStripMenuItem.Image = Image.FromFile(Files.brick_blue);
            brickToolStripButton.Image = Image.FromFile(Files.brick_blue);

            boxToolStripMenuItem.Image = Image.FromFile(Files.box1);
            boxToolStripButton.Image = Image.FromFile(Files.box1); ;

            pipeToolStripMenuItem.Image = Image.FromFile(Files.pipe_green_up);
            pipeToolStripButton.Image = Image.FromFile(Files.pipe_green_up); ;

            pointStarToolStripMenuItem.Image = Image.FromFile(Files.star);
            starToolStripButton.Image = Image.FromFile(Files.star); ;

            platformToolStripMenuItem.Image = Image.FromFile(Files.brick_blue);
            bricksToolStripMenuItem.Image = Image.FromFile(Files.brick_blue);
            bricksToolStripButton.Image = Image.FromFile(Files.brick_blue); ;

            enemyToolStripMenuItem.Image = AnimateImage.FirstImage(Files.turtle_green);
            turtleToolStripMenuItem.Image = AnimateImage.FirstImage(Files.turtle_green);
            turtleToolStripButton.Image = AnimateImage.FirstImage(Files.turtle_green); ;

            spikaToolStripMenuItem.Image = Image.FromFile(Files.spika_green);
            spikaToolStripButton.Image = Image.FromFile(Files.spika_green); ;

            gumbaToolStripMenuItem.Image = AnimateImage.FirstImage(Files.gumba_brown);
            gumbaToolStripButton.Image = AnimateImage.FirstImage(Files.gumba_brown); ;

            levelEndToolStripMenuItem.Image = Image.FromFile(Files.levelend_house);
            houseToolStripMenuItem.Image = Image.FromFile(Files.levelend_house);
            houseToolStripButton.Image = Image.FromFile(Files.levelend_house); ;

            gameLevelbeginToolStripMenuItem.Image = Image.FromFile(Files.icon_png);
            startGamecurrentToolStripMenuItem.Image = Image.FromFile(Files.icon_png);
            startGameCurrentToolStripButton.Image = Image.FromFile(Files.icon_png);
            #endregion

            newToolStripMenuItem_Click(new object(), new EventArgs());
        }


        #region Drag 'n' Drop Events
        private void level_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            List<GameObject> gos = world.AllElements;
            for (int i = gos.Count - 1; i >= 0; i--)
            {
                GameObject go = gos[i];
                if (go.Hit(e.Y, e.X))
                {
                    focus = go;
                    if (!pressedKeys.Contains(Keys.ControlKey))
                        properties.SelectedObjects = new object[] { focus };

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
                int movementTop = (e.Y - mouseY) - focus.Top;
                int movementLeft = (e.X - mouseX) - focus.Left;

                focus.Top += movementTop;
                focus.Left += movementLeft;
                
                for (int i = 0; i < properties.SelectedObjects.Length; i++)
                {
                    if (properties.SelectedObjects[i] != focus &&
                        properties.SelectedObjects[i] is GameObject)
                    {
                        ((GameObject)properties.SelectedObjects[i]).Top += movementTop;
                        ((GameObject)properties.SelectedObjects[i]).Left += movementLeft;
                    }
                }
                level.Invalidate();
            }
        }

        private void level_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
            moving = false;
            level.Invalidate(); // paint again, with background
        }

        private void ViewContextMenue(int x, int y)
        {
            for (int i = objectContext.Items.Count - 1; i > 2; i--) // start from end
            {                                               // easier to understand, because otherwise
                objectContext.Items.RemoveAt(i);            // you 've to remove always the 3. element
            }

            List<ToolStripItem> newitems = EditorUI.GenerateProperties(ref focus, ref level, ref properties);
            objectContext.Items.AddRange(newitems.ToArray());
            objectContext.Show(Cursor.Position.X, Cursor.Position.Y);
        }


        private void level_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (focus != null)
            {
                if (!pressedKeys.Contains(Keys.ControlKey))
                    properties.SelectedObjects = new object[] { focus };
                else
                {
                    if (Array.IndexOf<object>(properties.SelectedObjects, focus) == -1)
                    {
                        object[] selected = properties.SelectedObjects;
                        Array.Resize<object>(ref selected, selected.Length + 1);
                        selected[selected.Length - 1] = focus;

                        properties.SelectedObjects = selected;
                    }  
                }


                if (e.Button == MouseButtons.Right)
                {
                    ViewContextMenue(Cursor.Position.X, Cursor.Position.Y);
                    mouseX = e.X - focus.Left;
                    mouseY = e.Y - focus.Top;
                }
            }
        }
        #endregion


        private void showlevelSettings_Click(object sender, EventArgs e)
        {
            properties.SelectedObjects = new object[] { world.Settings };
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

        #region Main MenuBar Events
        private World getDefaultWorld()
        {
            return new World(700, 550, level.Invalidate, PlayMode.Editor);
        }
        private void SaveWorld(string filename)
        {
            if (world != null)
                scrollAll(scrolled);

            world.Serialize(filename);

            if (world != null)
                scrollAll(-scrolled);
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmpHandler.New();

            this.Text = "littleRunner Game Editor";
            world = getDefaultWorld();
            tmpHandler.SaveHandler = SaveWorld;
            setDelegateHandlers();

            showlevelSettings_Click(sender, e);
            trackBar.Value = 0;
            level.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tmpHandler.Open())
            {
                this.Text = "littleRunner Game Editor - " + tmpHandler.OrigFilename;
                trackBar.Value = 0;

                world = new World(tmpHandler.TmpFilename, level.Invalidate, new GameSession(), PlayMode.Editor);
                tmpHandler.SaveHandler = SaveWorld;
                setDelegateHandlers();

                showlevelSettings_Click(sender, e);
                level.Invalidate();
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmpHandler.SaveReal();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmpHandler.SaveAsReal();

            this.Text = "littleRunner Game Editor - " + tmpHandler.OrigFilename;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gameLevelbeginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmpHandler.updateTMP();

            g = new Game(programSwitcher, tmpHandler.TmpFilename, PlayMode.Game);

            g.ShowDialog();
            g = null;
        }

        private void startGamecurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmpHandler.updateTMP();

            int levelTop = this.Top + menu.Top + menubar.Top + tableLayout.Top + level.Top;
            int levelLeft = this.Left + tableLayout.Left + level.Left;
            g = new Game(programSwitcher, tmpHandler.TmpFilename, PlayMode.GameInEditor, levelTop, levelLeft);
            g.AI.Scroll(-trackBar.Value, false);

            string oldtext = this.Text;
            this.Text = "littleRunner Game Editor [press ESC to quit game]";

            g.ShowDialog();
            g = null;

            this.Text = oldtext;
        }



        private void editScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scripteditor se = new Scripteditor();
            se.ScriptText = world.Settings.Script;

            if (se.ShowDialog() == DialogResult.OK)
                world.Settings.Script = se.ScriptText;
        }

        private void checkScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (world != null)
            {
                string exception = world.InitScript();
                if (exception == "")
                    MessageBox.Show("Script seems to be OK!", "Script check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Script is not OK, exception traced:\n\n" + exception, "Script check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion


        #region Insert GameObject - Handlers
        private void addElement(GameObject go)
        {
            go.Init(world, GameAI.NullAiEventHandlerMethod);
            world.Add(go);
            level.Invalidate();

            // focus on it
            properties.SelectedObjects = new object[] { go };
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
            Turtle t = new Turtle(0, 0, TurtleStyle.Green);
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


        private void bricksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bricks b = new Bricks(0, 0, BrickColor.Blue);
            addElement(b);
        }



        private void menubar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == newToolStripButton)
                newToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == openToolStripButton)
                openToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == saveToolStripButton)
                saveToolStripMenuItem_Click(sender, e);


            else if (e.ClickedItem == floorToolStripButton)
                floorToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == treeToolStripButton)
                treeToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == boxToolStripButton)
                boxToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == brickToolStripButton)
                brickToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == pipeToolStripButton)
                pipeToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == starToolStripButton)
                pointStarToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == bricksToolStripButton)
                bricksToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == turtleToolStripButton)
                turtleToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == spikaToolStripButton)
                spikaToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == gumbaToolStripButton)
                gumbaToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == houseToolStripButton)
                houseToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == treeToolStripButton)
                treeToolStripMenuItem_Click(sender, e);


            else if (e.ClickedItem == startGameCurrentToolStripButton)
                startGamecurrentToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == editScriptToolStripButton)
                editScriptToolStripMenuItem_Click(sender, e);

            else if (e.ClickedItem == checkScriptToolStripButton)
                checkScriptToolStripMenuItem_Click(sender, e);
        }
        #endregion


        #region Contextmenue
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

                showlevelSettings_Click(sender, e);
            }
        }
        #endregion


        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmpHandler.saveChanges();
            tmpHandler.Dispose();
            tmpHandler = null;

            programSwitcher.Show();
        }

        private void level_Paint(object sender, PaintEventArgs e)
        {
            world.Draw(e.Graphics, enableBG ? (!moving) : false, properties.SelectedObjects);
        }

        private void properties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            level.Invalidate();
        }


        private void changedGameWindowWidth()
        {
            this.Width = 44 + world.Settings.GameWindowWidth + properties.Width;
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

        private void trackBar_MouseDown(object sender, MouseEventArgs e)
        {
            // disable background
            enableBG = false;
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            // enable background
            enableBG = true;
            level.Invalidate();
        }

        private void properties_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (properties.SelectedObjects.Length == 1)
                actualFocus.Text = "Focus: " + properties.SelectedObjects[0].GetType().Name;
            else
                actualFocus.Text = "selected " + properties.SelectedObjects.Length + " objects";
        }



        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pressedKeys.Contains(e.KeyCode))
                pressedKeys.Add(e.KeyCode);
        }

        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }
    }
}