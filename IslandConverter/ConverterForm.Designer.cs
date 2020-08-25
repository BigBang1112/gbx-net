using System.Reflection;

namespace IslandConverter
{
    partial class ConverterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Please drag and drop the maps here...");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConverterForm));
            this.pbThumbnail = new System.Windows.Forms.PictureBox();
            this.bConvertAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lBlockRange = new System.Windows.Forms.Label();
            this.lMapType = new System.Windows.Forms.Label();
            this.lBronzeM = new System.Windows.Forms.Label();
            this.lSilverM = new System.Windows.Forms.Label();
            this.lGoldM = new System.Windows.Forms.Label();
            this.lAuthorM = new System.Windows.Forms.Label();
            this.lMedals = new System.Windows.Forms.Label();
            this.lDecoration = new System.Windows.Forms.Label();
            this.lSize = new System.Windows.Forms.Label();
            this.lAuthor = new System.Windows.Forms.Label();
            this.lUID = new System.Windows.Forms.Label();
            this.lMapName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ProgramMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideMapPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideCommandLineOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeManiaPlanetUserdataLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiASuperSecretOption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.bOpenFolder = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.x45Button = new System.Windows.Forms.RadioButton();
            this.x32Button = new System.Windows.Forms.RadioButton();
            this.x31Button = new System.Windows.Forms.RadioButton();
            this.bConvertSelected = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbIgnoreMediaTracker = new System.Windows.Forms.CheckBox();
            this.cbCutoff = new System.Windows.Forms.CheckBox();
            this.lvMaps = new System.Windows.Forms.ListView();
            this.ilThumbnails = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ttBlockRange = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ProgramMenuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbThumbnail
            // 
            this.pbThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbThumbnail.Location = new System.Drawing.Point(8, 255);
            this.pbThumbnail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbThumbnail.Name = "pbThumbnail";
            this.pbThumbnail.Size = new System.Drawing.Size(245, 238);
            this.pbThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbThumbnail.TabIndex = 1;
            this.pbThumbnail.TabStop = false;
            this.pbThumbnail.WaitOnLoad = true;
            // 
            // bConvertAll
            // 
            this.bConvertAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bConvertAll.Enabled = false;
            this.bConvertAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bConvertAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bConvertAll.Location = new System.Drawing.Point(495, 258);
            this.bConvertAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConvertAll.Name = "bConvertAll";
            this.bConvertAll.Size = new System.Drawing.Size(188, 39);
            this.bConvertAll.TabIndex = 2;
            this.bConvertAll.Text = "Convert all maps";
            this.bConvertAll.UseVisualStyleBackColor = true;
            this.bConvertAll.Click += new System.EventHandler(this.bConvertAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.lBlockRange);
            this.groupBox1.Controls.Add(this.lMapType);
            this.groupBox1.Controls.Add(this.lBronzeM);
            this.groupBox1.Controls.Add(this.lSilverM);
            this.groupBox1.Controls.Add(this.lGoldM);
            this.groupBox1.Controls.Add(this.lAuthorM);
            this.groupBox1.Controls.Add(this.lMedals);
            this.groupBox1.Controls.Add(this.lDecoration);
            this.groupBox1.Controls.Add(this.lSize);
            this.groupBox1.Controls.Add(this.lAuthor);
            this.groupBox1.Controls.Add(this.lUID);
            this.groupBox1.Controls.Add(this.lMapName);
            this.groupBox1.Controls.Add(this.pbThumbnail);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(262, 501);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected map";
            // 
            // lBlockRange
            // 
            this.lBlockRange.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lBlockRange.AutoSize = true;
            this.lBlockRange.Location = new System.Drawing.Point(13, 212);
            this.lBlockRange.Name = "lBlockRange";
            this.lBlockRange.Size = new System.Drawing.Size(75, 15);
            this.lBlockRange.TabIndex = 4;
            this.lBlockRange.Text = "Block range:";
            // 
            // lMapType
            // 
            this.lMapType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lMapType.AutoSize = true;
            this.lMapType.Location = new System.Drawing.Point(13, 197);
            this.lMapType.Name = "lMapType";
            this.lMapType.Size = new System.Drawing.Size(60, 15);
            this.lMapType.TabIndex = 4;
            this.lMapType.Text = "Map type:";
            // 
            // lBronzeM
            // 
            this.lBronzeM.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lBronzeM.AutoSize = true;
            this.lBronzeM.Location = new System.Drawing.Point(21, 182);
            this.lBronzeM.Name = "lBronzeM";
            this.lBronzeM.Size = new System.Drawing.Size(49, 15);
            this.lBronzeM.TabIndex = 4;
            this.lBronzeM.Text = "Bronze:";
            // 
            // lSilverM
            // 
            this.lSilverM.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lSilverM.AutoSize = true;
            this.lSilverM.Location = new System.Drawing.Point(21, 167);
            this.lSilverM.Name = "lSilverM";
            this.lSilverM.Size = new System.Drawing.Size(40, 15);
            this.lSilverM.TabIndex = 4;
            this.lSilverM.Text = "Silver:";
            // 
            // lGoldM
            // 
            this.lGoldM.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lGoldM.AutoSize = true;
            this.lGoldM.Location = new System.Drawing.Point(21, 152);
            this.lGoldM.Name = "lGoldM";
            this.lGoldM.Size = new System.Drawing.Size(36, 15);
            this.lGoldM.TabIndex = 4;
            this.lGoldM.Text = "Gold:";
            // 
            // lAuthorM
            // 
            this.lAuthorM.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lAuthorM.AutoSize = true;
            this.lAuthorM.Location = new System.Drawing.Point(21, 137);
            this.lAuthorM.Name = "lAuthorM";
            this.lAuthorM.Size = new System.Drawing.Size(45, 15);
            this.lAuthorM.TabIndex = 4;
            this.lAuthorM.Text = "Author:";
            // 
            // lMedals
            // 
            this.lMedals.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lMedals.AutoSize = true;
            this.lMedals.Location = new System.Drawing.Point(13, 122);
            this.lMedals.Name = "lMedals";
            this.lMedals.Size = new System.Drawing.Size(51, 15);
            this.lMedals.TabIndex = 4;
            this.lMedals.Text = "Medals:";
            // 
            // lDecoration
            // 
            this.lDecoration.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lDecoration.AutoSize = true;
            this.lDecoration.Location = new System.Drawing.Point(13, 105);
            this.lDecoration.Name = "lDecoration";
            this.lDecoration.Size = new System.Drawing.Size(70, 15);
            this.lDecoration.TabIndex = 4;
            this.lDecoration.Text = "Decoration:";
            // 
            // lSize
            // 
            this.lSize.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lSize.AutoSize = true;
            this.lSize.Location = new System.Drawing.Point(13, 90);
            this.lSize.Name = "lSize";
            this.lSize.Size = new System.Drawing.Size(34, 15);
            this.lSize.TabIndex = 4;
            this.lSize.Text = "Size:";
            // 
            // lAuthor
            // 
            this.lAuthor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lAuthor.AutoSize = true;
            this.lAuthor.Location = new System.Drawing.Point(13, 75);
            this.lAuthor.Name = "lAuthor";
            this.lAuthor.Size = new System.Drawing.Size(45, 15);
            this.lAuthor.TabIndex = 4;
            this.lAuthor.Text = "Author:";
            // 
            // lUID
            // 
            this.lUID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lUID.AutoSize = true;
            this.lUID.Location = new System.Drawing.Point(13, 60);
            this.lUID.Name = "lUID";
            this.lUID.Size = new System.Drawing.Size(31, 15);
            this.lUID.TabIndex = 3;
            this.lUID.Text = "UID:";
            // 
            // lMapName
            // 
            this.lMapName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lMapName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lMapName.Location = new System.Drawing.Point(13, 27);
            this.lMapName.Name = "lMapName";
            this.lMapName.Size = new System.Drawing.Size(235, 23);
            this.lMapName.TabIndex = 2;
            this.lMapName.Text = "[MAP NAME]";
            this.lMapName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbLog);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Location = new System.Drawing.Point(7, 22);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 167);
            this.panel1.TabIndex = 4;
            // 
            // lbLog
            // 
            this.lbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 15;
            this.lbLog.Location = new System.Drawing.Point(0, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(667, 165);
            this.lbLog.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.Location = new System.Drawing.Point(7, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(683, 196);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // ProgramMenuStrip
            // 
            this.ProgramMenuStrip.BackColor = System.Drawing.SystemColors.Window;
            this.ProgramMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileItem,
            this.ViewItem,
            this.tsmiOptions,
            this.toolStripMenuItem2});
            this.ProgramMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.ProgramMenuStrip.Name = "ProgramMenuStrip";
            this.ProgramMenuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.ProgramMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ProgramMenuStrip.Size = new System.Drawing.Size(960, 24);
            this.ProgramMenuStrip.TabIndex = 5;
            this.ProgramMenuStrip.Text = "File";
            // 
            // FileItem
            // 
            this.FileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAMapToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.FileItem.Name = "FileItem";
            this.FileItem.Size = new System.Drawing.Size(37, 20);
            this.FileItem.Text = "File";
            // 
            // addAMapToolStripMenuItem
            // 
            this.addAMapToolStripMenuItem.Name = "addAMapToolStripMenuItem";
            this.addAMapToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addAMapToolStripMenuItem.Text = "Add maps...";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // ViewItem
            // 
            this.ViewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideMapPreviewToolStripMenuItem,
            this.showHideCommandLineOutputToolStripMenuItem});
            this.ViewItem.Name = "ViewItem";
            this.ViewItem.Size = new System.Drawing.Size(44, 20);
            this.ViewItem.Text = "View";
            // 
            // showHideMapPreviewToolStripMenuItem
            // 
            this.showHideMapPreviewToolStripMenuItem.Name = "showHideMapPreviewToolStripMenuItem";
            this.showHideMapPreviewToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.showHideMapPreviewToolStripMenuItem.Text = "Show/Hide Map Preview";
            this.showHideMapPreviewToolStripMenuItem.Click += new System.EventHandler(this.ShowHideMapPreviewToolStripMenuItem_Click);
            // 
            // showHideCommandLineOutputToolStripMenuItem
            // 
            this.showHideCommandLineOutputToolStripMenuItem.Name = "showHideCommandLineOutputToolStripMenuItem";
            this.showHideCommandLineOutputToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.showHideCommandLineOutputToolStripMenuItem.Text = "Show/Hide Command Line Output";
            this.showHideCommandLineOutputToolStripMenuItem.Click += new System.EventHandler(this.ShowHideCommandLineOutputToolStripMenuItem_Click);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChangeManiaPlanetUserdataLocation,
            this.tsmiASuperSecretOption});
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(61, 20);
            this.tsmiOptions.Text = "Options";
            // 
            // tsmiChangeManiaPlanetUserdataLocation
            // 
            this.tsmiChangeManiaPlanetUserdataLocation.Name = "tsmiChangeManiaPlanetUserdataLocation";
            this.tsmiChangeManiaPlanetUserdataLocation.Size = new System.Drawing.Size(283, 22);
            this.tsmiChangeManiaPlanetUserdataLocation.Text = "Change ManiaPlanet user-data location";
            // 
            // tsmiASuperSecretOption
            // 
            this.tsmiASuperSecretOption.Name = "tsmiASuperSecretOption";
            this.tsmiASuperSecretOption.Size = new System.Drawing.Size(283, 22);
            this.tsmiASuperSecretOption.Text = "... a super secret option";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(52, 20);
            this.toolStripMenuItem2.Text = "About";
            // 
            // bOpenFolder
            // 
            this.bOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOpenFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bOpenFolder.Location = new System.Drawing.Point(14, 258);
            this.bOpenFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bOpenFolder.Name = "bOpenFolder";
            this.bOpenFolder.Size = new System.Drawing.Size(188, 39);
            this.bOpenFolder.TabIndex = 7;
            this.bOpenFolder.Text = "Open Maps folder";
            this.bOpenFolder.UseVisualStyleBackColor = true;
            this.bOpenFolder.Click += new System.EventHandler(this.bOpenFolder_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.x45Button);
            this.groupBox3.Controls.Add(this.x32Button);
            this.groupBox3.Controls.Add(this.x31Button);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.Location = new System.Drawing.Point(7, 110);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(679, 114);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Map base size";
            // 
            // x45Button
            // 
            this.x45Button.AutoSize = true;
            this.x45Button.Checked = true;
            this.x45Button.Location = new System.Drawing.Point(7, 82);
            this.x45Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.x45Button.Name = "x45Button";
            this.x45Button.Size = new System.Drawing.Size(592, 19);
            this.x45Button.TabIndex = 6;
            this.x45Button.TabStop = true;
            this.x45Button.Text = "45x45 area with small border with Island background, requires OpenPlanet to work " +
    "(also recommended)";
            this.x45Button.UseVisualStyleBackColor = true;
            this.x45Button.CheckedChanged += new System.EventHandler(this.XSizeButton_CheckedChanged);
            // 
            // x32Button
            // 
            this.x32Button.AutoSize = true;
            this.x32Button.Location = new System.Drawing.Point(7, 52);
            this.x32Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.x32Button.Name = "x32Button";
            this.x32Button.Size = new System.Drawing.Size(612, 19);
            this.x32Button.TabIndex = 6;
            this.x32Button.TabStop = true;
            this.x32Button.Text = "32x32 area with normal border with Island background, doesn\'t require OpenPlanet " +
    "to work (recommended)";
            this.x32Button.UseVisualStyleBackColor = true;
            this.x32Button.CheckedChanged += new System.EventHandler(this.XSizeButton_CheckedChanged);
            // 
            // x31Button
            // 
            this.x31Button.AutoSize = true;
            this.x31Button.Location = new System.Drawing.Point(7, 23);
            this.x31Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.x31Button.Name = "x31Button";
            this.x31Button.Size = new System.Drawing.Size(527, 19);
            this.x31Button.TabIndex = 6;
            this.x31Button.TabStop = true;
            this.x31Button.Text = "31x31 area with small border and no Island background, doesn\'t require OpenPlanet" +
    " to work";
            this.x31Button.UseVisualStyleBackColor = true;
            this.x31Button.CheckedChanged += new System.EventHandler(this.XSizeButton_CheckedChanged);
            // 
            // bConvertSelected
            // 
            this.bConvertSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bConvertSelected.Enabled = false;
            this.bConvertSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bConvertSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bConvertSelected.Location = new System.Drawing.Point(299, 258);
            this.bConvertSelected.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConvertSelected.Name = "bConvertSelected";
            this.bConvertSelected.Size = new System.Drawing.Size(188, 39);
            this.bConvertSelected.TabIndex = 2;
            this.bConvertSelected.Text = "Convert selected maps";
            this.bConvertSelected.UseVisualStyleBackColor = true;
            this.bConvertSelected.Click += new System.EventHandler(this.bConvertSelected_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.Controls.Add(this.cbIgnoreMediaTracker);
            this.splitContainer1.Panel1.Controls.Add(this.cbCutoff);
            this.splitContainer1.Panel1.Controls.Add(this.lvMaps);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.bOpenFolder);
            this.splitContainer1.Panel1.Controls.Add(this.bConvertSelected);
            this.splitContainer1.Panel1.Controls.Add(this.bConvertAll);
            this.splitContainer1.Panel1MinSize = 300;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(690, 508);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 9;
            // 
            // cbIgnoreMediaTracker
            // 
            this.cbIgnoreMediaTracker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIgnoreMediaTracker.AutoSize = true;
            this.cbIgnoreMediaTracker.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbIgnoreMediaTracker.Location = new System.Drawing.Point(550, 232);
            this.cbIgnoreMediaTracker.Name = "cbIgnoreMediaTracker";
            this.cbIgnoreMediaTracker.Size = new System.Drawing.Size(133, 19);
            this.cbIgnoreMediaTracker.TabIndex = 10;
            this.cbIgnoreMediaTracker.Text = "Ignore MediaTracker";
            this.cbIgnoreMediaTracker.UseVisualStyleBackColor = true;
            // 
            // cbCutoff
            // 
            this.cbCutoff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCutoff.AutoSize = true;
            this.cbCutoff.Enabled = false;
            this.cbCutoff.Location = new System.Drawing.Point(14, 232);
            this.cbCutoff.Name = "cbCutoff";
            this.cbCutoff.Size = new System.Drawing.Size(236, 19);
            this.cbCutoff.TabIndex = 10;
            this.cbCutoff.Text = "Remove blocks outside of the map base";
            this.cbCutoff.UseVisualStyleBackColor = true;
            // 
            // lvMaps
            // 
            this.lvMaps.AllowDrop = true;
            this.lvMaps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMaps.HideSelection = false;
            this.lvMaps.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvMaps.LargeImageList = this.ilThumbnails;
            this.lvMaps.Location = new System.Drawing.Point(7, 6);
            this.lvMaps.Name = "lvMaps";
            this.lvMaps.Size = new System.Drawing.Size(679, 98);
            this.lvMaps.TabIndex = 9;
            this.lvMaps.UseCompatibleStateImageBehavior = false;
            this.lvMaps.View = System.Windows.Forms.View.Tile;
            this.lvMaps.SelectedIndexChanged += new System.EventHandler(this.LvMaps_SelectedIndexChanged);
            this.lvMaps.DragDrop += new System.Windows.Forms.DragEventHandler(this.LvMaps_DragDrop);
            this.lvMaps.DragEnter += new System.Windows.Forms.DragEventHandler(this.LvMaps_DragEnter);
            this.lvMaps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvMaps_KeyDown);
            // 
            // ilThumbnails
            // 
            this.ilThumbnails.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilThumbnails.ImageSize = new System.Drawing.Size(64, 64);
            this.ilThumbnails.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 24);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer2.Panel1MinSize = 690;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2MinSize = 260;
            this.splitContainer2.Size = new System.Drawing.Size(960, 508);
            this.splitContainer2.SplitterDistance = 690;
            this.splitContainer2.TabIndex = 10;
            this.splitContainer2.Text = "splitContainer2";
            // 
            // ConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(960, 532);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.ProgramMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ProgramMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(976, 571);
            this.Name = "ConverterForm";
            this.Text = "Island Converter by BigBang1112";
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ProgramMenuStrip.ResumeLayout(false);
            this.ProgramMenuStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbThumbnail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bOpenFolder;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bConvertSelected;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip ProgramMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileItem;
        private System.Windows.Forms.ToolStripMenuItem ViewItem;
        private System.Windows.Forms.ToolStripMenuItem addAMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideMapPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideCommandLineOutputToolStripMenuItem;
        private System.Windows.Forms.RadioButton x45Button;
        private System.Windows.Forms.RadioButton x32Button;
        private System.Windows.Forms.RadioButton x31Button;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeManiaPlanetUserdataLocation;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvMaps;
        private System.Windows.Forms.CheckBox cbCutoff;
        private System.Windows.Forms.ToolStripMenuItem tsmiASuperSecretOption;
        private System.Windows.Forms.ImageList ilThumbnails;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Label lMapName;
        private System.Windows.Forms.Label lSize;
        private System.Windows.Forms.Label lAuthor;
        private System.Windows.Forms.Label lUID;
        private System.Windows.Forms.Label lBlockRange;
        private System.Windows.Forms.Label lMapType;
        private System.Windows.Forms.Label lBronzeM;
        private System.Windows.Forms.Label lSilverM;
        private System.Windows.Forms.Label lGoldM;
        private System.Windows.Forms.Label lAuthorM;
        private System.Windows.Forms.Label lMedals;
        private System.Windows.Forms.Label lDecoration;
        private System.Windows.Forms.Button bConvertAll;
        private System.Windows.Forms.ToolTip ttBlockRange;
        private System.Windows.Forms.CheckBox cbIgnoreMediaTracker;
    }
}