﻿//    RandoMice
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

        public AllowPasting PastingDataFromClipboardIsAllowed { get; set; } = AllowPasting.False;

        public bool IsInputDataGridViewForExperiment { get; set; } = false;

        public string PastedString { get; private set; } = null;

        public bool AllowSorting { get; set; } = true;

        public bool AllowFiltering { get; set; } = false;

        public bool AllowViewDetails { get; set; } = true;

        public event EventHandler<DataPastedEventArgs> DataPasted;

        public event EventHandler<EventArgsWithValue> FilterByMarkersToChange;

        public event EventHandler<EventArgs> RemoveAllFilters;

        public event EventHandler<EventArgs> FilterByCategory;

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

                if (PastingDataFromClipboardIsAllowed != AllowPasting.False)
                {
                    var pasteMenuItem = new ToolStripMenuItem("Paste", Properties.Resources.Paste, MenuItemPasteClipboardContent);
                    contextMenu.Items.Add(pasteMenuItem);
                }

                if (AllowViewDetails)
                {
                    var viewDetailsMenuItem = new ToolStripMenuItem("View details", Properties.Resources.MagnifyingGlass, MenuItemViewDetails);
                    contextMenu.Items.Add(viewDetailsMenuItem);
                }

                if (AllowFiltering) //to filter blocksets based on condition
                {
                    var filterMenuItem = new ToolStripMenuItem("Filter results...", null, MenuItemFilter);
                    var filterByMarkersToChange = new ToolStripMenuItem("by markers to change", null, MenuItemFilterByMarkersToChange);
                    var filterByCategory = new ToolStripMenuItem("by category", null, MenuItemFilterByCategory);
                    var removeAllFilters = new ToolStripMenuItem("remove all filters", null, MenuItemRemoveAllFilters);
                    filterMenuItem.DropDownItems.AddRange(new[] { filterByMarkersToChange, filterByCategory, removeAllFilters });
                    contextMenu.Items.Add(filterMenuItem);
                }

                contextMenu.Show(this, new Point(e.X, e.Y));
            }

            base.OnMouseClick(e); //to ensure external event handlers are called
        }

        public void ShowFilterMenuItems(MouseEventArgs e, (int X, int Y) offset)
        {
            if (AllowFiltering) //to filter blocksets based on condition
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();

                var filterMenuItem = new ToolStripMenuItem("Filter results...", null, MenuItemFilter);
                var filterByMarkersToChange = new ToolStripMenuItem("by markers to change", null, MenuItemFilterByMarkersToChange);
                var filterByCategory = new ToolStripMenuItem("by category", null, MenuItemFilterByCategory);
                var removeAllFilters = new ToolStripMenuItem("remove all filters", null, MenuItemRemoveAllFilters);
                filterMenuItem.DropDownItems.AddRange(new[] { filterByMarkersToChange, filterByCategory, removeAllFilters });
                contextMenu.Items.Add(filterMenuItem);

                contextMenu.Show(this, new Point(e.X + offset.X, e.Y + offset.Y));
            }
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
            
            if (e.KeyCode == Keys.V && e.Control)
            {
                if (PastingDataFromClipboardIsAllowed == AllowPasting.True)
                    PasteString(GetPastableTextAsStringFromClipboard());
                else if (PastingDataFromClipboardIsAllowed == AllowPasting.IntoExistingCellsOnly)
                    PasteStringIntoExistingWritableCells(GetPastableTextAsStringFromClipboard());
            }
                

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
        /// the data that should be pasted to the DataGridView with headers in the first row.</param>
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

            //Save pasted string
            PastedString = stringToPaste;

            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// Pastes a string that is seperated by tabs and newLines, including headers in the first row,
        /// into existing writable (i.e. not readonly) cells into the DataGridView.
        /// </summary>
        /// <param name="stringToPaste">A string that is separated by tabs and newlines, containing
        /// the data that should be pasted to existing cells in the DataGridView.</param>
        public void PasteStringIntoExistingWritableCells(string stringToPaste)
        {
            if (string.IsNullOrEmpty(stringToPaste))
            {
                PastedString = null;
                return;
            }

            Cursor.Current = Cursors.WaitCursor; //give user feedback by changing the cursor

            //get all input rows from the input string
            string[] inputRows = System.Text.RegularExpressions.Regex.Split(stringToPaste.TrimEnd("\r\n".ToCharArray()), "\r\n");

            (int Row, int Cell) lastReachedCell = (0, 0);

            //define the starting cell for pasting the data (i.e. the top left user-selected cell)
            (int Row, int Column) pasteStartCell = (0, 0);

            try
            {
                pasteStartCell = (SelectedCells.Cast<DataGridViewCell>().OrderBy(cell => cell.RowIndex).ToArray()[0].RowIndex, SelectedCells.Cast<DataGridViewCell>().OrderBy(cell => cell.ColumnIndex).ToArray()[0].ColumnIndex);
            }
            catch
            {
                //do nothing, pasteStartCell will keep its default value
            }

            //deselect all cells
            ClearSelection();

            //assume the pasted data contains header if the row of the selected cell is 0
            //unless the number of columns to be pasted equals the number of experimental units
            int pasteHeaders = pasteStartCell.Row == 0 && inputRows.Count() != Rows.Count ? 1 : 0;

            //paste data
            for (int i = pasteStartCell.Row; i < Rows.Count && i - pasteStartCell.Row + pasteHeaders < inputRows.Count(); i++) //we assume that the first row contains column headers
            {
                string[] inputRowCells = inputRows[i - pasteStartCell.Row + pasteHeaders].Split(new char[] { '\t' }); //split input row by tabs

                for (int j = pasteStartCell.Column; j < Columns.Count; j++)
                {
                    if (j < inputRowCells.Length  + pasteStartCell.Column)
                    {
                        if(!Columns[j].ReadOnly)
                            Rows[i].Cells[j].Value = inputRowCells[j - pasteStartCell.Column];

                        lastReachedCell = (i, j);
                    }
                }
            }

            //select all cells that span the pasted data
            for (int i = pasteStartCell.Row; i <= lastReachedCell.Row; i++)
            {
                for (int j = pasteStartCell.Column; j <= lastReachedCell.Cell; j++)
                    Rows[i].Cells[j].Selected = true;
            }

            //auto resize columns
            AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// Pastes the content of the clipboard to the DataGridView.
        /// </summary>
        public void PasteClipboard()
        {
            if (PastingDataFromClipboardIsAllowed == AllowPasting.True)
                PasteString(GetPastableTextAsStringFromClipboard(), warnUserForInvalidDataPoints: true, askUserIfDatesShouldBeConvertedToValues: true);
            else if (PastingDataFromClipboardIsAllowed == AllowPasting.IntoExistingCellsOnly)
                PasteStringIntoExistingWritableCells(GetPastableTextAsStringFromClipboard());
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

        private void MenuItemFilter(object sender, EventArgs e)
        {
            //do nothing
        }

        private void MenuItemFilterByMarkersToChange(object sender, EventArgs e)
        {
            if (Global.FinishedExperiment != null && Global.FinishedExperiment.ExperimentalUnitsHaveMarkers)
            {
                using (RequestInteger requestMaximumMarkersToChange = new RequestInteger(title: "Filter results", mainText: "The number of markers to change must be smaller or equal to:"))
                {
                    var result = requestMaximumMarkersToChange.ShowDialog();

                    if (result == DialogResult.OK)
                        OnFilterByMarkersToChange(new EventArgsWithValue(requestMaximumMarkersToChange.Result));
                }
            }
            else if (!Global.FinishedExperiment.ExperimentalUnitsHaveMarkers)
                MessageBox.Show("The current experimental units do not have markers. Please provide markers, and re-run the program.", "No markers available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuItemFilterByCategory(object sender, EventArgs e)
        {
            using (RequestCategoryOfExperimentalUnitsForm requestCategoryOfExperimentalUnitsForm = new RequestCategoryOfExperimentalUnitsForm())
            {
                var result = requestCategoryOfExperimentalUnitsForm.ShowDialog();

                if (result == DialogResult.OK)
                    OnFilterByCategory(e);
            }
        }

        private void MenuItemRemoveAllFilters(object sender, EventArgs e)
        {
            OnRemoveAllFilters(e);
        }

        protected virtual void OnDataPasted(DataPastedEventArgs e)
        {
            EventHandler<DataPastedEventArgs> handler = DataPasted;
            handler?.Invoke(this, e);
        }

        protected virtual void OnFilterByMarkersToChange(EventArgsWithValue e)
        {
            EventHandler<EventArgsWithValue> handler = FilterByMarkersToChange;
            handler?.Invoke(this, e);
        }

        protected virtual void OnFilterByCategory(EventArgs e)
        {
            EventHandler<EventArgs> handler = FilterByCategory;
            handler?.Invoke(this, e);
        }

        protected virtual void OnRemoveAllFilters(EventArgs e)
        {
            EventHandler<EventArgs> handler = RemoveAllFilters;
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
