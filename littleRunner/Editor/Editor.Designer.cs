namespace littleRunner
{
    partial class Editor
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.level = new System.Windows.Forms.Panel();
            this.propertys = new System.Windows.Forms.PropertyGrid();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.menubar = new System.Windows.Forms.ToolStrip();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plainFloorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.designElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.brickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointStarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turtleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spikaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gumbaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toForegroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtRealWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.lWidth = new System.Windows.Forms.Label();
            this.lHeight = new System.Windows.Forms.Label();
            this.actualFocus = new System.Windows.Forms.Label();
            this.scrollBar = new System.Windows.Forms.HScrollBar();
            this.txtViewWidth = new System.Windows.Forms.TextBox();
            this.houseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.objectContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // level
            // 
            this.level.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.level.Location = new System.Drawing.Point(12, 64);
            this.level.Name = "level";
            this.level.Size = new System.Drawing.Size(652, 543);
            this.level.TabIndex = 7;
            this.level.Paint += new System.Windows.Forms.PaintEventHandler(this.level_Paint);
            this.level.MouseMove += new System.Windows.Forms.MouseEventHandler(this.level_MouseMove);
            this.level.MouseClick += new System.Windows.Forms.MouseEventHandler(this.level_MouseClick);
            this.level.MouseDown += new System.Windows.Forms.MouseEventHandler(this.level_MouseDown);
            this.level.MouseUp += new System.Windows.Forms.MouseEventHandler(this.level_MouseUp);
            // 
            // propertys
            // 
            this.propertys.Location = new System.Drawing.Point(670, 144);
            this.propertys.Name = "propertys";
            this.propertys.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertys.Size = new System.Drawing.Size(209, 485);
            this.propertys.TabIndex = 6;
            this.propertys.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertys_PropertyValueChanged);
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "lrl";
            this.openFile.FileName = "level.lrl";
            this.openFile.Filter = "littleRunner Levels (*.lrl)|*.lrl";
            this.openFile.InitialDirectory = "./Levels";
            this.openFile.RestoreDirectory = true;
            // 
            // menubar
            // 
            this.menubar.Location = new System.Drawing.Point(0, 24);
            this.menubar.Name = "menubar";
            this.menubar.Size = new System.Drawing.Size(883, 25);
            this.menubar.TabIndex = 5;
            this.menubar.Text = "toolStrip1";
            // 
            // saveFile
            // 
            this.saveFile.DefaultExt = "lrl";
            this.saveFile.FileName = "level.lrl";
            this.saveFile.Filter = "littleRunner Levels (*.lrl)|*.lrl";
            this.saveFile.InitialDirectory = "./Levels";
            this.saveFile.OverwritePrompt = false;
            this.saveFile.RestoreDirectory = true;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.startGameToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(883, 24);
            this.menu.TabIndex = 4;
            this.menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.floorToolStripMenuItem,
            this.designElementToolStripMenuItem,
            this.boxToolStripMenuItem,
            this.brickToolStripMenuItem,
            this.pipeToolStripMenuItem,
            this.pointStarToolStripMenuItem,
            this.enemyToolStripMenuItem,
            this.levelEndToolStripMenuItem});
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.insertToolStripMenuItem.Text = "Insert";
            // 
            // floorToolStripMenuItem
            // 
            this.floorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plainFloorToolStripMenuItem,
            this.floor2ToolStripMenuItem});
            this.floorToolStripMenuItem.Name = "floorToolStripMenuItem";
            this.floorToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.floorToolStripMenuItem.Text = "Floor";
            // 
            // plainFloorToolStripMenuItem
            // 
            this.plainFloorToolStripMenuItem.Name = "plainFloorToolStripMenuItem";
            this.plainFloorToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.plainFloorToolStripMenuItem.Text = "Plain Floor";
            this.plainFloorToolStripMenuItem.Click += new System.EventHandler(this.plainFloorToolStripMenuItem_Click);
            // 
            // floor2ToolStripMenuItem
            // 
            this.floor2ToolStripMenuItem.Name = "floor2ToolStripMenuItem";
            this.floor2ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.floor2ToolStripMenuItem.Text = "Floor";
            this.floor2ToolStripMenuItem.Click += new System.EventHandler(this.floor2ToolStripMenuItem_Click);
            // 
            // designElementToolStripMenuItem
            // 
            this.designElementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.treeToolStripMenuItem});
            this.designElementToolStripMenuItem.Name = "designElementToolStripMenuItem";
            this.designElementToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.designElementToolStripMenuItem.Text = "Design Element";
            // 
            // treeToolStripMenuItem
            // 
            this.treeToolStripMenuItem.Name = "treeToolStripMenuItem";
            this.treeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.treeToolStripMenuItem.Text = "Tree";
            this.treeToolStripMenuItem.Click += new System.EventHandler(this.treeToolStripMenuItem_Click);
            // 
            // boxToolStripMenuItem
            // 
            this.boxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boxToolStripMenuItem1});
            this.boxToolStripMenuItem.Name = "boxToolStripMenuItem";
            this.boxToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.boxToolStripMenuItem.Text = "Box";
            // 
            // boxToolStripMenuItem1
            // 
            this.boxToolStripMenuItem1.Name = "boxToolStripMenuItem1";
            this.boxToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.boxToolStripMenuItem1.Text = "Box";
            this.boxToolStripMenuItem1.Click += new System.EventHandler(this.boxToolStripMenuItem1_Click);
            // 
            // brickToolStripMenuItem
            // 
            this.brickToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yellowToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.redToolStripMenuItem,
            this.iceToolStripMenuItem});
            this.brickToolStripMenuItem.Name = "brickToolStripMenuItem";
            this.brickToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.brickToolStripMenuItem.Text = "Brick";
            // 
            // yellowToolStripMenuItem
            // 
            this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            this.yellowToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.yellowToolStripMenuItem.Text = "Yellow";
            this.yellowToolStripMenuItem.Click += new System.EventHandler(this.yellowToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.blueToolStripMenuItem.Text = "Blue";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.redToolStripMenuItem.Text = "Red";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);
            // 
            // iceToolStripMenuItem
            // 
            this.iceToolStripMenuItem.Name = "iceToolStripMenuItem";
            this.iceToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.iceToolStripMenuItem.Text = "Ice";
            this.iceToolStripMenuItem.Click += new System.EventHandler(this.iceToolStripMenuItem_Click);
            // 
            // pipeToolStripMenuItem
            // 
            this.pipeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upToolStripMenuItem,
            this.mainToolStripMenuItem});
            this.pipeToolStripMenuItem.Name = "pipeToolStripMenuItem";
            this.pipeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.pipeToolStripMenuItem.Text = "Pipe";
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.upToolStripMenuItem.Text = "Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.mainToolStripMenuItem.Text = "Main";
            this.mainToolStripMenuItem.Click += new System.EventHandler(this.mainToolStripMenuItem_Click);
            // 
            // pointStarToolStripMenuItem
            // 
            this.pointStarToolStripMenuItem.Name = "pointStarToolStripMenuItem";
            this.pointStarToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.pointStarToolStripMenuItem.Text = "Point (Star)";
            this.pointStarToolStripMenuItem.Click += new System.EventHandler(this.pointStarToolStripMenuItem_Click);
            // 
            // enemyToolStripMenuItem
            // 
            this.enemyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turtleToolStripMenuItem,
            this.spikaToolStripMenuItem,
            this.gumbaToolStripMenuItem});
            this.enemyToolStripMenuItem.Name = "enemyToolStripMenuItem";
            this.enemyToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.enemyToolStripMenuItem.Text = "Enemy";
            // 
            // turtleToolStripMenuItem
            // 
            this.turtleToolStripMenuItem.Name = "turtleToolStripMenuItem";
            this.turtleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.turtleToolStripMenuItem.Text = "Turtle";
            this.turtleToolStripMenuItem.Click += new System.EventHandler(this.turtleToolStripMenuItem_Click);
            // 
            // spikaToolStripMenuItem
            // 
            this.spikaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.greenToolStripMenuItem,
            this.orangeToolStripMenuItem,
            this.greyToolStripMenuItem});
            this.spikaToolStripMenuItem.Name = "spikaToolStripMenuItem";
            this.spikaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.spikaToolStripMenuItem.Text = "Spika";
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.greenToolStripMenuItem.Text = "Green";
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
            // 
            // orangeToolStripMenuItem
            // 
            this.orangeToolStripMenuItem.Name = "orangeToolStripMenuItem";
            this.orangeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.orangeToolStripMenuItem.Text = "Orange";
            this.orangeToolStripMenuItem.Click += new System.EventHandler(this.orangeToolStripMenuItem_Click);
            // 
            // greyToolStripMenuItem
            // 
            this.greyToolStripMenuItem.Name = "greyToolStripMenuItem";
            this.greyToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.greyToolStripMenuItem.Text = "Grey";
            this.greyToolStripMenuItem.Click += new System.EventHandler(this.greyToolStripMenuItem_Click);
            // 
            // gumbaToolStripMenuItem
            // 
            this.gumbaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brownToolStripMenuItem});
            this.gumbaToolStripMenuItem.Name = "gumbaToolStripMenuItem";
            this.gumbaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gumbaToolStripMenuItem.Text = "Gumba";
            // 
            // brownToolStripMenuItem
            // 
            this.brownToolStripMenuItem.Name = "brownToolStripMenuItem";
            this.brownToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.brownToolStripMenuItem.Text = "Brown";
            this.brownToolStripMenuItem.Click += new System.EventHandler(this.brownToolStripMenuItem_Click);
            // 
            // levelEndToolStripMenuItem
            // 
            this.levelEndToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.houseToolStripMenuItem});
            this.levelEndToolStripMenuItem.Name = "levelEndToolStripMenuItem";
            this.levelEndToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.levelEndToolStripMenuItem.Text = "Level End";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // startGameToolStripMenuItem
            // 
            this.startGameToolStripMenuItem.Name = "startGameToolStripMenuItem";
            this.startGameToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.startGameToolStripMenuItem.Text = "Start Game";
            this.startGameToolStripMenuItem.Click += new System.EventHandler(this.startGameToolStripMenuItem_Click);
            // 
            // objectContext
            // 
            this.objectContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toForegroundToolStripMenuItem,
            this.toBackgroundToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.objectContext.Name = "objectContext";
            this.objectContext.Size = new System.Drawing.Size(157, 70);
            // 
            // toForegroundToolStripMenuItem
            // 
            this.toForegroundToolStripMenuItem.Name = "toForegroundToolStripMenuItem";
            this.toForegroundToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.toForegroundToolStripMenuItem.Text = "To foreground";
            this.toForegroundToolStripMenuItem.Click += new System.EventHandler(this.toForegroundToolStripMenuItem_Click);
            // 
            // toBackgroundToolStripMenuItem
            // 
            this.toBackgroundToolStripMenuItem.Name = "toBackgroundToolStripMenuItem";
            this.toBackgroundToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.toBackgroundToolStripMenuItem.Text = "To background";
            this.toBackgroundToolStripMenuItem.Click += new System.EventHandler(this.toBackgroundToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // txtRealWidth
            // 
            this.txtRealWidth.Location = new System.Drawing.Point(802, 64);
            this.txtRealWidth.Name = "txtRealWidth";
            this.txtRealWidth.Size = new System.Drawing.Size(47, 20);
            this.txtRealWidth.TabIndex = 10;
            this.txtRealWidth.Text = "652";
            this.txtRealWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRealWidth_KeyUp);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(749, 90);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 20);
            this.txtHeight.TabIndex = 11;
            this.txtHeight.Text = "543";
            this.txtHeight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtHeight_KeyUp);
            // 
            // lWidth
            // 
            this.lWidth.AutoSize = true;
            this.lWidth.Font = new System.Drawing.Font("Bitstream Vera Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lWidth.Location = new System.Drawing.Point(670, 66);
            this.lWidth.Name = "lWidth";
            this.lWidth.Size = new System.Drawing.Size(50, 15);
            this.lWidth.TabIndex = 12;
            this.lWidth.Text = "Width";
            // 
            // lHeight
            // 
            this.lHeight.AutoSize = true;
            this.lHeight.Font = new System.Drawing.Font("Bitstream Vera Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeight.Location = new System.Drawing.Point(670, 92);
            this.lHeight.Name = "lHeight";
            this.lHeight.Size = new System.Drawing.Size(56, 15);
            this.lHeight.TabIndex = 13;
            this.lHeight.Text = "Height";
            // 
            // actualFocus
            // 
            this.actualFocus.AutoSize = true;
            this.actualFocus.Font = new System.Drawing.Font("Bitstream Vera Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actualFocus.Location = new System.Drawing.Point(670, 113);
            this.actualFocus.Name = "actualFocus";
            this.actualFocus.Size = new System.Drawing.Size(116, 15);
            this.actualFocus.TabIndex = 14;
            this.actualFocus.Text = "Focus: nothing";
            // 
            // scrollBar
            // 
            this.scrollBar.LargeChange = 30;
            this.scrollBar.Location = new System.Drawing.Point(12, 610);
            this.scrollBar.Maximum = 652;
            this.scrollBar.Minimum = 622;
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(652, 19);
            this.scrollBar.SmallChange = 20;
            this.scrollBar.TabIndex = 0;
            this.scrollBar.Value = 622;
            this.scrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBar_Scroll);
            // 
            // txtViewWidth
            // 
            this.txtViewWidth.Location = new System.Drawing.Point(749, 64);
            this.txtViewWidth.Name = "txtViewWidth";
            this.txtViewWidth.Size = new System.Drawing.Size(47, 20);
            this.txtViewWidth.TabIndex = 15;
            this.txtViewWidth.Text = "652";
            this.txtViewWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtViewWidth_KeyUp);
            // 
            // houseToolStripMenuItem
            // 
            this.houseToolStripMenuItem.Name = "houseToolStripMenuItem";
            this.houseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.houseToolStripMenuItem.Text = "House";
            this.houseToolStripMenuItem.Click += new System.EventHandler(this.houseToolStripMenuItem_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 639);
            this.Controls.Add(this.txtViewWidth);
            this.Controls.Add(this.actualFocus);
            this.Controls.Add(this.lHeight);
            this.Controls.Add(this.scrollBar);
            this.Controls.Add(this.lWidth);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtRealWidth);
            this.Controls.Add(this.level);
            this.Controls.Add(this.propertys);
            this.Controls.Add(this.menubar);
            this.Controls.Add(this.menu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Editor";
            this.Text = "littleRunner Game Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Editor_FormClosed);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.objectContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointStarToolStripMenuItem;
        private System.Windows.Forms.Panel level;
        private System.Windows.Forms.PropertyGrid propertys;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.ToolStrip menubar;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem brickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem designElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip objectContext;
        private System.Windows.Forms.ToolStripMenuItem toForegroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turtleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pipeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TextBox txtRealWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label lWidth;
        private System.Windows.Forms.Label lHeight;
        private System.Windows.Forms.Label actualFocus;
        private System.Windows.Forms.HScrollBar scrollBar;
        private System.Windows.Forms.TextBox txtViewWidth;
        private System.Windows.Forms.ToolStripMenuItem boxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem spikaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gumbaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem levelEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plainFloorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floor2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem houseToolStripMenuItem;
    }
}