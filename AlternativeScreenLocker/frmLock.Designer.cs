namespace AlternativeScreenLocker
{
    partial class frmLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLock));
            this.grpSetting = new System.Windows.Forms.GroupBox();
            this.btnDebugQuit = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.lblDes = new System.Windows.Forms.Label();
            this.cbMouse = new System.Windows.Forms.CheckBox();
            this.cbBG = new System.Windows.Forms.CheckBox();
            this.lblUptime = new System.Windows.Forms.Label();
            this.tmrSync = new System.Windows.Forms.Timer(this.components);
            this.pbBG = new System.Windows.Forms.PictureBox();
            this.ttMain = new System.Windows.Forms.TextBox();
            this.tmrMonitor = new System.Windows.Forms.Timer(this.components);
            this.tmrInit = new System.Windows.Forms.Timer(this.components);
            this.grpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBG)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSetting
            // 
            this.grpSetting.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grpSetting.Controls.Add(this.btnDebugQuit);
            this.grpSetting.Controls.Add(this.axWindowsMediaPlayer1);
            this.grpSetting.Controls.Add(this.lblDes);
            this.grpSetting.Controls.Add(this.cbMouse);
            this.grpSetting.Controls.Add(this.cbBG);
            this.grpSetting.Controls.Add(this.lblUptime);
            this.grpSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.grpSetting.Location = new System.Drawing.Point(12, 501);
            this.grpSetting.Name = "grpSetting";
            this.grpSetting.Size = new System.Drawing.Size(365, 280);
            this.grpSetting.TabIndex = 1;
            this.grpSetting.TabStop = false;
            // 
            // btnDebugQuit
            // 
            this.btnDebugQuit.Location = new System.Drawing.Point(276, 62);
            this.btnDebugQuit.Name = "btnDebugQuit";
            this.btnDebugQuit.Size = new System.Drawing.Size(75, 56);
            this.btnDebugQuit.TabIndex = 4;
            this.btnDebugQuit.Text = "Exit";
            this.btnDebugQuit.UseVisualStyleBackColor = true;
            this.btnDebugQuit.Click += new System.EventHandler(this.btnDebugQuit_Click);
            this.btnDebugQuit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myKeyUp);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(10, 129);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(341, 145);
            this.axWindowsMediaPlayer1.TabIndex = 4;
            // 
            // lblDes
            // 
            this.lblDes.AutoSize = true;
            this.lblDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblDes.Location = new System.Drawing.Point(6, 96);
            this.lblDes.Name = "lblDes";
            this.lblDes.Size = new System.Drawing.Size(19, 20);
            this.lblDes.TabIndex = 5;
            this.lblDes.Text = "0";
            // 
            // cbMouse
            // 
            this.cbMouse.AutoSize = true;
            this.cbMouse.Checked = true;
            this.cbMouse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMouse.Location = new System.Drawing.Point(241, 25);
            this.cbMouse.Name = "cbMouse";
            this.cbMouse.Size = new System.Drawing.Size(118, 24);
            this.cbMouse.TabIndex = 4;
            this.cbMouse.Text = "Move mouse";
            this.cbMouse.UseVisualStyleBackColor = true;
            this.cbMouse.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myKeyUp);
            // 
            // cbBG
            // 
            this.cbBG.AutoSize = true;
            this.cbBG.Checked = true;
            this.cbBG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBG.Location = new System.Drawing.Point(10, 25);
            this.cbBG.Name = "cbBG";
            this.cbBG.Size = new System.Drawing.Size(177, 24);
            this.cbBG.TabIndex = 3;
            this.cbBG.Text = "Random background";
            this.cbBG.UseVisualStyleBackColor = true;
            this.cbBG.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myKeyUp);
            // 
            // lblUptime
            // 
            this.lblUptime.AutoSize = true;
            this.lblUptime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.lblUptime.Location = new System.Drawing.Point(6, 62);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(64, 20);
            this.lblUptime.TabIndex = 2;
            this.lblUptime.Text = "Uptime:";
            // 
            // tmrSync
            // 
            this.tmrSync.Enabled = true;
            this.tmrSync.Interval = 1000;
            this.tmrSync.Tick += new System.EventHandler(this.tmrSync_Tick);
            // 
            // pbBG
            // 
            this.pbBG.Location = new System.Drawing.Point(361, 107);
            this.pbBG.Name = "pbBG";
            this.pbBG.Size = new System.Drawing.Size(100, 50);
            this.pbBG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBG.TabIndex = 2;
            this.pbBG.TabStop = false;
            // 
            // ttMain
            // 
            this.ttMain.Enabled = false;
            this.ttMain.Font = new System.Drawing.Font("Wingdings 2", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ttMain.Location = new System.Drawing.Point(197, 323);
            this.ttMain.Name = "ttMain";
            this.ttMain.ReadOnly = true;
            this.ttMain.Size = new System.Drawing.Size(528, 53);
            this.ttMain.TabIndex = 3;
            this.ttMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ttMain.TextChanged += new System.EventHandler(this.ttMain_TextChanged);
            // 
            // tmrMonitor
            // 
            this.tmrMonitor.Enabled = true;
            this.tmrMonitor.Tick += new System.EventHandler(this.tmrMonitor_Tick);
            // 
            // tmrInit
            // 
            this.tmrInit.Tick += new System.EventHandler(this.tmrInit_Tick);
            // 
            // frmLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(947, 793);
            this.ControlBox = false;
            this.Controls.Add(this.ttMain);
            this.Controls.Add(this.grpSetting);
            this.Controls.Add(this.pbBG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLock";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.frmLock_Activated);
            this.Deactivate += new System.EventHandler(this.frmLock_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLock_FormClosing);
            this.Load += new System.EventHandler(this.frmLock_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myKeyUp);
            this.grpSetting.ResumeLayout(false);
            this.grpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox grpSetting;
        private System.Windows.Forms.CheckBox cbMouse;
        private System.Windows.Forms.CheckBox cbBG;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Timer tmrSync;
        private System.Windows.Forms.PictureBox pbBG;
        private System.Windows.Forms.Label lblDes;
        private System.Windows.Forms.TextBox ttMain;
        private System.Windows.Forms.Timer tmrMonitor;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Timer tmrInit;
        private System.Windows.Forms.Button btnDebugQuit;
    }
}

