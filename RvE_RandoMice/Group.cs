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
    [Serializable]
    public class Group
    {
        public Group(char groupID, string groupName)
        {
            ID = groupID;
            Name = groupName;
        }

        public Group(char groupID)
        {
            ID = groupID;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Group);
        }

        public bool Equals(Group group)
        {
            // If Variable is null, return false.
            if (Object.ReferenceEquals(group, null))
                return false;

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != group.GetType())
                return false;

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (ID == group.ID);
        }

        public static bool operator ==(Group leftHandSide, Group rightHandSide)
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

        public static bool operator !=(Group leftHandSide, Group rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }

        /// <summary>
        /// Clones an instance of Group.
        /// </summary>
        /// <returns>A clone of an instance of Group.</returns>
        public Group Clone()
        {
            return new Group((char)Global.GroupID, (string)Name.Clone());
        }

        /// <summary>
        /// Clones an instance of Group, but preserves the ID.
        /// </summary>
        /// <returns>A clone of an instance of Group.</returns>
        /// <example>Preserving the ID is for example needed when
        /// randomizing blocks to groups.</example>
        public Group CloneButPreserveID()
        {
            return new Group(ID, (string)Name.Clone());
        }

        public char ID { get; private set; } = new char();

        public string Name { get; set; } = string.Empty;

        public bool IsValid { get { return this.Name != string.Empty; } }
    }
}
