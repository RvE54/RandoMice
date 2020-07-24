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
    public partial class CreditsForm : Form
    {
        public CreditsForm()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LicenceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to open the link.", "Link", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void VisitLink()
        {
            LicenceLinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.doi.org/10.1371/journal.pone.0237096");
        }
    }
}
