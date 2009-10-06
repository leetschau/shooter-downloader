namespace ShooterDownloader
{
    partial class SettingsForm
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
            this.chkEnableLog = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOpenLogFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVideoFileExt = new System.Windows.Forms.TextBox();
            this.cbConcurrenctNum = new System.Windows.Forms.ComboBox();
            this.chkEnableConvert = new System.Windows.Forms.CheckBox();
            this.lblTitleVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkEnableLog
            // 
            this.chkEnableLog.AutoSize = true;
            this.chkEnableLog.Location = new System.Drawing.Point(12, 159);
            this.chkEnableLog.Name = "chkEnableLog";
            this.chkEnableLog.Size = new System.Drawing.Size(98, 17);
            this.chkEnableLog.TabIndex = 0;
            this.chkEnableLog.Text = "啟動日誌功能";
            this.chkEnableLog.UseVisualStyleBackColor = true;
            this.chkEnableLog.CheckedChanged += new System.EventHandler(this.chkEnableLog_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(184, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "確定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(270, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOpenLogFolder
            // 
            this.btnOpenLogFolder.Location = new System.Drawing.Point(133, 155);
            this.btnOpenLogFolder.Name = "btnOpenLogFolder";
            this.btnOpenLogFolder.Size = new System.Drawing.Size(144, 25);
            this.btnOpenLogFolder.TabIndex = 4;
            this.btnOpenLogFolder.Text = "開啟日誌所在資料夾";
            this.btnOpenLogFolder.UseVisualStyleBackColor = true;
            this.btnOpenLogFolder.Click += new System.EventHandler(this.btnOpenLogFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "最大同時下載數:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "電影檔副檔名";
            // 
            // txtVideoFileExt
            // 
            this.txtVideoFileExt.Location = new System.Drawing.Point(12, 103);
            this.txtVideoFileExt.Name = "txtVideoFileExt";
            this.txtVideoFileExt.Size = new System.Drawing.Size(328, 20);
            this.txtVideoFileExt.TabIndex = 8;
            this.txtVideoFileExt.TextChanged += new System.EventHandler(this.txtVideoFileExt_TextChanged);
            // 
            // cbConcurrenctNum
            // 
            this.cbConcurrenctNum.FormattingEnabled = true;
            this.cbConcurrenctNum.Location = new System.Drawing.Point(108, 25);
            this.cbConcurrenctNum.Name = "cbConcurrenctNum";
            this.cbConcurrenctNum.Size = new System.Drawing.Size(44, 21);
            this.cbConcurrenctNum.TabIndex = 9;
            this.cbConcurrenctNum.SelectedIndexChanged += new System.EventHandler(this.cbConcurrenctNum_SelectedIndexChanged);
            // 
            // chkEnableConvert
            // 
            this.chkEnableConvert.AutoSize = true;
            this.chkEnableConvert.Location = new System.Drawing.Point(12, 196);
            this.chkEnableConvert.Name = "chkEnableConvert";
            this.chkEnableConvert.Size = new System.Drawing.Size(158, 17);
            this.chkEnableConvert.TabIndex = 10;
            this.chkEnableConvert.Text = "自動將簡體字幕轉成繁體";
            this.chkEnableConvert.UseVisualStyleBackColor = true;
            this.chkEnableConvert.CheckedChanged += new System.EventHandler(this.chkEnableConvert_CheckedChanged);
            // 
            // lblTitleVersion
            // 
            this.lblTitleVersion.AutoSize = true;
            this.lblTitleVersion.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitleVersion.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTitleVersion.Location = new System.Drawing.Point(12, 236);
            this.lblTitleVersion.Name = "lblTitleVersion";
            this.lblTitleVersion.Size = new System.Drawing.Size(65, 12);
            this.lblTitleVersion.TabIndex = 11;
            this.lblTitleVersion.Text = "Title Version";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(357, 308);
            this.ControlBox = false;
            this.Controls.Add(this.lblTitleVersion);
            this.Controls.Add(this.chkEnableConvert);
            this.Controls.Add(this.cbConcurrenctNum);
            this.Controls.Add(this.txtVideoFileExt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenLogFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkEnableLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "設定";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnableLog;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpenLogFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVideoFileExt;
        private System.Windows.Forms.ComboBox cbConcurrenctNum;
        private System.Windows.Forms.CheckBox chkEnableConvert;
        private System.Windows.Forms.Label lblTitleVersion;
    }
}