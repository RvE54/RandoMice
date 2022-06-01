namespace RvE_RandoMice
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StartRunButton = new System.Windows.Forms.Button();
            this.CreateBlockSetsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.CreateBlockSetsTimer = new System.Windows.Forms.Timer(this.components);
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.TimeElapsedLabel = new System.Windows.Forms.Label();
            this.AbortRunButton = new System.Windows.Forms.Button();
            this.ShowResultsButton = new System.Windows.Forms.Button();
            this.MeanCheckBox = new System.Windows.Forms.CheckBox();
            this.SDCheckBox = new System.Windows.Forms.CheckBox();
            this.MinCheckBox = new System.Windows.Forms.CheckBox();
            this.MedianCheckBox = new System.Windows.Forms.CheckBox();
            this.MaxCheckBox = new System.Windows.Forms.CheckBox();
            this.BlockCountLabel = new System.Windows.Forms.Label();
            this.BlockCountComboBox = new System.Windows.Forms.ComboBox();
            this.BlockSizesAreEqualCheckBox = new System.Windows.Forms.CheckBox();
            this.NumberOfExperimentalUnitsPerBlockLabel = new System.Windows.Forms.Label();
            this.DesiredUniqueBlockSetsToCreateLabel = new System.Windows.Forms.Label();
            this.ExperimentalUnitNamesInBlocksInBlockSet = new System.Windows.Forms.Label();
            this.BlockDescriptivesLabel = new System.Windows.Forms.Label();
            this.NumberOfBlockSetsToRememberLabel = new System.Windows.Forms.Label();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportResultsToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.TheoreticalUniqueBlockSetsLabel = new System.Windows.Forms.Label();
            this.SetDesiredUniqueBlockSetsToCreateToMax = new System.Windows.Forms.Button();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.BlocksTabPage = new System.Windows.Forms.TabPage();
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown = new RvE_RandoMice.MyNumericUpDown();
            this.UniqueSetsHelpPictureBox = new RvE_RandoMice.HelpPictureBox();
            this.DefineGroupNamesLabel = new System.Windows.Forms.Label();
            this.RememberSetsNumericUpDown = new RvE_RandoMice.MyNumericUpDown();
            this.DesiredUniqueSetsNumericUpDown = new RvE_RandoMice.MyNumericUpDown();
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton = new System.Windows.Forms.Button();
            this.VariablesTabPage = new System.Windows.Forms.TabPage();
            this.NoVariablesAvailableYetLabel = new System.Windows.Forms.Label();
            this.VariableDecimalPlacesLabel = new System.Windows.Forms.Label();
            this.VariableWeightsLabel = new System.Windows.Forms.Label();
            this.VariableNamesLabel = new System.Windows.Forms.Label();
            this.SubgroupsTabPage = new System.Windows.Forms.TabPage();
            this.PreferredSubgroupSizeNumericUpDown = new RvE_RandoMice.MyNumericUpDown();
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox = new System.Windows.Forms.ComboBox();
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel = new System.Windows.Forms.Label();
            this.SubgroupSizeLabel = new System.Windows.Forms.Label();
            this.CreateSubgroupsCheckBox = new System.Windows.Forms.CheckBox();
            this.MarkersHelpPictureBox = new RvE_RandoMice.HelpPictureBox();
            this.CheckForBlockSetUnicityCheckBox = new System.Windows.Forms.CheckBox();
            this.TimeRemainingLabel = new System.Windows.Forms.Label();
            this.ExportToExcelBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox = new System.Windows.Forms.ComboBox();
            this.UniqueNamesOfExperimentalUnitsLabel = new System.Windows.Forms.Label();
            this.SubgroupCompositionsLabel = new System.Windows.Forms.Label();
            this.ProgressPercentageLabel = new System.Windows.Forms.Label();
            this.ExportToFileBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PasteButton = new System.Windows.Forms.Button();
            this.ReDefineGroupNamesLabel = new System.Windows.Forms.Label();
            this.FilterResultsPictureBox = new System.Windows.Forms.PictureBox();
            this.RandomlyAssignGroupsToBlocks = new System.Windows.Forms.Button();
            this.InputGroupBox = new System.Windows.Forms.GroupBox();
            this.InputDataGridView = new RvE_RandoMice.MyDataGridView();
            this.ExperimentalUnitNamesHelpPictureBox = new RvE_RandoMice.HelpPictureBox();
            this.ProgressGroupBox = new System.Windows.Forms.GroupBox();
            this.BestBlockSetsGroupBox = new System.Windows.Forms.GroupBox();
            this.BlockSetsResultsDataGridView = new RvE_RandoMice.MyDataGridView();
            this.BlockSetDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.BlocksDescriptivesDataGridView = new RvE_RandoMice.MyDataGridView();
            this.SubgroupCompositionsDataGridView = new RvE_RandoMice.MyDataGridView();
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView = new RvE_RandoMice.MyDataGridView();
            this.HorizontalTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.VerticalTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ControlPanel1 = new System.Windows.Forms.Panel();
            this.ResizeFormTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenuStrip.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.BlocksTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfExperimentalUnitsPerBlockNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueSetsHelpPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RememberSetsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DesiredUniqueSetsNumericUpDown)).BeginInit();
            this.VariablesTabPage.SuspendLayout();
            this.SubgroupsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreferredSubgroupSizeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkersHelpPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterResultsPictureBox)).BeginInit();
            this.InputGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InputDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExperimentalUnitNamesHelpPictureBox)).BeginInit();
            this.ProgressGroupBox.SuspendLayout();
            this.BestBlockSetsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlockSetsResultsDataGridView)).BeginInit();
            this.BlockSetDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlocksDescriptivesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubgroupCompositionsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView)).BeginInit();
            this.HorizontalTableLayoutPanel.SuspendLayout();
            this.VerticalTableLayoutPanel.SuspendLayout();
            this.ControlPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartRunButton
            // 
            this.StartRunButton.Location = new System.Drawing.Point(249, 23);
            this.StartRunButton.Name = "StartRunButton";
            this.StartRunButton.Size = new System.Drawing.Size(75, 23);
            this.StartRunButton.TabIndex = 10;
            this.StartRunButton.Text = "Run";
            this.MainToolTip.SetToolTip(this.StartRunButton, "Start dividing experimental units into blocks.");
            this.StartRunButton.UseVisualStyleBackColor = true;
            this.StartRunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // CreateBlockSetsBackgroundWorker
            // 
            this.CreateBlockSetsBackgroundWorker.WorkerReportsProgress = true;
            this.CreateBlockSetsBackgroundWorker.WorkerSupportsCancellation = true;
            this.CreateBlockSetsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CreateBlockSetsBackGroundWorker_DoWork);
            this.CreateBlockSetsBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.CreateBlockSetsBackGroundWorker_ProgressChanged);
            this.CreateBlockSetsBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CreateBlockSetsBackGroundWorker_RunWorkerCompleted);
            // 
            // CreateBlockSetsTimer
            // 
            this.CreateBlockSetsTimer.Tick += new System.EventHandler(this.CreateBlockSetsTimer_Tick);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(7, 85);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(54, 13);
            this.ProgressLabel.TabIndex = 6;
            this.ProgressLabel.Tag = "";
            this.ProgressLabel.Text = "Progress..";
            // 
            // TimeElapsedLabel
            // 
            this.TimeElapsedLabel.AutoSize = true;
            this.TimeElapsedLabel.Location = new System.Drawing.Point(7, 56);
            this.TimeElapsedLabel.Name = "TimeElapsedLabel";
            this.TimeElapsedLabel.Size = new System.Drawing.Size(76, 13);
            this.TimeElapsedLabel.TabIndex = 7;
            this.TimeElapsedLabel.Tag = "";
            this.TimeElapsedLabel.Text = "Time elapsed: ";
            // 
            // AbortRunButton
            // 
            this.AbortRunButton.Location = new System.Drawing.Point(331, 22);
            this.AbortRunButton.Name = "AbortRunButton";
            this.AbortRunButton.Size = new System.Drawing.Size(75, 23);
            this.AbortRunButton.TabIndex = 11;
            this.AbortRunButton.Text = "Abort";
            this.MainToolTip.SetToolTip(this.AbortRunButton, "Stop creating new block sets and delete results.");
            this.AbortRunButton.UseVisualStyleBackColor = true;
            this.AbortRunButton.Click += new System.EventHandler(this.AbortRunButton_Click);
            // 
            // ShowResultsButton
            // 
            this.ShowResultsButton.Location = new System.Drawing.Point(413, 22);
            this.ShowResultsButton.Name = "ShowResultsButton";
            this.ShowResultsButton.Size = new System.Drawing.Size(75, 23);
            this.ShowResultsButton.TabIndex = 12;
            this.ShowResultsButton.Text = "Show results";
            this.MainToolTip.SetToolTip(this.ShowResultsButton, "Stop finding new block sets and display the current results.");
            this.ShowResultsButton.UseVisualStyleBackColor = true;
            this.ShowResultsButton.Click += new System.EventHandler(this.ShowResultsButton_Click);
            // 
            // MeanCheckBox
            // 
            this.MeanCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MeanCheckBox.AutoSize = true;
            this.MeanCheckBox.Checked = true;
            this.MeanCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MeanCheckBox.Location = new System.Drawing.Point(778, 35);
            this.MeanCheckBox.Name = "MeanCheckBox";
            this.MeanCheckBox.Size = new System.Drawing.Size(53, 17);
            this.MeanCheckBox.TabIndex = 14;
            this.MeanCheckBox.Tag = "RoundedMean";
            this.MeanCheckBox.Text = "Mean";
            this.MeanCheckBox.UseVisualStyleBackColor = true;
            this.MeanCheckBox.CheckedChanged += new System.EventHandler(this.DescriptiveCheckbox_CheckedChanged);
            // 
            // SDCheckBox
            // 
            this.SDCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SDCheckBox.AutoSize = true;
            this.SDCheckBox.Checked = true;
            this.SDCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SDCheckBox.Location = new System.Drawing.Point(778, 58);
            this.SDCheckBox.Name = "SDCheckBox";
            this.SDCheckBox.Size = new System.Drawing.Size(41, 17);
            this.SDCheckBox.TabIndex = 15;
            this.SDCheckBox.Tag = "RoundedSD";
            this.SDCheckBox.Text = "SD";
            this.SDCheckBox.UseVisualStyleBackColor = true;
            this.SDCheckBox.CheckedChanged += new System.EventHandler(this.DescriptiveCheckbox_CheckedChanged);
            // 
            // MinCheckBox
            // 
            this.MinCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinCheckBox.AutoSize = true;
            this.MinCheckBox.Location = new System.Drawing.Point(778, 81);
            this.MinCheckBox.Name = "MinCheckBox";
            this.MinCheckBox.Size = new System.Drawing.Size(43, 17);
            this.MinCheckBox.TabIndex = 16;
            this.MinCheckBox.Tag = "RoundedMin";
            this.MinCheckBox.Text = "Min";
            this.MinCheckBox.UseVisualStyleBackColor = true;
            this.MinCheckBox.CheckedChanged += new System.EventHandler(this.DescriptiveCheckbox_CheckedChanged);
            // 
            // MedianCheckBox
            // 
            this.MedianCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MedianCheckBox.AutoSize = true;
            this.MedianCheckBox.Location = new System.Drawing.Point(778, 104);
            this.MedianCheckBox.Name = "MedianCheckBox";
            this.MedianCheckBox.Size = new System.Drawing.Size(61, 17);
            this.MedianCheckBox.TabIndex = 17;
            this.MedianCheckBox.Tag = "RoundedMedian";
            this.MedianCheckBox.Text = "Median";
            this.MedianCheckBox.UseVisualStyleBackColor = true;
            this.MedianCheckBox.CheckedChanged += new System.EventHandler(this.DescriptiveCheckbox_CheckedChanged);
            // 
            // MaxCheckBox
            // 
            this.MaxCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxCheckBox.AutoSize = true;
            this.MaxCheckBox.Location = new System.Drawing.Point(778, 127);
            this.MaxCheckBox.Name = "MaxCheckBox";
            this.MaxCheckBox.Size = new System.Drawing.Size(46, 17);
            this.MaxCheckBox.TabIndex = 18;
            this.MaxCheckBox.Tag = "RoundedMax";
            this.MaxCheckBox.Text = "Max";
            this.MaxCheckBox.UseVisualStyleBackColor = true;
            this.MaxCheckBox.CheckedChanged += new System.EventHandler(this.DescriptiveCheckbox_CheckedChanged);
            // 
            // BlockCountLabel
            // 
            this.BlockCountLabel.AutoSize = true;
            this.BlockCountLabel.Location = new System.Drawing.Point(10, 5);
            this.BlockCountLabel.Name = "BlockCountLabel";
            this.BlockCountLabel.Size = new System.Drawing.Size(93, 13);
            this.BlockCountLabel.TabIndex = 19;
            this.BlockCountLabel.Text = "Number of blocks:";
            // 
            // BlockCountComboBox
            // 
            this.BlockCountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BlockCountComboBox.FormattingEnabled = true;
            this.BlockCountComboBox.Location = new System.Drawing.Point(242, 3);
            this.BlockCountComboBox.Name = "BlockCountComboBox";
            this.BlockCountComboBox.Size = new System.Drawing.Size(135, 21);
            this.BlockCountComboBox.TabIndex = 2;
            this.BlockCountComboBox.SelectedIndexChanged += new System.EventHandler(this.BlockCountComboBox_SelectedIndexChanged);
            // 
            // BlockSizesAreEqualCheckBox
            // 
            this.BlockSizesAreEqualCheckBox.AutoSize = true;
            this.BlockSizesAreEqualCheckBox.Checked = true;
            this.BlockSizesAreEqualCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BlockSizesAreEqualCheckBox.Location = new System.Drawing.Point(242, 30);
            this.BlockSizesAreEqualCheckBox.Name = "BlockSizesAreEqualCheckBox";
            this.BlockSizesAreEqualCheckBox.Size = new System.Drawing.Size(126, 17);
            this.BlockSizesAreEqualCheckBox.TabIndex = 3;
            this.BlockSizesAreEqualCheckBox.Text = "Block sizes are equal";
            this.MainToolTip.SetToolTip(this.BlockSizesAreEqualCheckBox, "Checking this checkbox will give you the option\r\nto set the block sizes of each g" +
        "roup individually.");
            this.BlockSizesAreEqualCheckBox.UseVisualStyleBackColor = true;
            this.BlockSizesAreEqualCheckBox.CheckedChanged += new System.EventHandler(this.BlockSizesAreEqualCheckBox_CheckedChanged);
            // 
            // NumberOfExperimentalUnitsPerBlockLabel
            // 
            this.NumberOfExperimentalUnitsPerBlockLabel.AutoSize = true;
            this.NumberOfExperimentalUnitsPerBlockLabel.Location = new System.Drawing.Point(10, 59);
            this.NumberOfExperimentalUnitsPerBlockLabel.Name = "NumberOfExperimentalUnitsPerBlockLabel";
            this.NumberOfExperimentalUnitsPerBlockLabel.Size = new System.Drawing.Size(58, 13);
            this.NumberOfExperimentalUnitsPerBlockLabel.TabIndex = 22;
            this.NumberOfExperimentalUnitsPerBlockLabel.Text = "Block size:";
            // 
            // DesiredUniqueBlockSetsToCreateLabel
            // 
            this.DesiredUniqueBlockSetsToCreateLabel.AutoSize = true;
            this.DesiredUniqueBlockSetsToCreateLabel.Location = new System.Drawing.Point(10, 87);
            this.DesiredUniqueBlockSetsToCreateLabel.Name = "DesiredUniqueBlockSetsToCreateLabel";
            this.DesiredUniqueBlockSetsToCreateLabel.Size = new System.Drawing.Size(182, 13);
            this.DesiredUniqueBlockSetsToCreateLabel.TabIndex = 24;
            this.DesiredUniqueBlockSetsToCreateLabel.Text = "Desired number of unique block sets:";
            // 
            // ExperimentalUnitNamesInBlocksInBlockSet
            // 
            this.ExperimentalUnitNamesInBlocksInBlockSet.AutoSize = true;
            this.ExperimentalUnitNamesInBlocksInBlockSet.Location = new System.Drawing.Point(3, 19);
            this.ExperimentalUnitNamesInBlocksInBlockSet.Name = "ExperimentalUnitNamesInBlocksInBlockSet";
            this.ExperimentalUnitNamesInBlocksInBlockSet.Size = new System.Drawing.Size(98, 13);
            this.ExperimentalUnitNamesInBlocksInBlockSet.TabIndex = 26;
            this.ExperimentalUnitNamesInBlocksInBlockSet.Text = "Block compositions";
            // 
            // BlockDescriptivesLabel
            // 
            this.BlockDescriptivesLabel.AutoSize = true;
            this.BlockDescriptivesLabel.Location = new System.Drawing.Point(555, 19);
            this.BlockDescriptivesLabel.Name = "BlockDescriptivesLabel";
            this.BlockDescriptivesLabel.Size = new System.Drawing.Size(93, 13);
            this.BlockDescriptivesLabel.TabIndex = 27;
            this.BlockDescriptivesLabel.Text = "Block descriptives";
            // 
            // NumberOfBlockSetsToRememberLabel
            // 
            this.NumberOfBlockSetsToRememberLabel.AutoSize = true;
            this.NumberOfBlockSetsToRememberLabel.Location = new System.Drawing.Point(10, 112);
            this.NumberOfBlockSetsToRememberLabel.Name = "NumberOfBlockSetsToRememberLabel";
            this.NumberOfBlockSetsToRememberLabel.Size = new System.Drawing.Size(142, 13);
            this.NumberOfBlockSetsToRememberLabel.TabIndex = 29;
            this.NumberOfBlockSetsToRememberLabel.Text = "Number of sets to remember:";
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(854, 24);
            this.MainMenuStrip.TabIndex = 31;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.toolStripSeparator1,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.LoadToolStripMenuItem,
            this.toolStripSeparator2,
            this.ExportResultsToExcelToolStripMenuItem,
            this.toolStripSeparator3,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.Image = global::RvE_RandoMice.Properties.Resources.New;
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.NewToolStripMenuItem.Text = "New";
            this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Image = global::RvE_RandoMice.Properties.Resources.Save;
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.SaveAsToolStripMenuItem.Text = "Save As...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // LoadToolStripMenuItem
            // 
            this.LoadToolStripMenuItem.Image = global::RvE_RandoMice.Properties.Resources.Open;
            this.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem";
            this.LoadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.LoadToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.LoadToolStripMenuItem.Text = "Load";
            this.LoadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // ExportResultsToExcelToolStripMenuItem
            // 
            this.ExportResultsToExcelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExcelToolStripMenuItem,
            this.TSVToolStripMenuItem,
            this.CSVToolStripMenuItem,
            this.TXTToolStripMenuItem});
            this.ExportResultsToExcelToolStripMenuItem.Name = "ExportResultsToExcelToolStripMenuItem";
            this.ExportResultsToExcelToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.ExportResultsToExcelToolStripMenuItem.Text = "Export...";
            // 
            // ExcelToolStripMenuItem
            // 
            this.ExcelToolStripMenuItem.Name = "ExcelToolStripMenuItem";
            this.ExcelToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.ExcelToolStripMenuItem.Tag = "Excel Workbook";
            this.ExcelToolStripMenuItem.Text = ".xls";
            this.ExcelToolStripMenuItem.Click += new System.EventHandler(this.ExportResultsToFileToolStripMenuItem_Click);
            // 
            // TSVToolStripMenuItem
            // 
            this.TSVToolStripMenuItem.Name = "TSVToolStripMenuItem";
            this.TSVToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.TSVToolStripMenuItem.Tag = "Tab-separated values";
            this.TSVToolStripMenuItem.Text = ".tsv";
            this.TSVToolStripMenuItem.Click += new System.EventHandler(this.ExportResultsToFileToolStripMenuItem_Click);
            // 
            // CSVToolStripMenuItem
            // 
            this.CSVToolStripMenuItem.Name = "CSVToolStripMenuItem";
            this.CSVToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.CSVToolStripMenuItem.Tag = "Comma separated values";
            this.CSVToolStripMenuItem.Text = ".csv";
            this.CSVToolStripMenuItem.Click += new System.EventHandler(this.ExportResultsToFileToolStripMenuItem_Click);
            // 
            // TXTToolStripMenuItem
            // 
            this.TXTToolStripMenuItem.Name = "TXTToolStripMenuItem";
            this.TXTToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.TXTToolStripMenuItem.Tag = "Text file";
            this.TXTToolStripMenuItem.Text = ".txt";
            this.TXTToolStripMenuItem.Click += new System.EventHandler(this.ExportResultsToFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(140, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewHelpToolStripMenuItem,
            this.CheckForUpdatesToolStripMenuItem,
            this.toolStripSeparator4,
            this.AboutToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpToolStripMenuItem.Text = "Help";
            // 
            // ViewHelpToolStripMenuItem
            // 
            this.ViewHelpToolStripMenuItem.Image = global::RvE_RandoMice.Properties.Resources.QuestionMark;
            this.ViewHelpToolStripMenuItem.Name = "ViewHelpToolStripMenuItem";
            this.ViewHelpToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.ViewHelpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.ViewHelpToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.ViewHelpToolStripMenuItem.Text = "View Help";
            this.ViewHelpToolStripMenuItem.Click += new System.EventHandler(this.ViewHelpToolStripMenuItem_Click);
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            this.CheckForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.CheckForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(170, 6);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.AboutToolStripMenuItem.Text = "About RandoMice";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.MainProgressBar.Location = new System.Drawing.Point(9, 104);
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(244, 23);
            this.MainProgressBar.Step = 1;
            this.MainProgressBar.TabIndex = 32;
            // 
            // TheoreticalUniqueBlockSetsLabel
            // 
            this.TheoreticalUniqueBlockSetsLabel.AutoSize = true;
            this.TheoreticalUniqueBlockSetsLabel.Location = new System.Drawing.Point(8, 26);
            this.TheoreticalUniqueBlockSetsLabel.Name = "TheoreticalUniqueBlockSetsLabel";
            this.TheoreticalUniqueBlockSetsLabel.Size = new System.Drawing.Size(144, 13);
            this.TheoreticalUniqueBlockSetsLabel.TabIndex = 33;
            this.TheoreticalUniqueBlockSetsLabel.Text = "0 unique sets can be formed.";
            // 
            // SetDesiredUniqueBlockSetsToCreateToMax
            // 
            this.SetDesiredUniqueBlockSetsToCreateToMax.Location = new System.Drawing.Point(341, 83);
            this.SetDesiredUniqueBlockSetsToCreateToMax.Name = "SetDesiredUniqueBlockSetsToCreateToMax";
            this.SetDesiredUniqueBlockSetsToCreateToMax.Size = new System.Drawing.Size(36, 22);
            this.SetDesiredUniqueBlockSetsToCreateToMax.TabIndex = 6;
            this.SetDesiredUniqueBlockSetsToCreateToMax.Text = "max";
            this.MainToolTip.SetToolTip(this.SetDesiredUniqueBlockSetsToCreateToMax, "Set the desired number of unique block sets to the theoretical maximum, or to the" +
        " software\'s maximum.");
            this.SetDesiredUniqueBlockSetsToCreateToMax.UseVisualStyleBackColor = true;
            this.SetDesiredUniqueBlockSetsToCreateToMax.Click += new System.EventHandler(this.SetDesiredUniqueBlockSetsToCreateToMax_Click);
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.BlocksTabPage);
            this.SettingsTabControl.Controls.Add(this.VariablesTabPage);
            this.SettingsTabControl.Controls.Add(this.SubgroupsTabPage);
            this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTabControl.Location = new System.Drawing.Point(3, 168);
            this.SettingsTabControl.MinimumSize = new System.Drawing.Size(444, 165);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(489, 165);
            this.SettingsTabControl.TabIndex = 1;
            this.SettingsTabControl.SelectedIndexChanged += new System.EventHandler(this.SettingsTabControl_SelectedIndexChanged);
            // 
            // BlocksTabPage
            // 
            this.BlocksTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.BlocksTabPage.Controls.Add(this.NumberOfExperimentalUnitsPerBlockNumericUpDown);
            this.BlocksTabPage.Controls.Add(this.UniqueSetsHelpPictureBox);
            this.BlocksTabPage.Controls.Add(this.DefineGroupNamesLabel);
            this.BlocksTabPage.Controls.Add(this.RememberSetsNumericUpDown);
            this.BlocksTabPage.Controls.Add(this.DesiredUniqueSetsNumericUpDown);
            this.BlocksTabPage.Controls.Add(this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton);
            this.BlocksTabPage.Controls.Add(this.BlockCountComboBox);
            this.BlocksTabPage.Controls.Add(this.SetDesiredUniqueBlockSetsToCreateToMax);
            this.BlocksTabPage.Controls.Add(this.BlockCountLabel);
            this.BlocksTabPage.Controls.Add(this.NumberOfBlockSetsToRememberLabel);
            this.BlocksTabPage.Controls.Add(this.BlockSizesAreEqualCheckBox);
            this.BlocksTabPage.Controls.Add(this.NumberOfExperimentalUnitsPerBlockLabel);
            this.BlocksTabPage.Controls.Add(this.DesiredUniqueBlockSetsToCreateLabel);
            this.BlocksTabPage.Location = new System.Drawing.Point(4, 22);
            this.BlocksTabPage.Name = "BlocksTabPage";
            this.BlocksTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.BlocksTabPage.Size = new System.Drawing.Size(481, 139);
            this.BlocksTabPage.TabIndex = 0;
            this.BlocksTabPage.Text = "Blocks";
            // 
            // NumberOfExperimentalUnitsPerBlockNumericUpDown
            // 
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Location = new System.Drawing.Point(242, 59);
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Name = "NumberOfExperimentalUnitsPerBlockNumericUpDown";
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Size = new System.Drawing.Size(135, 20);
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.TabIndex = 56;
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.ThousandsSeparator = true;
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumberOfExperimentalUnitsPerBlockNumericUpDown.ValueChanged += new System.EventHandler(this.SubgroupSizeNumericUpDown_ValueChanged);
            // 
            // UniqueSetsHelpPictureBox
            // 
            this.UniqueSetsHelpPictureBox.HelpText = resources.GetString("UniqueSetsHelpPictureBox.HelpText");
            this.UniqueSetsHelpPictureBox.HelpTextCaption = "Info: Creating block sets";
            this.UniqueSetsHelpPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("UniqueSetsHelpPictureBox.Image")));
            this.UniqueSetsHelpPictureBox.Location = new System.Drawing.Point(414, 86);
            this.UniqueSetsHelpPictureBox.Name = "UniqueSetsHelpPictureBox";
            this.UniqueSetsHelpPictureBox.Size = new System.Drawing.Size(16, 16);
            this.UniqueSetsHelpPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.UniqueSetsHelpPictureBox.TabIndex = 55;
            this.UniqueSetsHelpPictureBox.TabStop = false;
            // 
            // DefineGroupNamesLabel
            // 
            this.DefineGroupNamesLabel.AutoSize = true;
            this.DefineGroupNamesLabel.Location = new System.Drawing.Point(381, 7);
            this.DefineGroupNamesLabel.Name = "DefineGroupNamesLabel";
            this.DefineGroupNamesLabel.Size = new System.Drawing.Size(19, 13);
            this.DefineGroupNamesLabel.TabIndex = 46;
            this.DefineGroupNamesLabel.Text = "📌";
            this.MainToolTip.SetToolTip(this.DefineGroupNamesLabel, "Define group names");
            this.DefineGroupNamesLabel.Click += new System.EventHandler(this.DefineGroupNamesLabel_Click);
            // 
            // RememberSetsNumericUpDown
            // 
            this.RememberSetsNumericUpDown.Location = new System.Drawing.Point(242, 110);
            this.RememberSetsNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RememberSetsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RememberSetsNumericUpDown.Name = "RememberSetsNumericUpDown";
            this.RememberSetsNumericUpDown.Size = new System.Drawing.Size(98, 20);
            this.RememberSetsNumericUpDown.TabIndex = 8;
            this.RememberSetsNumericUpDown.ThousandsSeparator = true;
            this.MainToolTip.SetToolTip(this.RememberSetsNumericUpDown, "When creating block sets, the program will remember\r\nthis number of unique sets.\r" +
        "\n\r\nFor example: when this number is set to 100,\r\nthe best 100 created block sets" +
        " will be remembered.\r\n");
            this.RememberSetsNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.RememberSetsNumericUpDown.ValueChanged += new System.EventHandler(this.RememberSetsNumericUpDown_ValueChanged);
            // 
            // DesiredUniqueSetsNumericUpDown
            // 
            this.DesiredUniqueSetsNumericUpDown.Location = new System.Drawing.Point(242, 84);
            this.DesiredUniqueSetsNumericUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.DesiredUniqueSetsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DesiredUniqueSetsNumericUpDown.Name = "DesiredUniqueSetsNumericUpDown";
            this.DesiredUniqueSetsNumericUpDown.Size = new System.Drawing.Size(98, 20);
            this.DesiredUniqueSetsNumericUpDown.TabIndex = 5;
            this.DesiredUniqueSetsNumericUpDown.Tag = "";
            this.DesiredUniqueSetsNumericUpDown.ThousandsSeparator = true;
            this.MainToolTip.SetToolTip(this.DesiredUniqueSetsNumericUpDown, "When creating block sets, the program will stop when this number\r\nof unique sets " +
        "is created.\r\n\r\nYou can always press the \'Show result\' button when you are\r\nsatis" +
        "fied with the number of sets created.");
            this.DesiredUniqueSetsNumericUpDown.Value = new decimal(new int[] {
            8000000,
            0,
            0,
            0});
            this.DesiredUniqueSetsNumericUpDown.ValueChanged += new System.EventHandler(this.DesiredUniqueSetsNumericUpDown_ValueChanged);
            // 
            // SetDesiredUniqueBlockSetsToCreateTo99PercentButton
            // 
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Enabled = false;
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Location = new System.Drawing.Point(376, 83);
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Name = "SetDesiredUniqueBlockSetsToCreateTo99PercentButton";
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Size = new System.Drawing.Size(36, 22);
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.TabIndex = 7;
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Text = "99%";
            this.MainToolTip.SetToolTip(this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton, "Set the desired number of unique block sets to 99% of the theoretical maximum.");
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.UseVisualStyleBackColor = true;
            this.SetDesiredUniqueBlockSetsToCreateTo99PercentButton.Click += new System.EventHandler(this.SetDesiredUniqueBlockSetsToCreateTo99Percent_Click);
            // 
            // VariablesTabPage
            // 
            this.VariablesTabPage.AutoScroll = true;
            this.VariablesTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.VariablesTabPage.Controls.Add(this.NoVariablesAvailableYetLabel);
            this.VariablesTabPage.Controls.Add(this.VariableDecimalPlacesLabel);
            this.VariablesTabPage.Controls.Add(this.VariableWeightsLabel);
            this.VariablesTabPage.Controls.Add(this.VariableNamesLabel);
            this.VariablesTabPage.Location = new System.Drawing.Point(4, 22);
            this.VariablesTabPage.Name = "VariablesTabPage";
            this.VariablesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VariablesTabPage.Size = new System.Drawing.Size(481, 139);
            this.VariablesTabPage.TabIndex = 1;
            this.VariablesTabPage.Text = "Variables";
            // 
            // NoVariablesAvailableYetLabel
            // 
            this.NoVariablesAvailableYetLabel.AutoSize = true;
            this.NoVariablesAvailableYetLabel.Location = new System.Drawing.Point(10, 50);
            this.NoVariablesAvailableYetLabel.Name = "NoVariablesAvailableYetLabel";
            this.NoVariablesAvailableYetLabel.Size = new System.Drawing.Size(308, 13);
            this.NoVariablesAvailableYetLabel.TabIndex = 3;
            this.NoVariablesAvailableYetLabel.Text = "No variables exist yet. Paste data of your experimental units first.";
            // 
            // VariableDecimalPlacesLabel
            // 
            this.VariableDecimalPlacesLabel.AutoSize = true;
            this.VariableDecimalPlacesLabel.Location = new System.Drawing.Point(160, 5);
            this.VariableDecimalPlacesLabel.Name = "VariableDecimalPlacesLabel";
            this.VariableDecimalPlacesLabel.Size = new System.Drawing.Size(50, 13);
            this.VariableDecimalPlacesLabel.TabIndex = 2;
            this.VariableDecimalPlacesLabel.Text = "Decimals";
            // 
            // VariableWeightsLabel
            // 
            this.VariableWeightsLabel.AutoSize = true;
            this.VariableWeightsLabel.Location = new System.Drawing.Point(210, 5);
            this.VariableWeightsLabel.Name = "VariableWeightsLabel";
            this.VariableWeightsLabel.Size = new System.Drawing.Size(41, 13);
            this.VariableWeightsLabel.TabIndex = 1;
            this.VariableWeightsLabel.Text = "Weight";
            // 
            // VariableNamesLabel
            // 
            this.VariableNamesLabel.AutoSize = true;
            this.VariableNamesLabel.Location = new System.Drawing.Point(10, 5);
            this.VariableNamesLabel.Name = "VariableNamesLabel";
            this.VariableNamesLabel.Size = new System.Drawing.Size(35, 13);
            this.VariableNamesLabel.TabIndex = 0;
            this.VariableNamesLabel.Text = "Name";
            // 
            // SubgroupsTabPage
            // 
            this.SubgroupsTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.SubgroupsTabPage.Controls.Add(this.PreferredSubgroupSizeNumericUpDown);
            this.SubgroupsTabPage.Controls.Add(this.MarkersOfExperimentalUnitsInputColumnNumberComboBox);
            this.SubgroupsTabPage.Controls.Add(this.MarkersOfExperimentalUnitsInputColumnNumberLabel);
            this.SubgroupsTabPage.Controls.Add(this.SubgroupSizeLabel);
            this.SubgroupsTabPage.Controls.Add(this.CreateSubgroupsCheckBox);
            this.SubgroupsTabPage.Controls.Add(this.MarkersHelpPictureBox);
            this.SubgroupsTabPage.Location = new System.Drawing.Point(4, 22);
            this.SubgroupsTabPage.Name = "SubgroupsTabPage";
            this.SubgroupsTabPage.Size = new System.Drawing.Size(481, 139);
            this.SubgroupsTabPage.TabIndex = 2;
            this.SubgroupsTabPage.Text = "Subgroups";
            // 
            // PreferredSubgroupSizeNumericUpDown
            // 
            this.PreferredSubgroupSizeNumericUpDown.Enabled = false;
            this.PreferredSubgroupSizeNumericUpDown.Location = new System.Drawing.Point(234, 57);
            this.PreferredSubgroupSizeNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PreferredSubgroupSizeNumericUpDown.Name = "PreferredSubgroupSizeNumericUpDown";
            this.PreferredSubgroupSizeNumericUpDown.Size = new System.Drawing.Size(139, 20);
            this.PreferredSubgroupSizeNumericUpDown.TabIndex = 46;
            this.PreferredSubgroupSizeNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PreferredSubgroupSizeNumericUpDown.ValueChanged += new System.EventHandler(this.PreferredSubgroupSizeNumericUpDown_ValueChanged);
            this.PreferredSubgroupSizeNumericUpDown.EnabledChanged += new System.EventHandler(this.MarkersOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged);
            // 
            // MarkersOfExperimentalUnitsInputColumnNumberComboBox
            // 
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.Enabled = false;
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.FormattingEnabled = true;
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.Location = new System.Drawing.Point(234, 29);
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.Name = "MarkersOfExperimentalUnitsInputColumnNumberComboBox";
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.Size = new System.Drawing.Size(139, 21);
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.TabIndex = 43;
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.MarkersOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged);
            this.MarkersOfExperimentalUnitsInputColumnNumberComboBox.EnabledChanged += new System.EventHandler(this.MarkersOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged);
            // 
            // MarkersOfExperimentalUnitsInputColumnNumberLabel
            // 
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.AutoSize = true;
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.Location = new System.Drawing.Point(10, 32);
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.Name = "MarkersOfExperimentalUnitsInputColumnNumberLabel";
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.Size = new System.Drawing.Size(114, 13);
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.TabIndex = 42;
            this.MarkersOfExperimentalUnitsInputColumnNumberLabel.Text = "Markers are in column:";
            // 
            // SubgroupSizeLabel
            // 
            this.SubgroupSizeLabel.AutoSize = true;
            this.SubgroupSizeLabel.Location = new System.Drawing.Point(10, 59);
            this.SubgroupSizeLabel.Name = "SubgroupSizeLabel";
            this.SubgroupSizeLabel.Size = new System.Drawing.Size(208, 13);
            this.SubgroupSizeLabel.TabIndex = 41;
            this.SubgroupSizeLabel.Text = "Blocks have to be divided in subgroups of:";
            // 
            // CreateSubgroupsCheckBox
            // 
            this.CreateSubgroupsCheckBox.AutoSize = true;
            this.CreateSubgroupsCheckBox.Location = new System.Drawing.Point(234, 6);
            this.CreateSubgroupsCheckBox.Name = "CreateSubgroupsCheckBox";
            this.CreateSubgroupsCheckBox.Size = new System.Drawing.Size(109, 17);
            this.CreateSubgroupsCheckBox.TabIndex = 39;
            this.CreateSubgroupsCheckBox.Text = "Create subgroups";
            this.CreateSubgroupsCheckBox.UseVisualStyleBackColor = true;
            this.CreateSubgroupsCheckBox.CheckedChanged += new System.EventHandler(this.CreateSubgroupsCheckBox_CheckedChanged);
            // 
            // MarkersHelpPictureBox
            // 
            this.MarkersHelpPictureBox.HelpText = resources.GetString("MarkersHelpPictureBox.HelpText");
            this.MarkersHelpPictureBox.HelpTextCaption = "Info: Markers";
            this.MarkersHelpPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("MarkersHelpPictureBox.Image")));
            this.MarkersHelpPictureBox.Location = new System.Drawing.Point(376, 31);
            this.MarkersHelpPictureBox.Name = "MarkersHelpPictureBox";
            this.MarkersHelpPictureBox.Size = new System.Drawing.Size(16, 16);
            this.MarkersHelpPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MarkersHelpPictureBox.TabIndex = 45;
            this.MarkersHelpPictureBox.TabStop = false;
            // 
            // CheckForBlockSetUnicityCheckBox
            // 
            this.CheckForBlockSetUnicityCheckBox.AutoSize = true;
            this.CheckForBlockSetUnicityCheckBox.Checked = true;
            this.CheckForBlockSetUnicityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckForBlockSetUnicityCheckBox.Location = new System.Drawing.Point(249, 3);
            this.CheckForBlockSetUnicityCheckBox.Name = "CheckForBlockSetUnicityCheckBox";
            this.CheckForBlockSetUnicityCheckBox.Size = new System.Drawing.Size(207, 17);
            this.CheckForBlockSetUnicityCheckBox.TabIndex = 9;
            this.CheckForBlockSetUnicityCheckBox.Text = "Check for unicity of created block sets";
            this.MainToolTip.SetToolTip(this.CheckForBlockSetUnicityCheckBox, "Unchecking this box will significantly increase RandoMice\'s speed.\r\nHowever, you " +
        "risk finding duplicate block sets.");
            this.CheckForBlockSetUnicityCheckBox.UseVisualStyleBackColor = true;
            this.CheckForBlockSetUnicityCheckBox.CheckedChanged += new System.EventHandler(this.CheckForBlockSetUnicityCheckBox_CheckedChanged);
            // 
            // TimeRemainingLabel
            // 
            this.TimeRemainingLabel.AutoSize = true;
            this.TimeRemainingLabel.Location = new System.Drawing.Point(140, 56);
            this.TimeRemainingLabel.Name = "TimeRemainingLabel";
            this.TimeRemainingLabel.Size = new System.Drawing.Size(141, 13);
            this.TimeRemainingLabel.TabIndex = 37;
            this.TimeRemainingLabel.Tag = "";
            this.TimeRemainingLabel.Text = "Approximate time remaining: ";
            // 
            // ExportToExcelBackgroundWorker
            // 
            this.ExportToExcelBackgroundWorker.WorkerReportsProgress = true;
            this.ExportToExcelBackgroundWorker.WorkerSupportsCancellation = true;
            this.ExportToExcelBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ExportToExcelBackgroundWorker_DoWork);
            this.ExportToExcelBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ExportToFileBackgroundWorker_ProgressChanged);
            this.ExportToExcelBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ExportToFileBackgroundWorker_RunWorkerCompleted);
            // 
            // NamesOfExperimentalUnitsInputColumnNumberComboBox
            // 
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.FormattingEnabled = true;
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.Location = new System.Drawing.Point(246, 132);
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.Name = "NamesOfExperimentalUnitsInputColumnNumberComboBox";
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.Size = new System.Drawing.Size(219, 21);
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.TabIndex = 44;
            this.NamesOfExperimentalUnitsInputColumnNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.NamesOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged);
            // 
            // UniqueNamesOfExperimentalUnitsLabel
            // 
            this.UniqueNamesOfExperimentalUnitsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UniqueNamesOfExperimentalUnitsLabel.AutoSize = true;
            this.UniqueNamesOfExperimentalUnitsLabel.Location = new System.Drawing.Point(2, 134);
            this.UniqueNamesOfExperimentalUnitsLabel.Name = "UniqueNamesOfExperimentalUnitsLabel";
            this.UniqueNamesOfExperimentalUnitsLabel.Size = new System.Drawing.Size(243, 13);
            this.UniqueNamesOfExperimentalUnitsLabel.TabIndex = 44;
            this.UniqueNamesOfExperimentalUnitsLabel.Text = "Unique names of experimental units are in column:";
            // 
            // SubgroupCompositionsLabel
            // 
            this.SubgroupCompositionsLabel.AutoSize = true;
            this.SubgroupCompositionsLabel.Location = new System.Drawing.Point(246, 19);
            this.SubgroupCompositionsLabel.Name = "SubgroupCompositionsLabel";
            this.SubgroupCompositionsLabel.Size = new System.Drawing.Size(117, 13);
            this.SubgroupCompositionsLabel.TabIndex = 46;
            this.SubgroupCompositionsLabel.Text = "Subgroup compositions";
            // 
            // ProgressPercentageLabel
            // 
            this.ProgressPercentageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressPercentageLabel.AutoSize = true;
            this.ProgressPercentageLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressPercentageLabel.Location = new System.Drawing.Point(259, 108);
            this.ProgressPercentageLabel.Name = "ProgressPercentageLabel";
            this.ProgressPercentageLabel.Size = new System.Drawing.Size(21, 13);
            this.ProgressPercentageLabel.TabIndex = 48;
            this.ProgressPercentageLabel.Tag = "";
            this.ProgressPercentageLabel.Text = "0%";
            this.ProgressPercentageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExportToFileBackGroundWorker
            // 
            this.ExportToFileBackGroundWorker.WorkerReportsProgress = true;
            this.ExportToFileBackGroundWorker.WorkerSupportsCancellation = true;
            this.ExportToFileBackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ExportToFileBackgroundWorker_DoWork);
            this.ExportToFileBackGroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ExportToFileBackgroundWorker_ProgressChanged);
            this.ExportToFileBackGroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ExportToFileBackgroundWorker_RunWorkerCompleted);
            // 
            // MainToolTip
            // 
            this.MainToolTip.AutoPopDelay = 10000;
            this.MainToolTip.InitialDelay = 500;
            this.MainToolTip.ReshowDelay = 100;
            // 
            // PasteButton
            // 
            this.PasteButton.Location = new System.Drawing.Point(5, 19);
            this.PasteButton.Name = "PasteButton";
            this.PasteButton.Size = new System.Drawing.Size(131, 23);
            this.PasteButton.TabIndex = 0;
            this.PasteButton.Text = "Paste data with headers";
            this.MainToolTip.SetToolTip(this.PasteButton, resources.GetString("PasteButton.ToolTip"));
            this.PasteButton.UseVisualStyleBackColor = true;
            this.PasteButton.Click += new System.EventHandler(this.PasteButton_Click);
            // 
            // ReDefineGroupNamesLabel
            // 
            this.ReDefineGroupNamesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReDefineGroupNamesLabel.AutoSize = true;
            this.ReDefineGroupNamesLabel.Location = new System.Drawing.Point(777, 202);
            this.ReDefineGroupNamesLabel.Name = "ReDefineGroupNamesLabel";
            this.ReDefineGroupNamesLabel.Size = new System.Drawing.Size(19, 13);
            this.ReDefineGroupNamesLabel.TabIndex = 47;
            this.ReDefineGroupNamesLabel.Text = "✍";
            this.MainToolTip.SetToolTip(this.ReDefineGroupNamesLabel, "Re-define group names");
            this.ReDefineGroupNamesLabel.Click += new System.EventHandler(this.ReDefineGroupNamesLabel_Click);
            // 
            // FilterResultsPictureBox
            // 
            this.FilterResultsPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterResultsPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.FilterResultsPictureBox.Image = global::RvE_RandoMice.Properties.Resources.Filter;
            this.FilterResultsPictureBox.Location = new System.Drawing.Point(273, 7);
            this.FilterResultsPictureBox.Name = "FilterResultsPictureBox";
            this.FilterResultsPictureBox.Size = new System.Drawing.Size(16, 16);
            this.FilterResultsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterResultsPictureBox.TabIndex = 2;
            this.FilterResultsPictureBox.TabStop = false;
            this.MainToolTip.SetToolTip(this.FilterResultsPictureBox, "Filter...");
            this.FilterResultsPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterResultsPictureBox_MouseClick);
            // 
            // RandomlyAssignGroupsToBlocks
            // 
            this.RandomlyAssignGroupsToBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RandomlyAssignGroupsToBlocks.Location = new System.Drawing.Point(587, 196);
            this.RandomlyAssignGroupsToBlocks.Name = "RandomlyAssignGroupsToBlocks";
            this.RandomlyAssignGroupsToBlocks.Size = new System.Drawing.Size(188, 24);
            this.RandomlyAssignGroupsToBlocks.TabIndex = 49;
            this.RandomlyAssignGroupsToBlocks.Text = "Randomly assign blocks to groups";
            this.RandomlyAssignGroupsToBlocks.UseVisualStyleBackColor = true;
            this.RandomlyAssignGroupsToBlocks.Click += new System.EventHandler(this.RandomlyAssignBlocksToGroups_Click);
            // 
            // InputGroupBox
            // 
            this.InputGroupBox.Controls.Add(this.InputDataGridView);
            this.InputGroupBox.Controls.Add(this.ExperimentalUnitNamesHelpPictureBox);
            this.InputGroupBox.Controls.Add(this.PasteButton);
            this.InputGroupBox.Controls.Add(this.NamesOfExperimentalUnitsInputColumnNumberComboBox);
            this.InputGroupBox.Controls.Add(this.UniqueNamesOfExperimentalUnitsLabel);
            this.InputGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputGroupBox.Location = new System.Drawing.Point(3, 3);
            this.InputGroupBox.Name = "InputGroupBox";
            this.InputGroupBox.Size = new System.Drawing.Size(489, 159);
            this.InputGroupBox.TabIndex = 50;
            this.InputGroupBox.TabStop = false;
            this.InputGroupBox.Text = "Input data";
            // 
            // InputDataGridView
            // 
            this.InputDataGridView.AllowFiltering = false;
            this.InputDataGridView.AllowSorting = true;
            this.InputDataGridView.AllowUserToAddRows = false;
            this.InputDataGridView.AllowUserToDeleteRows = false;
            this.InputDataGridView.AllowViewDetails = true;
            this.InputDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.InputDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InputDataGridView.CurrentlySelectedValue = null;
            this.InputDataGridView.IsInputDataGridViewForExperiment = true;
            this.InputDataGridView.Location = new System.Drawing.Point(6, 42);
            this.InputDataGridView.MenuItemsEnabled = true;
            this.InputDataGridView.Name = "InputDataGridView";
            this.InputDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.True;
            this.InputDataGridView.ReadOnly = true;
            this.InputDataGridView.RowHeadersVisible = false;
            this.InputDataGridView.Size = new System.Drawing.Size(478, 84);
            this.InputDataGridView.TabIndex = 48;
            this.InputDataGridView.DataPasted += new System.EventHandler<RvE_RandoMice.DataPastedEventArgs>(this.InputDataGridView_DataPasted);
            // 
            // ExperimentalUnitNamesHelpPictureBox
            // 
            this.ExperimentalUnitNamesHelpPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExperimentalUnitNamesHelpPictureBox.HelpText = resources.GetString("ExperimentalUnitNamesHelpPictureBox.HelpText");
            this.ExperimentalUnitNamesHelpPictureBox.HelpTextCaption = "Info: Names of experimental units";
            this.ExperimentalUnitNamesHelpPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("ExperimentalUnitNamesHelpPictureBox.Image")));
            this.ExperimentalUnitNamesHelpPictureBox.Location = new System.Drawing.Point(468, 134);
            this.ExperimentalUnitNamesHelpPictureBox.Name = "ExperimentalUnitNamesHelpPictureBox";
            this.ExperimentalUnitNamesHelpPictureBox.Size = new System.Drawing.Size(16, 16);
            this.ExperimentalUnitNamesHelpPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ExperimentalUnitNamesHelpPictureBox.TabIndex = 54;
            this.ExperimentalUnitNamesHelpPictureBox.TabStop = false;
            // 
            // ProgressGroupBox
            // 
            this.ProgressGroupBox.Controls.Add(this.MainProgressBar);
            this.ProgressGroupBox.Controls.Add(this.ProgressPercentageLabel);
            this.ProgressGroupBox.Controls.Add(this.ProgressLabel);
            this.ProgressGroupBox.Controls.Add(this.TimeElapsedLabel);
            this.ProgressGroupBox.Controls.Add(this.TheoreticalUniqueBlockSetsLabel);
            this.ProgressGroupBox.Controls.Add(this.TimeRemainingLabel);
            this.ProgressGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressGroupBox.Location = new System.Drawing.Point(553, 3);
            this.ProgressGroupBox.MinimumSize = new System.Drawing.Size(281, 0);
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.Size = new System.Drawing.Size(292, 159);
            this.ProgressGroupBox.TabIndex = 51;
            this.ProgressGroupBox.TabStop = false;
            this.ProgressGroupBox.Text = "Progress";
            // 
            // BestBlockSetsGroupBox
            // 
            this.BestBlockSetsGroupBox.Controls.Add(this.FilterResultsPictureBox);
            this.BestBlockSetsGroupBox.Controls.Add(this.BlockSetsResultsDataGridView);
            this.BestBlockSetsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BestBlockSetsGroupBox.Enabled = false;
            this.BestBlockSetsGroupBox.Location = new System.Drawing.Point(553, 168);
            this.BestBlockSetsGroupBox.MinimumSize = new System.Drawing.Size(281, 0);
            this.BestBlockSetsGroupBox.Name = "BestBlockSetsGroupBox";
            this.BestBlockSetsGroupBox.Size = new System.Drawing.Size(292, 163);
            this.BestBlockSetsGroupBox.TabIndex = 52;
            this.BestBlockSetsGroupBox.TabStop = false;
            this.BestBlockSetsGroupBox.Text = "Best block sets";
            // 
            // BlockSetsResultsDataGridView
            // 
            this.BlockSetsResultsDataGridView.AllowFiltering = true;
            this.BlockSetsResultsDataGridView.AllowSorting = true;
            this.BlockSetsResultsDataGridView.AllowUserToAddRows = false;
            this.BlockSetsResultsDataGridView.AllowUserToDeleteRows = false;
            this.BlockSetsResultsDataGridView.AllowViewDetails = true;
            this.BlockSetsResultsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockSetsResultsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.BlockSetsResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BlockSetsResultsDataGridView.CurrentlySelectedValue = null;
            this.BlockSetsResultsDataGridView.IsInputDataGridViewForExperiment = false;
            this.BlockSetsResultsDataGridView.Location = new System.Drawing.Point(3, 23);
            this.BlockSetsResultsDataGridView.MenuItemsEnabled = true;
            this.BlockSetsResultsDataGridView.Name = "BlockSetsResultsDataGridView";
            this.BlockSetsResultsDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.False;
            this.BlockSetsResultsDataGridView.ReadOnly = true;
            this.BlockSetsResultsDataGridView.RowHeadersVisible = false;
            this.BlockSetsResultsDataGridView.Size = new System.Drawing.Size(286, 140);
            this.BlockSetsResultsDataGridView.TabIndex = 0;
            this.BlockSetsResultsDataGridView.DataPasted += new System.EventHandler<RvE_RandoMice.DataPastedEventArgs>(this.BlockSetsResultsDataGridView_DataPasted);
            this.BlockSetsResultsDataGridView.FilterByMarkersToChange += new System.EventHandler<RvE_RandoMice.EventArgsWithValue>(this.BlockSetsResultsDataGridView_FilterByMarkersToChange);
            this.BlockSetsResultsDataGridView.RemoveAllFilters += new System.EventHandler<System.EventArgs>(this.BlockSetsResultsDataGridView_RemoveAllFilters);
            this.BlockSetsResultsDataGridView.FilterByCategory += new System.EventHandler<System.EventArgs>(this.BlockSetsResultsDataGridView_FilterByCategory);
            this.BlockSetsResultsDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BlockSetsResultsDataGridView_CellClick);
            this.BlockSetsResultsDataGridView.SelectionChanged += new System.EventHandler(this.BlockSetsResultsDataGridView_SelectionChanged);
            // 
            // BlockSetDetailsGroupBox
            // 
            this.BlockSetDetailsGroupBox.Controls.Add(this.ReDefineGroupNamesLabel);
            this.BlockSetDetailsGroupBox.Controls.Add(this.BlocksDescriptivesDataGridView);
            this.BlockSetDetailsGroupBox.Controls.Add(this.SubgroupCompositionsDataGridView);
            this.BlockSetDetailsGroupBox.Controls.Add(this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView);
            this.BlockSetDetailsGroupBox.Controls.Add(this.SubgroupCompositionsLabel);
            this.BlockSetDetailsGroupBox.Controls.Add(this.BlockDescriptivesLabel);
            this.BlockSetDetailsGroupBox.Controls.Add(this.MeanCheckBox);
            this.BlockSetDetailsGroupBox.Controls.Add(this.SDCheckBox);
            this.BlockSetDetailsGroupBox.Controls.Add(this.MinCheckBox);
            this.BlockSetDetailsGroupBox.Controls.Add(this.MedianCheckBox);
            this.BlockSetDetailsGroupBox.Controls.Add(this.MaxCheckBox);
            this.BlockSetDetailsGroupBox.Controls.Add(this.ExperimentalUnitNamesInBlocksInBlockSet);
            this.BlockSetDetailsGroupBox.Controls.Add(this.RandomlyAssignGroupsToBlocks);
            this.BlockSetDetailsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BlockSetDetailsGroupBox.Enabled = false;
            this.BlockSetDetailsGroupBox.Location = new System.Drawing.Point(3, 403);
            this.BlockSetDetailsGroupBox.MinimumSize = new System.Drawing.Size(777, 176);
            this.BlockSetDetailsGroupBox.Name = "BlockSetDetailsGroupBox";
            this.BlockSetDetailsGroupBox.Size = new System.Drawing.Size(848, 226);
            this.BlockSetDetailsGroupBox.TabIndex = 53;
            this.BlockSetDetailsGroupBox.TabStop = false;
            this.BlockSetDetailsGroupBox.Text = "Block set details";
            // 
            // BlocksDescriptivesDataGridView
            // 
            this.BlocksDescriptivesDataGridView.AllowFiltering = false;
            this.BlocksDescriptivesDataGridView.AllowSorting = false;
            this.BlocksDescriptivesDataGridView.AllowUserToAddRows = false;
            this.BlocksDescriptivesDataGridView.AllowUserToDeleteRows = false;
            this.BlocksDescriptivesDataGridView.AllowViewDetails = true;
            this.BlocksDescriptivesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlocksDescriptivesDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.BlocksDescriptivesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BlocksDescriptivesDataGridView.CurrentlySelectedValue = null;
            this.BlocksDescriptivesDataGridView.IsInputDataGridViewForExperiment = false;
            this.BlocksDescriptivesDataGridView.Location = new System.Drawing.Point(558, 36);
            this.BlocksDescriptivesDataGridView.MenuItemsEnabled = true;
            this.BlocksDescriptivesDataGridView.Name = "BlocksDescriptivesDataGridView";
            this.BlocksDescriptivesDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.False;
            this.BlocksDescriptivesDataGridView.ReadOnly = true;
            this.BlocksDescriptivesDataGridView.RowHeadersVisible = false;
            this.BlocksDescriptivesDataGridView.Size = new System.Drawing.Size(216, 155);
            this.BlocksDescriptivesDataGridView.TabIndex = 52;
            // 
            // SubgroupCompositionsDataGridView
            // 
            this.SubgroupCompositionsDataGridView.AllowFiltering = false;
            this.SubgroupCompositionsDataGridView.AllowSorting = false;
            this.SubgroupCompositionsDataGridView.AllowUserToAddRows = false;
            this.SubgroupCompositionsDataGridView.AllowUserToDeleteRows = false;
            this.SubgroupCompositionsDataGridView.AllowViewDetails = true;
            this.SubgroupCompositionsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SubgroupCompositionsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.SubgroupCompositionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubgroupCompositionsDataGridView.CurrentlySelectedValue = null;
            this.SubgroupCompositionsDataGridView.IsInputDataGridViewForExperiment = false;
            this.SubgroupCompositionsDataGridView.Location = new System.Drawing.Point(249, 35);
            this.SubgroupCompositionsDataGridView.MenuItemsEnabled = true;
            this.SubgroupCompositionsDataGridView.Name = "SubgroupCompositionsDataGridView";
            this.SubgroupCompositionsDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.False;
            this.SubgroupCompositionsDataGridView.ReadOnly = true;
            this.SubgroupCompositionsDataGridView.RowHeadersVisible = false;
            this.SubgroupCompositionsDataGridView.Size = new System.Drawing.Size(292, 156);
            this.SubgroupCompositionsDataGridView.TabIndex = 51;
            // 
            // NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView
            // 
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.AllowFiltering = false;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.AllowSorting = true;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.AllowUserToAddRows = false;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.AllowUserToDeleteRows = false;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.AllowViewDetails = true;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.CurrentlySelectedValue = null;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.IsInputDataGridViewForExperiment = false;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.Location = new System.Drawing.Point(6, 35);
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.MenuItemsEnabled = true;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.Name = "NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView";
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.PastingDataFromClipboardIsAllowed = RvE_RandoMice.AllowPasting.False;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.ReadOnly = true;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.RowHeadersVisible = false;
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.Size = new System.Drawing.Size(226, 156);
            this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.TabIndex = 50;
            // 
            // HorizontalTableLayoutPanel
            // 
            this.HorizontalTableLayoutPanel.ColumnCount = 3;
            this.HorizontalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 495F));
            this.HorizontalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.HorizontalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalTableLayoutPanel.Controls.Add(this.InputGroupBox, 0, 0);
            this.HorizontalTableLayoutPanel.Controls.Add(this.SettingsTabControl, 0, 1);
            this.HorizontalTableLayoutPanel.Controls.Add(this.ProgressGroupBox, 2, 0);
            this.HorizontalTableLayoutPanel.Controls.Add(this.BestBlockSetsGroupBox, 2, 1);
            this.HorizontalTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HorizontalTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.HorizontalTableLayoutPanel.Name = "HorizontalTableLayoutPanel";
            this.HorizontalTableLayoutPanel.RowCount = 2;
            this.HorizontalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.HorizontalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.HorizontalTableLayoutPanel.Size = new System.Drawing.Size(848, 334);
            this.HorizontalTableLayoutPanel.TabIndex = 54;
            // 
            // VerticalTableLayoutPanel
            // 
            this.VerticalTableLayoutPanel.ColumnCount = 1;
            this.VerticalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalTableLayoutPanel.Controls.Add(this.HorizontalTableLayoutPanel, 0, 0);
            this.VerticalTableLayoutPanel.Controls.Add(this.BlockSetDetailsGroupBox, 0, 2);
            this.VerticalTableLayoutPanel.Controls.Add(this.ControlPanel1, 0, 1);
            this.VerticalTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerticalTableLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.VerticalTableLayoutPanel.Name = "VerticalTableLayoutPanel";
            this.VerticalTableLayoutPanel.RowCount = 3;
            this.VerticalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.VerticalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.VerticalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalTableLayoutPanel.Size = new System.Drawing.Size(854, 632);
            this.VerticalTableLayoutPanel.TabIndex = 55;
            // 
            // ControlPanel1
            // 
            this.ControlPanel1.Controls.Add(this.CheckForBlockSetUnicityCheckBox);
            this.ControlPanel1.Controls.Add(this.ShowResultsButton);
            this.ControlPanel1.Controls.Add(this.AbortRunButton);
            this.ControlPanel1.Controls.Add(this.StartRunButton);
            this.ControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlPanel1.Location = new System.Drawing.Point(3, 343);
            this.ControlPanel1.Name = "ControlPanel1";
            this.ControlPanel1.Size = new System.Drawing.Size(848, 54);
            this.ControlPanel1.TabIndex = 56;
            // 
            // ResizeFormTimer
            // 
            this.ResizeFormTimer.Interval = 10;
            this.ResizeFormTimer.Tag = "2";
            this.ResizeFormTimer.Tick += new System.EventHandler(this.ResizeFormTimer_Tick);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 656);
            this.Controls.Add(this.VerticalTableLayoutPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(870, 460);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RandoMice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.SettingsTabControl.ResumeLayout(false);
            this.BlocksTabPage.ResumeLayout(false);
            this.BlocksTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfExperimentalUnitsPerBlockNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueSetsHelpPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RememberSetsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DesiredUniqueSetsNumericUpDown)).EndInit();
            this.VariablesTabPage.ResumeLayout(false);
            this.VariablesTabPage.PerformLayout();
            this.SubgroupsTabPage.ResumeLayout(false);
            this.SubgroupsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreferredSubgroupSizeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkersHelpPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterResultsPictureBox)).EndInit();
            this.InputGroupBox.ResumeLayout(false);
            this.InputGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InputDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExperimentalUnitNamesHelpPictureBox)).EndInit();
            this.ProgressGroupBox.ResumeLayout(false);
            this.ProgressGroupBox.PerformLayout();
            this.BestBlockSetsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BlockSetsResultsDataGridView)).EndInit();
            this.BlockSetDetailsGroupBox.ResumeLayout(false);
            this.BlockSetDetailsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlocksDescriptivesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubgroupCompositionsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView)).EndInit();
            this.HorizontalTableLayoutPanel.ResumeLayout(false);
            this.VerticalTableLayoutPanel.ResumeLayout(false);
            this.ControlPanel1.ResumeLayout(false);
            this.ControlPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartRunButton;
        private System.ComponentModel.BackgroundWorker CreateBlockSetsBackgroundWorker;
        private System.Windows.Forms.Timer CreateBlockSetsTimer;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Label TimeElapsedLabel;
        private System.Windows.Forms.Button AbortRunButton;
        private System.Windows.Forms.Button ShowResultsButton;
        private System.Windows.Forms.CheckBox MeanCheckBox;
        private System.Windows.Forms.CheckBox SDCheckBox;
        private System.Windows.Forms.CheckBox MinCheckBox;
        private System.Windows.Forms.CheckBox MedianCheckBox;
        private System.Windows.Forms.CheckBox MaxCheckBox;
        private System.Windows.Forms.Label BlockCountLabel;
        private System.Windows.Forms.ComboBox BlockCountComboBox;
        private System.Windows.Forms.CheckBox BlockSizesAreEqualCheckBox;
        private System.Windows.Forms.Label NumberOfExperimentalUnitsPerBlockLabel;
        private System.Windows.Forms.Label DesiredUniqueBlockSetsToCreateLabel;
        private System.Windows.Forms.Label ExperimentalUnitNamesInBlocksInBlockSet;
        private System.Windows.Forms.Label BlockDescriptivesLabel;
        private System.Windows.Forms.Label NumberOfBlockSetsToRememberLabel;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.Label TheoreticalUniqueBlockSetsLabel;
        private System.Windows.Forms.Button SetDesiredUniqueBlockSetsToCreateToMax;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage BlocksTabPage;
        private System.Windows.Forms.TabPage VariablesTabPage;
        private System.Windows.Forms.Button SetDesiredUniqueBlockSetsToCreateTo99PercentButton;
        private System.Windows.Forms.Label VariableNamesLabel;
        private System.Windows.Forms.Label VariableDecimalPlacesLabel;
        private System.Windows.Forms.Label VariableWeightsLabel;
        private System.Windows.Forms.Label NoVariablesAvailableYetLabel;
        private System.Windows.Forms.CheckBox CheckForBlockSetUnicityCheckBox;
        private System.Windows.Forms.Label TimeRemainingLabel;
        private System.ComponentModel.BackgroundWorker ExportToExcelBackgroundWorker;
        private System.Windows.Forms.TabPage SubgroupsTabPage;
        private System.Windows.Forms.Label SubgroupSizeLabel;
        private System.Windows.Forms.CheckBox CreateSubgroupsCheckBox;
        private System.Windows.Forms.ComboBox MarkersOfExperimentalUnitsInputColumnNumberComboBox;
        private System.Windows.Forms.Label MarkersOfExperimentalUnitsInputColumnNumberLabel;
        private System.Windows.Forms.ComboBox NamesOfExperimentalUnitsInputColumnNumberComboBox;
        private System.Windows.Forms.Label UniqueNamesOfExperimentalUnitsLabel;
        private System.Windows.Forms.Label SubgroupCompositionsLabel;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStripMenuItem;
        private System.Windows.Forms.Label ProgressPercentageLabel;
        private System.Windows.Forms.ToolStripMenuItem ExportResultsToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker ExportToFileBackGroundWorker;
        private MyNumericUpDown RememberSetsNumericUpDown;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Button RandomlyAssignGroupsToBlocks;
        private System.Windows.Forms.Label DefineGroupNamesLabel;
        private System.Windows.Forms.Button PasteButton;
        private System.Windows.Forms.GroupBox InputGroupBox;
        private System.Windows.Forms.GroupBox ProgressGroupBox;
        private System.Windows.Forms.GroupBox BestBlockSetsGroupBox;
        private System.Windows.Forms.GroupBox BlockSetDetailsGroupBox;
        private MyDataGridView InputDataGridView;
        private MyDataGridView BlocksDescriptivesDataGridView;
        private MyDataGridView SubgroupCompositionsDataGridView;
        private MyDataGridView NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView;
        private MyDataGridView BlockSetsResultsDataGridView;
        private System.Windows.Forms.Label ReDefineGroupNamesLabel;
        private HelpPictureBox ExperimentalUnitNamesHelpPictureBox;
        private HelpPictureBox UniqueSetsHelpPictureBox;
        private HelpPictureBox MarkersHelpPictureBox;
        private MyNumericUpDown DesiredUniqueSetsNumericUpDown;
        private MyNumericUpDown NumberOfExperimentalUnitsPerBlockNumericUpDown;
        private MyNumericUpDown PreferredSubgroupSizeNumericUpDown;
        private System.Windows.Forms.TableLayoutPanel HorizontalTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel VerticalTableLayoutPanel;
        private System.Windows.Forms.Panel ControlPanel1;
        private System.Windows.Forms.Timer ResizeFormTimer;
        private System.Windows.Forms.ToolStripMenuItem CheckForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem ViewHelpToolStripMenuItem;
        private System.Windows.Forms.PictureBox FilterResultsPictureBox;
    }
}