namespace Boss_Timer_Overlay
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonUpdateBossInfo = new System.Windows.Forms.Button();
            this.buttonToggleOverlay = new System.Windows.Forms.Button();
            this.buttonGetBossName = new System.Windows.Forms.Button();
            this.buttonScrapeHtmlSource = new System.Windows.Forms.Button();
            this.textBoxHtml = new System.Windows.Forms.TextBox();
            this.buttonListOpenProcesses = new System.Windows.Forms.Button();
            this.textBoxProcesses = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonUpdateBossInfo);
            this.panel1.Controls.Add(this.buttonToggleOverlay);
            this.panel1.Controls.Add(this.buttonGetBossName);
            this.panel1.Controls.Add(this.buttonScrapeHtmlSource);
            this.panel1.Controls.Add(this.textBoxHtml);
            this.panel1.Controls.Add(this.buttonListOpenProcesses);
            this.panel1.Controls.Add(this.textBoxProcesses);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // buttonUpdateBossInfo
            // 
            this.buttonUpdateBossInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpdateBossInfo.Enabled = false;
            this.buttonUpdateBossInfo.Location = new System.Drawing.Point(510, 415);
            this.buttonUpdateBossInfo.Name = "buttonUpdateBossInfo";
            this.buttonUpdateBossInfo.Size = new System.Drawing.Size(96, 23);
            this.buttonUpdateBossInfo.TabIndex = 6;
            this.buttonUpdateBossInfo.Text = "Fetch Boss Info";
            this.buttonUpdateBossInfo.UseVisualStyleBackColor = true;
            this.buttonUpdateBossInfo.Click += new System.EventHandler(this.buttonUpdateBossInfo_Click);
            // 
            // buttonToggleOverlay
            // 
            this.buttonToggleOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonToggleOverlay.Location = new System.Drawing.Point(417, 415);
            this.buttonToggleOverlay.Name = "buttonToggleOverlay";
            this.buttonToggleOverlay.Size = new System.Drawing.Size(87, 23);
            this.buttonToggleOverlay.TabIndex = 5;
            this.buttonToggleOverlay.Text = "Show Overlay";
            this.buttonToggleOverlay.UseVisualStyleBackColor = true;
            this.buttonToggleOverlay.Click += new System.EventHandler(this.buttonRunOverlay_Click);
            // 
            // buttonGetBossName
            // 
            this.buttonGetBossName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGetBossName.Location = new System.Drawing.Point(272, 415);
            this.buttonGetBossName.Name = "buttonGetBossName";
            this.buttonGetBossName.Size = new System.Drawing.Size(139, 23);
            this.buttonGetBossName.TabIndex = 4;
            this.buttonGetBossName.Text = "Get Boss Name";
            this.buttonGetBossName.UseVisualStyleBackColor = true;
            this.buttonGetBossName.Click += new System.EventHandler(this.buttonGetBossName_Click);
            // 
            // buttonScrapeHtmlSource
            // 
            this.buttonScrapeHtmlSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonScrapeHtmlSource.Location = new System.Drawing.Point(133, 415);
            this.buttonScrapeHtmlSource.Name = "buttonScrapeHtmlSource";
            this.buttonScrapeHtmlSource.Size = new System.Drawing.Size(133, 23);
            this.buttonScrapeHtmlSource.TabIndex = 3;
            this.buttonScrapeHtmlSource.Text = "Scrape HTML Source";
            this.buttonScrapeHtmlSource.UseVisualStyleBackColor = true;
            this.buttonScrapeHtmlSource.Click += new System.EventHandler(this.buttonScrapeHtmlSource_Click);
            // 
            // textBoxHtml
            // 
            this.textBoxHtml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHtml.Location = new System.Drawing.Point(12, 12);
            this.textBoxHtml.Multiline = true;
            this.textBoxHtml.Name = "textBoxHtml";
            this.textBoxHtml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxHtml.Size = new System.Drawing.Size(399, 397);
            this.textBoxHtml.TabIndex = 2;
            this.textBoxHtml.WordWrap = false;
            // 
            // buttonListOpenProcesses
            // 
            this.buttonListOpenProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonListOpenProcesses.Location = new System.Drawing.Point(12, 415);
            this.buttonListOpenProcesses.Name = "buttonListOpenProcesses";
            this.buttonListOpenProcesses.Size = new System.Drawing.Size(115, 23);
            this.buttonListOpenProcesses.TabIndex = 1;
            this.buttonListOpenProcesses.Text = "List Open Processes";
            this.buttonListOpenProcesses.UseVisualStyleBackColor = true;
            this.buttonListOpenProcesses.Click += new System.EventHandler(this.buttonListOpenProcesses_Click);
            // 
            // textBoxProcesses
            // 
            this.textBoxProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProcesses.Location = new System.Drawing.Point(417, 12);
            this.textBoxProcesses.Multiline = true;
            this.textBoxProcesses.Name = "textBoxProcesses";
            this.textBoxProcesses.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxProcesses.Size = new System.Drawing.Size(371, 397);
            this.textBoxProcesses.TabIndex = 0;
            this.textBoxProcesses.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "BDO Boss Timer Overlay";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonListOpenProcesses;
        private System.Windows.Forms.TextBox textBoxProcesses;
        private System.Windows.Forms.Button buttonScrapeHtmlSource;
        private System.Windows.Forms.TextBox textBoxHtml;
        private System.Windows.Forms.Button buttonGetBossName;
        private System.Windows.Forms.Button buttonToggleOverlay;
        private System.Windows.Forms.Button buttonUpdateBossInfo;
    }
}

