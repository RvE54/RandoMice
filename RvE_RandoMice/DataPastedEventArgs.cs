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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvE_RandoMice
{
    public class DataPastedEventArgs : EventArgs
    {
        public bool WarnUserForInvalidDataPoints { get; private set; } = false;

        public bool AskUserIfDatesShouldBeConvertedToValues { get; private set; } = false;

        public DataPastedEventArgs(bool warnUserForInvalidDataPoints, bool askUserIfDatesShouldBeConvertedToValues)
        {
            WarnUserForInvalidDataPoints = warnUserForInvalidDataPoints;
            AskUserIfDatesShouldBeConvertedToValues = askUserIfDatesShouldBeConvertedToValues;
        }
    }
}
