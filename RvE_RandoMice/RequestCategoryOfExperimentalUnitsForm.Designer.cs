namespace RvE_RandoMice
{
    partial class RequestCategoryOfExperimentalUnitsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestCategoryOfExperimentalUnitsForm));
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.FilterByCategoryHelpPictureBox = new RvE_RandoMice.HelpPictureBox();
            this.mainDataGridView = new RvE_RandoMice.MyDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.FilterByCategoryHelpPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(501, 419);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.TabStop = false;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(582, 419);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // FilterByCategoryHelpPictureBox
            // 
            this.FilterByCategoryHelpPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterByCategoryHelpPictureBox.HelpText = resources.GetString("FilterByCategoryHelpPictureBox.HelpText");
            this.FilterByCategoryHelpPictureBox.HelpTextCaption = "Info: Filter by category";
            this.FilterByCategoryHelpPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("FilterByCategoryHelpPictureBox.Image")));
            this.FilterByCategoryHelpPictureBox.Location = new System.Drawing.Point(479, 422);
            this.FilterByCategoryHelpPictureBox.Name = "FilterByCategoryHelpPictureBox";
            this.FilterByCategoryHelpPictureBox.Size = new System.Drawing.Size(16, 16);
            this.FilterByCategoryHelpPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterByCategoryHelpPictureBox.TabIndex = 6;
            this.FilterByCategoryHelpPictureBox.TabStop = false;
            // 
            // mainDataGridView
            // 
            this.mainDataGridView.AllowFiltering = false;
            this.mainDataGridView.AllowSorting = false;
            this.mainDataGridView.AllowUserToAddRows = false;
            this.mainDataGridView.AllowUserToDeleteRows = false;
            this.mainDataGridView.AllowViewDetails = false;
            this.mainDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mainDataGridView.CurrentlySelectedValue = null;
            this.mainDataGridView.IsInputDataGridViewForExperiment = false;
            this.mainDataGridView.Location = new System.Drawing.Point(13, 13);
            this.mainDataGridView.MenuItemsEnabled = true;
            this.mainDataGridView.Name = "mainDataGridView";
            this.mainDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.IntoExistingCellsOnly;
            this.mainDataGridView.RowHeadersVisible = false;
            this.mainDataGridView.Size = new System.Drawing.Size(644, 404);
            this.mainDataGridView.TabIndex = 0;
            // 
            // RequestCategoryOfExperimentalUnitsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 453);
            this.Controls.Add(this.FilterByCategoryHelpPictureBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.mainDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RequestCategoryOfExperimentalUnitsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Provide categories";
            this.Load += new System.EventHandler(this.RequestCategoryOfExperimentalUnits_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FilterByCategoryHelpPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MyDataGridView mainDataGridView;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private HelpPictureBox FilterByCategoryHelpPictureBox;
    }
}