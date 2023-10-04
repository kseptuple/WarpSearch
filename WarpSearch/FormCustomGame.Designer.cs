namespace WarpSearch
{
    partial class FormCustomGame
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
            this.radioButtonHoDU = new System.Windows.Forms.RadioButton();
            this.radioButtonHoDJ = new System.Windows.Forms.RadioButton();
            this.radioButtonAoSU = new System.Windows.Forms.RadioButton();
            this.radioButtonAoSJ = new System.Windows.Forms.RadioButton();
            this.textBoxRoomPointer = new System.Windows.Forms.TextBox();
            this.textBoxMapPointer = new System.Windows.Forms.TextBox();
            this.textBoxLinePointer = new System.Windows.Forms.TextBox();
            this.labelRoomPointerAddr = new System.Windows.Forms.Label();
            this.labelMapGridAddr = new System.Windows.Forms.Label();
            this.labelMapLineAddr = new System.Windows.Forms.Label();
            this.groupBoxCustomize = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonHoDE = new System.Windows.Forms.RadioButton();
            this.radioButtonAoSE = new System.Windows.Forms.RadioButton();
            this.labelRomIs = new System.Windows.Forms.Label();
            this.labelCustomizeTip = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSaveSetting = new System.Windows.Forms.CheckBox();
            this.labelSaveSettingTip = new System.Windows.Forms.Label();
            this.groupBoxCustomize.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonHoDU
            // 
            this.radioButtonHoDU.AutoSize = true;
            this.radioButtonHoDU.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonHoDU.Location = new System.Drawing.Point(3, 3);
            this.radioButtonHoDU.Name = "radioButtonHoDU";
            this.radioButtonHoDU.Size = new System.Drawing.Size(88, 22);
            this.radioButtonHoDU.TabIndex = 0;
            this.radioButtonHoDU.TabStop = true;
            this.radioButtonHoDU.Text = "白夜美版";
            this.radioButtonHoDU.UseVisualStyleBackColor = true;
            this.radioButtonHoDU.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // radioButtonHoDJ
            // 
            this.radioButtonHoDJ.AutoSize = true;
            this.radioButtonHoDJ.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonHoDJ.Location = new System.Drawing.Point(263, 3);
            this.radioButtonHoDJ.Name = "radioButtonHoDJ";
            this.radioButtonHoDJ.Size = new System.Drawing.Size(88, 22);
            this.radioButtonHoDJ.TabIndex = 1;
            this.radioButtonHoDJ.TabStop = true;
            this.radioButtonHoDJ.Text = "白夜日版";
            this.radioButtonHoDJ.UseVisualStyleBackColor = true;
            this.radioButtonHoDJ.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // radioButtonAoSU
            // 
            this.radioButtonAoSU.AutoSize = true;
            this.radioButtonAoSU.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonAoSU.Location = new System.Drawing.Point(3, 31);
            this.radioButtonAoSU.Name = "radioButtonAoSU";
            this.radioButtonAoSU.Size = new System.Drawing.Size(88, 22);
            this.radioButtonAoSU.TabIndex = 2;
            this.radioButtonAoSU.TabStop = true;
            this.radioButtonAoSU.Text = "晓月美版";
            this.radioButtonAoSU.UseVisualStyleBackColor = true;
            this.radioButtonAoSU.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // radioButtonAoSJ
            // 
            this.radioButtonAoSJ.AutoSize = true;
            this.radioButtonAoSJ.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonAoSJ.Location = new System.Drawing.Point(263, 31);
            this.radioButtonAoSJ.Name = "radioButtonAoSJ";
            this.radioButtonAoSJ.Size = new System.Drawing.Size(88, 22);
            this.radioButtonAoSJ.TabIndex = 3;
            this.radioButtonAoSJ.TabStop = true;
            this.radioButtonAoSJ.Text = "晓月日版";
            this.radioButtonAoSJ.UseVisualStyleBackColor = true;
            this.radioButtonAoSJ.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // textBoxRoomPointer
            // 
            this.textBoxRoomPointer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxRoomPointer.Location = new System.Drawing.Point(261, 18);
            this.textBoxRoomPointer.Name = "textBoxRoomPointer";
            this.textBoxRoomPointer.Size = new System.Drawing.Size(116, 25);
            this.textBoxRoomPointer.TabIndex = 6;
            this.textBoxRoomPointer.Validated += new System.EventHandler(this.textBoxRoomPointer_Validated);
            // 
            // textBoxMapPointer
            // 
            this.textBoxMapPointer.Location = new System.Drawing.Point(261, 49);
            this.textBoxMapPointer.Name = "textBoxMapPointer";
            this.textBoxMapPointer.Size = new System.Drawing.Size(116, 25);
            this.textBoxMapPointer.TabIndex = 7;
            this.textBoxMapPointer.Validated += new System.EventHandler(this.textBoxMapPointer_Validated);
            // 
            // textBoxLinePointer
            // 
            this.textBoxLinePointer.Location = new System.Drawing.Point(261, 80);
            this.textBoxLinePointer.Name = "textBoxLinePointer";
            this.textBoxLinePointer.Size = new System.Drawing.Size(116, 25);
            this.textBoxLinePointer.TabIndex = 8;
            this.textBoxLinePointer.Validated += new System.EventHandler(this.textBoxLinePointer_Validated);
            // 
            // labelRoomPointerAddr
            // 
            this.labelRoomPointerAddr.AutoSize = true;
            this.labelRoomPointerAddr.Location = new System.Drawing.Point(6, 21);
            this.labelRoomPointerAddr.Name = "labelRoomPointerAddr";
            this.labelRoomPointerAddr.Size = new System.Drawing.Size(157, 15);
            this.labelRoomPointerAddr.TabIndex = 10;
            this.labelRoomPointerAddr.Text = "房间列表的指针地址：";
            // 
            // labelMapGridAddr
            // 
            this.labelMapGridAddr.AutoSize = true;
            this.labelMapGridAddr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMapGridAddr.Location = new System.Drawing.Point(6, 52);
            this.labelMapGridAddr.Name = "labelMapGridAddr";
            this.labelMapGridAddr.Size = new System.Drawing.Size(112, 15);
            this.labelMapGridAddr.TabIndex = 11;
            this.labelMapGridAddr.Text = "地图格子地址：";
            // 
            // labelMapLineAddr
            // 
            this.labelMapLineAddr.AutoSize = true;
            this.labelMapLineAddr.Location = new System.Drawing.Point(6, 83);
            this.labelMapLineAddr.Name = "labelMapLineAddr";
            this.labelMapLineAddr.Size = new System.Drawing.Size(112, 15);
            this.labelMapLineAddr.TabIndex = 12;
            this.labelMapLineAddr.Text = "地图框线地址：";
            // 
            // groupBoxCustomize
            // 
            this.groupBoxCustomize.Controls.Add(this.labelMapLineAddr);
            this.groupBoxCustomize.Controls.Add(this.labelMapGridAddr);
            this.groupBoxCustomize.Controls.Add(this.textBoxRoomPointer);
            this.groupBoxCustomize.Controls.Add(this.labelRoomPointerAddr);
            this.groupBoxCustomize.Controls.Add(this.textBoxMapPointer);
            this.groupBoxCustomize.Controls.Add(this.textBoxLinePointer);
            this.groupBoxCustomize.Location = new System.Drawing.Point(12, 104);
            this.groupBoxCustomize.Name = "groupBoxCustomize";
            this.groupBoxCustomize.Size = new System.Drawing.Size(781, 114);
            this.groupBoxCustomize.TabIndex = 13;
            this.groupBoxCustomize.TabStop = false;
            this.groupBoxCustomize.Text = "自定义参数";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.radioButtonHoDU, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonHoDJ, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonAoSU, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonHoDE, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonAoSE, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonAoSJ, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(781, 56);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // radioButtonHoDE
            // 
            this.radioButtonHoDE.AutoSize = true;
            this.radioButtonHoDE.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonHoDE.Location = new System.Drawing.Point(523, 3);
            this.radioButtonHoDE.Name = "radioButtonHoDE";
            this.radioButtonHoDE.Size = new System.Drawing.Size(88, 22);
            this.radioButtonHoDE.TabIndex = 4;
            this.radioButtonHoDE.TabStop = true;
            this.radioButtonHoDE.Text = "白夜欧版";
            this.radioButtonHoDE.UseVisualStyleBackColor = true;
            this.radioButtonHoDE.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // radioButtonAoSE
            // 
            this.radioButtonAoSE.AutoSize = true;
            this.radioButtonAoSE.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonAoSE.Location = new System.Drawing.Point(523, 31);
            this.radioButtonAoSE.Name = "radioButtonAoSE";
            this.radioButtonAoSE.Size = new System.Drawing.Size(88, 22);
            this.radioButtonAoSE.TabIndex = 5;
            this.radioButtonAoSE.TabStop = true;
            this.radioButtonAoSE.Text = "晓月欧版";
            this.radioButtonAoSE.UseVisualStyleBackColor = true;
            this.radioButtonAoSE.CheckedChanged += new System.EventHandler(this.radioButtonGame_CheckedChanged);
            // 
            // labelRomIs
            // 
            this.labelRomIs.AutoSize = true;
            this.labelRomIs.Location = new System.Drawing.Point(12, 9);
            this.labelRomIs.Name = "labelRomIs";
            this.labelRomIs.Size = new System.Drawing.Size(91, 15);
            this.labelRomIs.TabIndex = 14;
            this.labelRomIs.Text = "这个ROM是：";
            // 
            // labelCustomizeTip
            // 
            this.labelCustomizeTip.AutoSize = true;
            this.labelCustomizeTip.Location = new System.Drawing.Point(12, 86);
            this.labelCustomizeTip.Name = "labelCustomizeTip";
            this.labelCustomizeTip.Size = new System.Drawing.Size(414, 15);
            this.labelCustomizeTip.TabIndex = 15;
            this.labelCustomizeTip.Text = "部分Hack修改了指针地址，如果你知道的话，可以手动修改：";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOK.Location = new System.Drawing.Point(135, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(120, 34);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.Location = new System.Drawing.Point(525, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(120, 34);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOK, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 264);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(781, 40);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // checkBoxSaveSetting
            // 
            this.checkBoxSaveSetting.AutoSize = true;
            this.checkBoxSaveSetting.Checked = true;
            this.checkBoxSaveSetting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSaveSetting.Location = new System.Drawing.Point(15, 224);
            this.checkBoxSaveSetting.Name = "checkBoxSaveSetting";
            this.checkBoxSaveSetting.Size = new System.Drawing.Size(158, 19);
            this.checkBoxSaveSetting.TabIndex = 19;
            this.checkBoxSaveSetting.Text = "记住这个ROM的设置";
            this.checkBoxSaveSetting.UseVisualStyleBackColor = true;
            // 
            // labelSaveSettingTip
            // 
            this.labelSaveSettingTip.AutoSize = true;
            this.labelSaveSettingTip.Location = new System.Drawing.Point(12, 246);
            this.labelSaveSettingTip.Name = "labelSaveSettingTip";
            this.labelSaveSettingTip.Size = new System.Drawing.Size(183, 15);
            this.labelSaveSettingTip.TabIndex = 20;
            this.labelSaveSettingTip.Text = "最多记录1000个ROM的设置";
            // 
            // FormCustomGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 314);
            this.Controls.Add(this.labelSaveSettingTip);
            this.Controls.Add(this.checkBoxSaveSetting);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.labelCustomizeTip);
            this.Controls.Add(this.labelRomIs);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBoxCustomize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomGame";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "打开自定义ROM";
            this.groupBoxCustomize.ResumeLayout(false);
            this.groupBoxCustomize.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonHoDU;
        private System.Windows.Forms.RadioButton radioButtonHoDJ;
        private System.Windows.Forms.RadioButton radioButtonAoSU;
        private System.Windows.Forms.RadioButton radioButtonAoSJ;
        private System.Windows.Forms.TextBox textBoxRoomPointer;
        private System.Windows.Forms.TextBox textBoxMapPointer;
        private System.Windows.Forms.TextBox textBoxLinePointer;
        private System.Windows.Forms.Label labelRoomPointerAddr;
        private System.Windows.Forms.Label labelMapGridAddr;
        private System.Windows.Forms.Label labelMapLineAddr;
        private System.Windows.Forms.GroupBox groupBoxCustomize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelRomIs;
        private System.Windows.Forms.Label labelCustomizeTip;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButtonAoSE;
        private System.Windows.Forms.RadioButton radioButtonHoDE;
        private System.Windows.Forms.CheckBox checkBoxSaveSetting;
        private System.Windows.Forms.Label labelSaveSettingTip;
    }
}