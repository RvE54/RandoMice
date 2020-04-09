//    RandoMice
//    Copyright(C) 2019 R. van Eenige, Leiden University Medical Center
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
        private double mean = Global.Settings.MissingValue;
        private double sd = Global.Settings.MissingValue;
        private double min = Global.Settings.MissingValue;
        private double median = Global.Settings.MissingValue;
        private double max = Global.Settings.MissingValue;
        private double cv = Global.Settings.MissingValue;

        public Descriptives(int numberOfDecimalsForRounding)
        {
            this.Decimals = numberOfDecimalsForRounding;
        }
        public void Clear()
        {
            this.Mean = Global.Settings.MissingValue;
            this.SD = Global.Settings.MissingValue;
            this.Min = Global.Settings.MissingValue;
            this.Median = Global.Settings.MissingValue;
            this.Max = Global.Settings.MissingValue;
            this.CV = Global.Settings.MissingValue;
        }
        public double Mean { get => mean; set => mean = value; }
        public double RoundedMean { get => Math.Round(Mean, Decimals, MidpointRounding.AwayFromZero); }
        public double SD { get => sd; set => sd = value; }
        public double RoundedSD { get => Math.Round(SD, Decimals, MidpointRounding.AwayFromZero); }
        public double Min { get => min; set => min = value; }
        public double RoundedMin { get => Math.Round(Min, Decimals, MidpointRounding.AwayFromZero); }
        public double Median { get => median; set => median = value; }
        public double RoundedMedian { get => Math.Round(Median, Decimals, MidpointRounding.AwayFromZero); }
        public double Max { get => max; set => max = value; }
        public double RoundedMax { get => Math.Round(Max, Decimals, MidpointRounding.AwayFromZero); }
        public double CV { get => cv; set => cv = value; }
        public int Decimals { get => decimals; private set => decimals = value; }

        public double? this[string attributeName]
        {
            //this allows for calling a value by its name
            get { return this.GetType().GetProperty(attributeName).GetValue(this, null) as double?; }
            set { this.GetType().GetProperty(attributeName).SetValue(this, value, null); }
        }
    }
}
