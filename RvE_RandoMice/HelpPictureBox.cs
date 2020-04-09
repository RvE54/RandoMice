using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace RvE_RandoMice
{
    class HelpPictureBox : PictureBox
    {
        public HelpPictureBox()
        {
            Image = Properties.Resources.QuestionMark;
            Height = 16;
            Width = 16;
            SizeMode = PictureBoxSizeMode.Zoom;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            MessageBox.Show(HelpText.Replace("\r\n\r\n", "DoubleLineBreak").Replace("\r\n", " ").Replace("DoubleLineBreak", "\r\n\r\n"),
                HelpTextCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            base.OnMouseClick(e); //to ensure external event handlers are called
        }

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string HelpText { get; set; } = "";

        public string HelpTextCaption { get; set; } = "Info";
    }
}
