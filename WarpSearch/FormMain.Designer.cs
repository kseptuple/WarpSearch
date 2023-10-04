﻿namespace WarpSearch
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureMap = new System.Windows.Forms.PictureBox();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAosLast = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHodLast = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpenRom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeaprator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHackSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeaprator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMax = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelRoomPointer = new System.Windows.Forms.Label();
            this.comboRoomList = new System.Windows.Forms.ComboBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelSearchLevel = new System.Windows.Forms.Label();
            this.labelSearchOption1 = new System.Windows.Forms.Label();
            this.labelSearchOption2 = new System.Windows.Forms.Label();
            this.labelSearchOption3 = new System.Windows.Forms.Label();
            this.labelSearchOption4 = new System.Windows.Forms.Label();
            this.labelSearchOption5 = new System.Windows.Forms.Label();
            this.labelSearchLevel1 = new System.Windows.Forms.Label();
            this.labelSearchLevel3 = new System.Windows.Forms.Label();
            this.labelSearchLevel2 = new System.Windows.Forms.Label();
            this.labelSearchLevel4 = new System.Windows.Forms.Label();
            this.labelSearchLevel5 = new System.Windows.Forms.Label();
            this.trackBarSearchOption = new System.Windows.Forms.TrackBar();
            this.labelFindSourceTip = new System.Windows.Forms.Label();
            this.listSourceRoom = new System.Windows.Forms.ListBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelFindDestTip = new System.Windows.Forms.Label();
            this.radioButtonFindSource = new System.Windows.Forms.RadioButton();
            this.radioButtonFindDest = new System.Windows.Forms.RadioButton();
            this.trackBarResize = new System.Windows.Forms.TrackBar();
            this.labelScale = new System.Windows.Forms.Label();
            this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
            this.labelSelectedRoom = new System.Windows.Forms.Label();
            this.labelRoomPointerInfo = new System.Windows.Forms.Label();
            this.textRoomPointer = new System.Windows.Forms.TextBox();
            this.textSector = new System.Windows.Forms.TextBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.textRoomId = new System.Windows.Forms.TextBox();
            this.textDestFlag = new System.Windows.Forms.TextBox();
            this.labelDestRoomFlag = new System.Windows.Forms.Label();
            this.labelDestPointerInfo = new System.Windows.Forms.Label();
            this.textDestRoomPointer = new System.Windows.Forms.TextBox();
            this.textSrcRoomPointer = new System.Windows.Forms.TextBox();
            this.labelSourcePointerInfo = new System.Windows.Forms.Label();
            this.labelExitInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listFlag = new System.Windows.Forms.ListBox();
            this.panelFlag = new System.Windows.Forms.Panel();
            this.labelRequiredFlag = new System.Windows.Forms.Label();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusRomType = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripMenuItemOpenCustom = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMap)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.panelMax.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSearchOption)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResize)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelFlag.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureMap
            // 
            this.pictureMap.BackColor = System.Drawing.Color.Black;
            this.pictureMap.Location = new System.Drawing.Point(528, 31);
            this.pictureMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureMap.Name = "pictureMap";
            this.pictureMap.Size = new System.Drawing.Size(875, 599);
            this.pictureMap.TabIndex = 0;
            this.pictureMap.TabStop = false;
            this.pictureMap.Visible = false;
            this.pictureMap.Click += new System.EventHandler(this.PictureMap_Click);
            this.pictureMap.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureMap_Paint);
            this.pictureMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureMap_MouseMove);
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.SystemColors.Control;
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFile,
            this.ToolStripMenuItemSetting});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(1415, 30);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // ToolStripMenuItemFile
            // 
            this.ToolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAosLast,
            this.toolStripMenuItemHodLast,
            this.ToolStripMenuItemOpenRom,
            this.ToolStripMenuItemOpenCustom,
            this.toolStripSeaprator1,
            this.ToolStripMenuItemExit});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(71, 24);
            this.ToolStripMenuItemFile.Text = "文件(&F)";
            // 
            // toolStripMenuItemAosLast
            // 
            this.toolStripMenuItemAosLast.Enabled = false;
            this.toolStripMenuItemAosLast.Name = "toolStripMenuItemAosLast";
            this.toolStripMenuItemAosLast.ShortcutKeyDisplayString = "";
            this.toolStripMenuItemAosLast.Size = new System.Drawing.Size(256, 26);
            this.toolStripMenuItemAosLast.Text = "打开上次的晓月ROM(&A)";
            this.toolStripMenuItemAosLast.Click += new System.EventHandler(this.ToolStripMenuItemAosLast_Click);
            // 
            // toolStripMenuItemHodLast
            // 
            this.toolStripMenuItemHodLast.Enabled = false;
            this.toolStripMenuItemHodLast.Name = "toolStripMenuItemHodLast";
            this.toolStripMenuItemHodLast.Size = new System.Drawing.Size(256, 26);
            this.toolStripMenuItemHodLast.Text = "打开上次的白夜ROM(&H)";
            this.toolStripMenuItemHodLast.Click += new System.EventHandler(this.ToolStripMenuItemHodLast_Click);
            // 
            // ToolStripMenuItemOpenRom
            // 
            this.ToolStripMenuItemOpenRom.Name = "ToolStripMenuItemOpenRom";
            this.ToolStripMenuItemOpenRom.Size = new System.Drawing.Size(256, 26);
            this.ToolStripMenuItemOpenRom.Text = "打开ROM...";
            this.ToolStripMenuItemOpenRom.Click += new System.EventHandler(this.ToolStripMenuItemOpenRom_Click);
            // 
            // toolStripSeaprator1
            // 
            this.toolStripSeaprator1.Name = "toolStripSeaprator1";
            this.toolStripSeaprator1.Size = new System.Drawing.Size(253, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(256, 26);
            this.ToolStripMenuItemExit.Text = "退出(&X)";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // ToolStripMenuItemSetting
            // 
            this.ToolStripMenuItemSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemHackSupport,
            this.toolStripSeaprator2,
            this.ToolStripMenuItemLanguage});
            this.ToolStripMenuItemSetting.Name = "ToolStripMenuItemSetting";
            this.ToolStripMenuItemSetting.Size = new System.Drawing.Size(72, 24);
            this.ToolStripMenuItemSetting.Text = "设置(&S)";
            // 
            // ToolStripMenuItemHackSupport
            // 
            this.ToolStripMenuItemHackSupport.Checked = true;
            this.ToolStripMenuItemHackSupport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItemHackSupport.Name = "ToolStripMenuItemHackSupport";
            this.ToolStripMenuItemHackSupport.Size = new System.Drawing.Size(210, 26);
            this.ToolStripMenuItemHackSupport.Text = "启用Hack支持(&H)";
            this.ToolStripMenuItemHackSupport.Click += new System.EventHandler(this.ToolStripMenuItemHackSupport_Click);
            // 
            // toolStripSeaprator2
            // 
            this.toolStripSeaprator2.Name = "toolStripSeaprator2";
            this.toolStripSeaprator2.Size = new System.Drawing.Size(207, 6);
            this.toolStripSeaprator2.Visible = false;
            // 
            // ToolStripMenuItemLanguage
            // 
            this.ToolStripMenuItemLanguage.Name = "ToolStripMenuItemLanguage";
            this.ToolStripMenuItemLanguage.Size = new System.Drawing.Size(210, 26);
            this.ToolStripMenuItemLanguage.Text = "语言(&L)";
            // 
            // panelMax
            // 
            this.panelMax.Controls.Add(this.tableLayoutPanel2);
            this.panelMax.Controls.Add(this.panelBottom);
            this.panelMax.Controls.Add(this.panelTop);
            this.panelMax.Controls.Add(this.radioButtonFindSource);
            this.panelMax.Controls.Add(this.radioButtonFindDest);
            this.panelMax.Location = new System.Drawing.Point(12, 31);
            this.panelMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMax.Name = "panelMax";
            this.panelMax.Size = new System.Drawing.Size(505, 402);
            this.panelMax.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.labelRoomPointer, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboRoomList, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(499, 28);
            this.tableLayoutPanel2.TabIndex = 23;
            // 
            // labelRoomPointer
            // 
            this.labelRoomPointer.AutoSize = true;
            this.labelRoomPointer.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelRoomPointer.Location = new System.Drawing.Point(3, 0);
            this.labelRoomPointer.Name = "labelRoomPointer";
            this.labelRoomPointer.Size = new System.Drawing.Size(82, 28);
            this.labelRoomPointer.TabIndex = 22;
            this.labelRoomPointer.Text = "房间指针：";
            this.labelRoomPointer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboRoomList
            // 
            this.comboRoomList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRoomList.FormattingEnabled = true;
            this.comboRoomList.Location = new System.Drawing.Point(91, 2);
            this.comboRoomList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboRoomList.Name = "comboRoomList";
            this.comboRoomList.Size = new System.Drawing.Size(247, 23);
            this.comboRoomList.TabIndex = 10;
            this.comboRoomList.SelectedIndexChanged += new System.EventHandler(this.ComboRoomList_SelectedIndexChanged);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelSearchLevel);
            this.panelBottom.Controls.Add(this.labelSearchOption1);
            this.panelBottom.Controls.Add(this.labelSearchOption2);
            this.panelBottom.Controls.Add(this.labelSearchOption3);
            this.panelBottom.Controls.Add(this.labelSearchOption4);
            this.panelBottom.Controls.Add(this.labelSearchOption5);
            this.panelBottom.Controls.Add(this.labelSearchLevel1);
            this.panelBottom.Controls.Add(this.labelSearchLevel3);
            this.panelBottom.Controls.Add(this.labelSearchLevel2);
            this.panelBottom.Controls.Add(this.labelSearchLevel4);
            this.panelBottom.Controls.Add(this.labelSearchLevel5);
            this.panelBottom.Controls.Add(this.trackBarSearchOption);
            this.panelBottom.Controls.Add(this.labelFindSourceTip);
            this.panelBottom.Controls.Add(this.listSourceRoom);
            this.panelBottom.Enabled = false;
            this.panelBottom.Location = new System.Drawing.Point(9, 130);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(496, 268);
            this.panelBottom.TabIndex = 9;
            // 
            // labelSearchLevel
            // 
            this.labelSearchLevel.AutoSize = true;
            this.labelSearchLevel.Location = new System.Drawing.Point(132, 48);
            this.labelSearchLevel.Name = "labelSearchLevel";
            this.labelSearchLevel.Size = new System.Drawing.Size(82, 15);
            this.labelSearchLevel.TabIndex = 19;
            this.labelSearchLevel.Text = "搜索等级：";
            // 
            // labelSearchOption1
            // 
            this.labelSearchOption1.AutoSize = true;
            this.labelSearchOption1.Location = new System.Drawing.Point(132, 224);
            this.labelSearchOption1.Name = "labelSearchOption1";
            this.labelSearchOption1.Size = new System.Drawing.Size(22, 15);
            this.labelSearchOption1.TabIndex = 18;
            this.labelSearchOption1.Text = "✔";
            this.labelSearchOption1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchOption2
            // 
            this.labelSearchOption2.AutoSize = true;
            this.labelSearchOption2.Location = new System.Drawing.Point(132, 188);
            this.labelSearchOption2.Name = "labelSearchOption2";
            this.labelSearchOption2.Size = new System.Drawing.Size(22, 15);
            this.labelSearchOption2.TabIndex = 17;
            this.labelSearchOption2.Text = "✔";
            this.labelSearchOption2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchOption3
            // 
            this.labelSearchOption3.AutoSize = true;
            this.labelSearchOption3.Location = new System.Drawing.Point(132, 150);
            this.labelSearchOption3.Name = "labelSearchOption3";
            this.labelSearchOption3.Size = new System.Drawing.Size(22, 15);
            this.labelSearchOption3.TabIndex = 16;
            this.labelSearchOption3.Text = "✔";
            this.labelSearchOption3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchOption4
            // 
            this.labelSearchOption4.AutoSize = true;
            this.labelSearchOption4.Location = new System.Drawing.Point(132, 112);
            this.labelSearchOption4.Name = "labelSearchOption4";
            this.labelSearchOption4.Size = new System.Drawing.Size(22, 15);
            this.labelSearchOption4.TabIndex = 15;
            this.labelSearchOption4.Text = "✔";
            this.labelSearchOption4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchOption5
            // 
            this.labelSearchOption5.AutoSize = true;
            this.labelSearchOption5.Location = new System.Drawing.Point(132, 76);
            this.labelSearchOption5.Name = "labelSearchOption5";
            this.labelSearchOption5.Size = new System.Drawing.Size(22, 15);
            this.labelSearchOption5.TabIndex = 14;
            this.labelSearchOption5.Text = "✖";
            this.labelSearchOption5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchLevel1
            // 
            this.labelSearchLevel1.Location = new System.Drawing.Point(160, 224);
            this.labelSearchLevel1.Name = "labelSearchLevel1";
            this.labelSearchLevel1.Size = new System.Drawing.Size(333, 38);
            this.labelSearchLevel1.TabIndex = 13;
            this.labelSearchLevel1.Text = "搜索红门传送（仅白夜）";
            // 
            // labelSearchLevel3
            // 
            this.labelSearchLevel3.Location = new System.Drawing.Point(160, 150);
            this.labelSearchLevel3.Name = "labelSearchLevel3";
            this.labelSearchLevel3.Size = new System.Drawing.Size(333, 38);
            this.labelSearchLevel3.TabIndex = 12;
            this.labelSearchLevel3.Text = "搜索一格宽房间右边第二格（仅白夜）";
            // 
            // labelSearchLevel2
            // 
            this.labelSearchLevel2.Location = new System.Drawing.Point(160, 188);
            this.labelSearchLevel2.Name = "labelSearchLevel2";
            this.labelSearchLevel2.Size = new System.Drawing.Size(333, 38);
            this.labelSearchLevel2.TabIndex = 11;
            this.labelSearchLevel2.Text = "搜索斜角换版";
            // 
            // labelSearchLevel4
            // 
            this.labelSearchLevel4.Location = new System.Drawing.Point(160, 112);
            this.labelSearchLevel4.Name = "labelSearchLevel4";
            this.labelSearchLevel4.Size = new System.Drawing.Size(333, 38);
            this.labelSearchLevel4.TabIndex = 10;
            this.labelSearchLevel4.Text = "搜索远距离换版";
            // 
            // labelSearchLevel5
            // 
            this.labelSearchLevel5.Location = new System.Drawing.Point(160, 76);
            this.labelSearchLevel5.Name = "labelSearchLevel5";
            this.labelSearchLevel5.Size = new System.Drawing.Size(333, 38);
            this.labelSearchLevel5.TabIndex = 9;
            this.labelSearchLevel5.Text = "搜索非正常房间内换版";
            // 
            // trackBarSearchOption
            // 
            this.trackBarSearchOption.AutoSize = false;
            this.trackBarSearchOption.LargeChange = 1;
            this.trackBarSearchOption.Location = new System.Drawing.Point(241, 42);
            this.trackBarSearchOption.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarSearchOption.Maximum = 5;
            this.trackBarSearchOption.Name = "trackBarSearchOption";
            this.trackBarSearchOption.Size = new System.Drawing.Size(252, 30);
            this.trackBarSearchOption.TabIndex = 8;
            this.trackBarSearchOption.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSearchOption.Value = 4;
            this.trackBarSearchOption.Scroll += new System.EventHandler(this.trackBarSearchOption_Scroll);
            // 
            // labelFindSourceTip
            // 
            this.labelFindSourceTip.Location = new System.Drawing.Point(129, 2);
            this.labelFindSourceTip.Name = "labelFindSourceTip";
            this.labelFindSourceTip.Size = new System.Drawing.Size(364, 38);
            this.labelFindSourceTip.TabIndex = 7;
            this.labelFindSourceTip.Text = "右键点击选择目标房间，左键点击选择起始房间";
            // 
            // listSourceRoom
            // 
            this.listSourceRoom.FormattingEnabled = true;
            this.listSourceRoom.ItemHeight = 15;
            this.listSourceRoom.Location = new System.Drawing.Point(5, 2);
            this.listSourceRoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listSourceRoom.Name = "listSourceRoom";
            this.listSourceRoom.Size = new System.Drawing.Size(120, 244);
            this.listSourceRoom.TabIndex = 0;
            this.listSourceRoom.SelectedIndexChanged += new System.EventHandler(this.ListSourceRoom_SelectedIndexChanged);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelFindDestTip);
            this.panelTop.Location = new System.Drawing.Point(9, 58);
            this.panelTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(496, 42);
            this.panelTop.TabIndex = 7;
            // 
            // labelFindDestTip
            // 
            this.labelFindDestTip.Location = new System.Drawing.Point(-3, 0);
            this.labelFindDestTip.Name = "labelFindDestTip";
            this.labelFindDestTip.Size = new System.Drawing.Size(496, 38);
            this.labelFindDestTip.TabIndex = 0;
            this.labelFindDestTip.Text = "右键点击选择房间，左键点击选择换版位置";
            // 
            // radioButtonFindSource
            // 
            this.radioButtonFindSource.AutoSize = true;
            this.radioButtonFindSource.Location = new System.Drawing.Point(9, 105);
            this.radioButtonFindSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonFindSource.Name = "radioButtonFindSource";
            this.radioButtonFindSource.Size = new System.Drawing.Size(178, 19);
            this.radioButtonFindSource.TabIndex = 6;
            this.radioButtonFindSource.TabStop = true;
            this.radioButtonFindSource.Text = "从哪里出城可以到这里";
            this.radioButtonFindSource.UseVisualStyleBackColor = true;
            this.radioButtonFindSource.CheckedChanged += new System.EventHandler(this.RadioButtonFindSource_CheckedChanged);
            // 
            // radioButtonFindDest
            // 
            this.radioButtonFindDest.AutoSize = true;
            this.radioButtonFindDest.Checked = true;
            this.radioButtonFindDest.Location = new System.Drawing.Point(9, 32);
            this.radioButtonFindDest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonFindDest.Name = "radioButtonFindDest";
            this.radioButtonFindDest.Size = new System.Drawing.Size(178, 19);
            this.radioButtonFindDest.TabIndex = 5;
            this.radioButtonFindDest.TabStop = true;
            this.radioButtonFindDest.Text = "从这里出城可以到哪里";
            this.radioButtonFindDest.UseVisualStyleBackColor = true;
            this.radioButtonFindDest.CheckedChanged += new System.EventHandler(this.RadioButtonFindDest_CheckedChanged);
            // 
            // trackBarResize
            // 
            this.trackBarResize.AutoSize = false;
            this.trackBarResize.LargeChange = 50;
            this.trackBarResize.Location = new System.Drawing.Point(149, 438);
            this.trackBarResize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarResize.Maximum = 400;
            this.trackBarResize.Minimum = 100;
            this.trackBarResize.Name = "trackBarResize";
            this.trackBarResize.Size = new System.Drawing.Size(364, 30);
            this.trackBarResize.SmallChange = 5;
            this.trackBarResize.TabIndex = 6;
            this.trackBarResize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarResize.Value = 300;
            this.trackBarResize.Scroll += new System.EventHandler(this.TrackBarResize_Scroll);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(12, 444);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(52, 15);
            this.labelScale.TabIndex = 7;
            this.labelScale.Text = "缩放：";
            // 
            // openFileDialogMain
            // 
            this.openFileDialogMain.Filter = "GBA 文件|*.gba";
            // 
            // labelSelectedRoom
            // 
            this.labelSelectedRoom.AutoSize = true;
            this.labelSelectedRoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSelectedRoom.Location = new System.Drawing.Point(3, 0);
            this.labelSelectedRoom.Name = "labelSelectedRoom";
            this.labelSelectedRoom.Size = new System.Drawing.Size(112, 30);
            this.labelSelectedRoom.TabIndex = 8;
            this.labelSelectedRoom.Text = "选中房间信息：";
            this.labelSelectedRoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRoomPointerInfo
            // 
            this.labelRoomPointerInfo.AutoSize = true;
            this.labelRoomPointerInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelRoomPointerInfo.Location = new System.Drawing.Point(3, 30);
            this.labelRoomPointerInfo.Name = "labelRoomPointerInfo";
            this.labelRoomPointerInfo.Size = new System.Drawing.Size(82, 30);
            this.labelRoomPointerInfo.TabIndex = 9;
            this.labelRoomPointerInfo.Text = "房间指针：";
            this.labelRoomPointerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textRoomPointer
            // 
            this.textRoomPointer.Location = new System.Drawing.Point(138, 32);
            this.textRoomPointer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textRoomPointer.Name = "textRoomPointer";
            this.textRoomPointer.ReadOnly = true;
            this.textRoomPointer.Size = new System.Drawing.Size(100, 25);
            this.textRoomPointer.TabIndex = 10;
            // 
            // textSector
            // 
            this.textSector.Location = new System.Drawing.Point(138, 62);
            this.textSector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textSector.Name = "textSector";
            this.textSector.ReadOnly = true;
            this.textSector.Size = new System.Drawing.Size(100, 25);
            this.textSector.TabIndex = 11;
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelArea.Location = new System.Drawing.Point(3, 60);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(52, 30);
            this.labelArea.TabIndex = 12;
            this.labelArea.Text = "区域：";
            this.labelArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelNumber.Location = new System.Drawing.Point(3, 90);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(52, 30);
            this.labelNumber.TabIndex = 13;
            this.labelNumber.Text = "序号：";
            this.labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textRoomId
            // 
            this.textRoomId.Location = new System.Drawing.Point(138, 92);
            this.textRoomId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textRoomId.Name = "textRoomId";
            this.textRoomId.ReadOnly = true;
            this.textRoomId.Size = new System.Drawing.Size(100, 25);
            this.textRoomId.TabIndex = 14;
            // 
            // textDestFlag
            // 
            this.textDestFlag.Location = new System.Drawing.Point(138, 212);
            this.textDestFlag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textDestFlag.Name = "textDestFlag";
            this.textDestFlag.ReadOnly = true;
            this.textDestFlag.Size = new System.Drawing.Size(100, 25);
            this.textDestFlag.TabIndex = 21;
            // 
            // labelDestRoomFlag
            // 
            this.labelDestRoomFlag.AutoSize = true;
            this.labelDestRoomFlag.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDestRoomFlag.Location = new System.Drawing.Point(3, 210);
            this.labelDestRoomFlag.Name = "labelDestRoomFlag";
            this.labelDestRoomFlag.Size = new System.Drawing.Size(129, 34);
            this.labelDestRoomFlag.TabIndex = 20;
            this.labelDestRoomFlag.Text = "目标房间的flag：";
            this.labelDestRoomFlag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDestPointerInfo
            // 
            this.labelDestPointerInfo.AutoSize = true;
            this.labelDestPointerInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDestPointerInfo.Location = new System.Drawing.Point(3, 180);
            this.labelDestPointerInfo.Name = "labelDestPointerInfo";
            this.labelDestPointerInfo.Size = new System.Drawing.Size(112, 30);
            this.labelDestPointerInfo.TabIndex = 19;
            this.labelDestPointerInfo.Text = "目标房间指针：";
            this.labelDestPointerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textDestRoomPointer
            // 
            this.textDestRoomPointer.Location = new System.Drawing.Point(138, 182);
            this.textDestRoomPointer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textDestRoomPointer.Name = "textDestRoomPointer";
            this.textDestRoomPointer.ReadOnly = true;
            this.textDestRoomPointer.Size = new System.Drawing.Size(100, 25);
            this.textDestRoomPointer.TabIndex = 18;
            // 
            // textSrcRoomPointer
            // 
            this.textSrcRoomPointer.Location = new System.Drawing.Point(138, 152);
            this.textSrcRoomPointer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textSrcRoomPointer.Name = "textSrcRoomPointer";
            this.textSrcRoomPointer.ReadOnly = true;
            this.textSrcRoomPointer.Size = new System.Drawing.Size(100, 25);
            this.textSrcRoomPointer.TabIndex = 17;
            // 
            // labelSourcePointerInfo
            // 
            this.labelSourcePointerInfo.AutoSize = true;
            this.labelSourcePointerInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSourcePointerInfo.Location = new System.Drawing.Point(3, 150);
            this.labelSourcePointerInfo.Name = "labelSourcePointerInfo";
            this.labelSourcePointerInfo.Size = new System.Drawing.Size(97, 30);
            this.labelSourcePointerInfo.TabIndex = 16;
            this.labelSourcePointerInfo.Text = "源房间指针：";
            this.labelSourcePointerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelExitInfo
            // 
            this.labelExitInfo.AutoSize = true;
            this.labelExitInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelExitInfo.Location = new System.Drawing.Point(3, 120);
            this.labelExitInfo.Name = "labelExitInfo";
            this.labelExitInfo.Size = new System.Drawing.Size(82, 30);
            this.labelExitInfo.TabIndex = 15;
            this.labelExitInfo.Text = "换版信息：";
            this.labelExitInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelNumber, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textSrcRoomPointer, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.textDestRoomPointer, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.textDestFlag, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.textRoomId, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDestRoomFlag, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.textSector, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDestPointerInfo, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.textRoomPointer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelSourcePointerInfo, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelSelectedRoom, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelExitInfo, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelArea, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRoomPointerInfo, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 472);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(317, 244);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // listFlag
            // 
            this.listFlag.FormattingEnabled = true;
            this.listFlag.ItemHeight = 15;
            this.listFlag.Location = new System.Drawing.Point(3, 25);
            this.listFlag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listFlag.Name = "listFlag";
            this.listFlag.Size = new System.Drawing.Size(162, 214);
            this.listFlag.TabIndex = 23;
            // 
            // panelFlag
            // 
            this.panelFlag.Controls.Add(this.labelRequiredFlag);
            this.panelFlag.Controls.Add(this.listFlag);
            this.panelFlag.Location = new System.Drawing.Point(335, 472);
            this.panelFlag.Name = "panelFlag";
            this.panelFlag.Size = new System.Drawing.Size(168, 244);
            this.panelFlag.TabIndex = 24;
            this.panelFlag.Visible = false;
            // 
            // labelRequiredFlag
            // 
            this.labelRequiredFlag.AutoSize = true;
            this.labelRequiredFlag.Location = new System.Drawing.Point(5, 8);
            this.labelRequiredFlag.Name = "labelRequiredFlag";
            this.labelRequiredFlag.Size = new System.Drawing.Size(99, 15);
            this.labelRequiredFlag.TabIndex = 24;
            this.labelRequiredFlag.Text = "需要的flag：";
            this.labelRequiredFlag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusRomType});
            this.statusStripMain.Location = new System.Drawing.Point(0, 828);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1415, 26);
            this.statusStripMain.TabIndex = 25;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusRomType
            // 
            this.toolStripStatusRomType.IsLink = true;
            this.toolStripStatusRomType.LinkColor = System.Drawing.Color.Blue;
            this.toolStripStatusRomType.Name = "toolStripStatusRomType";
            this.toolStripStatusRomType.Size = new System.Drawing.Size(189, 20);
            this.toolStripStatusRomType.Text = "toolStripStatusRomType";
            this.toolStripStatusRomType.VisitedLinkColor = System.Drawing.Color.Blue;
            this.toolStripStatusRomType.Click += new System.EventHandler(this.toolStripStatusRomType_Click);
            // 
            // ToolStripMenuItemOpenCustom
            // 
            this.ToolStripMenuItemOpenCustom.Name = "ToolStripMenuItemOpenCustom";
            this.ToolStripMenuItemOpenCustom.Size = new System.Drawing.Size(256, 26);
            this.ToolStripMenuItemOpenCustom.Text = "打开自定义ROM...";
            this.ToolStripMenuItemOpenCustom.Click += new System.EventHandler(this.ToolStripMenuItemOpenCustom_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 854);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelFlag);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.trackBarResize);
            this.Controls.Add(this.panelMax);
            this.Controls.Add(this.pictureMap);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.Text = "晓月/白夜出城搜索器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureMap)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.panelMax.ResumeLayout(false);
            this.panelMax.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSearchOption)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResize)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelFlag.ResumeLayout(false);
            this.panelFlag.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureMap;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeaprator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.Panel panelMax;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.RadioButton radioButtonFindSource;
        private System.Windows.Forms.RadioButton radioButtonFindDest;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.TrackBar trackBarResize;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.OpenFileDialog openFileDialogMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAosLast;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHodLast;
        private System.Windows.Forms.Label labelFindDestTip;
        private System.Windows.Forms.Label labelSelectedRoom;
        private System.Windows.Forms.Label labelRoomPointerInfo;
        private System.Windows.Forms.TextBox textRoomPointer;
        private System.Windows.Forms.TextBox textSector;
        private System.Windows.Forms.TextBox textRoomId;
        private System.Windows.Forms.TextBox textDestFlag;
        private System.Windows.Forms.Label labelDestRoomFlag;
        private System.Windows.Forms.Label labelDestPointerInfo;
        private System.Windows.Forms.TextBox textDestRoomPointer;
        private System.Windows.Forms.TextBox textSrcRoomPointer;
        private System.Windows.Forms.Label labelSourcePointerInfo;
        private System.Windows.Forms.Label labelExitInfo;
        private System.Windows.Forms.ComboBox comboRoomList;
        private System.Windows.Forms.Label labelRoomPointer;
        private System.Windows.Forms.ListBox listSourceRoom;
        private System.Windows.Forms.Label labelFindSourceTip;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpenRom;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSetting;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHackSupport;
        private System.Windows.Forms.TrackBar trackBarSearchOption;
        private System.Windows.Forms.Label labelSearchLevel1;
        private System.Windows.Forms.Label labelSearchLevel3;
        private System.Windows.Forms.Label labelSearchLevel2;
        private System.Windows.Forms.Label labelSearchLevel4;
        private System.Windows.Forms.Label labelSearchOption1;
        private System.Windows.Forms.Label labelSearchOption2;
        private System.Windows.Forms.Label labelSearchOption3;
        private System.Windows.Forms.Label labelSearchOption4;
        private System.Windows.Forms.Label labelSearchOption5;
        private System.Windows.Forms.Label labelSearchLevel;
        private System.Windows.Forms.Label labelSearchLevel5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeaprator2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListBox listFlag;
        private System.Windows.Forms.Panel panelFlag;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLanguage;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.Label labelRequiredFlag;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusRomType;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpenCustom;
    }
}

