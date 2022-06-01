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
    public class BlockOfExperimentalUnits
    {
        /// <summary>
        /// Sorts a list of ExperimentalUnits by their names and name lengths.
        /// </summary>
        public void SortExperimentalUnitsByName()
        {
            //this function allows for easy sorting of ExperimentalUnits  by name and length, therefore yielding sequences such as 1,2,3,4,5 instead of 1,10,11,12 etc.
            ExperimentalUnits = ExperimentalUnits.OrderBy(s => s.Name).OrderBy(s => s.Name.Length).ToList();
        }

        /// <summary>
        /// Clears all descriptives in the current BlockOfExperimentalUnits.
        /// </summary>
        private void ClearDescriptives()
        {
            for (int i = 0; i < this.Descriptives.Count; i++)
            {
                Descriptives[i].Clear();
            }
        }

        /// <summary>
        /// Calculates the descriptives of the ExperimentalUnits in the current BlockOfExperimentalUnits.
        /// </summary>
        /// <param name="Variables">The list of Variables of the current BlockOfExperimentalUnits's parent Run</param>
        /// <param name="calculateAllDescriptives">Optional bool which must be true if it is desired to calculate the min,
        /// median and max besides the mean and standard deviation of the current BlockOfExperimentalUnits. Default is false.</param>
        public void CalculateDescriptives(List<Variable> Variables, bool calculateAllDescriptives = false)
        {            
            ClearDescriptives();
            
            int VariableCount = Variables.Count();

            for (int i = 0; i < VariableCount; i++)
            {
                Descriptives.Add(new Descriptives(Variables[i].DecimalPlaces)); //add empty statistics variable to the collection
                                                                                 //this makes sure that each Statistics in the list corresponds with a Variable by index

                bool minMaxAndMedianAreValid = true;
                List<double> validVariableValues = new List<double>();
                double sumOfSquares = 0;

                //find all valid values of the current Variable and calculate their mean
                validVariableValues = ExperimentalUnits.Where(ExperimentalUnit => ExperimentalUnit.Values[i] != Global.Settings.MissingValue).Select(o => o.Values[i]).ToList();

                if (validVariableValues.Count != 0)
                    Descriptives[i].Mean = validVariableValues.Average();

                if (calculateAllDescriptives)
                {
                    //calculateAllDescriptives must be passed as true to calculate min, median, max, CV and SD
                    if(validVariableValues.Count > 0)
                    {
                        Descriptives[i].Min = validVariableValues.Min(); //do not round any values until the very end for displaying purposes, to prevent potential rounding errors
                        Descriptives[i].Median = Calc.Median(validVariableValues);
                        Descriptives[i].Max = validVariableValues.Max();
                    }
                    else
                    {
                        //the Variable contains no valid data point at all
                        minMaxAndMedianAreValid = false;//set value to false, which allows for an extra check below
                        Descriptives[i].Min = Global.Settings.MissingValue;
                        Descriptives[i].Median = Global.Settings.MissingValue;
                        Descriptives[i].Max = Global.Settings.MissingValue;
                        Descriptives[i].CV = Global.Settings.MissingValue;
                        Descriptives[i].SD = Global.Settings.MissingValue;
                    }
                }

                if (minMaxAndMedianAreValid)
                {
                    //calculate sumOfSquares, follewed by SD and CV
                    foreach (double VariableValue in validVariableValues)
                        sumOfSquares += Math.Pow((VariableValue - Descriptives[i].Mean), 2);

                    Descriptives[i].SD = Math.Pow(sumOfSquares / (validVariableValues.Count - 1), 0.5);
                    Descriptives[i].CV = (Descriptives[i].SD / Descriptives[i].Mean);
                }
            }
        }

        /// <summary>
        /// Iterates through each ExperimentalUnit in the current block and finds the names
        /// and frequencies of markers.
        /// </summary>
        private void UpdateMarkers()
        {
            Markers.Clear();

            foreach (ExperimentalUnit ExperimentalUnit in ExperimentalUnits)
            {
                ExperimentalUnit.CanBePlacedInASubgroup = true; //reset value

                //get occurence of each marker
                if (Markers.Where(marker => marker.Name == ExperimentalUnit.Marker.Name).ToList().Count > 0)
                    Markers.Where(marker => marker.Name == ExperimentalUnit.Marker.Name).ToList().First().ExperimentalUnitsContainingCurrentMarkerNotYetDividedIntoSubgroups++;
                else
                    Markers.Add(new Marker(ExperimentalUnit.Marker.Name));
            }

            Markers = Markers.OrderByDescending(marker => marker.ExperimentalUnitsContainingCurrentMarkerNotYetDividedIntoSubgroups).ToList(); //sort the markers based on occurence
        }

        /// <summary>
        /// Divides all ExperimentalUnits into subgroups, either randomly or based on markers.
        /// </summary>
        /// <param name="subgroupSizes">A list of shorts containing the size of each subgroup.</param>
        /// <param name="ExperimentalUnitsHaveMarker">A bool which is true if ExperimentalUnits have markers.</param>
        /// <param name="random">An instance of the Random class.</param>
        public void DivideExperimentalUnitsIntoSubgroups(List<short> subgroupSizes, bool ExperimentalUnitsHaveMarker, Random random)
        {
            //subgroup sizes are defined by List<short> subgroupSizes
            //for example, two blocks of 14 will be passed as 14, 14

            Subgroups.Clear();

            if (ExperimentalUnitsHaveMarker)
                UpdateMarkers(); //sorts markers based on occurance

            List<ExperimentalUnit> copyOfExperimentalUnits = new List<ExperimentalUnit>(ExperimentalUnits); //create a copy of all ExperimentalUnits in the current block

            for (int i = 0; i < subgroupSizes.Count(); i++) //for the number of subgroups that should be made from the current block
            {
                Subgroups.Add(new BlockOfExperimentalUnits());
                int skipMarker = 0;

                for (int j = 0; j < subgroupSizes[i]; j++)
                {
                    if (ExperimentalUnitsHaveMarker)
                    {
                        //pick ExperimentalUnits based on the occurence of the markers
                        bool validMarkerFound = false;

                        while (!validMarkerFound && skipMarker + j < Markers.Count())
                        {
                            //markers were previously sorted based on occurance.
                            //Now, ExperimentalUnits will be picked by marker, starting with the most frequently occuring marker.
                            //This assures that the ExperimentalUnits will be sorted most optimally, lowering the need to change markers afterwards.
                            if (Markers[j + skipMarker].ExperimentalUnitsContainingCurrentMarkerNotYetDividedIntoSubgroups != 0)
                                validMarkerFound = true;
                            else
                                skipMarker++;
                        }

                        bool ExperimentalUnitFound = false;

                        for (int k = 0; k < copyOfExperimentalUnits.Count && !ExperimentalUnitFound && j + skipMarker < Markers.Count(); k++)
                        {
                            //find ExperimentalUnit that has a corresponding marker
                            //note: this step is not random!
                            if (copyOfExperimentalUnits[k].Marker.Name == Markers[j + skipMarker].Name)
                            {
                                Markers[j + skipMarker].ExperimentalUnitsContainingCurrentMarkerNotYetDividedIntoSubgroups--;
                                Subgroups[i].ExperimentalUnits.Add(copyOfExperimentalUnits[k]);
                                copyOfExperimentalUnits.RemoveAt(k); //remove ExperimentalUnit from copyList
                                ExperimentalUnitFound = true;
                            }
                        }
                    }
                    else
                    {
                        //randomly pick ExperimentalUnits 
                        int rndIndex = random.Next(0, copyOfExperimentalUnits.Count); //create a random index to select a random ExperimentalUnit
                        Subgroups[i].ExperimentalUnits.Add(copyOfExperimentalUnits[rndIndex]);
                        copyOfExperimentalUnits.RemoveAt(rndIndex);
                    }
                }
            }

            foreach (ExperimentalUnit copyOfExperimentalUnit in copyOfExperimentalUnits) //for those ExperimentalUnits remaining (i.e. that cannot be placed in a subgroup)
                copyOfExperimentalUnit.CanBePlacedInASubgroup = false;
        }

        /// <summary>
        /// A list of descriptives (e.g. mean, standard deviation) of each Variable.
        /// The order of Descriptives should correspond to the order of Variables in Experiment.Variables.
        /// </summary>
        public List<Descriptives> Descriptives { get; set; } = new List<Descriptives>();

        public List<ExperimentalUnit> ExperimentalUnits { get; set; } = new List<ExperimentalUnit>();

        public List<BlockOfExperimentalUnits> Subgroups { get; set; } = new List<BlockOfExperimentalUnits>();

        public List<Marker> Markers { get; set; } = new List<Marker>();
    }
}
