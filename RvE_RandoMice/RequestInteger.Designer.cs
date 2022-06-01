
namespace RvE_RandoMice
{
    partial class RequestInteger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestInteger));
            this.MainText = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ResultNumericUpDown = new RvE_RandoMice.MyNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ResultNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // MainText
            // 
            this.MainText.AutoSize = true;
            this.MainText.Location = new System.Drawing.Point(13, 16);
            this.MainText.MaximumSize = new System.Drawing.Size(300, 1000);
            this.MainText.Name = "MainText";
            this.MainText.Size = new System.Drawing.Size(51, 13);
            this.MainText.TabIndex = 0;
            this.MainText.Text = "MainText";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.OKButton.Location = new System.Drawing.Point(250, 58);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(61, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(169, 58);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ResultNumericUpDown
            // 
            this.ResultNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultNumericUpDown.Location = new System.Drawing.Point(218, 32);
            this.ResultNumericUpDown.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.ResultNumericUpDown.Name = "ResultNumericUpDown";
            this.ResultNumericUpDown.Size = new System.Drawing.Size(93, 20);
            this.ResultNumericUpDown.TabIndex = 0;
            this.ResultNumericUpDown.ValueChanged += new System.EventHandler(this.ResultNumericUpDown_ValueChanged);
            this.ResultNumericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ResultNumericUpDown_KeyDown);
            // 
            // RequestInteger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 93);
            this.Controls.Add(this.ResultNumericUpDown);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MainText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RequestInteger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RequestInteger";
            ((System.ComponentModel.ISupportInitialize)(this.ResultNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainText;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private MyNumericUpDown ResultNumericUpDown;
    }
}