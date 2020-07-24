namespace RvE_RandoMice
{
    partial class CreditsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditsForm));
            this.CloseButton = new System.Windows.Forms.Button();
            this.CreditsPanel = new System.Windows.Forms.Panel();
            this.LicenceLinkLabel = new System.Windows.Forms.LinkLabel();
            this.LicenceLabel = new System.Windows.Forms.Label();
            this.CreditsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(292, 252);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // CreditsPanel
            // 
            this.CreditsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreditsPanel.AutoScroll = true;
            this.CreditsPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CreditsPanel.Controls.Add(this.LicenceLinkLabel);
            this.CreditsPanel.Controls.Add(this.LicenceLabel);
            this.CreditsPanel.Location = new System.Drawing.Point(12, 12);
            this.CreditsPanel.Name = "CreditsPanel";
            this.CreditsPanel.Size = new System.Drawing.Size(355, 236);
            this.CreditsPanel.TabIndex = 1;
            // 
            // LicenceLinkLabel
            // 
            this.LicenceLinkLabel.AutoSize = true;
            this.LicenceLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.LicenceLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 34);
            this.LicenceLinkLabel.Location = new System.Drawing.Point(3, 214);
            this.LicenceLinkLabel.Name = "LicenceLinkLabel";
            this.LicenceLinkLabel.Size = new System.Drawing.Size(180, 13);
            this.LicenceLinkLabel.TabIndex = 2;
            this.LicenceLinkLabel.TabStop = true;
            this.LicenceLinkLabel.Text = "van Eenige et. al. PLOS ONE (2020)";
            this.LicenceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenceLinkLabel_LinkClicked);
            // 
            // LicenceLabel
            // 
            this.LicenceLabel.AutoSize = true;
            this.LicenceLabel.Location = new System.Drawing.Point(3, 5);
            this.LicenceLabel.Name = "LicenceLabel";
            this.LicenceLabel.Size = new System.Drawing.Size(322, 208);
            this.LicenceLabel.TabIndex = 3;
            this.LicenceLabel.Text = resources.GetString("LicenceLabel.Text");
            // 
            // CreditsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(379, 286);
            this.Controls.Add(this.CreditsPanel);
            this.Controls.Add(this.CloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreditsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credits";
            this.CreditsPanel.ResumeLayout(false);
            this.CreditsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Panel CreditsPanel;
        private System.Windows.Forms.LinkLabel LicenceLinkLabel;
        private System.Windows.Forms.Label LicenceLabel;
    }
}