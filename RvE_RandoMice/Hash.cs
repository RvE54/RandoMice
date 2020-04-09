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
    static class Hash
    {
        /// <summary>
        /// Sorts the characters of a string.
        /// </summary>
        /// <param name="input">The string to sort.</param>
        /// <returns></returns>
        public static string SortString(string input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            
            return new string(characters);
        }

        /// <summary>
        /// Calculates a stable hash code of a string.
        /// </summary>
        /// <param name="hashableString">The string from which to calculate a stable hash code.</param>
        /// <returns>A stable hash code of a string.</returns>
        public static long GetStableHashCode(string hashableString)
        {
            //This function is a copy of the 64bit GetHashCode() that will
            //remain stable for future .NET versions.
            //Credits to Scott Chamberlain in https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings
            unchecked
            {
                long hash1 = 5381;
                long hash2 = hash1;
                
                for (int i = 0; i < hashableString.Length && hashableString[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ hashableString[i];
                    
                    if (i == hashableString.Length - 1 || hashableString[i + 1] == '\0')
                        break;

                    hash2 = ((hash2 << 5) + hash2) ^ hashableString[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
