//    RandoMice
//    Copyright(C) 2019-2021 R. van Eenige, Leiden University Medical Center
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public class MyDataGridView : DataGridView
    {
        public MyDataGridView()
        {
            //set defaults
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            RowHeadersVisible = false;
            SortCompare += SortNaturally;
        }

        public bool MenuItemsEnabled { get; set; } = true;

        public bool PastingDataFromClipboardIsAllowed { get; set; } = false;

        public bool IsInputDataGridViewForExperiment { get; set; } = false;

        public string PastedString { get; private set; } = null;

        public bool AllowSorting { get; set; } = true;

        public event EventHandler<DataPastedEventArgs> DataPasted;

        public int? CurrentlySelectedValue { get; set; } = null;

        private void SortNaturally(object sender, DataGridViewSortCompareEventArgs e)
        {
            e.SortResult = CompareNaturally(e.CellValue1.ToString(), e.CellValue2.ToString());
            e.Handled = true;
        }

        public int CompareNaturally(string leftHandSide, string rightHandSide)
        {
            if (leftHandSide == rightHandSide)
                return 0;

            //try to parse strings to double
            double x = Global.Settings.MissingValue;
            double y = Global.Settings.MissingValue;
            if (double.TryParse(leftHandSide, NumberStyles.Any, CultureInfo.CurrentCulture, out double leftDouble))
                x = leftDouble;
            else if (double.TryParse(leftHandSide, NumberStyles.Any, CultureInfo.InvariantCulture, out leftDouble))
                x = leftDouble;

            if (double.TryParse(rightHandSide, NumberStyles.Any, CultureInfo.CurrentCulture, out double rightDouble))
                y = rightDouble;
            else if (double.TryParse(rightHandSide, NumberStyles.Any, CultureInfo.InvariantCulture, out rightDouble))
                y = rightDouble;

            //if both strings were successfully parsed to doubles, sort accordingly
            if (x != Global.Settings.MissingValue && y != Global.Settings.MissingValue)
            {
                if (x > y)
                    return 1;
                else if (x != y)
                    return -1;
                else
                    return 0;
            }
            else
                //compare strings as regular strings
                return string.Compare(leftHandSide, rightHandSide);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (MenuItemsEnabled && e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();

                var copyMenuItem = new ToolStripMenuItem("Copy", Properties.Resources.Copy, MenuItemCopy);
                var copyCellMenuItem = new ToolStripMenuItem("Copy selected cell(s)");
                copyCellMenuItem.Click += MenuItemCopyCell;
                contextMenu.Items.Add(copyMenuItem);
                contextMenu.Items.Add(copyCellMenuItem);

                if (PastingDataFromClipboardIsAllowed)
                {
                    var pasteMenuItem = new ToolStripMenuItem("Paste", Properties.Resources.Paste, MenuItemPasteClipboardContent);
                    contextMenu.Items.Add(pasteMenuItem);
                }

                var viewDetailsMenuItem = new ToolStripMenuItem("View details", Properties.Resources.MagnifyingGlass, MenuItemViewDetails);
                contextMenu.Items.Add(viewDetailsMenuItem);

                contextMenu.Show(this, new Point(e.X, e.Y));
            }

            base.OnMouseClick(e); //to ensure external event handlers are called
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                if (SelectedCells.Count == 1)
                    CopyAllContentToClipboard();
                else
                    CopySelectedCellsToClipboard();
            }
            
            if (PastingDataFromClipboardIsAllowed && e.KeyCode == Keys.V && e.Control)
                PasteString(GetPastableTextAsStringFromClipboard());

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Copies the content of a DataGridView to clipboard.
        /// </summary>
        private void CopyAllContentToClipboard()
        {
            try
            {
                //create a copy of the current DataGridView to avoid having to change the current selection
                MyDataGridView copy = new MyDataGridView();
                copy.PasteString(this.ToString());
                copy.SelectAll();
                Clipboard.SetDataObject(copy.GetClipboardContent());
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                MessageBox.Show("The Clipboard could not be accessed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NullReferenceException)
            {
                //likely, nothing is selected.
                //no action needed
            }
            catch
            {
                //no action needed
            }
        }

        /// <summary>
        /// Copies the content of a selected cell in a DataGridView to clipboard.
        /// </summary>
        private void CopySelectedCellsToClipboard()
        {
            try
            {
                // Add the selected cell to the clipboard.
                Clipboard.SetDataObject(GetClipboardContent());
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                MessageBox.Show("The Clipboard could not be accessed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Clears all rows and columns, making sure that PastedString is nulled.
        /// </summary>
        public void ClearRowsAndColumns()
        {
            PasteString(null);
        }

        /// <summary>
        /// Pastes a string that is seperated by tabs and newLines, into the DataGridView.
        /// </summary>
        /// <param name="stringToPaste">A string that is separated by tabs and newlines, containing
        /// the data that should be pasted to the DataGridView.</param>
        /// <param name="warnUserForInvalidDataPoints">A bool to indicate whether or not the user should
        /// be warned if the pasted string contains invalid data points. Default is false.</param>
        /// <param name="askUserIfDatesShouldBeConvertedToValues">A bool to indicate whether or not the user should
        /// be asked if strings containing a valid DateTime should be converted into values. Default is false.</param>
        public void PasteString(string stringToPaste, bool warnUserForInvalidDataPoints = false, bool askUserIfDatesShouldBeConvertedToValues = false)
        {
            Rows.Clear();
            Columns.Clear();

            if (string.IsNullOrEmpty(stringToPaste))
            {
                PastedString = null;
                return;
            }

            Cursor.Current = Cursors.WaitCursor; //give user feedback by changing the cursor
            bool columnsAdded = false;

            if (IsInputDataGridViewForExperiment)
                Global.CurrentExperiment.InputData = stringToPaste;

            //get all input rows from the input string
            string[] inputRows = System.Text.RegularExpressions.Regex.Split(stringToPaste.TrimEnd("\r\n".ToCharArray()), "\r\n");

            foreach (string inputRow in inputRows)
            {
                string[] inputRowCells = inputRow.Split(new char[] { '\t' }); //split input row by tabs

                if (!columnsAdded)
                {
                    for (int i = 0; i < inputRowCells.Length; i++) //we assume that the first row contains column headers
                    {
                        Columns.Add("col" + i, inputRowCells[i]);

                        if (!AllowSorting)
                            Columns[ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    columnsAdded = true;
                }
                else
                    Rows.Add(inputRowCells); //paste cells into DataGridView as a new row
            }

            //auto resize columns
            AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //Raise OnDataPasted event
            OnDataPasted(new DataPastedEventArgs(warnUserForInvalidDataPoints, askUserIfDatesShouldBeConvertedToValues));

            //Save pasted string in memory to allow 
            PastedString = stringToPaste;

            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// Pastes the content of the clipboard to the DataGridView.
        /// </summary>
        public void PasteClipboard()
        {
            PasteString(GetPastableTextAsStringFromClipboard(), warnUserForInvalidDataPoints: true, askUserIfDatesShouldBeConvertedToValues: true);
        }

        private string GetPastableTextAsStringFromClipboard()
        {
            DataObject dataObject = (DataObject)Clipboard.GetDataObject();

            if (dataObject.GetDataPresent(DataFormats.Text))
                return dataObject.GetData(DataFormats.Text).ToString(); //get input from clipboard
            else
                return null;
        }

        private void MenuItemCopy(object sender, EventArgs e)
        {
            CopyAllContentToClipboard();
        }

        private void MenuItemCopyCell(object sender, EventArgs e)
        {
            CopySelectedCellsToClipboard();
        }

        private void MenuItemPasteClipboardContent(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        private void MenuItemViewDetails(object sender, EventArgs e)
        {
            using (DisplayDataGridViewDetailsForm displayDataGridViewDetails = new DisplayDataGridViewDetailsForm(this))
                displayDataGridViewDetails.ShowDialog();
        }

        protected virtual void OnDataPasted(DataPastedEventArgs e)
        {
            EventHandler<DataPastedEventArgs> handler = DataPasted;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Clones some settings from a given source DataGridView.
        /// </summary>
        /// <param name="sourceDataGridView">The source DataGridView from which settings need to be copied.</param>
        public void CloneSettingsFrom(MyDataGridView sourceDataGridView)
        {
            //copies some settings from the sourceDataGridView to the current target DataGridView
            AllowSorting = sourceDataGridView.AllowSorting;
            ReadOnly = sourceDataGridView.ReadOnly;
            ClipboardCopyMode = sourceDataGridView.ClipboardCopyMode;
            PasteString(sourceDataGridView.ToString());

            for (int i = 0; i < sourceDataGridView.ColumnCount; i++)
            {
                for (int j = 0; j < sourceDataGridView.RowCount; j++)
                    Rows[j].Cells[i].Style.BackColor = sourceDataGridView.Rows[j].Cells[i].Style.BackColor;  
            }
        }

        public override string ToString()
        {

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < ColumnCount; i++)
            {
                stringBuilder.Append(Columns[i].HeaderText);

                if (i < ColumnCount - 1)
                    stringBuilder.Append("\t");
            }

            stringBuilder.Append("\r\n");

            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColumnCount; i++)
                {
                    stringBuilder.Append(Rows[j].Cells[i].Value.ToString());

                    if (i < ColumnCount - 1)
                        stringBuilder.Append("\t");
                }

                stringBuilder.Append("\r\n");
            }

            return stringBuilder.ToString();
        }
    }
}
