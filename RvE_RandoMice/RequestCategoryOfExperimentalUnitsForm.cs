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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public partial class RequestCategoryOfExperimentalUnitsForm : Form
    {
        public RequestCategoryOfExperimentalUnitsForm()
        {
            InitializeComponent();
        }

        private void RequestCategoryOfExperimentalUnits_Load(object sender, EventArgs e)
        {
            int minimumColumnWidth = Global.Settings.RequestCategoryDataGridViewColumnMinimumWidth;
            mainDataGridView.PasteString("Experimental unit\tCategory\r\n" + string.Join("\r\n", Global.FinishedExperiment.AllExperimentalUnits.Select(experimentalUnit => experimentalUnit.Name + "\t" + experimentalUnit.Category)));
            mainDataGridView.Columns[0].MinimumWidth = minimumColumnWidth;
            mainDataGridView.Columns[1].MinimumWidth = minimumColumnWidth;

            mainDataGridView.Columns[0].ReadOnly = true; //allow user to edit the category strings only

            //change appearance of readonly cells
            DataGridViewCellStyle DataGridViewReadOnlyCellStyle = new DataGridViewCellStyle();
            DataGridViewReadOnlyCellStyle.ForeColor = Color.DarkGray;

            foreach (DataGridViewRow row in mainDataGridView.Rows)
                row.Cells[0].Style = DataGridViewReadOnlyCellStyle;

            //get corrections needed to resize the form
            int totalHeightOfDataGridView = mainDataGridView.Rows.GetRowsHeight(DataGridViewElementStates.None) + mainDataGridView.ColumnHeadersHeight;
            int heightCorrection = totalHeightOfDataGridView - mainDataGridView.Height;

            int totalWidthOfDataGridView = mainDataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.None);
            int widthCorrection = totalWidthOfDataGridView - mainDataGridView.Width + 4; //small correction needed

            //resize form height
            if (Height + heightCorrection < Screen.FromControl(this).WorkingArea.Height / 2)
                Height += heightCorrection;
            else
                Height = Screen.FromControl(this).WorkingArea.Height / 2;

            //adjust correction if a vertical scrollbar is visible
            if (mainDataGridView.Controls.OfType<VScrollBar>().First().Visible)
                widthCorrection += SystemInformation.VerticalScrollBarWidth;

            //resize form width
            if (Width + widthCorrection < Screen.FromControl(this).WorkingArea.Width)
                Width += widthCorrection;
            else
                Width = Screen.FromControl(this).WorkingArea.Width;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < Global.FinishedExperiment.AllExperimentalUnits.Count; i++)
                    Global.FinishedExperiment.AllExperimentalUnits[i].Category = mainDataGridView.Rows[i].Cells[1].Value.ToString();

                DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("Something went wrong with the interpretation of the category names. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
