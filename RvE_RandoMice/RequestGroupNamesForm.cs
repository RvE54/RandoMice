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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public partial class RequestGroupNamesForm : Form
    {
        public RequestGroupNamesForm(int blockCount, List<Group> groups)
        {
            InitializeComponent();

            if (groups.Count == blockCount && Groups.Where(group => group.IsValid == false).Count() == 0) //groupNames should never be able to contain empty strings.
                Groups = groups;
            else
                for (int i = 0; i < blockCount; i++)
                    Groups.Add(new Group((char)Global.GroupID));

            OriginalGroupNames = new List<string>(Groups.Select(group => group.Name).ToArray()); //save original group names so names can be reset upon Cancel button click.
        }

        private void RequestGroupNamesForm_Load(object sender, EventArgs e)
        {
            CreateGroupNameControls();

            //shift GroupNamesLabel according to values in Global.Settings.
            GroupNamesLabel.Left = Global.Settings.DefaultGroupNameControl.Left + Global.Settings.DefaultGroupNameLabelWidth;
            
            //get corrections needed to resize the form
            int totalHeightOfControls = (Groups.Count + 2) * Global.Settings.DefaultGroupNameControl.Height;
            int heightCorrection = totalHeightOfControls - GroupNameControlsPanel.Height;

            int totalWidthOfControls = Global.Settings.DefaultGroupNameControl.Left + Global.Settings.DefaultGroupNameLabelWidth + Global.Settings.DefaultGroupNameTextBoxWidth;
            int widthCorrection = totalWidthOfControls - GroupNameControlsPanel.Width;

            //resize form height
            if (Height + heightCorrection < Global.Settings.MaxGroupNamesPanelHeight)
                Height += heightCorrection;
            else
                Height = Global.Settings.MaxGroupNamesPanelHeight;

            //adjust correction if a vertical scrollbar is visible
            if (GroupNameControlsPanel.VerticalScroll.Visible)
            {
                //GroupNameControlsPanel.Width -= SystemInformation.VerticalScrollBarWidth;
                widthCorrection += SystemInformation.VerticalScrollBarWidth;
            }

            //resize form width
            if (Width + widthCorrection < Screen.FromControl(this).WorkingArea.Width)
                Width += widthCorrection;
            else
                Width = Screen.FromControl(this).WorkingArea.Width;

            CenterToScreen();
        }

        /// <summary>
        /// Creates labels and textboxes thereby allowing the user to define group names.
        /// </summary>
        public void CreateGroupNameControls()
        {
            int verticalScrollCorrection = GroupNameControlsPanel.AutoScrollPosition.Y;
            int topOfNewControl = Global.Settings.DefaultGroupNameControl.Top + verticalScrollCorrection;

            for (int i = 0; i < Groups.Count; i++)
            {
                string groupName = string.Empty;

                if (Groups.Where(group => group.IsValid == false).Count() == 0)
                    groupName = Groups[i].Name; //user wants to edit existing group names

                Label newGroupNameLabel = new Label
                {
                    Name = Global.Settings.GroupNameLabelNameBasis + i.ToString(),
                    Text = "Group " + (i + 1).ToString(),
                    Left = Global.Settings.DefaultGroupNameControl.Left,
                    Top = topOfNewControl,
                    Width = Global.Settings.DefaultGroupNameLabelWidth
                };

                TextBox newGroupNameTextBox = new TextBox
                {
                    Name = Global.Settings.GroupNameTextBoxNameBasis + i.ToString(),
                    Text = groupName,
                    Left = Global.Settings.DefaultGroupNameControl.Left + newGroupNameLabel.Width,
                    Top = topOfNewControl,
                    Width = Global.Settings.DefaultGroupNameTextBoxWidth,
                    TabStop = true,
                    TabIndex = 0
                };

                //set TextChanged events
                newGroupNameTextBox.TextChanged += NewGroupNameTextBox_TextChanged;
                newGroupNameTextBox.KeyDown += NewGroupNameTextBox_KeyDown;

                //add new controls to the group names panel
                GroupNameControlsPanel.Controls.AddRange(new Control[] { newGroupNameLabel, newGroupNameTextBox });

                topOfNewControl += Global.Settings.DefaultGroupNameControl.Height;
            }

            //resize width and location of controls if a vertical scroll bar is visible.
            if (GroupNameControlsPanel.VerticalScroll.Visible && !ControlWidthIsCorrectedForVerticalScrollBar)
                this.Width += SystemInformation.VerticalScrollBarWidth;
        }

        private void NewGroupNameTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox senderTextBox = sender as TextBox;
                int GroupNumberIndex = int.Parse(senderTextBox.Name.Replace(Global.Settings.GroupNameTextBoxNameBasis, string.Empty)); //find group name index from the TextBox's name
                Groups[GroupNumberIndex].Name = senderTextBox.Text;
            }
            catch
            {
                //do nothing
            }
        }

        private void NewGroupNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool lastTextBoxHasFocus = GroupNameControlsPanel.Controls[Global.Settings.GroupNameTextBoxNameBasis + (Groups.Count - 1).ToString()].Focused;

            if (e.KeyCode == Keys.Enter)
                if(lastTextBoxHasFocus)
                    CheckIfGroupNamesAreValidAndCloseForm(); //allow users to close the form by pressing the enter key
                else
                    ProcessTabKey(forward: true);
            else if(e.KeyCode == Keys.Up) //allow users to intuitively navigate through the controls by keys up and down
                ProcessTabKey(forward: false);
            else if(e.KeyCode == Keys.Down)
                ProcessTabKey(forward: true);
        }

        /// <summary>
        /// Closes form if group names are valid or warns user.
        /// </summary>
        private void CheckIfGroupNamesAreValidAndCloseForm()
        {
            if (Groups.Where(group => group.IsValid == false).Count() == 0) //if all group names contain valid strings
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("Some group names have not yet been defined.", "Undefined group names", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Groups.Count; i++)
                Groups[i].Name = OriginalGroupNames[i]; //reset names to original state

            this.DialogResult = DialogResult.Cancel;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            CheckIfGroupNamesAreValidAndCloseForm();
        }

        public List<Group> Groups { get; private set; } = new List<Group>();

        private List<string> OriginalGroupNames { get; set; } = new List<string>();

        private bool ControlWidthIsCorrectedForVerticalScrollBar { get; set; } = false;
    }
}
