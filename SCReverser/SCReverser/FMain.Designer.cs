namespace SCReverser
{
    partial class FMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInstructions = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.GridOpCode = new System.Windows.Forms.DataGridView();
            this.dcOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcOpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcArgument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelRegisters = new System.Windows.Forms.Panel();
            this.stacks = new SCReverser.Controls.UStacks();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.Registers = new System.Windows.Forms.PropertyGrid();
            this.Jumps = new SCReverser.Controls.UCPJump();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Hex = new Be.Windows.Forms.HexBox();
            this.SplitterHex = new System.Windows.Forms.Splitter();
            this.TreeModules = new System.Windows.Forms.TreeView();
            this.imageMethods = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.stepIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabInstructions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridOpCode)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.PanelRegisters.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripButton1,
            this.toolStripButton8,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton7,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator4});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Image = global::SCReverser.Res.Format;
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::SCReverser.Res.OpenIcon;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton8, "toolStripButton8");
            this.toolStripButton8.Image = global::SCReverser.Res.SaveIcon;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Image = global::SCReverser.Res.PlayIcon;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
            this.toolStripButton7.Image = global::SCReverser.Res.Stop;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Image = global::SCReverser.Res.Into;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.stepIntoToolStripMenuItem_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.Image = global::SCReverser.Res.Over;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Image = global::SCReverser.Res.Out;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgressBar,
            this.tsInfo});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Name = "tsProgressBar";
            resources.ApplyResources(this.tsProgressBar, "tsProgressBar");
            // 
            // tsInfo
            // 
            this.tsInfo.Name = "tsInfo";
            resources.ApplyResources(this.tsInfo, "tsInfo");
            this.tsInfo.Spring = true;
            this.tsInfo.Click += new System.EventHandler(this.tsInfo_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInstructions);
            this.tabControl1.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabInstructions
            // 
            this.tabInstructions.Controls.Add(this.splitter1);
            this.tabInstructions.Controls.Add(this.GridOpCode);
            this.tabInstructions.Controls.Add(this.PanelRegisters);
            this.tabInstructions.Controls.Add(this.Jumps);
            resources.ApplyResources(this.tabInstructions, "tabInstructions");
            this.tabInstructions.Name = "tabInstructions";
            this.tabInstructions.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // GridOpCode
            // 
            this.GridOpCode.AllowUserToAddRows = false;
            this.GridOpCode.AllowUserToDeleteRows = false;
            this.GridOpCode.AllowUserToResizeRows = false;
            this.GridOpCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GridOpCode.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.GridOpCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridOpCode.ColumnHeadersVisible = false;
            this.GridOpCode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcOffset,
            this.dcOpCode,
            this.dcArgument,
            this.dcComment});
            this.GridOpCode.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.GridOpCode, "GridOpCode");
            this.GridOpCode.Name = "GridOpCode";
            this.GridOpCode.RowHeadersVisible = false;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.GridOpCode.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.GridOpCode.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridOpCode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridOpCode.VirtualMode = true;
            this.GridOpCode.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridOpCode_CellFormatting);
            this.GridOpCode.CurrentCellChanged += new System.EventHandler(this.GridOpCode_CurrentCellChanged);
            this.GridOpCode.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.GridOpCode_RowPrePaint);
            this.GridOpCode.Scroll += new System.Windows.Forms.ScrollEventHandler(this.GridOpCode_Scroll);
            this.GridOpCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridOpCode_KeyDown);
            this.GridOpCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridOpCode_MouseDown);
            // 
            // dcOffset
            // 
            this.dcOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcOffset.DataPropertyName = "OffsetHex";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dcOffset.DefaultCellStyle = dataGridViewCellStyle1;
            this.dcOffset.Frozen = true;
            resources.ApplyResources(this.dcOffset, "dcOffset");
            this.dcOffset.Name = "dcOffset";
            this.dcOffset.ReadOnly = true;
            this.dcOffset.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dcOffset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcOpCode
            // 
            this.dcOpCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcOpCode.DataPropertyName = "OpCode";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Ivory;
            this.dcOpCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.dcOpCode.Frozen = true;
            resources.ApplyResources(this.dcOpCode, "dcOpCode");
            this.dcOpCode.Name = "dcOpCode";
            this.dcOpCode.ReadOnly = true;
            this.dcOpCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dcOpCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcArgument
            // 
            this.dcArgument.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dcArgument.DataPropertyName = "Argument";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dcArgument.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.dcArgument, "dcArgument");
            this.dcArgument.Name = "dcArgument";
            this.dcArgument.ReadOnly = true;
            this.dcArgument.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcComment
            // 
            this.dcComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcComment.DataPropertyName = "Comment";
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dcComment.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.dcComment, "dcComment");
            this.dcComment.Name = "dcComment";
            this.dcComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goHereToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // goHereToolStripMenuItem
            // 
            this.goHereToolStripMenuItem.Name = "goHereToolStripMenuItem";
            resources.ApplyResources(this.goHereToolStripMenuItem, "goHereToolStripMenuItem");
            this.goHereToolStripMenuItem.Click += new System.EventHandler(this.goHereToolStripMenuItem_Click);
            // 
            // PanelRegisters
            // 
            this.PanelRegisters.BackColor = System.Drawing.SystemColors.Control;
            this.PanelRegisters.Controls.Add(this.stacks);
            this.PanelRegisters.Controls.Add(this.splitter2);
            this.PanelRegisters.Controls.Add(this.Registers);
            resources.ApplyResources(this.PanelRegisters, "PanelRegisters");
            this.PanelRegisters.Name = "PanelRegisters";
            // 
            // stacks
            // 
            resources.ApplyResources(this.stacks, "stacks");
            this.stacks.Name = "stacks";
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // Registers
            // 
            resources.ApplyResources(this.Registers, "Registers");
            this.Registers.LineColor = System.Drawing.SystemColors.ControlDark;
            this.Registers.Name = "Registers";
            this.Registers.ToolbarVisible = false;
            this.Registers.ViewBackColor = System.Drawing.SystemColors.Control;
            this.Registers.ViewBorderColor = System.Drawing.SystemColors.Control;
            // 
            // Jumps
            // 
            resources.ApplyResources(this.Jumps, "Jumps");
            this.Jumps.Grid = this.GridOpCode;
            this.Jumps.Name = "Jumps";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Hex);
            this.tabPage1.Controls.Add(this.SplitterHex);
            this.tabPage1.Controls.Add(this.TreeModules);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Hex
            // 
            this.Hex.ColumnInfoVisible = true;
            resources.ApplyResources(this.Hex, "Hex");
            this.Hex.LineInfoVisible = true;
            this.Hex.Name = "Hex";
            this.Hex.ReadOnly = true;
            this.Hex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.Hex.StringViewVisible = true;
            this.Hex.VScrollBarVisible = true;
            // 
            // SplitterHex
            // 
            resources.ApplyResources(this.SplitterHex, "SplitterHex");
            this.SplitterHex.Name = "SplitterHex";
            this.SplitterHex.TabStop = false;
            // 
            // TreeModules
            // 
            resources.ApplyResources(this.TreeModules, "TreeModules");
            this.TreeModules.ImageList = this.imageMethods;
            this.TreeModules.Name = "TreeModules";
            this.TreeModules.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeModules_AfterSelect);
            this.TreeModules.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeModules_NodeMouseDoubleClick);
            // 
            // imageMethods
            // 
            this.imageMethods.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageMethods.ImageStream")));
            this.imageMethods.TransparentColor = System.Drawing.Color.Transparent;
            this.imageMethods.Images.SetKeyName(0, "Module");
            this.imageMethods.Images.SetKeyName(1, "Method");
            this.imageMethods.Images.SetKeyName(2, "OffsetFrom");
            this.imageMethods.Images.SetKeyName(3, "OffsetTo");
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debuggerToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.Image = global::SCReverser.Res.Format;
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            resources.ApplyResources(this.formatToolStripMenuItem, "formatToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SCReverser.Res.OpenIcon;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::SCReverser.Res.SaveIcon;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::SCReverser.Res.SaveIcon;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::SCReverser.Res.Stop;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // debuggerToolStripMenuItem
            // 
            this.debuggerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripMenuItem2,
            this.stepIntoToolStripMenuItem,
            this.stepOverToolStripMenuItem,
            this.stepOutToolStripMenuItem});
            this.debuggerToolStripMenuItem.Name = "debuggerToolStripMenuItem";
            resources.ApplyResources(this.debuggerToolStripMenuItem, "debuggerToolStripMenuItem");
            // 
            // executeToolStripMenuItem
            // 
            resources.ApplyResources(this.executeToolStripMenuItem, "executeToolStripMenuItem");
            this.executeToolStripMenuItem.Image = global::SCReverser.Res.PlayIcon;
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            resources.ApplyResources(this.stopToolStripMenuItem, "stopToolStripMenuItem");
            this.stopToolStripMenuItem.Image = global::SCReverser.Res.Stop;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // stepIntoToolStripMenuItem
            // 
            resources.ApplyResources(this.stepIntoToolStripMenuItem, "stepIntoToolStripMenuItem");
            this.stepIntoToolStripMenuItem.Image = global::SCReverser.Res.Into;
            this.stepIntoToolStripMenuItem.Name = "stepIntoToolStripMenuItem";
            this.stepIntoToolStripMenuItem.Click += new System.EventHandler(this.stepIntoToolStripMenuItem_Click);
            // 
            // stepOverToolStripMenuItem
            // 
            resources.ApplyResources(this.stepOverToolStripMenuItem, "stepOverToolStripMenuItem");
            this.stepOverToolStripMenuItem.Image = global::SCReverser.Res.Over;
            this.stepOverToolStripMenuItem.Name = "stepOverToolStripMenuItem";
            this.stepOverToolStripMenuItem.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
            // 
            // stepOutToolStripMenuItem
            // 
            resources.ApplyResources(this.stepOutToolStripMenuItem, "stepOutToolStripMenuItem");
            this.stepOutToolStripMenuItem.Image = global::SCReverser.Res.Out;
            this.stepOutToolStripMenuItem.Name = "stepOutToolStripMenuItem";
            this.stepOutToolStripMenuItem.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.json";
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            // 
            // FMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FMain";
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FMain_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabInstructions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridOpCode)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.PanelRegisters.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInstructions;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripMenuItem debuggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem stepIntoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepOverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripStatusLabel tsInfo;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid Registers;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goHereToolStripMenuItem;
        private Controls.UCPJump Jumps;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcOpCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcArgument;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcComment;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ImageList imageMethods;
        private Be.Windows.Forms.HexBox Hex;
        public System.Windows.Forms.Splitter SplitterHex;
        public System.Windows.Forms.TreeView TreeModules;
        public System.Windows.Forms.DataGridView GridOpCode;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        public System.Windows.Forms.Panel PanelRegisters;
        private Controls.UStacks stacks;
    }
}

