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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvE_RandoMice
{
    [Serializable]
    public class ExperimentalUnit
    {
        public ExperimentalUnit(char numberOfExperimentalUnit, string nameOfExperimentalUnit)
        {
            ID = numberOfExperimentalUnit;
            Name = nameOfExperimentalUnit;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExperimentalUnit);
        }

        public bool Equals(ExperimentalUnit experimentalUnit)
        {
            // If Variable is null, return false.
            if (Object.ReferenceEquals(experimentalUnit, null))
                return false;

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != experimentalUnit.GetType())
                return false;

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (Name == experimentalUnit.Name) && (Values.SequenceEqual(experimentalUnit.Values)) && (Marker.Name == experimentalUnit.Marker.Name);
        }

        public static bool operator ==(ExperimentalUnit leftHandSide, ExperimentalUnit rightHandSide)
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

        public static bool operator !=(ExperimentalUnit leftHandSide, ExperimentalUnit rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }

        /// <summary>
        /// Clones an instance of ExperimentalUnit.
        /// </summary>
        /// <returns>A clone of an instance of ExperimentalUnit.</returns>
        public ExperimentalUnit Clone()
        {
            ExperimentalUnit cloneOfExperimentalUnit = new ExperimentalUnit(this.ID, this.Name)
            {
                Values = Values.Select(value => value).ToList(),
                Marker = new Marker(Marker.Name),
                CanBePlacedInASubgroup = CanBePlacedInASubgroup
            };

            return cloneOfExperimentalUnit;
        }

        public char ID { get; private set; } = new char();

        public string Name { get; set; } = "unnamed";

        public List<double> Values { get; set; } = new List<double>();

        public Marker Marker { get; set; } = new Marker(string.Empty);

        public bool CanBePlacedInASubgroup { get; set; } = true;

        public string Category { get; set; } = string.Empty;
    }
}
