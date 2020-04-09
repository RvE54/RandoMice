namespace RvE_RandoMice
{
    partial class LicenceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenceForm));
            this.LicenceLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LicenceLabel
            // 
            this.LicenceLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LicenceLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LicenceLabel.Location = new System.Drawing.Point(12, 9);
            this.LicenceLabel.Name = "LicenceLabel";
            this.LicenceLabel.Padding = new System.Windows.Forms.Padding(6);
            this.LicenceLabel.Size = new System.Drawing.Size(391, 156);
            this.LicenceLabel.TabIndex = 0;
            this.LicenceLabel.Text = resources.GetString("LicenceLabel.Text");
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(328, 168);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LicenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(415, 202);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.LicenceLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Licence";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LicenceLabel;
        private System.Windows.Forms.Button CloseButton;
    }
}