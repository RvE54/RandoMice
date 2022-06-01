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
    public partial class RequestInteger : Form
    {
        public RequestInteger(string title, string mainText, int min = 0, int max = 200000)
        {
            InitializeComponent();

            this.Text = title;
            MainText.Text = mainText;

            if (max > min)
            {
                ResultNumericUpDown.Minimum = min;
                ResultNumericUpDown.Maximum = max;
            }

            this.Width = MainText.Left * 2 + MainText.Width + 16;
            this.Height += MainText.Height - 13; //13 is default height of a label containing one line of text

            //set focus to NumericUpDown, and select all text
            ResultNumericUpDown.Focus();
            ResultNumericUpDown.Select(0, ResultNumericUpDown.Text.Length);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ResultNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Result = (int)ResultNumericUpDown.Value;
        }

        private void ResultNumericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKButton_Click(sender, e);

            base.OnKeyDown(e);
        }

        public int Result { get; private set; } = 0;

    }
}
