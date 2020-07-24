//    RandoMice
//    Copyright(C) 2019-2020 R. van Eenige, Leiden University Medical Center
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
    public partial class DisplayDataGridViewDetailsForm : Form
    {
        private MyDataGridView SourceDataGridView { get; set; } = null;

        public DisplayDataGridViewDetailsForm(MyDataGridView sourceDataGridView)
        {
            InitializeComponent();
            SourceDataGridView = sourceDataGridView;
        }

        private void DisplayDataGridView_Load(object sender, EventArgs e)
        {
            mainDataGridView.CloneSettingsFrom(SourceDataGridView);

            //get corrections needed to resize the form
            int totalHeightOfDataGridView = mainDataGridView.Rows.GetRowsHeight(DataGridViewElementStates.None) + mainDataGridView.ColumnHeadersHeight;
            int heightCorrection = totalHeightOfDataGridView - mainDataGridView.Height;

            int totalWidthOfDataGridView = mainDataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.None);
            int widthCorrection = totalWidthOfDataGridView - mainDataGridView.Width + 4; //small correction needed

            //resize form height
            if (Height + heightCorrection < Screen.FromControl(this).WorkingArea.Height)
                Height += heightCorrection;
            else
                Height = Screen.FromControl(this).WorkingArea.Height;

            //adjust correction if a vertical scrollbar is visible
            if (mainDataGridView.Controls.OfType<VScrollBar>().First().Visible)
                widthCorrection += SystemInformation.VerticalScrollBarWidth;

            //resize form width
            if (Width + widthCorrection < Screen.FromControl(this).WorkingArea.Width)
                Width += widthCorrection;
            else
                Width = Screen.FromControl(this).WorkingArea.Width;

            CenterToScreen();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
