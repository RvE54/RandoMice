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
    public class Variable
    {
        public Variable(string name, byte decimals, short inputDatagridViewColumnNumber)
        {
            this.Name = name;
            this.DecimalPlaces = Math.Min(decimals, Global.Settings.VariableDecimalPlaces.MaxValue);
            this.InputDataGridViewColumnNumber = inputDatagridViewColumnNumber;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Variable);
        }

        public bool Equals(Variable Variable)
        {
            // If Variable is null, return false.
            if (Object.ReferenceEquals(Variable, null))
                return false;

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != Variable.GetType())
                return false;

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (Name == Variable.Name) && (InputDataGridViewColumnNumber == Variable.InputDataGridViewColumnNumber) && (DecimalPlaces == Variable.DecimalPlaces) && (Weight == Variable.Weight);
        }

        public static bool operator ==(Variable leftHandSide, Variable rightHandSide)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(leftHandSide, null))
            {
                if (Object.ReferenceEquals(rightHandSide, null))
                    return true; // null == null = true.

                // Only the left side is null.
                return false;
            }

            // Equals handles case of null on right side.
            return leftHandSide.Equals(rightHandSide);
        }

        public static bool operator !=(Variable leftHandSide, Variable rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }

        /// <summary>
        /// Clones an instance of Variable.
        /// </summary>
        /// <returns>A clone of an instance of Variable.</returns>
        public Variable Clone()
        {
            Variable cloneOfVariable = new Variable((string)Name.Clone(), DecimalPlaces, InputDataGridViewColumnNumber)
            {
                Weight = Weight
            };

            return cloneOfVariable;
        }

        public string Name { get; set; } = "unnamed";

        /// <summary>
        /// The column number of InputDataGridView which contains values of the current Variable.
        /// </summary>
        public short InputDataGridViewColumnNumber { get; set; } = 0;

        public byte DecimalPlaces { get; set; } = 1;

        public double Weight { get; set; } = 1;
    }
}
