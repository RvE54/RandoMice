namespace RvE_RandoMice
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.AboutLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.LicenceButton = new System.Windows.Forms.Button();
            this.CreditsButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AboutLabel
            // 
            this.AboutLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AboutLabel.Location = new System.Drawing.Point(20, 65);
            this.AboutLabel.Name = "AboutLabel";
            this.AboutLabel.Size = new System.Drawing.Size(309, 88);
            this.AboutLabel.TabIndex = 1;
            this.AboutLabel.Text = "Developed by R. van Eenige\r\n\r\nCopyright © 2019\r\nLeiden University Medical Center " +
    "(LUMC), R. van Eenige\r\nand individual contributors.";
            this.AboutLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VersionLabel
            // 
            this.VersionLabel.BackColor = System.Drawing.SystemColors.Control;
            this.VersionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(20, 18);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(309, 37);
            this.VersionLabel.TabIndex = 2;
            this.VersionLabel.Text = "RandoMice {version}";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LicenceButton
            // 
            this.LicenceButton.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.LicenceButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicenceButton.Location = new System.Drawing.Point(119, 161);
            this.LicenceButton.Name = "LicenceButton";
            this.LicenceButton.Size = new System.Drawing.Size(93, 35);
            this.LicenceButton.TabIndex = 1;
            this.LicenceButton.Text = "Licence";
            this.LicenceButton.UseVisualStyleBackColor = false;
            this.LicenceButton.Click += new System.EventHandler(this.LicenceButton_Click);
            // 
            // CreditsButton
            // 
            this.CreditsButton.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.CreditsButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsButton.Location = new System.Drawing.Point(20, 161);
            this.CreditsButton.Name = "CreditsButton";
            this.CreditsButton.Size = new System.Drawing.Size(93, 35);
            this.CreditsButton.TabIndex = 0;
            this.CreditsButton.Text = "Credits";
            this.CreditsButton.UseVisualStyleBackColor = false;
            this.CreditsButton.Click += new System.EventHandler(this.CreditsButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.Location = new System.Drawing.Point(236, 161);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(93, 35);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(348, 207);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.CreditsButton);
            this.Controls.Add(this.LicenceButton);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.AboutLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About RandoMice";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label AboutLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button LicenceButton;
        private System.Windows.Forms.Button CreditsButton;
        private System.Windows.Forms.Button CloseButton;
    }
}