//    RandoMice
//    Copyright(C) 2019-2022 R. van Eenige, Leiden University Medical Center
//    and individual contributors.
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program. If not, see <https://www.gnu.org/licenses/>.
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace RvE_RandoMice
{
    public partial class MainForm : Form
    {
        private readonly Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
        private readonly string ArgumentFilePath = null;
        private bool SuppressWarningThatSubgroupSizesMayHaveChanged { get; set; } = true;
        private bool IsCreatingSubgroups { get; set; } = false;

        public MainForm(string[] arguments)
        {
            InitializeComponent();

            //display version in form text
            this.Text += " " + Global.CurrentAssemblyVersion;
            this.Size = this.MinimumSize;

            //fill a list with all backgroundworkers and fill a list with all descriptives checkboxes
            Global.BackgroundWorkers = new List<BackgroundWorker> { CreateBlockSetsBackgroundWorker, ExportToExcelBackgroundWorker, ExportToFileBackGroundWorker };
            Global.DescriptiveCheckboxes = new List<CheckBox> { MeanCheckBox, SDCheckBox, MinCheckBox, MedianCheckBox, MaxCheckBox };

            //briefly display disclaimer
            ShowAboutForm(duration: Global.DisplayAboutFormOnStartDuration);

            //copy all settings from Properties.Settings.Default and from controls to Global.Settings
            //the Variables for the constructor are properties of the MainForm and must be passed to be known by Settings
            Global.Settings = new Settings((int)DesiredUniqueSetsNumericUpDown.Value,
                                           (int)DesiredUniqueSetsNumericUpDown.Maximum,
                                           (int)RememberSetsNumericUpDown.Value,
                                           CheckForBlockSetUnicityCheckBox.Checked,
                                           TheoreticalUniqueBlockSetsLabel.Text,
                                           TimeElapsedLabel.Text,
                                           TimeRemainingLabel.Text,
                                           ProgressLabel.Text,
                                           ProgressPercentageLabel.Text);

            //weekly check for updates
            if (DateTime.Now.Subtract(Global.Settings.LastCheckedForUpdate).TotalDays > 7)
                CheckForUpdates(displayNoUpdateAvailableWarning: false, displayErrorMessage: false);

            //create an empty experiment and set various Global values for use
            Global.CurrentExperiment = new Experiment();
            Global.CheckForBlockSetUnicity = Global.Settings.DefaultCheckForBlockSetUnicityCheckBoxChecked;
            Global.DesiredUniqueSets = Global.Settings.DefaultDesiredUniqueBlockSets;
            Global.RememberSets = Global.Settings.DefaultRememberSets;

            //set interval of timer
            CreateBlockSetsTimer.Interval = Global.Settings.CreateBlockSetsTimerInterval;

            //move some labels in the Variable TabPage according to settings
            VariableNamesLabel.Left = Global.Settings.DefaultVariableControl.Left;
            VariableDecimalPlacesLabel.Left = Global.Settings.DefaultVariableControl.Left + Global.Settings.DefaultVariableNameTextBoxWidth;
            VariableWeightsLabel.Left = Global.Settings.DefaultVariableControl.Left + Global.Settings.DefaultVariableNameTextBoxWidth + Global.Settings.DefaultVariableDecimalPlacesNumericUpDownWidth;

            //save the file path of the .rndm file that may have been passed as an argument for loading in MainForm_Load
            //the file cannot be loaded now directly because not all required components of MainForm have been instantiated yet
            if (arguments.Length != 0)
                ArgumentFilePath = arguments[0];
        }

        public int? SelectedBlockSetIndex
        {
            get
            {
                try
                {
                    int selectedBlockSetIndex = Convert.ToInt32(BlockSetsResultsDataGridView.Rows[BlockSetsResultsDataGridView.CurrentRow.Index].Cells[0].Value.ToString()) - 1;
                    return selectedBlockSetIndex;
                }
                catch
                {
                    return null; //default
                }
            }
        }

        #region formEvents
        /// <summary>
        /// Resizes working area if needed. Loads the .rndm file, if passed as an argument.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //resize screen if needed
            if (this.Width > Screen.GetWorkingArea(this).Width && this.Height > Screen.GetWorkingArea(this).Height)
            {
                this.Width = Screen.GetWorkingArea(this).Width;
                this.Height = Screen.GetWorkingArea(this).Height;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                if (this.Width > Screen.GetWorkingArea(this).Width)
                    this.Width = Screen.GetWorkingArea(this).Width;
                else if (this.Height > Screen.GetWorkingArea(this).Height)
                    this.Height = Screen.GetWorkingArea(this).Height;
            }

            //load the .rndm file that may have been passed as an argument
            if (ArgumentFilePath != null)
                LoadAndProcessExperiment(ArgumentFilePath, suppressLoadSuccessMessage: true);
        }

        /// <summary>
        /// Prompts user if save is wanted.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveState saveState = SaveExperimentIfPossibleNeededAndWanted("Closing...");

            e.Cancel = (saveState == SaveState.Cancelled);

            if (saveState != SaveState.NotNeeded)
                ShowSaveMessageBox(saveState); //don't show this message if no saveable data exists or when "no" is pressed by the user when asked to save data
        }

        /// <summary>
        /// Displays the move DragDropEffect on DragEnter.
        /// </summary>
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Loads data of a drag-and-dropped file.
        /// </summary>
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //get the file list, and try to load the first file in the sequence
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadAndProcessExperiment(filePaths.First(), suppressLoadSuccessMessage: true);
            }
            catch
            {
                //error message is displayed in LoadExperiment(), so do nothing here
            }
        }

        /// <summary>
        /// Handle an UI Exception by showing an error message to the user,
        /// and try to rescue any data by asking the user to save the results.
        /// </summary>
        public static void MainForm_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            DialogResult result = DialogResult.Cancel; //default value

            try
            {
                string errorMessage = "A fatal error occurred and the program will close." +
                    "\nPlease try again or contact your adminstrator." +
                    "\n\nIf the error is recurrent, please consider posting the issue on the RandoMice GitHub page.";

                result = MessageBox.Show(errorMessage, "Fatal error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                try
                {
                    MainForm mainForm = Application.OpenForms[0] as MainForm;
                    ShowSaveMessageBox(mainForm.SaveExperimentIfPossibleNeededAndWanted("Do you want to try and rescue any results by saving?", 
                        forceSave: false, forceAskUserForFilePath: true), suppressSaveNotNeededMessage: true);
                }
                catch
                {
                    //save failed. No action needed
                }
            }
            catch
            {
                //do nothing, and exit application below
            }

            Application.Exit();
        }

        private void BlockSetsResultsDataGridView_DataPasted(object sender, DataPastedEventArgs e)
        {
            ResizeFormTimer.Start();
        }

        private void ResizeFormTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height < Global.Settings.MainFormWithResultsSize.Height)
                this.Height += Int32.Parse(ResizeFormTimer.Tag.ToString());
            else
                ResizeFormTimer.Stop();
        }

        #endregion

        #region saveAndLoadExperiment

        /// <summary>
        /// Checks if a finished experiment can be saved, has changed since last save
        /// and if user wants to save the experiment. Then, saves experiment accordingly.
        /// </summary>
        /// <param name="dialogTitle">A string containing the dialog title that is presented to the user.</param>
        /// <param name="forceSave">Optional bool which is true if a save needs to be forced, even if the finished experiment has not changed since last save. Default is false.</param>
        /// <param name="forceAskUserForFilePath">Optional bool which is true if the user is required to provide a (new) SaveFilePath.</param>
        /// <returns>A SaveState value.</returns>
        private SaveState SaveExperimentIfPossibleNeededAndWanted(string dialogTitle = "Save results", bool forceSave = false, bool forceAskUserForFilePath = false)
        {
            //check if a finished experiment exists, and check if it has changed since last save.
            if (Global.FinishedExperiment != null)
            {
                if (forceSave)
                    return Global.FinishedExperiment.Save(forceAskUserForFilePath);
                else if (Global.FinishedExperiment.HasChangedSinceLastSave)
                {
                    //if the finished experiment has changed since last save, ask user if a new save is needed.
                    DialogResult dialogResult = MessageBox.Show("Do you want to save your results?", dialogTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            return Global.FinishedExperiment.Save(forceAskUserForFilePath);
                        case DialogResult.No:
                            return SaveState.NotNeeded;
                        case DialogResult.Cancel:
                            return SaveState.Cancelled;
                    }
                }
            }

            return SaveState.NotNeeded;
        }

        /// <summary>
        /// Prompts user and loads a finished Experiment if the user selects a valid file location.
        /// </summary>
        /// <returns>An enum LoadState which reflects if the load was successful or not.</returns>
        private LoadState LoadExperiment(string filePath = "")
        {
            LoadState loadState = LoadState.Cancelled; //default value

            if (filePath == string.Empty)
            {
                //ask user what file to deserialize
                OpenFileDialog LoadExperimentOpenFileDialog = new OpenFileDialog
                {
                    Filter = "RandoMice files (*.rndm)|*.rndm"
                };

                if (LoadExperimentOpenFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = LoadExperimentOpenFileDialog.FileName;
                else
                    return loadState; //user pressed cancel
            }

            try
            {
                IFormatter formatter = new BinaryFormatter();

                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    //try to deserialize finishedExperiment from stream
                    Global.FinishedExperiment = (Experiment)formatter.Deserialize(stream);
                    Global.FinishedExperiment.ResetExperimentHasChangedProperties();
                    Global.FinishedExperiment.SaveFilePath = filePath;

                    loadState = LoadState.Success;
                }
            }
            catch
            {
                loadState = LoadState.Failed;
                MessageBox.Show("Could not open the specified file. Please try again or select a different file.", "Failed to load data.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return loadState;
        }

        #endregion

        #region manipulateControls

        /// <summary>
        /// Finds all Child Controls of a provided Parent Control.
        /// </summary>
        /// <param name="parentControl">A Control of which its Child Controls should be returned.</param>
        /// <param name="includeContainerControls">A bool indicating if the Parent Control must be returned in addition to its Child Controls.</param>
        /// <returns>A list of Child Controls of a Parent Control (optionally including the Parent Control itself),
        /// or only the Parent Control if no Child Controls are present.</returns>
        private List<Control> GetChildControls(Control parentControl, bool includeContainerControls)
        {
            List<Control> childControls = new List<Control>(parentControl.Controls.OfType<Control>());
            List<Control> result = new List<Control>();

            if (childControls.Count == 0 || parentControl.GetType() == typeof(DataGridView) || parentControl.GetType() == typeof(MyNumericUpDown) || parentControl.GetType() == typeof(MyDataGridView))
                result.Add(parentControl);
            else
            {
                foreach (Control childControl in childControls)
                    result.AddRange(GetChildControls(childControl, includeContainerControls));

                if (includeContainerControls)
                    result.Add(parentControl);
            }

            return result;
        }

        /// <summary>
        /// Enables or disables all controls in the current Form and its containers, except for Labels and containers themselves.
        /// </summary>
        /// <param name="enableOrDisable">A bool which should reflect the desired control.Enabled state.</param>
        /// <returns>A list of tuples containing each altered Control and its original Enabled state.</returns>
        private List<(Control alteredControls, bool originalEnabledState)> EnableOrDisableAllControls(EnableOrDisable enableOrDisable)
        {
            //add all controls within the current Form
            List<Control> formControls = new List<Control>(this.Controls.OfType<Control>());
            List<Control> allControls = new List<Control>();
            List<(Control Control, bool OriginalEnabledState)> alteredControlsAndOriginalEnabledStates = new List<(Control Control, bool OriginalEnabledState)>();
            bool includeContainerControls = enableOrDisable == EnableOrDisable.Enable;

            //add all controls within container controls
            foreach (Control control in formControls)
                allControls.AddRange(GetChildControls(control, includeContainerControls)); //also include container controls themselves if the control should be enabled

            //enable or disable all controls, except for labels
            foreach (Control control in allControls)
            {
                if (control.GetType() != typeof(Label) && !control.IsDisposed)
                {
                    alteredControlsAndOriginalEnabledStates.Add((control, control.Enabled));
                    control.Invoke(new Action(() => { control.Enabled = Convert.ToBoolean(enableOrDisable); }));
                }
            }

            return alteredControlsAndOriginalEnabledStates;
        }

        /// <summary>
        /// Reverts the Enabled state of each Control from a list of tuples.
        /// </summary>
        /// <param name="controlsAndOriginalEnabledStates">A list of tuples containing each Control and its OriginalEnabledState that needs to be reverted.</param>
        private void RevertEnabledStateOfControls(List<(Control Control, bool OriginalEnabledState)> controlsAndOriginalEnabledStates)
        {
            foreach (var controlAndOriginalEnabledState in controlsAndOriginalEnabledStates)
                controlAndOriginalEnabledState.Control.Invoke(new Action(() => { controlAndOriginalEnabledState.Control.Enabled = controlAndOriginalEnabledState.OriginalEnabledState; }));
        }

        /// <summary>
        /// Enables or disables one or more desired controls in the current Form.
        /// </summary>
        /// <param name="enableOrDisable">A bool which should reflect the desired control.Enabled state.</param>
        /// <param name="controls">An array of controls which should be enabled or disabled.</param>
        private void EnableOrDisableControls(EnableOrDisable enableOrDisable, Control[] controls)
        {
            foreach (Control control in controls)
                control.Invoke(new Action(() => { control.Enabled = Convert.ToBoolean(enableOrDisable); }));
        }

        /// <summary>
        /// Resets existing controls, and then adds new or removes existing controls
        /// in the Variable-tabpage that are needed for the user
        /// to change the number of decimals and weight of each Variable.
        /// </summary>
        /// <param name="oldVariableCount">The previous number of Variables.</param>
        /// <param name="newVariableCount">The new number of Variables.</param>
        public void SetVariableControls(int oldVariableCount, int newVariableCount)
        {
            int topOfNewControls = Global.Settings.DefaultVariableControl.Top + (Global.Settings.DefaultVariableControl.Height * oldVariableCount);

            //hide or unhide warning label that mentions that no Variables yet exist
            NoVariablesAvailableYetLabel.Visible = (newVariableCount <= 0);

            //firstly, reset values of existing controls according to currentExperiment.Variables
            UpdateValuesInVariableControls(oldVariableCount, newVariableCount);

            //secondly, remove any surplus controls, if needed (when oldVariableCount > newVariableCount)
            for (int i = newVariableCount; i < oldVariableCount; i++)
            {
                //remove surplus controls
                SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls.RemoveByKey(Global.Settings.VariableNameTextBoxNameBasis + i.ToString());
                SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls.RemoveByKey(Global.Settings.VariableDecimalPlacesNumericUpDownNameBasis + i.ToString());
                SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls.RemoveByKey(Global.Settings.VariableWeightNumericUpDownNameBasis + i.ToString());
            }

            //thirdly, add new controls, if needed (when newVariableCount > oldVariableCount)
            for (int i = oldVariableCount; i < newVariableCount; i++)
            {
                TextBox newVariableNameTextBox = new TextBox
                {
                    Name = Global.Settings.VariableNameTextBoxNameBasis + i.ToString(),
                    Text = Global.CurrentExperiment.ActiveVariables[i].Name,
                    Left = Global.Settings.DefaultVariableControl.Left,
                    Top = topOfNewControls,
                    Width = Global.Settings.DefaultVariableNameTextBoxWidth,
                    ReadOnly = true,
                    BackColor = System.Drawing.SystemColors.Window
                };

                NumericUpDown newVariableDecimalPlacesNumericUpDown = new MyNumericUpDown
                {
                    Name = Global.Settings.VariableDecimalPlacesNumericUpDownNameBasis + i.ToString(),
                    Value = Global.CurrentExperiment.ActiveVariables[i].DecimalPlaces,
                    Minimum = 0,
                    Maximum = Math.Min(Global.Settings.VariableDecimalPlaces.MaxValue, byte.MaxValue), //variable decimal places are of type byte, so the value must not exceed byte.MaxValue, 
                    Left = Global.Settings.DefaultVariableControl.Left + newVariableNameTextBox.Width,
                    Top = topOfNewControls,
                    Width = Global.Settings.DefaultVariableDecimalPlacesNumericUpDownWidth
                };

                NumericUpDown newVariableWeightNumericUpDown = new MyNumericUpDown
                {
                    Name = Global.Settings.VariableWeightNumericUpDownNameBasis + i.ToString(),
                    Value = (decimal)Global.CurrentExperiment.ActiveVariables[i].Weight,
                    DecimalPlaces = 2,
                    Minimum = (decimal)0.01, //minimum must be bigger than 0, else BlockSet.CalculateRank() will always return infinite
                    Maximum = Math.Min(Global.Settings.MaxVariableWeight, long.MaxValue), //variable weights are of type double, so the value must not exceed double.MaxValue.
                                                                                          //However, converting double.MaxValue to decimal is tricky, so we use long.MaxValue which should suffice
                    Left = Global.Settings.DefaultVariableControl.Left + newVariableNameTextBox.Width + newVariableDecimalPlacesNumericUpDown.Width,
                    Top = topOfNewControls,
                    Width = Global.Settings.DefaultVariableWeightNumericUpDownWidth,
                };

                //set ValueChanged events
                newVariableDecimalPlacesNumericUpDown.ValueChanged += NewVariableDecimalPlacesNumericUpDown_ValueChanged;
                newVariableWeightNumericUpDown.ValueChanged += NewVariableWeightNumericUpDown_ValueChanged;

                //add new controls to the Variables-TabPage
                SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls.AddRange(new Control[] { newVariableNameTextBox, newVariableDecimalPlacesNumericUpDown, newVariableWeightNumericUpDown });

                topOfNewControls += Global.Settings.DefaultVariableControl.Height;
            }
        }

        /// <summary>
        /// Updates the values in the Variable controls in the Variable-TabPage.
        /// </summary>
        /// <param name="oldVariableCount">The previous number of Variables.</param>
        /// <param name="newVariableCount">The new number of Variables.</param>
        private void UpdateValuesInVariableControls(int oldVariableCount, int newVariableCount)
        {
            for (int i = 0; i < oldVariableCount && i < newVariableCount; i++)
            {
                //reset Variable name
                SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[Global.Settings.VariableNameTextBoxNameBasis + i.ToString()].Text = Global.CurrentExperiment.ActiveVariables[i].Name;

                //get control containing the current Variable's decimal places
                NumericUpDown VariableDecimalPlacesNumericUpDown = SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[Global.Settings.VariableDecimalPlacesNumericUpDownNameBasis + i.ToString()] as NumericUpDown;

                //get control containing the current Variable's weight
                NumericUpDown VariableWeightNumericUpDown = SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[Global.Settings.VariableWeightNumericUpDownNameBasis + i.ToString()] as NumericUpDown;

                bool errorEncountered = false;

                try
                {
                    //reset the current Variable's decimal places and weight
                    VariableWeightNumericUpDown.Value = (decimal)Global.CurrentExperiment.ActiveVariables[i].Weight;
                    VariableDecimalPlacesNumericUpDown.Value = Global.CurrentExperiment.ActiveVariables[i].DecimalPlaces;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    errorEncountered = true;
                    VariableWeightNumericUpDown.Value = 1; //default VariableWeight
                    VariableDecimalPlacesNumericUpDown.Value = Global.Settings.VariableDecimalPlaces.DefaultValue; //default number of decimals
                }

                if (errorEncountered)
                    MessageBox.Show("Because of an exception, some Variable weights and decimal places are reset to default.\nPlease review the Variable settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Find the Variable index from the NumericUpDown's name and
        /// sets the DecimalPlaces of the corresponding Variable in currentExperiment.
        /// </summary>
        private void NewVariableDecimalPlacesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown senderNumericUpDown = sender as NumericUpDown;
                int VariableIndex = int.Parse(senderNumericUpDown.Name.Replace(Global.Settings.VariableDecimalPlacesNumericUpDownNameBasis, string.Empty)); //find Variable index from the NumericUpDown's name
                Global.CurrentExperiment.ActiveVariables[VariableIndex].DecimalPlaces = (byte)senderNumericUpDown.Value; //set the DecimalPlaces of the corresponding Variable in currentExperiment
            }
            catch
            {
                //do nothing
            }
        }

        private void SettingsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SettingsTabControl.SelectedTab.Name == Global.Settings.SubgroupSizesTabPageName)
                SuppressWarningThatSubgroupSizesMayHaveChanged = false;
        }

        /// <summary>
        /// Find the Variable index from the NumericUpDown's name and
        /// sets the Weight of the corresponding Variable in currentExperiment.
        /// </summary>
        private void NewVariableWeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown senderNumericUpDown = sender as NumericUpDown;
                int VariableIndex = int.Parse(senderNumericUpDown.Name.Replace(Global.Settings.VariableWeightNumericUpDownNameBasis, string.Empty)); //find Variable index from the NumericUpDown's name
                Global.CurrentExperiment.ActiveVariables[VariableIndex].Weight = (double)senderNumericUpDown.Value; //set the Weight of the corresponding Variable in currentExperiment
            }
            catch
            {
                //do nothing
            }
        }

        /// <summary>
        /// Adds a ComboBox to the subgroup-tabpage for a given block.
        /// </summary>
        /// <param name="currentBlockNumber">A one-based integer containing the block number for which controls need to be added.</param>
        /// <param name="preferredSubgroupSize">The preferred value of new ComboBoxes,
        /// which corresponds with the preferred subgroup size.</param>
        /// <param name="scrollNewControlsIntoView">A boolean which is should be true if the newly created controls should be scrolled into view.</param>
        /// <param name="lastSubgroupNumberOfCurrentBlock">An optional zero-based integer containing the last subgroup number of the current block.</param>
        /// <remarks>An example: this function is called when subgroups should be made
        /// and the number of blocks changes.
        /// The lastSubgroupNumberOfCurrentBlock may be provided if many subgroup controls should be added shortly after each other to increase performance.</remarks>
        private void AddSubgroupControl(int currentBlockNumber, int preferredSubgroupSize, bool scrollNewControlsIntoView = false, int? lastSubgroupNumberOfCurrentBlock = null)
        {            
            if (SettingsTabControl.TabPages.ContainsKey(Global.Settings.SubgroupSizesTabPageName))
            {
                TabPage subgroupSizesTabPage = SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName];
                
                int verticalScrollCorrection = subgroupSizesTabPage.AutoScrollPosition.Y;
                int horizontalScrollCorrection = subgroupSizesTabPage.AutoScrollPosition.X;
                int top = Global.Settings.DefaultTabPageMargin
                    + ((Global.Settings.DefaultControl.Height + 1) * currentBlockNumber)
                    + verticalScrollCorrection;

                Button addControlButtonOfCurrentBlock = new Button();

                //if the last subgroup number is not provided,
                //find the last subgroup number within the current block by checking the names of all NumericUpDowns
                if (lastSubgroupNumberOfCurrentBlock == null)
                {
                    lastSubgroupNumberOfCurrentBlock = 0;

                    if (subgroupSizesTabPage.Controls.OfType<Label>().Where(label => label.Name.Contains(Global.Settings.BlockNumberLabelNameBasis + currentBlockNumber.ToString())).FirstOrDefault() != null)
                    {
                        lastSubgroupNumberOfCurrentBlock = subgroupSizesTabPage.Controls.OfType<NumericUpDown>()
                            .Where(numericUpDown => numericUpDown.Name.Contains(Global.Settings.SubgroupSizeNumericUpDownNameBasis + currentBlockNumber.ToString())
                                && int.Parse(numericUpDown.Name.Split('.')[1]) > lastSubgroupNumberOfCurrentBlock)
                            .Select(numericUpDown => int.Parse(numericUpDown.Name.Split('.')[1])).LastOrDefault();
                    }
                    else
                    {
                        //subgroupNumber remains 0 (thus: no controls yet exist for this subgroup)
                    }
                }

                //if no comboboxes exist yet for the current block,
                //add a label to the tabpage to display the block number
                if (lastSubgroupNumberOfCurrentBlock == 0)
                {
                    Label blockLabel = new Label
                    {
                        Name = Global.Settings.BlockNumberLabelNameBasis + currentBlockNumber.ToString(),
                        Text = "Block " + currentBlockNumber.ToString(),
                        Width = Global.Settings.DefaultSubgroupBlockLabelWidth,
                        Top = top,
                        Left = Global.Settings.DefaultTabPageMargin,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleLeft,
                    };

                    subgroupSizesTabPage.Controls.Add(blockLabel);
                }

                //the new subgroupNumber for which to add a new ComboBox is the last subgroup number within the current block + 1
                int newSubgroupNumber = (int)lastSubgroupNumberOfCurrentBlock + 1;

                //check if a label containing the new subgroupNumber exists. If not, create a label.
                if (subgroupSizesTabPage.Controls.OfType<Label>().Where(label => label.Name == Global.Settings.SubgroupNumberLabelNameBasis + newSubgroupNumber.ToString()).Count() == 0)
                {
                    //if subgroupnumber == 1, add the text "Subgroup " to the subgroup label, and align the text right
                    //else, use the following defaults
                    string optionalSubgroupText = "";
                    ContentAlignment alignment = ContentAlignment.MiddleCenter;
                    int subgroupLabelWidth = Global.Settings.DefaultSubgroupNumericUpDownWidth;
                    int subgroupLabelLeft = Global.Settings.DefaultTabPageMargin + Global.Settings.DefaultSubgroupBlockLabelWidth + ((newSubgroupNumber - 1) * Global.Settings.DefaultSubgroupNumericUpDownWidth) + horizontalScrollCorrection;
                    
                    if (newSubgroupNumber == 1)
                    {
                        optionalSubgroupText = "Subgroup ";
                        int optionalSubgroupTextWidth = (int)TimeRemainingLabel.CreateGraphics().MeasureString(optionalSubgroupText, TimeRemainingLabel.Font).Width;
                        alignment = ContentAlignment.MiddleRight;
                        subgroupLabelLeft -= optionalSubgroupTextWidth + subgroupLabelWidth / 2; //make sure the subgroupNumber ("1") is still aligned middle compared to the numericUpDowns
                        subgroupLabelWidth += optionalSubgroupTextWidth;
                    }

                    //add label with subgroupnumber
                    Label subgroupLabel = new Label
                    {
                        Name = Global.Settings.SubgroupNumberLabelNameBasis + newSubgroupNumber.ToString(),
                        Text = optionalSubgroupText + newSubgroupNumber.ToString(),
                        Width = subgroupLabelWidth,
                        Top = verticalScrollCorrection,
                        Left = subgroupLabelLeft,
                        AutoSize = false,
                        TextAlign = alignment
                    };

                    subgroupSizesTabPage.Controls.Add(subgroupLabel);
                }

                //check if the desired NumericUpDown already exists
                string nameOfNumericUpDownToFind = Global.Settings.SubgroupSizeNumericUpDownNameBasis + currentBlockNumber.ToString() + "." + newSubgroupNumber.ToString();
                NumericUpDown newSubgroupSizeNumericUpDown = subgroupSizesTabPage.Controls.OfType<NumericUpDown>().Where(control => control.Name == nameOfNumericUpDownToFind).FirstOrDefault();

                if (newSubgroupSizeNumericUpDown == null)
                {
                    //add a new control
                    newSubgroupSizeNumericUpDown = new MyNumericUpDown
                    {
                        Name = Global.Settings.SubgroupSizeNumericUpDownNameBasis + currentBlockNumber.ToString() + "." + newSubgroupNumber.ToString(),
                        Top = top,
                        Width = Global.Settings.DefaultSubgroupNumericUpDownWidth,
                        Left = Global.Settings.DefaultTabPageMargin + Global.Settings.DefaultSubgroupBlockLabelWidth + ((newSubgroupNumber - 1) * Global.Settings.DefaultSubgroupNumericUpDownWidth) + horizontalScrollCorrection,
                        Minimum = 1,
                        Increment = 1,
                        Maximum = Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count, short.MaxValue) //subgroup sizes are of type short, so the value must not exceed short.MaxValue
                    };

                    newSubgroupSizeNumericUpDown.MouseUp += SubgroupSizeNumericUpDown_MouseClick;

                    subgroupSizesTabPage.Controls.Add(newSubgroupSizeNumericUpDown);
                }
                else
                    newSubgroupSizeNumericUpDown.Maximum = Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count, short.MaxValue); //subgroup sizes are of type short, so the value must not exceed short.MaxValue
                
                //set index of the new combobox for the selected item to match the preferred subgroup size
                if (preferredSubgroupSize < newSubgroupSizeNumericUpDown.Maximum && preferredSubgroupSize >= newSubgroupSizeNumericUpDown.Minimum)
                    newSubgroupSizeNumericUpDown.Value = preferredSubgroupSize;

                //find a button of the current block that allows the user to add new subgroup controls
                addControlButtonOfCurrentBlock = subgroupSizesTabPage.Controls.OfType<Button>()
                    .Where(button => button.Name.Contains(Global.Settings.AddSubgroupControlsButtonNameBasis + currentBlockNumber.ToString())).FirstOrDefault();

                //if the button doesn't exist yet, create one.
                if (addControlButtonOfCurrentBlock == null)
                {
                    addControlButtonOfCurrentBlock = new Button(); //the next line throws a NullReferenceException if the new Button has not yet been initialized.
                    addControlButtonOfCurrentBlock = new Button()
                    {
                        Name = Global.Settings.AddSubgroupControlsButtonNameBasis + currentBlockNumber.ToString(),
                        Top = top - 1, //for some reason the button is normally placed one pixel too low, so we must correct for that.
                        Text = "+",
                        Width = addControlButtonOfCurrentBlock.Height,
                        Left = newSubgroupSizeNumericUpDown.Left + Global.Settings.DefaultSubgroupNumericUpDownWidth,

                    };

                    addControlButtonOfCurrentBlock.Click += AddSubgroupComboboxButton_Click;
                    subgroupSizesTabPage.Controls.Add(addControlButtonOfCurrentBlock);
                }
                else
                {
                    //else, shift the existing button right, and optionally scroll it into view
                    addControlButtonOfCurrentBlock.Left = newSubgroupSizeNumericUpDown.Left + Global.Settings.DefaultSubgroupNumericUpDownWidth;

                    if (scrollNewControlsIntoView)
                        subgroupSizesTabPage.ScrollControlIntoView(addControlButtonOfCurrentBlock);
                }
            }
        }

        /// <summary>
        /// Clears a given ComboBox, adds a range of items and sets index as preferred.
        /// Optionally, an extra item can be inserted at a given index.
        /// </summary>
        /// <param name="comboboxToReset">The target ComboBox.</param>
        /// <param name="newMinValue">The value of the first integer in the sequence to add to the ComboBox.</param>
        /// <param name="newItemsCount">The number of sequential integers to add to the ComboBox.</param>
        /// <param name="defaultSelectedIndex">The index that is to be selected by default, if possible.</param>
        /// <param name="optionalExtraItem">An optional string to insert in at a given index.</param>
        /// <param name="insertOptionalExtraItemAtIndex">The index at which to insert an optional given string.</param>
        private void ResetComboBox(ComboBox comboboxToReset, int newMinValue, int newItemsCount, int defaultSelectedIndex = -2, string optionalExtraItem = "", int insertOptionalExtraItemAtIndex = 0)
        {
            int originalIndex = comboboxToReset.SelectedIndex;

            //clear combobox and add new range
            comboboxToReset.Items.Clear();

            if (newItemsCount < newMinValue)
                return;
            else
                comboboxToReset.Items.AddRange(Enumerable.Range(newMinValue, newItemsCount).Select(value => value.ToString()).ToArray());
            
            //optionally, add an extra item to the combobox
            if (optionalExtraItem != string.Empty && insertOptionalExtraItemAtIndex < comboboxToReset.Items.Count)
                comboboxToReset.Items.Insert(insertOptionalExtraItemAtIndex, optionalExtraItem);

            //set index to preferred value, if possible
            if (comboboxToReset.Items.Count - 1 >= originalIndex)
                comboboxToReset.SelectedIndex = originalIndex;

            //reset selectedIndex to default, if needed and possible
            if (defaultSelectedIndex != -2 && defaultSelectedIndex < comboboxToReset.Items.Count && comboboxToReset.SelectedIndex == -1)
                comboboxToReset.SelectedIndex = defaultSelectedIndex;
        }
        
        private void AddSubgroupComboboxButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;

            int currentBlockNumber = int.Parse(senderButton.Name.Substring(Global.Settings.AddSubgroupControlsButtonNameBasis.Length));
            AddSubgroupControl(currentBlockNumber, (Int32)PreferredSubgroupSizeNumericUpDown.Value, scrollNewControlsIntoView: true);
        }
        #endregion

        #region createExperimentalUnits,Blocks,SubgroupsAndVariables
        private void InputDataGridView_DataPasted(object sender, DataPastedEventArgs e)
        {
            InterpretPastedData(e.WarnUserForInvalidDataPoints, e.AskUserIfDatesShouldBeConvertedToValues);
        }

        /// <summary>
        /// Clears any existing ExperimentalUnits. Then
        /// iterates through each row in the inputDataGridView,
        /// creates new ExperimentalUnits that contain all Variable values,
        /// and adds the ExperimentalUnits to currentExperiment.
        /// </summary>
        private void CreateExperimentalUnitsOfCurrentExperiment()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.CurrentExperiment.AllExperimentalUnits.Clear();
        
            for (int i = 0; i < InputDataGridView.RowCount; i++)
            {
                if (Global.NamesOfExperimentalUnitsInputColumnNumber < InputDataGridView.Rows[i].Cells.Count)
                {
                    //The sequential ID of the ExperimentalUnits MUST start at (char)1, not (char)0, as '\0' is interpreted as the end of a string.
                    //This would create a problem when hashing the sets
                    ExperimentalUnit newExperimentalUnit = new ExperimentalUnit((char)(i + 1), InputDataGridView.Rows[i].Cells[Global.NamesOfExperimentalUnitsInputColumnNumber].Value.ToString());
                    
                    for (int j = 0; j < InputDataGridView.ColumnCount; j++)
                    {
                        //go through all columns and add all values to the current ExperimentalUnit
                        if (Global.CurrentExperiment.ExperimentalUnitsHaveMarkers && j == Global.ExperimentalUnitsMarkerInputColumnNumber)
                            //column contains markers of ExperimentalUnits
                            newExperimentalUnit.Marker = new Marker(InputDataGridView.Rows[i].Cells[j].Value.ToString());
                        else if (j == Global.NamesOfExperimentalUnitsInputColumnNumber)
                        { 
                            //column contains the names of ExperimentalUnits. The name is already set at object initialisation above, so no action is needed here.
                        }
                        else
                        {
                            //column contains an actual Variable. The VariableValue now needs to be added to the ExperimentalUnit.
                            if (double.TryParse(InputDataGridView.Rows[i].Cells[j].Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out double VariableValue))
                                newExperimentalUnit.Values.Add(VariableValue);
                            else if (double.TryParse(InputDataGridView.Rows[i].Cells[j].Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out VariableValue))
                                newExperimentalUnit.Values.Add(VariableValue);
                            else
                                newExperimentalUnit.Values.Add(Global.Settings.MissingValue); //missing value
                        }
                    }
        
                    Global.CurrentExperiment.AllExperimentalUnits.Add(newExperimentalUnit);
                }
            }
        
            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// Clears any existing Variables, then
        /// iterates through each cell in the inputDataGridView,
        /// and defines new Variables to add to currentExperiment.
        /// If wanted, the user will be warned if invalid data is
        /// encountered. Also if wanted, the user may be asked if strings
        /// that can be parsed as dates should be converted to values.
        /// </summary>
        /// <param name="inputDataGridViewDataHasChanged">A bool to indicate whether or not the values in the 
        /// InputDataGridView has changed. If true, existing Variables will be re-used, else new Variables will be created.</param>
        /// <param name="warnUserForInvalidDataPoints">A bool to indicate whether or not the user should
        /// be warned if the pasted string contains invalid data points. Default is false.</param>
        /// <param name="askUserIfDatesShouldBeConvertedToValues">A bool to indicate whether or not the user should
        /// be asked if strings containing a valid DateTime should be converted into values. Default is false.</param>
        private void CreateVariablesOfCurrentExperiment(bool inputDataGridViewDataHasChanged, bool warnUserForInvalidDataPoints = false, bool askUserIfDatesShouldBeConvertedToValues = false)
        {
            int oldVariableCount = Global.CurrentExperiment.ActiveVariables.Count;
            List<Variable> newVariables = new List<Variable>();
            bool userIsWarned = false;
            bool? convertDateTimeStringsToValues = null;
            DateTime now = DateTime.Now;
            int dateDecimalplaces = 2;

            for (int i = 0; i < InputDataGridView.ColumnCount; i++)
            {
                if (!inputDataGridViewDataHasChanged && Global.CurrentExperiment.AllVariables.Select(Variable => Variable.InputDataGridViewColumnNumber).ToList().Contains((short)i))
                {
                    //if the content of InputDataGridView has not changed,
                    //and the currentExperiment already has a Variable that corresponds to the
                    //current InputDataGridView column number, then re-use the same Variable.
                    newVariables.Add(Global.CurrentExperiment.AllVariables.Where(Variable => Variable.InputDataGridViewColumnNumber == i).First());
                }
                else
                {
                    //else, the InputDataGridView has changed,
                    //so read the data in InputDataGridView
                    //and create a new Variable accordingly.
                    int sumDecimals = 0;
                    int VariableValuesCount = 0;
        
                    for (int j = 0; j < InputDataGridView.RowCount; j++)
                    {
                        if (double.TryParse(InputDataGridView.Rows[j].Cells[i].Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out double Value))
                        {
                            sumDecimals += Calc.GetDecimalPlaces(Value);
                            VariableValuesCount++;
                        }
                        else if (double.TryParse(InputDataGridView.Rows[j].Cells[i].Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out Value))
                        {
                            sumDecimals += Calc.GetDecimalPlaces(Value);
                            VariableValuesCount++;
                        }
                        else if(askUserIfDatesShouldBeConvertedToValues && DateTime.TryParse(InputDataGridView.Rows[j].Cells[i].Value.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime Date))
                        {
                            if(!convertDateTimeStringsToValues.HasValue)
                                convertDateTimeStringsToValues = MessageBox.Show("Some data points are recognized as dates and need to be converted to values to be used by RandoMice in further steps.\n\n" +
                                    "Would you like RandoMice to convert the dates for you? " +
                                    "The unit will be hours; " +
                                    "please manually check whether RandoMice performed this conversion correctly.", 
                                    "Convert dates into values?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

                            if (convertDateTimeStringsToValues.Value == true)
                                InputDataGridView.Rows[j].Cells[i].Value = Math.Round((now - Date).TotalHours, dateDecimalplaces, MidpointRounding.AwayFromZero);
                            else
                                InputDataGridView.Rows[j].Cells[i].Style.BackColor = Color.Bisque;

                            sumDecimals += dateDecimalplaces;
                            VariableValuesCount++;
                        }
                        else
                        {
                            if(warnUserForInvalidDataPoints && !userIsWarned)
                                MessageBox.Show("Warning: RandoMice cannot interpret all data points as *values*. " +
                                    "Please be aware that RandoMice will thus ignore these data points in further steps, " +
                                    "unless you select their column(s) to be used as unique names of experimental units and/or as markers.\n\n" +
                                    "All applicable cells are highlighted.", 
                                    "Data in wrong format", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            //highlight cells containing data that could not be interpreted
                            InputDataGridView.Rows[j].Cells[i].Style.BackColor = Color.Bisque;

                            userIsWarned = true;
                        }
                    }

                    //if date values were converted to values, adapt the PastedString in the CurrentExperiment accordingly
                    if (convertDateTimeStringsToValues.HasValue)
                        Global.CurrentExperiment.InputData = InputDataGridView.ToString();

                    //calculate the average number of decimal places (rounded up) of the values of the current Variable
                    byte VariableDecimals = (byte)Math.Ceiling((double)sumDecimals / (double)VariableValuesCount);
        
                    //find the default name
                    string VariableName = InputDataGridView.Columns[i].HeaderText;
        
                    //add new Variable to currentExperiment
                    newVariables.Add(new Variable(VariableName, VariableDecimals, inputDatagridViewColumnNumber: (short)i));
                }
            }

            newVariables = newVariables.OrderBy(variable => variable.InputDataGridViewColumnNumber).ToList();

            //update Variables in CurrentExperiment
            Global.CurrentExperiment.AllVariables = newVariables;
            Global.CurrentExperiment.ActiveVariables = newVariables.Where(variable => variable.InputDataGridViewColumnNumber != Global.NamesOfExperimentalUnitsInputColumnNumber && variable.InputDataGridViewColumnNumber != Global.ExperimentalUnitsMarkerInputColumnNumber).ToList();
        
            SetVariableControls(oldVariableCount, Global.CurrentExperiment.ActiveVariables.Count);
        }

        /// <summary>
        /// Creates controls allowing the user to set subgroup sizes for each block.
        /// </summary>
        private void CreateSubgroupControls()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.CurrentExperiment.CreateSubgroups = CreateSubgroupsCheckBox.Checked;
            IsCreatingSubgroups = true; //set bool to true to make sure that some other tasks can be skipped while creating subgroup controls

            if (Global.CurrentExperiment.CreateSubgroups)
            {
                MarkersOfExperimentalUnitsInputColumnNumberComboBox.Enabled = true;
                PreferredSubgroupSizeNumericUpDown.Enabled = true;

                //add a TabPage in which subgroupSizes can be defined by the user,
                //if such TabPage does not yet exist
                if (!SettingsTabControl.TabPages.ContainsKey(Global.Settings.SubgroupSizesTabPageName))
                {
                    TabPage newSubgroupSizesTabPage = new TabPage
                    {
                        Text = Global.Settings.SubgroupSizesTabPageName,
                        Name = Global.Settings.SubgroupSizesTabPageName,
                        AutoScroll = true,
                        BackColor = SystemColors.Control
                    };

                    //add a help PictureBox
                    HelpPictureBox subgroupsHelpPictureBox = new HelpPictureBox
                    {
                        Name = Global.Settings.SubgroupsHelpPictureBoxName,
                        HelpText = Global.Settings.SubgroupsHelpPictureBoxToolTipText,
                        HelpTextCaption = "Info: Creating subgroups",
                        Top = Global.Settings.DefaultTabPageMargin,
                        Left = Global.Settings.DefaultTabPageMargin
                    };

                    newSubgroupSizesTabPage.Controls.Add(subgroupsHelpPictureBox);

                    if (SettingsTabControl.TabPages.ContainsKey(Global.Settings.BlockSizesTabPageName))
                        //make sure that the order in which the subgroup sizes TabPage and the block sizes TabPage appear, remains constant
                        SettingsTabControl.TabPages.Insert(SettingsTabControl.TabPages.IndexOfKey(Global.Settings.BlockSizesTabPageName), newSubgroupSizesTabPage);
                    else
                        SettingsTabControl.TabPages.Add(newSubgroupSizesTabPage);
                }
                else
                {
                    //remove all controls in the existing TabPage, except for the help PictureBox
                    for (int i = SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.Count; i > 0; i--)
                    {
                        if(SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls[i - 1].Name != Global.Settings.SubgroupsHelpPictureBoxName)
                            SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls[i - 1].Dispose();
                    }
                }

                int preferredSubgroupSize = (Int32)PreferredSubgroupSizeNumericUpDown.Value;
                double totalNumberOfSubgroups = Global.CurrentExperiment.BlockSizes.Select(blockSize => Math.Floor((double)blockSize / (double)preferredSubgroupSize)).ToList().Sum(minimalSubgroupsPerBlock => minimalSubgroupsPerBlock);
                double subgroupsPerBlock = 0;

                if (Global.CurrentExperiment.BlockSizes.Count != 0)
                    subgroupsPerBlock = Global.CurrentExperiment.BlockSizes.Select(blockSize => Math.Ceiling((double)blockSize / (double)preferredSubgroupSize)).ToList().Max();

                //create a copy of the current blockSizes
                List<short> copyOfBlockSizes = new List<short>(Global.CurrentExperiment.BlockSizes);

                if (Global.CurrentExperiment.BlockSizes.Count <= Global.Settings.MaximalSubgroupSizeControls.Vertical && subgroupsPerBlock <= Global.Settings.MaximalSubgroupSizeControls.Horizontal)
                {
                    Global.CurrentExperiment.SubgroupSizesAreDefinedViaFormControls = true ;

                    //foreach block, create subgroup controls
                    for (int i = 0; i < copyOfBlockSizes.Count; i++)
                    {
                        //create as many subgroup-controls as possible, with a size of preferredSubgroupSize
                        for (int j = 0; copyOfBlockSizes[i] >= preferredSubgroupSize; j++)
                        {
                            AddSubgroupControl(i + 1, preferredSubgroupSize, lastSubgroupNumberOfCurrentBlock: j);
                            copyOfBlockSizes[i] -= (short)preferredSubgroupSize;
                        }

                        //finally add one subgroup-control containing the number of remaining ExperimentalUnits in this block (that is > 0 and < preferredSubgroupSizes)
                        if (copyOfBlockSizes[i] > 0)
                        {
                            AddSubgroupControl(i + 1, copyOfBlockSizes[i]);
                            copyOfBlockSizes[i] = 0;
                        }
                    }
                }
                else
                {
                    //too many subgroup controls need to be made.
                    //Instead, offer the user the possbility to import a .txt file containig semicolon-separated subgroup sizes,
                    //and the possibility to download a template with subgroup sizes pre-set.
                    Global.CurrentExperiment.SubgroupSizesAreDefinedViaFormControls = false;

                    //add a Label explaining the possbilities
                    Label subgroupSizesTemplateLabel = new Label
                    {
                        Name = Global.Settings.SubgroupSizesTemplateLabelName,
                        Text = Global.Settings.SubgroupSizesTemplateLabelText,
                        Top = Global.Settings.SubgroupSizesTemplateLabel.Top,
                        Left = Global.Settings.SubgroupSizesTemplateLabel.Left,
                        AutoSize = true
                    };

                    Button downloadSubgroupSizesTemplateButton = new Button
                    {
                        Name = Global.Settings.exportSubgroupSizesTemplateButtonName,
                        Text = Global.Settings.ExportSubgroupSizesTemplateButtonText,
                        Top = 85,// subgroupSizesTemplateLabel.Top + subgroupSizesTemplateLabel.Height,
                        Left = subgroupSizesTemplateLabel.Left,
                        Width = Global.Settings.DownloadSubgroupSizesTemplateButtonWidth
                    };

                    downloadSubgroupSizesTemplateButton.Click += ExportSubgroupSizesTemplateButton_Click;

                    Button importSubgroupSizesFromTemplateButton = new Button
                    {
                        Name = Global.Settings.ImportSubgroupSizesFromTemplateButtonName,
                        Text = Global.Settings.ImportSubgroupSizesFromTemplateButtonText,
                        Top = downloadSubgroupSizesTemplateButton.Top,
                        Left = downloadSubgroupSizesTemplateButton.Left + downloadSubgroupSizesTemplateButton.Width,
                        Width = Global.Settings.ImportSubgroupSizesFromTemplateButtonWidth
                    };

                    importSubgroupSizesFromTemplateButton.Click += ImportSubgroupSizesFromTemplateButton_Click;

                    SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.AddRange(new Control[] { subgroupSizesTemplateLabel, downloadSubgroupSizesTemplateButton, importSubgroupSizesFromTemplateButton });

                    //finally, add subgroup sizes to the CurrentExperiment
                    //those subgroup sizes can only be overwritten by importing a .txt file using the ImportSubgroupSizesFromTemplateButton.
                    Global.CurrentExperiment.SubgroupSizesOfEachBlock.Clear();

                    for (int i = 0; i < copyOfBlockSizes.Count; i++)
                    {
                        Global.CurrentExperiment.SubgroupSizesOfEachBlock.Add(new List<short>());

                        for (int j = 0; copyOfBlockSizes[i] >= preferredSubgroupSize; j++)
                        {
                            Global.CurrentExperiment.SubgroupSizesOfEachBlock[i].Add((short)preferredSubgroupSize);
                            copyOfBlockSizes[i] -= (short)preferredSubgroupSize;
                        }

                        if (copyOfBlockSizes[i] > 0)
                        {
                            Global.CurrentExperiment.SubgroupSizesOfEachBlock[i].Add(copyOfBlockSizes[i]);
                            copyOfBlockSizes[i] = 0;
                        }
                    }
                }

                //if no controls exist yet (except for the help PictureBox), then display an explanatory label
                if (SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.Count == 1)
                {
                    Label explanatoryLabel = new Label
                    {
                        Name = Global.Settings.SubgroupsCannotBeFormedExplanatoryLabelName,
                        Text = Global.Settings.SubgroupsCannotBeFormedExplanatoryLabelText,
                        AutoSize = true,
                        Top = Global.Settings.SubgroupsCannotBeFormedExplanatoryLabelTop,
                        Left = Global.Settings.DefaultVariableControl.Left
                    };

                    SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.Add(explanatoryLabel);
                }
            }
            else
            {
                //CreateSubgroupsCheckBox.Checked == false, so disable relevant controls and remove the TabKey
                Global.CurrentExperiment.ExperimentalUnitsHaveMarkers = false;

                MarkersOfExperimentalUnitsInputColumnNumberComboBox.Enabled = false;
                PreferredSubgroupSizeNumericUpDown.Enabled = false;

                SettingsTabControl.TabPages.RemoveByKey(Global.Settings.SubgroupSizesTabPageName);
            }

            IsCreatingSubgroups = false;
            Cursor.Current = Cursors.Default;
        }

        private void ImportSubgroupSizesFromTemplateButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog SubgroupSizesOpenFileDialog = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt"
                };

                if (SubgroupSizesOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string line;
                    List<List<short>> subgroupSizesOfEachGroup = new List<List<short>>();

                    //try to read the subgroup sizes of each block from the selected file
                    using (System.IO.StreamReader file = new System.IO.StreamReader(SubgroupSizesOpenFileDialog.FileName))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            System.Console.WriteLine(line);
                            subgroupSizesOfEachGroup.Add(line.Split(';').Select(value => short.Parse(value)).ToList());
                        }
                    }

                    //finally, set the subgroup sizes of each block in Global.CurrentExperiment
                    Global.CurrentExperiment.SubgroupSizesOfEachBlock.Clear();
                    Global.CurrentExperiment.SubgroupSizesOfEachBlock = subgroupSizesOfEachGroup;

                    MessageBox.Show("Subgroup sizes were imported successfully!", "Import successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong while reading the file. Please make sure the file is in the correct format, and try again.", "Error reading file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExportSubgroupSizesTemplateButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SubgroupSizesSaveFileDialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt",
                    FileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString("d2") + "_" + DateTime.Now.Day.ToString("d2") + " " + "RandoMice subgroup sizes template.txt"
                };

                if (SubgroupSizesSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<string> subgroupSizesOfEachBlock = new List<string>();

                    foreach (List<short> subgroupSizes in Global.CurrentExperiment.SubgroupSizesOfEachBlock)
                        subgroupSizesOfEachBlock.Add(String.Join(";", subgroupSizes));

                    //try to write the subgroup sizes of each block
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(SubgroupSizesSaveFileDialog.FileName, append: false))
                    {
                        foreach (string subgroupSizes in subgroupSizesOfEachBlock)
                            file.WriteLine(subgroupSizes);
                    }

                    if (File.Exists(SubgroupSizesSaveFileDialog.FileName))
                    {
                        //try to open explorer and select the newly created file
                        string argument = "/select, \"" + SubgroupSizesSaveFileDialog.FileName + "\"";

                        Process.Start("explorer.exe", argument);
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occurred while creating a template. Please try again.", "Error creating template", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Clears and (re-)fills the list containing the block sizes in currentExperiment
        /// </summary>
        private void SetBlockSizes()
        {
            Global.CurrentExperiment.BlockSizes.Clear();

            if (BlockCountComboBox.Text == string.Empty)
                return; //no value is selected, so return

            if (BlockSizesAreEqualCheckBox.Checked)
            {
                //if subgroup sizes are equal, replace all values in the blockSizes-list by 
                //the value currently selected in NumberOfExperimentalUnitsPerBlockNumericUpDown.Value
                for (int i = 0; i < int.Parse(BlockCountComboBox.Text); i++)
                    Global.CurrentExperiment.BlockSizes.Add((short)NumberOfExperimentalUnitsPerBlockNumericUpDown.Value);
            }
            else
            {
                //if subgroup sizes are not equal, collect the user-defined blockSizes in the blockSizes-TabPage
                for (int i = 0; i < int.Parse(BlockCountComboBox.Text); i++)
                {
                    if (SettingsTabControl.TabPages.ContainsKey(Global.Settings.BlockSizesTabPageName))
                    {
                        //find the control in the blockSizes-TabPage that corresponds with the current block,
                        //and set the blockSize to its selected value
                        if (SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.ContainsKey(Global.Settings.BlockSizeNumericUpDownNameBasis + i.ToString()))
                            Global.CurrentExperiment.BlockSizes.Add((short)((NumericUpDown)SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls[Global.Settings.BlockSizeNumericUpDownNameBasis + i.ToString()]).Value);
                    }
                }
            }

            CreateSubgroupControls();
        }

        /// <summary>
        /// Gets the user-defined subgroup sizes of each block of ExperimentalUnits
        /// from NumericUpDowns in the subgroupSizes-TabPage,
        /// and sets the two-dimensional list in currentExperiment.
        /// Also checks if all subgroup sizes are valid (i.e. can be formed with
        /// the given block sizes).
        /// </summary>
        /// <returns>An enum which is Valid if all subgroup sizes can be formed
        /// with the current block sizes in currentExperiment.</returns>
        private ValidOrInvalid SetSubgroupSizes()
        {
            if (Global.CurrentExperiment.SubgroupSizesAreDefinedViaFormControls)
            {
                Global.CurrentExperiment.SubgroupSizesOfEachBlock.Clear();

                //foreach block in currentExperiment
                for (int i = 0; i < Global.CurrentExperiment.BlockSizes.Count; i++)
                {
                    List<short> subgroupSizes = new List<short>();

                    //get subgroup sizes from comboboxes in the subgroupSizes-TabPage
                    if (CreateSubgroupsCheckBox.Checked)
                    {
                        subgroupSizes.AddRange(SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.OfType<NumericUpDown>()
                            .Where(numericUpDown => numericUpDown.Name.Contains(Global.Settings.SubgroupSizeNumericUpDownNameBasis + (i + 1).ToString() + "."))
                            .Select(numericUpDown => (short)numericUpDown.Value).ToList());
                    }

                    Global.CurrentExperiment.SubgroupSizesOfEachBlock.Add(subgroupSizes);
                }
            }

            return Global.CurrentExperiment.SubgroupSizesAreValid;
        }

        /// <summary>
        /// Checks if a valid column number containing the markers of ExperimentalUnits is selected,
        /// and updates the ExperimentalUnits and Variables.
        /// </summary>
        private void MarkersOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Global.AnyBackgroundWorkerIsBusy)
                return; //the backgroundworkers lock the control (by setting .Enabled = false), thereby changing the selectedIndex. This change must be ignored, so, return.

            if (MarkersOfExperimentalUnitsInputColumnNumberComboBox.SelectedIndex == -1 || !MarkersOfExperimentalUnitsInputColumnNumberComboBox.Enabled || MarkersOfExperimentalUnitsInputColumnNumberComboBox.Text == "None")
            {
                //if ExperimentalUnits do not have markers
                Global.ExperimentalUnitsMarkerInputColumnNumber = -1;
                Global.CurrentExperiment.ExperimentalUnitsHaveMarkers = false;
            }
            else
            {
                Global.ExperimentalUnitsMarkerInputColumnNumber = int.Parse(MarkersOfExperimentalUnitsInputColumnNumberComboBox.Text) - 1;
                Global.CurrentExperiment.ExperimentalUnitsHaveMarkers = true;
            }

            //create new ExperimentalUnits and Variables, as the data in InputDataGridView needs to be re-interpreted.
            CreateVariablesOfCurrentExperiment(inputDataGridViewDataHasChanged: false); 
            CreateExperimentalUnitsOfCurrentExperiment();
        }

        /// <summary>
        /// Checks if a valid column number containing the names of ExperimentalUnits is selected,
        /// and updates the ExperimentalUnits and Variables.
        /// </summary>
        private void NamesOfExperimentalUnitsInputColumnNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (BackgroundWorker backgroundWorker in Global.BackgroundWorkers)
            {
                if (backgroundWorker.IsBusy)
                    return; //the backgroundworkers lock the control (by setting .Enabled = false), thereby changing the selectedIndex. This change must be ignored, so, return.
            }

            if (NamesOfExperimentalUnitsInputColumnNumberComboBox.SelectedIndex == -1 || !NamesOfExperimentalUnitsInputColumnNumberComboBox.Enabled)
                Global.NamesOfExperimentalUnitsInputColumnNumber = -1;
            else
                Global.NamesOfExperimentalUnitsInputColumnNumber = int.Parse(NamesOfExperimentalUnitsInputColumnNumberComboBox.Text) - 1;
            
            CreateVariablesOfCurrentExperiment(inputDataGridViewDataHasChanged: false);
            CreateExperimentalUnitsOfCurrentExperiment();
        }
        #endregion

        #region createBlockSets
        /// <summary>
        /// Checks if the CreateGoupSetsBackgroundWorker can start, and starts the BackgroundWorker.
        /// </summary>
        private void RunButton_Click(object sender, EventArgs e)
        {
            Global.DoNotInterruptBlockSetCreation = true;
            bool backgroundWorkerCanStart = false;

            if (Global.CurrentExperiment.BlockSizes.Sum(blockSize => blockSize) == 0)
                MessageBox.Show("The number of blocks and their sizes are not defined yet. Please provide all information.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.CurrentExperiment.BlockSizesAreTooLarge)
                MessageBox.Show("The defined blocks cannot be created with the provided experimental units. Please lower the block size(s).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.RememberSets < 1)
                MessageBox.Show("Please enter the number of sets that the program should remember.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.DesiredUniqueSets < 1)
                MessageBox.Show("Please enter the desired number of unique sets that the program should create.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.ExperimentalUnitsMarkerInputColumnNumber == Global.NamesOfExperimentalUnitsInputColumnNumber)
                MessageBox.Show("The column containing names of the experimental units must be different from the column containing markers of the experimental units.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.CurrentExperiment.CreateSubgroups && SetSubgroupSizes() == ValidOrInvalid.Invalid)
                MessageBox.Show("The subgroups cannot be created with the current information. Please provide all correct information.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.CurrentExperiment.BlockSizes.Contains(Global.Settings.MissingBlockSize))
                MessageBox.Show("Some block sizes have not yet been defined. Please provide all block sizes.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.TheoreticalUniqueBlockSets == 0)
                MessageBox.Show("Sets cannot be created with the current settings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Global.CurrentExperiment.ActiveVariables.Count == 0)
            {
                //without Variables, the program should only create one BlockSet in order to randomly divide the ExperimentalUnits.
                //backgroundworker, therefore, can start if the user wants so
                DialogResult dialogResult = MessageBox.Show("Your data does not contain any Variables!\n\n"
                    + "The program will generate *one* random block set.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                backgroundWorkerCanStart = (dialogResult == DialogResult.OK);
                DesiredUniqueSetsNumericUpDown.Value = DesiredUniqueSetsNumericUpDown.Minimum;
            }
            else
                backgroundWorkerCanStart = true;

            if (backgroundWorkerCanStart && Global.CurrentExperiment.AllExperimentalUnits.Select(experimentalUnit => experimentalUnit.Name.Replace(" ", "")).ToList().Distinct().Count() < Global.CurrentExperiment.AllExperimentalUnits.Count)
            {
                //check if the names of experimental units are unique. If that is not the case, the user cannot distinguish between those experimental units when reviewing the results.
                DialogResult dialogResult = MessageBox.Show("Some experimental units have identical names. It will be impossible " +
                    "to distinguish those experimental units when reviewing the results. Continue anyways?",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                backgroundWorkerCanStart = (dialogResult == DialogResult.Yes);
            }

            if (backgroundWorkerCanStart && Global.DesiredUniqueSets > Global.TheoreticalUniqueBlockSets)
            {
                //check if it is possible to find the desired number of unique sets
                DialogResult dialogResult = MessageBox.Show(String.Format("{0} blocks cannot be created, as theoretically only {1} unique sets exist.\n" +
                        "Therefore, the program will continue and generate {1} unique sets instead.", Global.DesiredUniqueSets.ToString("N0"), Global.TheoreticalUniqueBlockSets.ToString("N0")),
                        "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                backgroundWorkerCanStart = (dialogResult == DialogResult.OK);
                DesiredUniqueSetsNumericUpDown.Value = (decimal)Global.TheoreticalUniqueBlockSets;
            }

            if (backgroundWorkerCanStart)
                backgroundWorkerCanStart = (SaveExperimentIfPossibleNeededAndWanted() != SaveState.Cancelled);

            //check if the backgroundworker is not busy, the bool backgroundWorkerCanStart is true
            if (!CreateBlockSetsBackgroundWorker.IsBusy && backgroundWorkerCanStart)
            {
                // Start the asynchronous operation.
                Stopwatch.Restart();
                CreateBlockSetsTimer.Start();
                CreateBlockSetsBackgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Cancels all currently busy BackGroundWorker.
        /// </summary>
        private void AbortRunButton_Click(object sender, EventArgs e)
        {
            foreach (BackgroundWorker backGroundWorker in Global.BackgroundWorkers)
            {
                if (backGroundWorker.IsBusy && MessageBox.Show("Are you sure to abort?", "Aborting...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    backGroundWorker.CancelAsync();
            }
        }

        /// <summary>
        /// Sets Global.doNotInterruptBlockCreation to false, allowing for the main loop in CreateBlockSetsBackGroundWorker to break.
        /// </summary>
        private void ShowResultsButton_Click(object sender, EventArgs e)
        {
            Global.DoNotInterruptBlockSetCreation = false;
        }

        /// <summary>
        /// The main logic of the program. In short, a BackgroundWorker creates a new Run,
        /// then continuously creates new BlockSets, checks for their unicity, and adds it to
        /// the new Run if the Rank of the new BlockSet is low enough.
        /// </summary>
        private void CreateBlockSetsBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //Use FinishedExperiment to add a new run to if the current experiment equals the finished experiment
            if (Global.FinishedExperiment == Global.CurrentExperiment)
                Global.CurrentExperiment = Global.FinishedExperiment;

            //create new Run
            Global.CurrentRun = Global.CurrentExperiment.CreateNewRun(Global.CheckForBlockSetUnicity);

            //calculate theoretical unique sets available
            UpdateTheoreticalUniqueBlockSetsLabel();
            Global.TheoreticalUniqueBlockSets = CalculateTheoreticalNumberOfUniqueSets();

            //lock controls
            Control[] controlsToEnable = { AbortRunButton, ShowResultsButton, MainProgressBar };
            Global.ControlsAndEnabledStates = EnableOrDisableAllControls(EnableOrDisable.Disable);
            EnableOrDisableControls(EnableOrDisable.Enable, controlsToEnable);

            double theoreticalRequiredAttempts = Global.DesiredUniqueSets;

            if (Global.CurrentRun.CheckForUnicity)
            {
                //Only if the program checks for unicity, the theoretical average number of required
                //attempts that is needed to find the desired number of unique sets, needs to be calculated.
                //This number will be larger than the desired number of unique sets, as duplicate BlockSets may be encountered.
                theoreticalRequiredAttempts = CalculateTheoreticalRequiredAttempts(Global.DesiredUniqueSets, Global.TheoreticalUniqueBlockSets);
            }

            //restart timer
            Stopwatch.Restart();

            Random random = new Random(); //do not create a new Random() within a loop, because that may cause the same seed to be re-used
            Global.CurrentRun.UniqueSetsCreated = 0;
            DateTime dateTimeOfLastReportProgressEvent = DateTime.Now;
            TimeSpan timeDifferenceSinceLastReportProgressEvent = new TimeSpan();
            Global.CurrentRun.ProgressPercentage = 0;
            int totalSetsConsidered = 0;
            int progressPercentage = 0;

            //return if abort button is pressed already
            if (e.Cancel == true)
                return;

            while (Global.DoNotInterruptBlockSetCreation)
            {
                if (Global.CurrentRun.UniqueSetsCreated < Global.DesiredUniqueSets) //keep running until desired number of unique sets is created
                {
                    bool setIsUnique = true; //default is true
                    BlockSet newSet = Global.CurrentRun.CreateNewBlockSet(random);

                    //check if newSet is unique, if desired
                    if (Global.CurrentRun.CheckForUnicity)
                        setIsUnique = Global.CurrentRun.BlockSetHashes.Add(newSet.StableHashCode); //setHashes is a SortedSet which will return false if an item is added, but already exists in the SortedSet

                    if (setIsUnique)
                    {
                        Global.CurrentRun.UniqueSetsCreated++;
                        newSet.CalculateRank();
                        
                        //Always remember the newly created set, if the number of unique sets is smaller than the number of sets that will be remembered
                        if (Global.CurrentRun.BlockSets.Count < Global.RememberSets)
                        {
                            Global.CurrentRun.BlockSets.Add(newSet);
                            Global.CurrentRun.SortBlockSetsByRank();
                        }
                        else
                        {
                            //else, add the newSet only if its rank is lower than the last (i.e. "worst") BlockSet
                            if (newSet.Rank < Global.CurrentRun.BlockSets.Last().Rank)
                            {
                                Global.CurrentRun.BlockSets.RemoveAt(Global.RememberSets - 1);
                                Global.CurrentRun.BlockSets.Add(newSet);
                                Global.CurrentRun.SortBlockSetsByRank(); //finally, sort the BlockSets by rank
                            }
                        }
                    }

                    //keep track of the total number of sets considered to calculate the progressPercentage
                    totalSetsConsidered++;

                    //calculate time difference since last ReportProgress event
                    timeDifferenceSinceLastReportProgressEvent = DateTime.Now - dateTimeOfLastReportProgressEvent;

                    if ((int)timeDifferenceSinceLastReportProgressEvent.TotalMilliseconds > Global.Settings.BackgroundWorkerReportProgressPeriod)
                    {
                        timeDifferenceSinceLastReportProgressEvent = new TimeSpan();

                        //set Global.currentRun.ProgressPercentage and not the local progressPercentage, so that double can be used to accurately calculate the time remaining
                        Global.CurrentRun.ProgressPercentage = (double)totalSetsConsidered * 100 / theoreticalRequiredAttempts;
                        progressPercentage = (int)Math.Floor(Global.CurrentRun.ProgressPercentage);
                        worker.ReportProgress(progressPercentage, Global.CurrentRun.UniqueSetsCreated);
                        dateTimeOfLastReportProgressEvent = DateTime.Now;
                    }
                }
                else
                    Global.DoNotInterruptBlockSetCreation = false; //desired number of unique sets is created, so allow the while loop to break

                if (worker.CancellationPending)
                {
                    //abort button is pressed
                    e.Cancel = true;
                    progressPercentage = 100; //update to 100%
                    worker.ReportProgress(progressPercentage, Global.CurrentRun.UniqueSetsCreated);
                    Global.DoNotInterruptBlockSetCreation = false;
                }
            }

            if (Global.CurrentRun.UniqueSetsCreated == Global.DesiredUniqueSets)
                Global.CurrentRun.ProgressPercentage = 100; //update to 100%

            //reset values
            Global.CurrentRun.ProgressPercentage = 0;
            Global.DoNotInterruptBlockSetCreation = true; 

            //store values in CurrentRun
            Global.CurrentRun.TotalTimeElapsedForCreatingBlockSets = Stopwatch.Elapsed;
            Global.CurrentRun.TotalNumberOfSetsConsidered = totalSetsConsidered;

            //if no block sets are yet created (which could be the case if the user very quickly presses the showResultsButton),
            //respond as if the abort button was pressed.
            if (Global.CurrentRun.BlockSets.Count == 0)
                e.Cancel = true;

            //if abort button has not been pressed, add the current run to the current experiment
            if (!e.Cancel)
                Global.CurrentExperiment.Runs.Add(Global.CurrentRun);

            //if currentExperiment contains at least one run (i.e. when abort button was not pressed, or if currentExperiment already contained at least one run).
            if (Global.CurrentExperiment.Runs.Count > 0)
            {
                //set finished experiment
                Global.FinishedExperiment = Global.CurrentExperiment;

                //clone current experiment, to make sure that Global.FinishedExperiment and Global.CurrentExperiment are not referencing the same Experiment instance.
                Global.CurrentExperiment = Global.CurrentExperiment.Clone();
            }
        }

        /// <summary>
        /// Updates the progress label to display the number of BlockSets created,
        /// the desired number of BlockSets to find, and the progress percentage
        /// </summary>
        /// <param name="blockSetsCreated">The number of BlockSets already created.</param>
        /// <param name="desiredBlockSets">The desired number of BlockSets.</param>
        /// <param name="progressPercentage">A value ranging between 0 and 100 containing the progress percentage.</param>
        /// <param name="setsAreUnique">A bool which is true if the BlockSets are checked for unicity.</param>
        /// <param name="exportingSets">A bool which is true if the current backgroundWorker is exporting sets.</param>
        private void ProgressChanged(long blockSetsCreated, int desiredBlockSets, int progressPercentage, bool setsAreUnique = false, bool exportingSets = false)
        {
            string setsAreUniqueString = string.Empty, setsCreatedOrExported = "created";

            //only mention that sets are unique if the BlockSets are checked for unicity.
            if (setsAreUnique)
                setsAreUniqueString = "unique ";

            //if the calling backgroundWorker is exporting sets (e.g. ExportToFileBackgroundWorker), then mention "x sets exported" instead of "x sets created"
            if (exportingSets)
                setsCreatedOrExported = "exported";

            //update the progress label.
            ProgressLabel.Text = string.Format("{0}/{1} {2}sets {3}.", blockSetsCreated.ToString("N0"), desiredBlockSets.ToString("N0"), setsAreUniqueString, setsCreatedOrExported);

            //update the progress bar
            if (progressPercentage <= 100)
            {
                MainProgressBar.Value = progressPercentage;
                ProgressPercentageLabel.Text = progressPercentage.ToString() + "%";
            }
        }

        private void CreateBlockSetsBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged(Convert.ToInt64(e.UserState), Global.DesiredUniqueSets, e.ProgressPercentage, Global.CheckForBlockSetUnicity);
        }

        private void CreateBlockSetsBackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RevertEnabledStateOfControls(Global.ControlsAndEnabledStates);
            EnableOrDisableControls(EnableOrDisable.Enable, controls: GetChildControls(BestBlockSetsGroupBox, includeContainerControls: true).ToArray().Concat(GetChildControls(BlockSetDetailsGroupBox, includeContainerControls: true)).ToArray()); // MeanCheckBox, SDCheckBox, MinCheckBox, MedianCheckBox, MaxCheckBox,  })
            CreateBlockSetsTimer.Stop();

            if (e.Cancelled == true)
            {
                try
                {
                    if (!ProgressLabel.Text.Contains("Cancelled"))
                        ProgressLabel.Text += " - Cancelled!";
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    ProgressLabel.Text += " - Cancelled!";
                }
            }
            else if (e.Error != null)
            {
                ProgressLabel.Text = " - Error!";
                MessageBox.Show("Unknown error encountered, please try again." + "\n\nMessage:" + e.Error.Message + "\nInnerException:" + e.Error.InnerException + "\nSource:" + e.Error.Source + "\nStackTrace:" + e.Error.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                //update progress once more to make sure that the correct number is displayed
                ProgressChanged((int)Global.FinishedExperiment.Runs.Last().UniqueSetsCreated, Global.DesiredUniqueSets, (int)Global.FinishedExperiment.Runs.Last().ProgressPercentage, Global.CheckForBlockSetUnicity);

                if (!ProgressLabel.Text.Contains("Finished"))
                    ProgressLabel.Text += " - Finished!";

                //Reset time remaining label
                TimeRemainingLabel.Text = Global.Settings.DefaultTimeRemainingLabelText + "\n" + TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

                Global.FinishedExperiment.Runs.Last().DivideAllBlockSetsIntoSubgroups(); //divide blocks into subgroups, if needed
                    
                BlockSetsResultsDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString());
            }

            MainProgressBar.Value = 0;
            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// Displays the time elapsed in TimeElapsedLabel,
        /// and displays the calculated time remaining in TimeRemainingLabel.
        /// </summary>
        private void CreateBlockSetsTimer_Tick(object sender, EventArgs e)
        {
            if (Global.CurrentRun != null)
            {
                TimeElapsedLabel.Text = Global.Settings.DefaultTimeElapsedLabelText + "\n" + Stopwatch.Elapsed.ToString(Global.Settings.TimeSpanStringFormat);
                TimeRemainingLabel.Text = Global.Settings.DefaultTimeRemainingLabelText + "\n" + Global.CurrentRun.CalculateTimeRemaining(Stopwatch);
            }
        }

        /// <summary>
        /// Calculates the theoretical number of unique sets that can be made with the current ExperimentalUnits and given block sizes.
        /// </summary>
        /// <returns>A double containing the theoretical number of unique sets that can be made with the current ExperimentalUnits and given block sizes.</returns>
        private double CalculateTheoreticalNumberOfUniqueSets()
        {
            double theoreticalUniqueBlocks = 0; //default value
            
            //calculates the theoretical number of unique combinations that can be created
            //with the total number of ExperimentalUnits, and the currently given block sizes.
            if (Global.CurrentExperiment.BlockSizes.Count != 0 && !Global.CurrentExperiment.BlockSizes.Contains(Global.Settings.MissingBlockSize) && !Global.CurrentExperiment.BlockSizesAreTooLarge)
            {
                double result;
                double ExperimentalUnitsRemaining = (double)Global.CurrentExperiment.AllExperimentalUnits.Count;
                List<double> uniqueBlocks = new List<double>();
                double uniqueCombinations;
                
                foreach (int blockSize in Global.CurrentExperiment.BlockSizes)
                {
                    result = Calc.Factorial(ExperimentalUnitsRemaining);
                    
                    if (ExperimentalUnitsRemaining - blockSize != 0)
                        result /= Calc.Factorial(ExperimentalUnitsRemaining - blockSize);
                    
                    result /= Calc.Factorial(blockSize);
                    ExperimentalUnitsRemaining -= blockSize;
                    uniqueBlocks.Add(result);
                }
                
                uniqueCombinations = uniqueBlocks[0];
                
                for (int i = 1; i < uniqueBlocks.Count; i++)
                    uniqueCombinations *= uniqueBlocks[i];
                
                uniqueCombinations /= Calc.Factorial(Global.CurrentExperiment.BlockSizes.Count);

                if (double.IsNaN(uniqueCombinations) || double.IsInfinity(uniqueCombinations))
                    theoreticalUniqueBlocks = double.MaxValue;
                else if (uniqueCombinations > 0)
                    theoreticalUniqueBlocks = Math.Round(uniqueCombinations, 0, MidpointRounding.AwayFromZero);
            }

            Global.TheoreticalUniqueBlockSets = theoreticalUniqueBlocks;
            EnableOrDisableControls((EnableOrDisable)Convert.ToInt32(theoreticalUniqueBlocks != 0 && theoreticalUniqueBlocks <= Global.Settings.MaximalDesiredUniqueBlockSets), new Control[] { SetDesiredUniqueBlockSetsToCreateTo99PercentButton });

            return theoreticalUniqueBlocks;
        }

        /// <summary>
        /// Updates the label text to display the number of sets that can theoretically be created.
        /// </summary>
        private void UpdateTheoreticalUniqueBlockSetsLabel()
        {
            CalculateTheoreticalNumberOfUniqueSets();
            
            if (Global.TheoreticalUniqueBlockSets < 1000000)
                TheoreticalUniqueBlockSetsLabel.BeginInvoke(new Action(() => { TheoreticalUniqueBlockSetsLabel.Text = Global.TheoreticalUniqueBlockSets.ToString("N0") + " unique sets can be created."; }));
            else if (Global.TheoreticalUniqueBlockSets < double.MaxValue)
                TheoreticalUniqueBlockSetsLabel.BeginInvoke(new Action(() => { TheoreticalUniqueBlockSetsLabel.Text = Global.TheoreticalUniqueBlockSets.ToString("0.0##E+00") + " unique sets can be created."; }));
            else
                TheoreticalUniqueBlockSetsLabel.BeginInvoke(new Action(() => { TheoreticalUniqueBlockSetsLabel.Text = "More than " + double.MaxValue.ToString("0.0##E+00") + " unique sets can be created."; }));
        }

        /// <summary>
        /// Calculates the theoretical required attempts that are needed to find a given number of unique blocks.
        /// </summary>
        /// <param name="desiredUniqueSets">The desired number of unique block sets to find.</param>
        /// <param name="theoreticalUniqueBlockSets">The theoretical number of unique blocks available.</param>
        /// <returns>A double containing the theoretical required attempts needed to find x number of unique blocks.</returns>
        private double CalculateTheoreticalRequiredAttempts(double desiredUniqueSets, double theoreticalUniqueBlockSets)
        {
            double theoreticalRequiredAttempts = 0;
            
            for (int i = 0; i < desiredUniqueSets; i++)
                theoreticalRequiredAttempts += (theoreticalUniqueBlockSets / (theoreticalUniqueBlockSets - (double)i));
            
            return theoreticalRequiredAttempts;
        }
        #endregion

        #region changeGlobalVariablesByControls
        private void BlockCountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender.GetType().Name == "ComboBox") //only if the change is caused by the combobox itself.
                IfNeededWarnUserThatSubgroupSizesMayHaveChanged();

            AddOrRemoveBlockSizesControls();
            UpdateTheoreticalUniqueBlockSetsLabel();
            Global.CurrentExperiment.Groups = new List<Group>(); //the number of blocks has changed, so the groups should be newly added to match the new block count
        }

        private void SubgroupSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            IfNeededWarnUserThatSubgroupSizesMayHaveChanged();
            SetBlockSizes();
            UpdateTheoreticalUniqueBlockSetsLabel();
        }

        /// <summary>
        /// Warns user that the subgroup sizes may have been reset, if needed.
        /// </summary>
        private void IfNeededWarnUserThatSubgroupSizesMayHaveChanged()
        {
            if (SettingsTabControl.TabPages.ContainsKey(Global.Settings.SubgroupSizesTabPageName)
                && !SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName].Controls.ContainsKey(Global.Settings.SubgroupsCannotBeFormedExplanatoryLabelName))
            {
                if (!SuppressWarningThatSubgroupSizesMayHaveChanged)
                {
                    MessageBox.Show("One or more settings may have changed.\nTherefore, any previous changes made in the subgroup sizes are reset.\n" +
                          "Please review the subgroup sizes.", "Subgroup sizes changed.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SuppressWarningThatSubgroupSizesMayHaveChanged = true; //prevent multiple warnings
                                                                           //this boolean is again set to false once the user selects the SubgroupSizesTabPage
                }
            }
        }

        /// <summary>
        /// Adds controls to the blockSizes-tabpage that allows the user to enter the block sizes, if block sizes are not equal.
        /// </summary>
        /// <param name="previousNumberOfBlocks">The previous total number of blocks.</param>
        /// <param name="currentNumberOfBlocks">The current total number of blocks.</param>
        private void AddUnequalBlockSizesControls(int previousNumberOfBlocks, int currentNumberOfBlocks)
        {
            int top = Global.Settings.DefaultTabPageMargin
                + (Global.Settings.DefaultControl.Height * previousNumberOfBlocks)
                + Global.Settings.DefaultBlockNumberLabel.Height
                + Global.Settings.DefaultTabPageMargin; //extra margin
            int left = Global.Settings.DefaultTabPageMargin;

            Cursor.Current = Cursors.WaitCursor; //give user feedback by changing the cursor

            //add a Label containing the block number, and a ComboBox used to change the corresponding block size
            for (int i = previousNumberOfBlocks; i < currentNumberOfBlocks; i++)
            {
                Label newBlockSizeLabel = new Label
                {
                    Name = Global.Settings.BlockSizeLabelNameBasis + i.ToString(),
                    Text = "Block " + (i + 1).ToString(),
                    Top = top,
                    Left = left,
                    Width = Global.Settings.DefaultBlockNumberLabel.Width
                };

                MyNumericUpDown newBlockSizeNumericUpDown = new MyNumericUpDown
                {
                    Name = Global.Settings.BlockSizeNumericUpDownNameBasis + i.ToString(),
                    Top = top,
                    Left = left + newBlockSizeLabel.Width,
                    Minimum = 1,
                    Increment = 1,
                    Maximum = Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count, short.MaxValue) //block sizes are of type short, so the value must not exceed short.MaxValue
                };

                newBlockSizeNumericUpDown.ValueChanged += SubgroupSizeNumericUpDown_ValueChanged;

                if (i < Global.CurrentExperiment.BlockSizes.Count)
                    newBlockSizeNumericUpDown.Value = Global.CurrentExperiment.BlockSizes[i];
                else
                    newBlockSizeNumericUpDown.Value = NumberOfExperimentalUnitsPerBlockNumericUpDown.Value; //get a user set value from NumberOfExperimentalUnitsPerBlockNumericUpDown

                //add control to blocksizes-tabpage
                SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.AddRange(new Control[] { newBlockSizeLabel, newBlockSizeNumericUpDown });

                top += newBlockSizeNumericUpDown.Height + 1; //extra margin
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Adds or removes controls in a TabPage, in which the user can set individual block sizes.
        /// </summary>
        private void AddOrRemoveBlockSizesControls()
        {
            NumberOfExperimentalUnitsPerBlockNumericUpDown.Enabled = BlockSizesAreEqualCheckBox.Checked;

            if (!BlockSizesAreEqualCheckBox.Checked)
            {
                //if block sizes differ from each other in size
                if (!SettingsTabControl.TabPages.ContainsKey(Global.Settings.BlockSizesTabPageName))
                {
                    //create a TabPage, if it does not already exist, that may be filled
                    //with controls that allows the user to set the block sizes
                    TabPage blockSizesTabPage = new TabPage
                    {
                        Text = Global.Settings.BlockSizesTabPageName,
                        Name = Global.Settings.BlockSizesTabPageName,
                        AutoScroll = true,
                        BackColor = SystemColors.Control
                    };

                    Label blockSizeLabel = new Label
                    {
                        Text = "Block size",
                        Name = Global.Settings.BlockSizeLabel,
                        Left = Global.Settings.DefaultTabPageMargin + Global.Settings.DefaultBlockNumberLabel.Width,
                        Height = Global.Settings.DefaultBlockNumberLabel.Height,
                        Top = Global.Settings.DefaultTabPageMargin
                    };

                    blockSizesTabPage.Controls.Add(blockSizeLabel);
                    SettingsTabControl.TabPages.Add(blockSizesTabPage);
                }

                int numericUpDownsInSettingsTabControlCount = SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.OfType<MyNumericUpDown>().Count();

                //remove warning label that says that the number of blocks has not been chosen yet
                if (SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.OfType<Label>().Select(label => label.Name).ToList().Contains(Global.Settings.NoBlocksExistYetLabelName))
                    SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.RemoveByKey(Global.Settings.NoBlocksExistYetLabelName);

                if (int.TryParse(BlockCountComboBox.Text, out int blockCount))
                {
                    if (numericUpDownsInSettingsTabControlCount < blockCount)
                        //fill tabpage with controls
                        AddUnequalBlockSizesControls(numericUpDownsInSettingsTabControlCount, blockCount);
                    else
                    {
                        //else, remove surplus controls
                        for (int i = numericUpDownsInSettingsTabControlCount; i >= blockCount; i--)
                        {
                            SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.RemoveByKey(Global.Settings.BlockSizeNumericUpDownNameBasis + i.ToString());
                            SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.RemoveByKey(Global.Settings.BlockSizeLabelNameBasis + i.ToString());
                        }
                    }
                }
                else
                {
                    //add a warning label that says that the number of blocks has not been chosen yet
                    Label noBlocksExistYetLabel = new Label
                    {
                        Name = Global.Settings.NoBlocksExistYetLabelName,
                        Left = Global.Settings.DefaultVariableControl.Left,
                        Top = Global.Settings.NoBlocksExistYetLabel.Top,
                        Width = Global.Settings.NoBlocksExistYetLabel.Width,
                        Text = Global.Settings.NoBlocksExistYetLabelText
                    };

                    SettingsTabControl.TabPages[Global.Settings.BlockSizesTabPageName].Controls.Add(noBlocksExistYetLabel);
                }
            }
            else
                //the TabPage to set the block sizes is not needed, thus: remove it
                SettingsTabControl.TabPages.RemoveByKey(Global.Settings.BlockSizesTabPageName);

            SetBlockSizes();
        }

        private void BlockSizesAreEqualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveBlockSizesControls();
            UpdateTheoreticalUniqueBlockSetsLabel();
        }

        private void RememberSetsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Global.RememberSets = (int)RememberSetsNumericUpDown.Value;
        }

        private void DesiredUniqueSetsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Global.DesiredUniqueSets = (int)DesiredUniqueSetsNumericUpDown.Value;
        }

        /// <summary>
        /// Changes the Value of the NumericUpDown containing the desired BlockSets
        /// to match the maximum possible number of unique sets that can theoretically be created.
        /// </summary>
        private void SetDesiredUniqueBlockSetsToCreateToMax_Click(object sender, EventArgs e)
        {
            if(CalculateTheoreticalNumberOfUniqueSets() == 0)
            {
                //do nothing
            }
            else if(CalculateTheoreticalNumberOfUniqueSets() < (double)DesiredUniqueSetsNumericUpDown.Maximum)
                DesiredUniqueSetsNumericUpDown.Value = (decimal)CalculateTheoreticalNumberOfUniqueSets();
            else
                DesiredUniqueSetsNumericUpDown.Value = DesiredUniqueSetsNumericUpDown.Maximum;
        }

        /// <summary>
        /// Changes the Value of the NumericUpDown containing the desired BlockSets
        /// to match 99% of the maximum possible number of unique sets that can theoretically be created.
        /// </summary>
        private void SetDesiredUniqueBlockSetsToCreateTo99Percent_Click(object sender, EventArgs e)
        {
            double maxUniqueSets = CalculateTheoreticalNumberOfUniqueSets() * 0.99;

            if (maxUniqueSets == 0)
            {
                //do nothing
            }
            else if ((decimal)maxUniqueSets < DesiredUniqueSetsNumericUpDown.Maximum)
                DesiredUniqueSetsNumericUpDown.Value = Math.Round((decimal)maxUniqueSets, 0, MidpointRounding.AwayFromZero); //round decimal value here to avoid inconsistencies when
                                                                                                                             //casting the decimal to an integer
                                                                                                                             //at DesiredUniqueSetsNumericUpDown_ValueChanged
            else
                DesiredUniqueSetsNumericUpDown.Value = DesiredUniqueSetsNumericUpDown.Maximum;
        }

        private void CreateSubgroupsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CreateSubgroupControls();
        }
        private void PreferredSubgroupSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            IfNeededWarnUserThatSubgroupSizesMayHaveChanged();
            CreateSubgroupControls();
        }

        private void CheckForBlockSetUnicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Global.CheckForBlockSetUnicity = CheckForBlockSetUnicityCheckBox.Checked;
        }
        #endregion

        #region menuItems

        /// <summary>
        /// Disposes the selected ComboBox.
        /// </summary>
        private void MenuItemRemoveComboBox(object sender, EventArgs e)
        {
            if (!SettingsTabControl.TabPages.ContainsKey(Global.Settings.SubgroupSizesTabPageName))
                return;

            var senderMenuItem = (MenuItem)sender;
            Control senderControl = (Control)senderMenuItem.Tag;
            
            //get block number and subgroup number from the control's name
            int blockNumber = int.Parse(senderControl.Name.Split('.')[0].Substring(Global.Settings.SubgroupSizeNumericUpDownNameBasis.Length));
            int subgroupNumber = int.Parse(senderControl.Name.Split('.')[1]);

            TabPage subgroupSizesTabPage = SettingsTabControl.TabPages[Global.Settings.SubgroupSizesTabPageName];
            List<MyNumericUpDown> numericUpDownsOfCurrentBlock = new List<MyNumericUpDown>();

            //collect all comboboxes within the current block, but with a larger subgroup number
            numericUpDownsOfCurrentBlock.AddRange(subgroupSizesTabPage.Controls.OfType<MyNumericUpDown>()
                .Where(numericUpDown => numericUpDown.Name.Split('.')[0] == Global.Settings.SubgroupSizeNumericUpDownNameBasis + blockNumber.ToString()
                && int.Parse(numericUpDown.Name.Split('.')[1]) > subgroupNumber).ToArray());

            //dispose the sender control
            senderControl.Dispose();

            int lastSubgroupOfCurrentBlock = subgroupNumber;

            //move the addButton left
            ((Button)subgroupSizesTabPage.Controls.Find(Global.Settings.AddSubgroupControlsButtonNameBasis + blockNumber.ToString(), false)[0]).Left -= senderControl.Width;
            
            //rename and move all comboboxes that are within the same block
            if (numericUpDownsOfCurrentBlock.Count != 0)
            {
                //sort the previously collected comboboxes
                numericUpDownsOfCurrentBlock = numericUpDownsOfCurrentBlock
                    .OrderBy(numericUpDown => numericUpDown.Name.Split('.')[1])
                    .OrderBy(numericUpDown => numericUpDown.Name.Split('.')[1].Length).ToList();

                //rename the remaining comboboxes by changeing the subgroupNumber
                foreach (MyNumericUpDown numericUpDown in numericUpDownsOfCurrentBlock)
                {
                    lastSubgroupOfCurrentBlock = int.Parse(numericUpDown.Name.Split('.')[1]); //find the last subgroup number within the current block
                    numericUpDown.Name = numericUpDown.Name.Split('.')[0] + '.' + (int.Parse(numericUpDown.Name.Split('.')[1]) - 1).ToString(); //rename control
                    numericUpDown.Left -= numericUpDown.Width; //finally, move the controls left
                }
            }

            //finally, if no numericUpDowns can be created that correspond to the last subgroup, it means that the subgroupLabel can be removed
            if (!(subgroupSizesTabPage.Controls.OfType<MyNumericUpDown>()
                .Where(comboBox => comboBox.Name.Split('.')[0].Contains(Global.Settings.SubgroupSizeNumericUpDownNameBasis)
                && comboBox.Name.Split('.')[1] == lastSubgroupOfCurrentBlock.ToString()).Count() > 0))
            {
                subgroupSizesTabPage.Controls.RemoveByKey(Global.Settings.SubgroupNumberLabelNameBasis + lastSubgroupOfCurrentBlock.ToString());
            }
        }

        private void SubgroupSizeNumericUpDown_MouseClick(object sender, MouseEventArgs e)
        {
            NumericUpDown senderNumericUpDown = sender as MyNumericUpDown;
            
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();

                var RemoveControlMenuItem = new MenuItem("Delete", MenuItemRemoveComboBox)
                {
                    Tag = sender
                };

                contextMenu.MenuItems.Add(RemoveControlMenuItem);

                contextMenu.Show(senderNumericUpDown, new Point(e.X, e.Y));
            }
        }

        /// <summary>
        /// Displays a MessageBox describing the value of a given SaveState.
        /// </summary>
        /// <param name="saveState">The desired SaveState for which to display a MessageBox.</param>
        /// <param name="suppressSaveFailedMessage">An optional boolean which should be true if no message should be displayed on a successful save.</param>
        /// <param name="suppressSaveNotNeededMessage">An optional boolean which should be true if no message should be displayed when a save is not needed.</param>
        /// <param name="suppressSaveSuccessMessage">An optional boolean which should be true if no message should be displayed on a failed save.</param>
        private static void ShowSaveMessageBox(SaveState saveState, bool suppressSaveSuccessMessage = false, bool suppressSaveNotNeededMessage = false, bool suppressSaveFailedMessage = false)
        {
            if (saveState == SaveState.Success && !suppressSaveSuccessMessage)
                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            else if (saveState == SaveState.NotNeeded && !suppressSaveNotNeededMessage)
                MessageBox.Show("No saveable results exist yet!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (saveState == SaveState.Failed && !suppressSaveFailedMessage)
                MessageBox.Show("Data could not be saved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSaveMessageBox(SaveExperimentIfPossibleNeededAndWanted(forceSave: true));
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSaveMessageBox(SaveExperimentIfPossibleNeededAndWanted(forceSave: true, forceAskUserForFilePath: true));
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveExperimentIfPossibleNeededAndWanted("Loading data...") != SaveState.Cancelled)
                LoadAndProcessExperiment();
        }

        /// <summary>
        /// Loads an Experiment from a file and displays its results in the DataGridViews, if load is successful.
        /// </summary>
        /// <param name="filePath">An optional string containing the full path of a file that needs to be loaded.</param>
        /// <param name="suppressLoadSuccessMessage">An optional boolean which should be true if no message should be displayed when data is loaded successfully.</param>
        private void LoadAndProcessExperiment(string filePath = "", bool suppressLoadSuccessMessage = false)
        {
            LoadState loadState = LoadExperiment(filePath);

            if (loadState == LoadState.Success)
            {
                EnableOrDisableAllControls(EnableOrDisable.Enable);
                InputDataGridView.PasteString(Global.FinishedExperiment.InputData);
                InterpretPastedData();

                BlockSetsResultsDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString());

                if(!suppressLoadSuccessMessage)
                    MessageBox.Show("Data loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else if (loadState == LoadState.Failed)
                MessageBox.Show("Data could not be loaded. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Resets the entire MainForm.
        /// </summary>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetUI();
        }
        
        /// <summary>
        /// Resets the current user interface and resets the current Experiment.
        /// </summary>
        private void ResetUI()
        {
            if (SaveExperimentIfPossibleNeededAndWanted(dialogTitle: "Resetting...") != SaveState.Cancelled)
            {
                SuppressWarningThatSubgroupSizesMayHaveChanged = true;

                //Clear Experiments
                Global.CurrentExperiment = new Experiment();
                Global.FinishedExperiment = null;

                //Clear result-dataGridViews and clear dgvInput
                List<MyDataGridView> allDataGridViews = new List<MyDataGridView> { InputDataGridView, BlockSetsResultsDataGridView, NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView, SubgroupCompositionsDataGridView, BlocksDescriptivesDataGridView };

                foreach (MyDataGridView dataGridView in allDataGridViews)
                    dataGridView.ClearRowsAndColumns();

                InterpretPastedData();
                NamesOfExperimentalUnitsInputColumnNumberComboBox.SelectedIndex = -1;

                //Reset buttons etc.
                BlockSizesAreEqualCheckBox.Checked = true;
                BlockCountComboBox.SelectedIndex = -1;
                NumberOfExperimentalUnitsPerBlockNumericUpDown.Value = NumberOfExperimentalUnitsPerBlockNumericUpDown.Minimum;

                DesiredUniqueSetsNumericUpDown.Value = Global.Settings.DefaultDesiredUniqueBlockSets;
                RememberSetsNumericUpDown.Value = Global.Settings.DefaultRememberSets;

                CheckForBlockSetUnicityCheckBox.Checked = Global.Settings.DefaultCheckForBlockSetUnicityCheckBoxChecked;
                CreateSubgroupsCheckBox.Checked = false;

                MeanCheckBox.Checked = true;
                SDCheckBox.Checked = true;
                MinCheckBox.Checked = false;
                MedianCheckBox.Checked = false;
                MaxCheckBox.Checked = false;

                //Reset progress bar and related controls
                MainProgressBar.Value = 0;
                TheoreticalUniqueBlockSetsLabel.Text = Global.Settings.DefaultTheoreticalUniqueBlockSetsLabelText;
                TimeElapsedLabel.Text = Global.Settings.DefaultTimeElapsedLabelText;
                TimeRemainingLabel.Text = Global.Settings.DefaultTimeRemainingLabelText;
                ProgressLabel.Text = Global.Settings.DefaultProgressLabelText;
                ProgressPercentageLabel.Text = Global.Settings.DefaultProgressPercentageLabelText;

                //reset Variables and controls
                CreateExperimentalUnitsOfCurrentExperiment();

                String[] namesOfControlsToKeep = new string[] { NoVariablesAvailableYetLabel.Name, VariableNamesLabel.Name, VariableDecimalPlacesLabel.Name, VariableWeightsLabel.Name };

                for (int i = SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls.Count - 1; i >= 0; i--)
                {
                    if (!namesOfControlsToKeep.Contains(SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[i].Name))
                        SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[i].Dispose();
                    else if (SettingsTabControl.TabPages[Global.Settings.VariablesTabPageName].Controls[i].Name == NoVariablesAvailableYetLabel.Name)
                        NoVariablesAvailableYetLabel.Visible = true;
                }

                CreateVariablesOfCurrentExperiment(inputDataGridViewDataHasChanged: true);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm newAboutForm = new AboutForm(Global.CurrentAssemblyVersion))
            {
                var result = newAboutForm.ShowDialog(); //this makes sure that only one instance of the AboutForm can be initialized.
            }
        }
        #endregion
        
        #region results

        /// <summary>
        /// Displays information of a BlockSet in DatagridViews.
        /// </summary>
        /// <param name="selectedBlockSetIndex">The index value of the BlockSet in Global.finishedExperiment.Runs.Last().BlockSets.</param>
        private void DisplayBlockSetResults(int selectedBlockSetIndex)
        {
            if (Global.FinishedExperiment != null && selectedBlockSetIndex < Global.FinishedExperiment.Runs.Last().BlockSets.Count  && selectedBlockSetIndex >= 0)
            {
                //paste BlockSet details into the datagridviews
                NamesOfExperimentalUnitsInBlocksOfBlockSetDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerBlockAsString(selectedBlockSetIndex));
                BlocksDescriptivesDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetDescriptivesOfExperimentalUnitsAsString(selectedBlockSetIndex, Global.DescriptiveCheckboxes));

                if (Global.FinishedExperiment.CreateSubgroups)
                    SubgroupCompositionsDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerSubgroupAsString(selectedBlockSetIndex));
                else
                    SubgroupCompositionsDataGridView.ClearRowsAndColumns();
            }
        }

        /// <summary>
        /// Gets the block set number from the currently selected row and displays information
        /// of the corresponding BlockSet in other DatagridViews.
        /// </summary>
        private void BlockSetsResultsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ExportToExcelBackgroundWorker.IsBusy == true)
                return; //prevents that the ExportToExcelBackgroundWorker triggers the current event

            if (SelectedBlockSetIndex == null)
                return;

            if (BlockSetsResultsDataGridView.CurrentlySelectedValue != (Int32)SelectedBlockSetIndex)
            {
                //save selected block set index for later use
                BlockSetsResultsDataGridView.CurrentlySelectedValue = (Int32)SelectedBlockSetIndex;

                //Display information of the BlockSet corresponding to the selected row index
                DisplayBlockSetResults((Int32)SelectedBlockSetIndex);
            }
        }

        /// <summary>
        /// Filters block sets based on the maximum number of markers to change provided by the user.
        /// </summary>
        private void BlockSetsResultsDataGridView_FilterByMarkersToChange(object sender, EventArgsWithValue e)
        {
            int maximumMarkersToChange = e.IntegerValue;

            if (Global.FinishedExperiment != null && Global.FinishedExperiment.ExperimentalUnitsHaveMarkers)
            {
                //clone FinishedExperiment, and create a new run containing all BlockSets from FinishedExperiment's last Run that match the criterium.
                Experiment filteredExperiment = Global.FinishedExperiment.Clone();
                Global.FinishedExperiment.Runs.Last().NumberBlockSetsByRank();
                filteredExperiment.Runs.Add(filteredExperiment.CreateNewRun());
                filteredExperiment.Runs.Last().BlockSets = Global.FinishedExperiment.Runs.Last().BlockSets.Where(blockset => blockset.NonDistributableExperimentalUnits.Count <= maximumMarkersToChange).Select(blockset => blockset).ToList(); //clone list

                //display filteredExperiment in BlockSetsResultsDataGridView
                if (filteredExperiment.Runs.Last().BlockSets.Count > 0)
                    BlockSetsResultsDataGridView.PasteString(filteredExperiment.Runs.Last().GetSetsAndRanksAsString());
                else
                    MessageBox.Show("None of the results match the filter criterium.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// Filters block sets based on the categories provided by the user.
        /// </summary>
        private void BlockSetsResultsDataGridView_FilterByCategory(object sender, EventArgs e)
        {
            if (Global.FinishedExperiment != null)
            {
                //clone FinishedExperiment, and create a new run containing all BlockSets from FinishedExperiment's last Run that match the criterium.
                Experiment filteredExperiment = Global.FinishedExperiment.Clone();
                Global.FinishedExperiment.Runs.Last().NumberBlockSetsByRank();
                filteredExperiment.Runs.Add(filteredExperiment.CreateNewRun());
                filteredExperiment.Runs.Last().BlockSets = Global.FinishedExperiment.Runs.Last().BlockSets.Where(blockset => blockset.EachBlockContainsAllExperimentalUnitCategories).Select(blockset => blockset).ToList(); //clone list

                //display filteredExperiment in BlockSetsResultsDataGridView
                if (filteredExperiment.Runs.Last().BlockSets.Count > 0)
                    BlockSetsResultsDataGridView.PasteString(filteredExperiment.Runs.Last().GetSetsAndRanksAsString());
                else
                    MessageBox.Show("None of the results match the filter criterium.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Removes all filters, i.e. displays all block sets.
        /// </summary>
        private void BlockSetsResultsDataGridView_RemoveAllFilters(object sender, EventArgs e)
        {
            if (Global.FinishedExperiment != null)
                BlockSetsResultsDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString());
        }

        /// <summary>
        /// Gets the block set number from the currently selected row and displays information
        /// of the corresponding BlockSet in other DatagridViews.
        /// </summary>
        /// <example>This event is usually raised when moving through the datagridview with the arrow keys.</example>
        private void BlockSetsResultsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectedBlockSetIndex == null || Global.AnyBackgroundWorkerIsBusy)
                return;

            DisplayBlockSetResults((Int32)SelectedBlockSetIndex);
        }

        /// <summary>
        /// Re-fills data in DescriptivesOfBlocksInBlockSetDataGridView
        /// to display only the selected descriptives.
        /// </summary>
        private void DescriptiveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedBlockSetIndex == null)
                return;

            try
            {
                if (SelectedBlockSetIndex < Global.FinishedExperiment.Runs.Last().BlockSets.Count())
                    BlocksDescriptivesDataGridView.PasteString(Global.FinishedExperiment.Runs.Last().GetDescriptivesOfExperimentalUnitsAsString((Int32)SelectedBlockSetIndex, Global.DescriptiveCheckboxes));
            }
            catch (System.NullReferenceException)
            {
                //likely no FinishedExperiment yet exists.
                return;
            }
        }
        #endregion

        #region exportAndImportResults
        /// <summary>
        /// Finds the occurance of a given string within a given string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="substring">The substring of which to find the occurance.</param>
        /// <returns>The occurance of the substring in the string as an integer value.</returns>
        private int OccuranceOfSubstring(string source, string substring)
        {
            int count = 0, n = 0;

            if (substring != string.Empty)
            {
                while ((n = source.IndexOf(substring, n, StringComparison.InvariantCulture)) != -1)
                {
                    n += substring.Length;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Releases a ComObject that is needed, for example, when finished creating Excel files.
        /// </summary>
        /// <param name="ComObjectToRelease">The ComObject that is to be released.</param>
        private void ReleaseObject(object ComObjectToRelease)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ComObjectToRelease);
                ComObjectToRelease = null;
            }
            catch (Exception ex)
            {
                ComObjectToRelease = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Check if any results can be exported, and
        /// exports results to a desired file location.
        /// </summary>
        private void ExportResultsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string fileDescription = menuItem.Tag.ToString(), fileExtension = menuItem.Text, delimiter = string.Empty;

            //set delimiter
            if (fileExtension == ".csv")
                delimiter = ",";
            else if (fileExtension == ".tsv")
                delimiter = "\t";
            else if (fileExtension == ".txt")
                delimiter = ";";

            //check if any results can be exported
            try
            {
                if (Global.FinishedExperiment.Runs.Last().BlockSets.Count() < 1)
                {
                    MessageBox.Show("Before exporting the results, first run the program.", "Please run the program first", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("I'm sorry, no exportable results could be found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //ask user for file save location
            SaveFileDialog exportFileSaveFileDialog = new SaveFileDialog
            {
                Filter = fileDescription + " (*" + fileExtension + ")| *" + fileExtension,
                FileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString("d2") + "_" + DateTime.Now.Day.ToString("d2") + " " + "RandoMice Results" + menuItem.Text
            };

            //start BackgroundWorker
            if (exportFileSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stopwatch.Restart();

                if (fileExtension == ".xls" && !ExportToExcelBackgroundWorker.IsBusy) //export to Excel
                {
                    CreateBlockSetsTimer.Start();
                    ExportToExcelBackgroundWorker.RunWorkerAsync(argument: (exportFileSaveFileDialog.FileName));
                }
                else if (!ExportToFileBackGroundWorker.IsBusy) //export to another file type
                {
                    CreateBlockSetsTimer.Start();
                    ExportToFileBackGroundWorker.RunWorkerAsync(argument: new Tuple<string, string>(exportFileSaveFileDialog.FileName, delimiter));
                }
            }

            exportFileSaveFileDialog.Dispose();
        }

        /// <summary>
        /// Pastes a string that is separated by tabs and newlines into a given Excel WorkSheet.
        /// </summary>
        /// <param name="outputString">A string containing the content that needs to be pasted into the given Excel Worksheet.</param>
        /// <param name="xlWorkSheet">An Excel Worksheet in which the inputString needs to be pasted.</param>
        /// <param name="startRow">The start row number,
        /// where the first line of the inputString needs to be pasted.</param>
        /// <param name="startCol">The start column number,
        /// where the first column of the inputString needs to be pasted.</param>
        /// <returns>A tuple containing the last row and column to which data was pasted.</returns>
        private (int EndRow, int EndCol) PasteDataToExcel(string outputString, Excel.Worksheet xlWorkSheet, int startRow, int startCol)
        {
            int columnNumber = startCol;
            int rowNumber = startRow;

            //trim the input string on \r\n to find all rows to past
            string[] outputRows = System.Text.RegularExpressions.Regex.Split(outputString.TrimEnd("\r\n".ToCharArray()), "\r\n");
            
            for (int i = 0; i < outputRows.Count(); i++)
            {
                columnNumber = startCol;

                //split output row by tabs
                string[] outputColumns = System.Text.RegularExpressions.Regex.Split(outputRows[i].TrimEnd("\t".ToCharArray()), "\t");
                
                for (int j = 0; j < outputColumns.Count(); j++)
                {
                    //paste values to the Excel WorkSheet
                    if (double.TryParse(outputColumns[j], out double value))
                        xlWorkSheet.Cells[rowNumber, columnNumber] = value;
                    else
                        xlWorkSheet.Cells[rowNumber, columnNumber] = outputColumns[j];
                    
                    columnNumber++;
                }
                
                rowNumber++;
            }

            return (rowNumber - 1, columnNumber - 1);
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            //Paste data into datagridview
            InputDataGridView.PasteClipboard(); //Interpretation of the data will be done upon the OnPasteData event of the InputDataGridView
        }

        /// <summary>
        /// Resets various ComboBoxes to default values.
        /// </summary>
        /// <param name="warnUserForInvalidDataPoints">A bool to indicate whether or not the user should
        /// be warned if the pasted string contains invalid data points. Default is false.</param>
        /// <param name="askUserIfDatesShouldBeConvertedToValues">A bool to indicate whether or not the user should
        /// be asked if strings containing a valid DateTime should be converted into values. Default is false.</param>
        /// <remarks>For example, this method needs to be called after the user changes data of the InputDataGridView.</remarks>
        public void InterpretPastedData(bool warnUserForInvalidDataPoints = false, bool askUserIfDatesShouldBeConvertedToValues = false)
        {
            //make sure ExperimentalUnits and Variables are up to date
            CreateVariablesOfCurrentExperiment(inputDataGridViewDataHasChanged: true, warnUserForInvalidDataPoints, askUserIfDatesShouldBeConvertedToValues);
            CreateExperimentalUnitsOfCurrentExperiment();

            //empty and re-fill comboboxes
            ResetComboBox(BlockCountComboBox, newMinValue: 2, newItemsCount: Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count - 1, Global.Settings.MaximalNumberOfBlocks - 1), defaultSelectedIndex: 0);
            ResetComboBox(MarkersOfExperimentalUnitsInputColumnNumberComboBox, newMinValue: 1, newItemsCount: InputDataGridView.ColumnCount, defaultSelectedIndex: 0, optionalExtraItem: "None");
            ResetComboBox(NamesOfExperimentalUnitsInputColumnNumberComboBox, defaultSelectedIndex: 0, newMinValue: 1, newItemsCount: InputDataGridView.ColumnCount);

            //reset NumberOfExperimentalUnitsPerBlockNumericUpDown
            NumberOfExperimentalUnitsPerBlockNumericUpDown.Value = NumberOfExperimentalUnitsPerBlockNumericUpDown.Minimum;
            if (Global.CurrentExperiment.AllExperimentalUnits.Count != 0)
                NumberOfExperimentalUnitsPerBlockNumericUpDown.Maximum = Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count, short.MaxValue); //block sizes are of type short, so a block size must not exceed short.MaxValue
            else
                NumberOfExperimentalUnitsPerBlockNumericUpDown.Maximum = NumberOfExperimentalUnitsPerBlockNumericUpDown.Minimum;

            //reset PreferredSubgroupSizeNumericUpDown
            PreferredSubgroupSizeNumericUpDown.Value = PreferredSubgroupSizeNumericUpDown.Minimum;
            if(Global.CurrentExperiment.AllExperimentalUnits.Count != 0)
                PreferredSubgroupSizeNumericUpDown.Maximum = Math.Min(Global.CurrentExperiment.AllExperimentalUnits.Count, short.MaxValue); //subgroupsizes are of type short, so must not exceed short.MaxValue
            else
                NumberOfExperimentalUnitsPerBlockNumericUpDown.Maximum = NumberOfExperimentalUnitsPerBlockNumericUpDown.Minimum;

            //update ExperimentalUnits and Variables, because the markerInputColumn and nameInputColumn are reset above
            CreateVariablesOfCurrentExperiment(inputDataGridViewDataHasChanged: true, warnUserForInvalidDataPoints: false, askUserIfDatesShouldBeConvertedToValues: false);
            CreateExperimentalUnitsOfCurrentExperiment();

            //warnings are suppressed in ResetCombobox().
            //so, finally if needed warn the users that subgroup sizes may have changed once.
            IfNeededWarnUserThatSubgroupSizesMayHaveChanged();
        }

        /// <summary>
        /// Copies the content of a given DataGridView to the clipboard.
        /// </summary>
        /// <param name="sourceDataGridView">A DataGridView containing data that needs to be copied to the clipboard.</param>
        private void CopyAlltoClipboard(DataGridView sourceDataGridView)
        {
            sourceDataGridView.SelectAll();
            DataObject dataObj = sourceDataGridView.GetClipboardContent();
            
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        /// <summary>
        /// Exports information of finishedExperiment to Excel.
        /// </summary>
        private void ExportToExcelBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string fileName = (string)e.Argument;

            Control[] controlsToEnable = { AbortRunButton, MainProgressBar };
            Global.ControlsAndEnabledStates = EnableOrDisableAllControls(EnableOrDisable.Disable);
            EnableOrDisableControls(EnableOrDisable.Enable, controlsToEnable);

            //find number of block sets to export
            int numberOfBlockSetsToExport = Global.FinishedExperiment.Runs.Last().BlockSets.Count();
            
            //if more than 100 block sets need to be exported, warn user and ask how to continue
            if (Global.FinishedExperiment.Runs.Last().BlockSets.Count() > 100)
            {
                DialogResult dialogResult = MessageBox.Show("The program will export more than 100 sets.\n" +
                    "Would you like to export the best 100 sets only?", "Export to excel", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
                if (dialogResult == DialogResult.Yes)
                    numberOfBlockSetsToExport = 100;
            }

            //set Excel application
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlExcel = new Excel.Application
            {
                DisplayAlerts = false // If DisplayAlerts == true, you will get two confirm overwrite prompts
            };

            //create new workbook
            Excel.Workbook xlWorkBook = xlExcel.Workbooks.Add(misValue);

            try
            {
                xlWorkBook.Worksheets.Item[1].Delete(); //delete all existing worksheets
                xlWorkBook.Worksheets.Item[1].Delete();
            }
            catch
            {
                //do nothing, likely no Worksheet was found that could be deleted
            }

            //try to save workbook to see if saving is possible
            try
            {
                xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Cannot save the excel file. Close all currently open excel files and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //create new worksheet
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
            xlWorkSheet.Name = "Overview of set and ranks";

            try
            {
                xlWorkBook.Worksheets.Item[1].Delete(); //delete existing worksheet "sheet1"
            }
            catch
            {
                //do nothing, likely no Worksheet was found that could be deleted
            }

            //Paste block sets with ranks and optional markers to Excel
            PasteDataToExcel(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString(), xlWorkSheet, 1, 1);

            //Paste run info into new sheet
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
            xlWorkSheet.Name = "Run info";
            string runInfo = "RandoMice Version " + Global.CurrentAssemblyVersion;
            runInfo += "\r\nSets created: " + Global.FinishedExperiment.Runs.Last().UniqueSetsCreated.ToString();
            runInfo += "\r\nTime elapsed (hh:mm:ss): " + Global.FinishedExperiment.Runs.Last().TotalTimeElapsedForCreatingBlockSets.ToString(Global.Settings.TimeSpanStringFormat);
            PasteDataToExcel(runInfo, xlWorkSheet, 1, 1);

            //Paste input data into new sheet
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
            xlWorkSheet.Name = "Data of experimental units";
            PasteDataToExcel(Global.FinishedExperiment.InputData, xlWorkSheet, 1, 1);

            //deselect all in excel
            Excel.Range xlRange = (Excel.Range)xlWorkSheet.Cells[1, 1];
            xlRange.Select();

            Stopwatch.Restart();
            int progressPercentage = 0;

            DateTime datetimeLastProgressChangedUpdate = DateTime.Now;
            TimeSpan timeDiff = new TimeSpan();

            for (int i = 0; i < numberOfBlockSetsToExport; i++)
            {
                //create a new worksheet for each block set to export
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
                xlWorkSheet.Name = "Set " + (i + 1).ToString();

                int defaultHorizontalOffset = 2;
                ((int Row, int Column) TopLeftCell, (int Row, int Column) BottomRightCell) locationExperimentalUnitNames = ((1, 1), (1, 1));
                ((int Row, int Column) TopLeftCell, (int Row, int Column) BottomRightCell) locationSubgroups = ((1, 1), (1, 1));
                ((int Row, int Column) TopLeftCell, (int Row, int Column) BottomRightCell) locationDescriptives = ((1, 1), (1, 1));
                ((int Row, int Column) TopLeftCell, (int Row, int Column) BottomRightCell) locationExperimentalUnitNamesAndVariables = ((1, 1), (1, 1));

                //paste names of experimental units
                locationExperimentalUnitNames.BottomRightCell = PasteDataToExcel(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerBlockAsString(i), xlWorkSheet, locationExperimentalUnitNames.TopLeftCell.Row, locationExperimentalUnitNames.TopLeftCell.Column);

                //paste subgroup details, if needed
                if (Global.FinishedExperiment.CreateSubgroups)
                {
                    locationSubgroups.TopLeftCell = (locationExperimentalUnitNames.BottomRightCell.Row + defaultHorizontalOffset, 1);
                    locationSubgroups.BottomRightCell = PasteDataToExcel(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerSubgroupAsString(i), xlWorkSheet, locationSubgroups.TopLeftCell.Row, locationSubgroups.TopLeftCell.Column);
                }

                //paste descriptives of each block
                locationDescriptives.TopLeftCell = (1, Math.Max(locationExperimentalUnitNames.BottomRightCell.Column, locationSubgroups.BottomRightCell.Column) + defaultHorizontalOffset);
                locationDescriptives.BottomRightCell = PasteDataToExcel(Global.FinishedExperiment.Runs.Last().GetDescriptivesOfExperimentalUnitsAsString(i, Global.DescriptiveCheckboxes), xlWorkSheet, locationDescriptives.TopLeftCell.Row, locationDescriptives.TopLeftCell.Column);

                //per block, paste the names and variables of each experimental unit
                locationExperimentalUnitNamesAndVariables.TopLeftCell = (new[] { locationDescriptives.BottomRightCell.Row, locationSubgroups.BottomRightCell.Row, locationExperimentalUnitNames.BottomRightCell.Row }.Max() + defaultHorizontalOffset, 1);
                locationExperimentalUnitNamesAndVariables.BottomRightCell = PasteDataToExcel(Global.FinishedExperiment.Runs.Last().GetNamesAndVariablesOfExperimentalUnitsPerBlockAsString(i), xlWorkSheet, locationExperimentalUnitNamesAndVariables.TopLeftCell.Row, locationExperimentalUnitNamesAndVariables.TopLeftCell.Column);

                //report progress
                timeDiff = DateTime.Now - datetimeLastProgressChangedUpdate;

                if ((int)timeDiff.TotalMilliseconds > Global.Settings.BackgroundWorkerReportProgressPeriod)
                {
                    Global.FinishedExperiment.Runs.Last().ProgressPercentage = (double)(i + 1) / (double)numberOfBlockSetsToExport * 100;//to calculate time remaining
                    progressPercentage = (int)Math.Floor(Global.FinishedExperiment.Runs.Last().ProgressPercentage);
                    worker.ReportProgress(progressPercentage, new int[] { i + 1, numberOfBlockSetsToExport });
                    datetimeLastProgressChangedUpdate = DateTime.Now;
                }

                //check if user pressed the abort button
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                    //backgroundworker will finish as normal, but any remaining sets will not be exported
                }
            }

            //Select the first worksheet
            try
            {
                ((Excel.Worksheet)xlWorkBook.Worksheets[1]).Select();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                //do nothing
            }

            //Save the excel file under the captured location from the SaveFileDialog
            try
            {
                xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Cannot save the Excel file. Please close any currently open Excel files and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }

            //clean up
            xlExcel.DisplayAlerts = true;
            xlWorkBook.Close(true, misValue, misValue);
            xlExcel.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlExcel);

            if (!e.Cancel)
            {
                //report progress a final time
                progressPercentage = 100; //update to 100%
                worker.ReportProgress(progressPercentage, new int[] { numberOfBlockSetsToExport, numberOfBlockSetsToExport });
            }

            e.Result = fileName; //for use in RunWorkerCompleted

            //Open explorer and select the newly created Excel file
            if (File.Exists(fileName))
            {
                string argument = "/select, \"" + fileName + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
                throw new InvalidOperationException("File cannot be found."); //error will be handled by RunWorkerCompleted
        }

        private void ExportToFileBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Tuple<string, string> arguments = e.Argument as Tuple<string, string>;
            string fileName = (string)arguments.Item1, delimiter = (string)arguments.Item2;

            //later on, it is useful to replace the decimal separator if it equals the file delimiter
            bool replaceDecimalSeparator = (delimiter == Global.CultureNumberFormatInfo.NumberDecimalSeparator);

            //lock controls
            Control[] controlsToEnable = { AbortRunButton, MainProgressBar };
            Global.ControlsAndEnabledStates = EnableOrDisableAllControls(EnableOrDisable.Disable);
            EnableOrDisableControls(EnableOrDisable.Enable, controlsToEnable);

            Run selectedRun = Global.FinishedExperiment.Runs.Last();
            int numberOfBlockSetsToExport = selectedRun.BlockSets.Count;

            Cursor.Current = Cursors.WaitCursor;

            Stopwatch.Restart();
            int progressPercentage = 0;

            DateTime datetimeLastUpdate = DateTime.Now;
            TimeSpan timeDiff;

            try
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(fileName))
                {
                    //write some info about the run
                    file.WriteLine("RandoMice Version " + Global.CurrentAssemblyVersion);
                    file.WriteLine("Sets created: " + Global.FinishedExperiment.Runs.Last().UniqueSetsCreated.ToString());
                    file.WriteLine("Time elapsed (hh:mm:ss): " + Global.FinishedExperiment.Runs.Last().TotalTimeElapsedForCreatingBlockSets.ToString(Global.Settings.TimeSpanStringFormat));
                    file.WriteLine(); //write empty line
                    file.WriteLine("Input:");

                    //write data corresponding to the original input
                    if (!replaceDecimalSeparator)
                        file.WriteLine(Global.FinishedExperiment.InputData.Replace("\t", delimiter));
                    else
                        file.WriteLine(Global.FinishedExperiment.InputData.Replace(delimiter, ".").Replace("\t", delimiter));

                    file.WriteLine();//write empty line

                    //write block set numbers and ranks
                    if (!replaceDecimalSeparator)
                        file.WriteLine(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString().Replace("\t", delimiter));
                    else
                        file.WriteLine(Global.FinishedExperiment.Runs.Last().GetSetsAndRanksAsString().Replace(delimiter, ".").Replace("\t", delimiter));

                    file.WriteLine();//write empty line

                    //for each block set, write the block compositions, optionally the subgroup compositions, the block composition with variable values, and the block descriptives
                    for (int i = 0; i < numberOfBlockSetsToExport; i++)
                    {
                        file.WriteLine("Set number " + (i + 1).ToString() + ":");

                        //write block compositions
                        string[] lines = System.Text.RegularExpressions.Regex.Split(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerBlockAsString(i).TrimEnd("\r\n".ToCharArray()), "\r\n"); ;

                        foreach (string line in lines)
                        {
                            if (!replaceDecimalSeparator)
                                file.WriteLine(line.Replace("\t", delimiter));
                            else
                                file.WriteLine(line.Replace(delimiter, ".").Replace("\t", delimiter));
                        }

                        file.WriteLine();//write empty line

                        //optionally write subgroup compositions
                        if (Global.FinishedExperiment.CreateSubgroups)
                        {
                            lines = System.Text.RegularExpressions.Regex.Split(Global.FinishedExperiment.Runs.Last().GetExperimentalUnitNamesPerSubgroupAsString(i).TrimEnd("\r\n".ToCharArray()), "\r\n"); ;

                            foreach (string line in lines)
                            {
                                if (!replaceDecimalSeparator)
                                    file.WriteLine(line.Replace("\t", delimiter));
                                else
                                    file.WriteLine(line.Replace(delimiter, ".").Replace("\t", delimiter));
                            }

                            file.WriteLine();//write empty line
                        }

                        //write block composition, with corresponding variable values
                        lines = System.Text.RegularExpressions.Regex.Split(Global.FinishedExperiment.Runs.Last().GetNamesAndVariablesOfExperimentalUnitsPerBlockAsString(i).TrimEnd("\r\n".ToCharArray()), "\r\n"); ;

                        foreach (string line in lines)
                        {
                            if (!replaceDecimalSeparator)
                                file.WriteLine(line.Replace("\t", delimiter));
                            else
                                file.WriteLine(line.Replace(delimiter, ".").Replace("\t", delimiter));
                        }

                        file.WriteLine();//write empty line

                        //finally write block descriptives
                        lines = System.Text.RegularExpressions.Regex.Split(Global.FinishedExperiment.Runs.Last().GetDescriptivesOfExperimentalUnitsAsString(i, Global.DescriptiveCheckboxes).TrimEnd("\r\n".ToCharArray()), "\r\n"); ;

                        foreach (string line in lines)
                        {
                            if (!replaceDecimalSeparator)
                                file.WriteLine(line.Replace("\t", delimiter));
                            else
                                file.WriteLine(line.Replace(delimiter, ".").Replace("\t", delimiter));
                        }

                        file.WriteLine();//write empty line

                        //report progress 20 times per second
                        timeDiff = DateTime.Now - datetimeLastUpdate;

                        if ((int)timeDiff.TotalMilliseconds > Global.Settings.BackgroundWorkerReportProgressPeriod)
                        {
                            Global.SetsExported = i + 1;
                            Global.FinishedExperiment.Runs.Last().ProgressPercentage = (double)(i + 1) / (double)numberOfBlockSetsToExport * 100;//to calculate time remaining
                            progressPercentage = (int)Math.Floor(Global.FinishedExperiment.Runs.Last().ProgressPercentage);
                            worker.ReportProgress(progressPercentage, new int[] { Global.SetsExported, numberOfBlockSetsToExport });
                            datetimeLastUpdate = DateTime.Now;
                        }

                        //check if user pressed the abort button
                        if (worker.CancellationPending == true)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Cannot access the file. Please close any currently open files and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }

            if (!e.Cancel)
                progressPercentage = 100;

            worker.ReportProgress(progressPercentage, new int[] { Global.SetsExported, numberOfBlockSetsToExport });

            e.Result = fileName; //for use in RunWorkerCompleted

            if (!e.Cancel)
            {
                //Open explorer and select the newly created file
                if (File.Exists(fileName))
                {
                    string argument = "/select, \"" + fileName + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
                else
                    throw new InvalidOperationException("File cannot be found."); //error will be handled by RunWorkerCompleted
            }
        }

        private void ExportToFileBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int[] progress = (int[])e.UserState;
            ProgressChanged(progress[0], progress[1], (int)Global.FinishedExperiment.Runs.Last().ProgressPercentage, exportingSets: true);
        }

        private void ExportToFileBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Arrow;
            
            if (e.Cancelled == true)
            {
                try
                {
                    if (ProgressLabel.Text.Substring(ProgressLabel.Text.Length - " - Cancelled!".Length) != " - Cancelled!")
                        ProgressLabel.Text += " - Cancelled!";
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    ProgressLabel.Text += " - Cancelled!";
                }
            }
            else if (e.Error != null)
            {
                ProgressLabel.Text = " - Error!"; //+ e.Error.Message;
                MessageBox.Show("Unknown error encountered, please try again." + "\n\nMessage:" + e.Error.Message + "\nInnerException:" + e.Error.InnerException + "\nSource:" + e.Error.Source + "\nStackTrace:" + e.Error.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    if (!ProgressLabel.Text.Contains(" - Finished!"))
                        ProgressLabel.Text += " - Finished!";

                    MessageBox.Show("Data sucessfully exported to " + e.Result.ToString(), "Export successful", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                catch
                {
                    ProgressLabel.Text += " - Finished!";
                }
            }

            CreateBlockSetsTimer.Stop();
            MainProgressBar.Value = 0;
            RevertEnabledStateOfControls(Global.ControlsAndEnabledStates);
        }
        #endregion

        #region ToolstripMenuItemEvents
        /// <summary>
        /// Displays the About form for a given duration.
        /// </summary>
        /// <param name="duration">The desired duration in milliseconds.</param>
        private void ShowAboutForm(int duration)
        {
            var newAboutForm = new AboutForm(Global.CurrentAssemblyVersion, displayLicenceAndCreditsButtons: false)
            {
                Text = this.Text,
                ControlBox = false
            };

            newAboutForm.Show();
            newAboutForm.Refresh();
            System.Threading.Thread.Sleep(duration); //display frmAbout for given duration before closing.
            newAboutForm.Close();
            newAboutForm.Dispose();
        }

        private void ViewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Global.Settings.ReadmePage);
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        /// <summary>
        /// Opens an url, and extracts an embedded xml document.
        /// </summary>
        /// <param name="url">A string containing the url of the web page that needs to be opened.</param>
        /// <param name="XmlTag">A string containing the Xml tag enclosing the embedded document.</param>
        /// <returns>An instance of XmlDocument based on the embedded Xml code.</returns>
        private XmlDocument LoadWebpageAndGetEmbeddedXmlDocument(string url, string XmlTag)
        {
            XmlDocument xmlDocument = new XmlDocument();
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.UserAgent = ".NET Framework Client"; //anything but an empty string

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string htmlText = reader.ReadToEnd();

                string xmlText = htmlText.Substring(htmlText.IndexOf("<" + XmlTag + ">"));
                xmlText = xmlText.Substring(0, xmlText.IndexOf(@"</" + XmlTag + ">") + ("</" + XmlTag + ">").Length + 1);

                xmlDocument.LoadXml(xmlText);
                return xmlDocument;
            }
        }

        /// <summary>
        /// Check the internet if an update is available.
        /// </summary>
        /// <param name="displayNoUpdateAvailableWarning">A boolean which is determines if a messagebox will be displayed if no updates are available. Default value is true.</param>
        /// <param name="displayErrorMessage">A boolean which is determines if a messagebox will be displayed if an error is thrown. Default is true.</param>
        private void CheckForUpdates(bool displayNoUpdateAvailableWarning = true, bool displayErrorMessage = true)
        {
            string checkForUpdatesURL = Global.Settings.CheckForUpdatesPage;

            try
            {
                XmlDocument updateInfoXmlDocument = LoadWebpageAndGetEmbeddedXmlDocument(checkForUpdatesURL, "randomicelatestversionxmldocument");

                //extract data from xml document
                int newMajor = int.Parse(updateInfoXmlDocument.SelectSingleNode("//currentversion/major").InnerText);
                int newMinor = int.Parse(updateInfoXmlDocument.SelectSingleNode("//currentversion/minor").InnerText);
                int newBuild = int.Parse(updateInfoXmlDocument.SelectSingleNode("//currentversion/build").InnerText);
                string newAssemblyVersion = "v" + string.Join(".", new string[] { newMajor.ToString(), newMinor.ToString(), newBuild.ToString() });

                string newVersionReleasesPageURL = updateInfoXmlDocument.SelectSingleNode("//releasespageurl").InnerText;

                //compare new version to old version
                bool navigateToDownloadsPage = false;

                if (newAssemblyVersion != Global.CurrentAssemblyVersion)
                {
                    string message = string.Format("Update available:\n\nCurrent version {0}\nNew version {1}\n\nDo you want to navigate to RandoMice's releases page now?",
                        Global.CurrentAssemblyVersion,
                        "v" + string.Join(".", new string[] { newMajor.ToString(), newMinor.ToString(), newBuild.ToString() }));

                    navigateToDownloadsPage = MessageBox.Show(message, "Update available", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes;
                }
                else if (displayNoUpdateAvailableWarning)
                    MessageBox.Show("Your version of RandoMice is up to date!", "RandoMice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //navigate to RandoMice's GitHub page if needed
                if (navigateToDownloadsPage)
                    Process.Start(newVersionReleasesPageURL);

                //save current date
                Properties.Settings.Default.LastCheckedForUpdates = string.Join("_", new string[] { DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString() });
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
                if (displayErrorMessage)
                {
                    if (MessageBox.Show("Error while checking for updates. Please try again later or manually check the web for updates.\n\nDo you want to navigate to RandoMice's releases page now?", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) == DialogResult.Yes)
                        Process.Start(Global.Settings.ReleasesPage);
                }
            }
        }
        #endregion

        #region defineGroupNames
        /// <summary>
        /// Displays a new form in which the user can provide group names.
        /// </summary>
        /// <param name="experiment">An experiment to which new Groups need to be added.</param>
        /// <param name="blockCount">An integer representing the number of blocks.</param>
        /// <returns>A bool which is true if group names have been successfully defined by the user.</returns>
        private bool DefineGroupNames(Experiment experiment, int blockCount)
        {
            using (RequestGroupNamesForm requestGroupNamesForm = new RequestGroupNamesForm(blockCount, experiment.Groups))
            {
                var result = requestGroupNamesForm.ShowDialog();

                if (result == DialogResult.OK)
                    experiment.Groups = requestGroupNamesForm.Groups.Select(group => group).ToList(); //values preserved after closing of form
            }

            return experiment.GroupNamesHaveBeenDefinedByUser;
        }

        /// <summary>
        /// Requests the user to define group names to be set in CurrentExperiment and,
        /// if CurrentExperiment and FinishedExperiment are equal, also sets the defined
        /// group names in FinishedExperiment.
        /// </summary>
        private void DefineGroupNamesLabel_Click(object sender, EventArgs e)
        {
            if (int.TryParse(BlockCountComboBox.Text, out int blockCount))
            {
                //only when group names have not yet been defined by user for FinishedExperiment,
                //and if FinishedExperiment equals CurrentExperiment, the Group names of 
                //FinishedExperiment should also be set by clicking the current Label.
                bool groupNamesOfFinishedExperimentShouldAlsoBeUpdated = Global.FinishedExperiment != null
                    && !Global.FinishedExperiment.GroupNamesHaveBeenDefinedByUser 
                    && Global.FinishedExperiment == Global.CurrentExperiment;

                //Define group names for CurrentExperiment, and if group names of FinishedExperiment should
                //also be updated, do so.
                if (DefineGroupNames(Global.CurrentExperiment, blockCount) && groupNamesOfFinishedExperimentShouldAlsoBeUpdated)
                    Global.FinishedExperiment.Groups = Global.CurrentExperiment.Groups.Select(group => group.Clone()).ToList();
                        //create a clone of CurrentExperiment.Group names to prevent that
                        //FinishedExperiment.Groups points to the same list of strings
            }
        }

        /// <summary>
        /// Asks the user to provide group names to be set in FinishedExperiment if needed,
        /// then randomly assigns blocks of the selected block set to the given group names
        /// and finally updates the result-DataGridViews.
        /// </summary>
        private void RandomlyAssignBlocksToGroups_Click(object sender, EventArgs e)
        {
            if (SelectedBlockSetIndex != null)
            {
                //only when group names have not yet been defined by user for CurrentExperiment and FinishedExperiment,
                //and if CurrentExperiment equals FinishedExperiment, the Group names of 
                //CurrentExperiment should also be set by clicking the current Button.
                bool groupNamesOfCurrentExperimentShouldAlsoBeUpdated = !Global.FinishedExperiment.GroupNamesHaveBeenDefinedByUser
                    && !Global.CurrentExperiment.GroupNamesHaveBeenDefinedByUser
                    && Global.CurrentExperiment == Global.FinishedExperiment;

                if (!Global.FinishedExperiment.GroupNamesHaveBeenDefinedByUser)
                {
                    MessageBox.Show("Please define the group names first.", "Group names have not yet been defined", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DefineGroupNames(Global.FinishedExperiment, Global.FinishedExperiment.BlockSizes.Count);
                }

                if (Global.FinishedExperiment.GroupNamesHaveBeenDefinedByUser)
                {
                    Global.FinishedExperiment.Runs.Last().RandomizeBlocksOfSet((Int32)SelectedBlockSetIndex); //Randomly assign BlocksOfExperimentalUnits to the Groups.
                    DisplayBlockSetResults((Int32)SelectedBlockSetIndex); //update dataGridViews

                    if (groupNamesOfCurrentExperimentShouldAlsoBeUpdated)
                        Global.CurrentExperiment.Groups = Global.FinishedExperiment.Groups.Select(group => group.Clone()).ToList();
                }
            }
        }

        private void ReDefineGroupNamesLabel_Click(object sender, EventArgs e)
        {
            if (Global.FinishedExperiment != null)
            {
                DefineGroupNames(Global.FinishedExperiment, Global.FinishedExperiment.BlockSizes.Count);
                DisplayBlockSetResults((Int32)SelectedBlockSetIndex);
            }
        }
        #endregion

        private void FilterResultsPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            (int X, int Y) offset = (FilterResultsPictureBox.Left - BlockSetsResultsDataGridView.Left, FilterResultsPictureBox.Top - BlockSetsResultsDataGridView.Top);
            BlockSetsResultsDataGridView.ShowFilterMenuItems(e, offset);
        }
    }
}