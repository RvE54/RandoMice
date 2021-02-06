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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public partial class AboutForm : Form
    {
        public AboutForm(string version, bool displayLicenceAndCreditsButtons = true)
        {
            InitializeComponent();
            VersionLabel.Text = VersionLabel.Text.Replace("{version}", version);

            if (!displayLicenceAndCreditsButtons)
            {
                LicenceButton.Visible = false;
                CreditsButton.Visible = false;
                CloseButton.Visible = false;
                this.Height -= 37;
            }
        }

        private void LicenceButton_Click(object sender, EventArgs e)
        {
            using (LicenceForm newLicenceForm = new LicenceForm())
            {
                var result = newLicenceForm.ShowDialog(); //this makes sure that only one instance of the CreditsForm can be initialized.
            }
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            using (CreditsForm newCreditsForm = new CreditsForm())
            {
                var result = newCreditsForm.ShowDialog(); //this makes sure that only one instance of the CreditsForm can be initialized.
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
