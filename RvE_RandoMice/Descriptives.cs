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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvE_RandoMice
{
    [Serializable]
    public class Descriptives
    {
        private int decimals = 2;
        public Descriptives(int numberOfDecimalsForRounding)
        {
            Decimals = numberOfDecimalsForRounding;
        }

        public void Clear()
        {
            Mean = Global.Settings.MissingValue;
            SD = Global.Settings.MissingValue;
            Min = Global.Settings.MissingValue;
            Median = Global.Settings.MissingValue;
            Max = Global.Settings.MissingValue;
            CV = Global.Settings.MissingValue;
        }

        public double Mean { get; set; } = Global.Settings.MissingValue;
        public double RoundedMean { get => Math.Round(Mean, Decimals, MidpointRounding.AwayFromZero); }
        public double SD { get; set; } = Global.Settings.MissingValue;
        public double RoundedSD { get => Math.Round(SD, Decimals, MidpointRounding.AwayFromZero); }
        public double Min { get; set; } = Global.Settings.MissingValue;
        public double RoundedMin { get => Math.Round(Min, Decimals, MidpointRounding.AwayFromZero); }
        public double Median { get; set; } = Global.Settings.MissingValue;
        public double RoundedMedian { get => Math.Round(Median, Decimals, MidpointRounding.AwayFromZero); }
        public double Max { get; set; } = Global.Settings.MissingValue;
        public double RoundedMax { get => Math.Round(Max, Decimals, MidpointRounding.AwayFromZero); }
        public double CV { get; set; } = Global.Settings.MissingValue;
        public int Decimals { get => decimals; private set => decimals = value; }

        public double? this[string attributeName]
        {
            //this allows for calling a value by its name
            get { return this.GetType().GetProperty(attributeName).GetValue(this, null) as double?; }
            set { this.GetType().GetProperty(attributeName).SetValue(this, value, null); }
        }
    }
}
