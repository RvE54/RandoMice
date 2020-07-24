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

namespace RvE_RandoMice
{
    static class Calc
    {
        /// <summary>
        /// Finds the median of a list of doubles.
        /// </summary>
        /// <param name="list">The list of doubles of which the median is desired.</param>
        /// <returns>A double containing the median of a given list of doubles.</returns>
        public static double Median(List<double> list)
        {
            List<double> sortedList = new List<double>(list);
            sortedList.Sort();

            double median = 0; //default is zero

            if (sortedList.Count % 2 != 0)
            {
                //list.count = odd
                median = sortedList[sortedList.Count / 2];
            }
            else
            {
                //list.count = even
                double topIndex = (sortedList.Count / 2) - 1; //minus 1 because we are working with indexes later
                int top = (int)Math.Ceiling(topIndex + 0.5);

                double bottomIndex = sortedList.Count / 2 - 1;
                int bottom = (int)Math.Floor(bottomIndex + 0.5);

                median = (sortedList[top] + sortedList[bottom]) / 2;
            }

            return median;
        }

        /// <summary>
        /// Calculates the factorial of a long.
        /// </summary>
        /// <param name="Number">The long of which the factorial is desired.</param>
        /// <returns>A Long containing the factorial of a given long.</returns>
        public static double Factorial(double Number)
        {
            //although slightly counter-intuitive, the factorial must be calculated using doubles
            //and not, for example, long values. Using long values will return wrong results.
            double result = Number;

            for (double i = Number - 1; i >= 1; i--)
                result *= i;

            return result;
        }

        /// <summary>
        /// Determines the number of decimal places of a given double.
        /// </summary>
        /// <param name="inputValue">A double of which to determine the number of decimal places.</param>
        /// <returns>The number of decimal places of the given double.</returns>
        public static int GetDecimalPlaces(double inputValue)
        {
            int decimalPlaces = 0; //default value
            string[] splittedInputValue = inputValue.ToString().Split(new[] { Global.CultureNumberFormatInfo.NumberDecimalSeparator }, StringSplitOptions.None);

            if (splittedInputValue.Length > 1)
                decimalPlaces = splittedInputValue[1].Length;

            return decimalPlaces;
        }
    }
}
