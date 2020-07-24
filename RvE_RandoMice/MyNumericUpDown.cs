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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public class MyNumericUpDown : NumericUpDown
    {
        public MyNumericUpDown()
        {

        }

        /// <summary>
        /// Makes sure that the increment of scrolling equals the Increment property
        /// and prevents users from scrolling in controls that are disabled.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs handledMouseEventArgs = e as HandledMouseEventArgs;
            if (handledMouseEventArgs != null)
                handledMouseEventArgs.Handled = true;

            if (this.Enabled)
            {
                if (e.Delta > 0)
                {
                    if (this.Value + this.Increment < this.Maximum)
                        this.Value += this.Increment;
                    else
                        this.Value = this.Maximum;
                }
                else if (e.Delta < 0) 
                {
                    if (this.Value - this.Increment > this.Minimum)
                        this.Value -= this.Increment;
                    else
                        this.Value = this.Minimum;
                }
            }
        }
    }
}
