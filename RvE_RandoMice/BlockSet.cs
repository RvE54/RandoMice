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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    [Serializable]
    public class BlockSet
    {
        /// <summary>
        /// Requests the list of Variables in the current BlockSet's parent Run.
        /// A Run contains a list of BlockSets and a list of Variables.
        /// Each BlockSet must be able to request the list of Variables from its parent.
        /// </summary>
        /// <returns>A list of Variables in the current BlockSet's parent Run.</returns>
        protected List<Variable> RequestVariables()
        {
            if (OnRequestVariables == null)
                throw new Exception("OnRequestVariables handler is not assigned");

            return OnRequestVariables();
        }

        public Func<List<Variable>> OnRequestVariables;

        /// <summary>
        /// Resets all descriptive values in the current BlockSet.
        /// </summary>
        public void ClearDescriptives()
        {
            Rank = 0;
            SDs.Clear();
        }

        /// <summary>
        /// Divides the blocks of ExperimentalUnits into subgroups of desired sizes.
        /// </summary>
        /// <param name="subgroupSizesPerBlock">A two-dimensional list of shorts containing the subgroup sizes per block.</param>
        /// <param name="experimentalUnitsHaveMarker">A bool which is true if the ExperimentalUnits have Markers.</param>
        /// <param name="random">An instance of the Random class.</param>
        public void DivideBlocksIntoSubgroups(List<List<short>> subgroupSizesPerBlock, bool experimentalUnitsHaveMarker, Random random)
        {
            NonDistributableExperimentalUnits.Clear();

            for (int i = 0; i < BlocksOfExperimentalUnits.Count; i++)
            {
                BlocksOfExperimentalUnits[i].DivideExperimentalUnitsIntoSubgroups(subgroupSizesPerBlock[i], experimentalUnitsHaveMarker, random);
                NonDistributableExperimentalUnits.AddRange(BlocksOfExperimentalUnits[i].ExperimentalUnits.Where(ExperimentalUnit => ExperimentalUnit.CanBePlacedInASubgroup == false).ToList());
            }
        }

        /// <summary>
        /// Randomly assigns BlocksOfExperimentalUnits to Groups.
        /// </summary>
        /// <param name="random">An instance of the random class.</param>
        /// <param name="groups">A list of Groups.</param>
        public void RandomlyAssignBlockSetsToGroups(Random random, List<Group> groups)
        {
            List<Group> copyOfGroups = groups.Select(group => group.CloneButPreserveID()).ToList();

            if (!BlocksHaveBeenAssignedToGroups)
            {
                List<Group> randomlyOrderedGroups = new List<Group>();
                int blockCount = BlocksOfExperimentalUnits.Count;

                for (int i = 0; i < blockCount; i++)
                {
                    //randomly select each Group, and place them in the temporary list randomlyOrderedGroupNames
                    int randomIndex = random.Next(0, copyOfGroups.Count());
                    randomlyOrderedGroups.Add(groups.Where(group => group.ID == copyOfGroups[randomIndex].ID).First());
                    copyOfGroups.RemoveAt(randomIndex);
                }

                Groups = randomlyOrderedGroups; //finally, re-add all blocks to the original list
                BlocksHaveBeenAssignedToGroups = true;
            }
            else
                MessageBox.Show("Blocks of the current block set have already been assigned to groups.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Calculates the BlockSet descriptives of all its ExperimentalUnits, that is required for ultimately calculating the BlockSet's Rank.
        /// </summary>
        private void CalculateDescriptives()
        {
            ClearDescriptives();

            int VariableCount = RequestVariables().Count();

            for (int i = 0; i < VariableCount; i++)
            {
                List<double> VariableValues = new List<double>();
                double sumOfSquares = 0;

                foreach (BlockOfExperimentalUnits blockOfExperimentalUnit in BlocksOfExperimentalUnits)
                    VariableValues.AddRange(blockOfExperimentalUnit.ExperimentalUnits.Where(inidividual => inidividual.Values[i] != Global.Settings.MissingValue).Select(ExperimentalUnit => ExperimentalUnit.Values[i]).ToList()); //collect all valid values

                if (VariableValues.Count > 0)
                {
                    //calculate the mean and with that, the sumOfSquares
                    double mean = VariableValues.Average();

                    foreach (double VariableValue in VariableValues)
                        sumOfSquares += Math.Pow((VariableValue - mean), 2);

                    //finally, calculate standard deviation
                    SDs.Add(Math.Pow(sumOfSquares / (VariableValues.Count - 1), 0.5));
                }
                else
                    SDs.Add(Global.Settings.MissingValue);
            }
        }

        /// <summary>
        /// Calculates the Rank of the current BlockSet.
        /// </summary>
        public void CalculateRank()
        {
            double totalRank = 0; //default
            
            CalculateDescriptives(); //calculate descriptives of entire set
            CalculateDescriptivesOfBlocks(calculateAllDescriptives: false); //calculate descriptives of the individual blocks

            int VariableCount = RequestVariables().Count();
            
            for (int i = 0; i < VariableCount; i++)
            {
                //calculate rank per Variable (i.e. VariableRank)
                List<double> blockMeans = new List<double>();
                List<double> blockCVs = new List<double>();
            
                for (int j = 0; j < BlocksOfExperimentalUnits.Count(); j++)
                {
                    //get means and CVs of each block
                    blockMeans.Add(BlocksOfExperimentalUnits[j].Descriptives[i].Mean);
                    blockCVs.Add(BlocksOfExperimentalUnits[j].Descriptives[i].CV);
                }
                
                if (!blockMeans.Contains(Global.Settings.MissingValue) && !blockCVs.Contains(Global.Settings.MissingValue))
                {
                    //If one of the block means or CVs is invalid (== -999), the VariableRank is not included in the totalRank
                    //this means that this BlockSet has an unfair advantage.
                    //a better solution may be to give a penalty for invalid means or CVs, although that might introduce a bias
                    double variableRank = 0;
                    
                    if (SDs[i] != 0) //In some rare cases, the SD of the block set may equal 0. In those cases blockMeans.Max() - blockMeans.Min() also equals 0.
                                     //This check therefore ensures that the variableRank remains 0 and prevents that the variableRank and subsequently the totalRank becomes NaN.
                    {
                        variableRank = ((blockMeans.Max() - blockMeans.Min()) / SDs[i]); //VariableRank = "how many standard deviations do the maxValue and minValue differ?"
                        variableRank += (blockCVs.Max() - blockCVs.Min()); //then, add the difference in maxCV and minCV to the VariableRank, so that the software also matches on SD
                        variableRank *= RequestVariables()[i].Weight;
                    }
                    
                    totalRank += variableRank; //totalRank is the sum of all valid VariableRanks
                }
            }
            
            int roundRankByNumberOfDecimals = 12;
            Rank = Math.Round(totalRank, roundRankByNumberOfDecimals, MidpointRounding.AwayFromZero);
        }
        
        /// <summary>
        /// Calculates the descriptives (the mean and standard deviation, and optionally the min, median and max)
        /// in each BlockOfExperimentalUnits in the current BlockSet.
        /// </summary>
        /// <param name="calculateAllDescriptives">Optional bool which must be true if it is desired to calculate the min,
        /// median and max besides the mean and standard deviation of each BlockOfExperimentalUnits. Default is false.</param>
        public void CalculateDescriptivesOfBlocks(bool calculateAllDescriptives = false)
        {
            foreach (BlockOfExperimentalUnits block in BlocksOfExperimentalUnits)
                block.CalculateDescriptives(RequestVariables(), calculateAllDescriptives);
        }

        public long StableHashCode
        {
            get
            {
                long stableHashCode;

                //Because a set with blockA+blockB == blockB+blockA, the program first creates a list of groupHashes.
                //Then, the blockHashes are sorted and merged.
                //Finally, the merged string is hashed to obtain a setHash
                List<long> blockHashes = new List<long>();

                foreach (BlockOfExperimentalUnits block in this.BlocksOfExperimentalUnits)
                {
                    string hashableBlockString = string.Empty;

                    foreach (ExperimentalUnit ExperimentalUnit in block.ExperimentalUnits)
                        hashableBlockString += ExperimentalUnit.ID;

                    blockHashes.Add(Hash.GetStableHashCode(Hash.SortString(hashableBlockString)));
                }

                blockHashes.Sort();

                string hashableSetString = string.Empty;

                foreach (int blockHash in blockHashes)
                    hashableSetString += blockHash.ToString();

                stableHashCode = Hash.GetStableHashCode(hashableSetString);

                return stableHashCode;
            }
        }

        public double Rank { get; set; } = new double();

        public List<double> SDs { get; set; } = new List<double>();

        public List<BlockOfExperimentalUnits> BlocksOfExperimentalUnits { get; set; } = new List<BlockOfExperimentalUnits>();

        public List<ExperimentalUnit> NonDistributableExperimentalUnits { get; private set; } = new List<ExperimentalUnit>();

        public bool BlocksHaveBeenAssignedToGroups { get; private set; } = false;

        /// <summary>
        /// A list of Groups of the BlocksOfExperimentalUnits.
        /// The names of the groups should be set to "Block 1", "Block 2" etc. when blocks of experimental units are
        /// added to the list BlocksOfExperimentalUnits.
        /// The function RandomlyAssignBlockSetsToGroups() will use the list of Groups from the BlockSet's parent Run,
        /// but the order of the current list of Groups is random as to randomly assign the BlocksOfExperimentalUnits to groups.
        /// </summary>
        public List<Group> Groups { get; private set; } = new List<Group>();
    }
}
