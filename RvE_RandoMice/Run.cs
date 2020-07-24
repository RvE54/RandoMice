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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    [Serializable]
    public class Run
    {
        public Run(bool checkForBlockSetUnicity)
        {
            this.CheckForUnicity = checkForBlockSetUnicity;
        }

        /// <summary>
        /// Creates a new BlockSet, randomly fills blocks with ExperimentsUnits and sets the OnRequest handlers.
        /// </summary>
        /// <param name="random">An instance of the Random class.</param>
        /// <returns>A new instance of BlockSet with randomly filled blocks and with OnRequest handlers set.</returns>
        public BlockSet CreateNewBlockSet(Random random)
        {
            BlockSet newBlockSet = new BlockSet();
            newBlockSet.OnRequestVariables += new Func<List<Variable>>(delegate { return RequestVariables(); });
            
            List<short> blockSizes = RequestBlockSizes();

            List<ExperimentalUnit> copyOfAllExperimentalUnits = new List<ExperimentalUnit>(RequestAllExperimentalUnits()); //copy ExperimentalUnits (by reference)

            //fill the newBlockSet with new blocks of ExperimentalUnits
            for (int i = 0; i < blockSizes.Count; i++)
            {
                BlockOfExperimentalUnits newBlockOfExperimentalUnits = new BlockOfExperimentalUnits();

                for (int j = 0; j < blockSizes[i]; j++)
                {
                    int r = random.Next(0, copyOfAllExperimentalUnits.Count);
                    newBlockOfExperimentalUnits.ExperimentalUnits.Add(copyOfAllExperimentalUnits[r]);
                    copyOfAllExperimentalUnits.RemoveAt(r);
                }

                newBlockSet.BlocksOfExperimentalUnits.Add(newBlockOfExperimentalUnits);
                newBlockSet.Groups.Add(new Group((char)999, "Block " + (i + 1).ToString())); //until the user instructed the software to randomly allocate group names to blocks
                                                                           //(i.e. the function RandomizeGroups is called within the selected BlockSet),
                                                                           //the group names should be "Block 1", "Block 2" etc.
            }

            return (newBlockSet);
        }

        /// <summary>
        /// Requests the bool CreateSubgroups in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a bool CreateSubgroups.
        /// Each Run must to be able to request the bool CreateSubgroups from its parent.
        /// </summary>
        /// <returns>The bool CreateSubgroups in the current Run's parent Experiment.</returns>
        protected bool RequestCreateSubgroups()
        {
            if (OnRequestCreateSubgroups == null)
                throw new Exception("OnRequestCreateSubgroups handler is not assigned");

            return OnRequestCreateSubgroups();
        }

        /// <summary>
        /// Requests the bool ExperimentalUnitsHaveMarker in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a bool ExperimentalUnitsHaveMarker.
        /// Each Run must to be able to request the bool ExperimentalUnitsHaveMarker from its parent.
        /// </summary>
        /// <returns>The bool ExperimentalUnitsHaveMarker in the current Run's parent Experiment.</returns>
        protected bool RequestExperimentalUnitsHaveMarker()
        {
            if (OnRequestExperimentalUnitsHaveMarker == null)
                throw new Exception("OnRequestExperimentalUnitsHaveMarker handler is not assigned");

            return OnRequestExperimentalUnitsHaveMarker();
        }

        /// <summary>
        /// Requests the list of Variables in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a list of Variables.
        /// Each Run must to be able to request the list of Variables from its parent.
        /// </summary>
        /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
        protected List<Variable> RequestVariables()
        {
            if (OnRequestVariables == null)
                throw new Exception("OnRequestVariables handler is not assigned");

            return OnRequestVariables();
        }

        /// <summary>
        /// Requests the list of ExperimentalUnits in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a list of ExperimentalUnits.
        /// Each Run must to be able to request the list of Variables from its parent.
        /// </summary>
        /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
        protected List<ExperimentalUnit> RequestAllExperimentalUnits()
        {
            if (OnRequestAllExperimentalUnits == null)
                throw new Exception("OnRequestAllExperimentalUnits handler is not assigned");

            return OnRequestAllExperimentalUnits();
        }

        /// <summary>
        /// Requests the list of GroupNames in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a list of group names.
        /// Each Run must to be able to request the list of GroupNames from its parent.
        /// </summary>
        /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
        protected List<Group> RequestGroups()
        {
            if (OnRequestGroups == null)
                throw new Exception("OnRequestGroups handler is not assigned");

            return OnRequestGroups();
        }

        /// <summary>
        /// Requests the two-dimensional list of shorts SubgroupSizesPerBlock in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a two-dimensional list of shorts SubgroupSizesPerBlock.
        /// Each Run must to be able to request the SubgroupSizesPerBlock from its parent.
        /// </summary>
        /// <returns>The two-dimensional list of shorts SubgroupSizesPerBlock in the current Run's parent Experiment.</returns>
        protected List<List<short>> RequestSubgroupSizesPerBlock()
        {
            if (OnRequestSubgroupSizesPerBlock == null)
                throw new Exception("OnRequestSubgroupSizesPerBlock handler is not assigned");

            return OnRequestSubgroupSizesPerBlock();
        }

        /// <summary>
        /// Requests the list of shorts BlockSizes in the current Run's parent Experiment.
        /// An Experiment contains a list of Runs and a list of shorts BlockSizes.
        /// Each Run must to be able to request the BlockSizes from its parent.
        /// </summary>
        /// <returns>The list of shorts BlockSizes in the current Run's parent Experiment.</returns>
        protected List<short> RequestBlockSizes()
        {
            if (OnRequestBlockSizes == null)
                throw new Exception("OnRequestBlockSizes handler is not assigned");

            return OnRequestBlockSizes();
        }

        public Func<bool> OnRequestCreateSubgroups;
        
        public Func<bool> OnRequestExperimentalUnitsHaveMarker;
        
        public Func<List<Variable>> OnRequestVariables;

        public Func<List<ExperimentalUnit>> OnRequestAllExperimentalUnits;

        public Func<List<Group>> OnRequestGroups;

        public Func<List<List<short>>> OnRequestSubgroupSizesPerBlock;

        public Func<List<short>> OnRequestBlockSizes;

        /// <summary>
        /// Divides all BlockSets in the current Run into subgroups.
        /// </summary>
        public void DivideAllBlockSetsIntoSubgroups()
        {
            //divides the groups of all sets into subgroups, either completely randomly or semi-randomly (based on markers)
            if (RequestCreateSubgroups())
            {
                Random random = new Random();

                foreach (BlockSet BlockSet in BlockSets)
                    BlockSet.DivideBlocksIntoSubgroups(RequestSubgroupSizesPerBlock(), RequestExperimentalUnitsHaveMarker(), random);
            }
        }

        /// <summary>
        /// Randomly assigns BlocksOfExperimentalUnits of the given BlockSet to the GroupNames.
        /// </summary>
        public void RandomizeBlocksOfSet(int blockSetIndex)
        {
            Random random = new Random();
            BlockSets[blockSetIndex].RandomlyAssignBlockSetsToGroups(random, RequestGroups());
        }

        /// <summary>
        /// Calculates the descriptives (the mean and standard deviation, and optionally the min, median and max)
        /// in each BlockOfExperimentalUnits in the desired BlockSet.
        /// </summary>
        /// <param name="blockSetIndex">The index of the BlockSet in which to calculate the descriptives.</param>
        /// <param name="calculateAllDescriptives">Optional bool which must be true if it is desired to calculate the min,
        /// median and max besides the mean and standard deviation of each BlockOfExperimentalUnits. Default is false.</param>
        public void CalculateDescriptivesOfBlocksInBlockSet(int blockSetIndex, bool calculateAllDescriptives = false)
        {
            BlockSets[blockSetIndex].CalculateDescriptivesOfBlocks(calculateAllDescriptives);
        }

        /// <summary>
        /// Calculates the Rank in the desired BlockSet.
        /// </summary>
        /// <param name="blockSetIndex">The index of the BlockSet in which to calculate the Rank</param>
        public void CalculateSetRank(int blockSetIndex)
        {
            BlockSets[blockSetIndex].CalculateRank();
        }

        /// <summary>
        /// Sorts all BlockSets in the current Run by their Rank.
        /// </summary>
        public void SortBlockSetsByRank()
        {
            BlockSets = BlockSets.OrderBy(blockSet => blockSet.Rank).ToList();
        }

        /// <summary>
        /// Calculates the time remaining based on the progress percentage
        /// and a given Stopwatch.
        /// </summary>
        /// <param name="stopwatch">A Stopwatch that started simultaneously with the BackgroundWorker
        /// that created the current Run.</param>
        /// <returns>A string containing the average time remaining of multiple calculations.</returns>
        public string CalculateTimeRemaining(Stopwatch stopwatch)
        {
            if (ProgressPercentage != 0)
            {
                //calculate time remaining
                long msTimeRemaining = (long)(stopwatch.Elapsed.Ticks * (100 / ProgressPercentage)) - stopwatch.Elapsed.Ticks;

                //add calculated time remaining to list of calculated times remaining
                CalculatedTimesRemaining.Add(TimeSpan.FromTicks(msTimeRemaining));
            }

            //calculated the average of all times remaining, and clear the times remaining
            if (CalculatedTimesRemaining.Count >= 20)
            {
                TimeRemaining = TimeSpan.FromSeconds((int)CalculatedTimesRemaining.Select(t => t.TotalSeconds).ToList().Average());
                CalculatedTimesRemaining.Clear();
            }
            else if (TimeRemaining.TotalSeconds != 0)
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromMilliseconds(Global.Settings.CreateBlockSetsTimerInterval));
                //by default, substract the interval of CreateBlockSetsTimer,
                //as this timer calls the current function once in each interval.

            return TimeRemaining.ToString(Global.Settings.TimeSpanStringFormat);
        }

        public double ProgressPercentage { get; set; } = 0;

        /// <summary>
        /// Gets a copyable string seperated by tabs and newlines,
        /// containing the set numbers, markers to change and ranks of all sets.
        /// </summary>
        /// <param name="copyToClipboard">Optional bool that must be true if the output should be
        /// copied to the clipboard. Default is false.</param>
        /// <returns>A string seperated by tabs and newlines, containing all set numbers, 
        /// the number of markers that need to be changed (if applicable), and the set ranks.
        /// The sets are sorted ascendingly by their rank. Each line contains information of one set.</returns>
        /// <remarks>Example of a return string:
        /// Set number\tMarkers to change\tRank\r\n
        /// 1\t5\t0.225465489657\r\n
        /// 2\t2\t0.168489463217
        /// </remarks>
        public string GetSetsAndRanksAsString(bool copyToClipboard = false)
        {
            //this functions returns a string to visualise set ranks and optionally the number of markers to add
            string newline = "\r\n";
            string delimiter = "\t";
            StringBuilder output = new StringBuilder(null);

            output.Append("Set number");
            output.Append(delimiter);

            if (RequestExperimentalUnitsHaveMarker())
            {
                output.Append("Markers to change");
                output.Append(delimiter);
            }
            
            output.Append("Rank");

            if (RequestExperimentalUnitsHaveMarker())
            {
                for (int i = 0; i < BlockSets.Count; i++)
                {
                    output.Append(newline);
                    output.Append((i + 1).ToString() + delimiter);
                    output.Append(BlockSets[i].NonDistributableExperimentalUnits.Count.ToString() + delimiter);
                    output.Append(BlockSets[i].Rank.ToString());
                }
            }
            else
            {
                for (int i = 0; i < BlockSets.Count; i++)
                {
                    output.Append(newline);
                    output.Append((i + 1).ToString() + delimiter);
                    output.Append(BlockSets[i].Rank.ToString());
                }
            }

            if (copyToClipboard)
                Clipboard.SetDataObject(output);

            return output.ToString();
        }

        /// <summary>
        /// Gets a copyable string seperated by tabs and newlines,
        /// containing the names of all ExperimentalUnits within each block of a desired BlockSet.
        /// </summary>
        /// <param name="setNumber">The index of the desired BlockSet.</param>
        /// <param name="copyToClipboard">Optional bool that must be true if the output should be
        /// copied to the clipboard. Default is false.</param>
        /// <returns>A string seperated by tabs and newlines, containing
        /// all names of ExperimentalUnits in each block of a desired BlockSet.
        /// The ExperimentalUnits are sorted ascendingly by their name.
        /// Each line contains one name within each block.</returns>
        /// <remarks>Example of a return string:
        /// Block 1\tBlock2\r\n
        /// ExperimentalUnit1\tExperimentalUnit5\r\n
        /// ExperimentalUnit3\tExperimentalUnit6</remarks>
        public string GetExperimentalUnitNamesPerBlockAsString(int setNumber, bool copyToClipboard = false)
        {
            string newline = "\r\n";
            string delimiter = "\t";
            StringBuilder output = new StringBuilder(null);

            for (int i = 0; i < BlockSets[setNumber].BlocksOfExperimentalUnits.Count(); i++)
            {
                output.Append(BlockSets[setNumber].Groups[i].Name);

                if (i < BlockSets[setNumber].BlocksOfExperimentalUnits.Count() - 1)
                    output.Append(delimiter);
            }

            int largestBlockSize = BlockSets[setNumber].BlocksOfExperimentalUnits.Select(blockSize => blockSize.ExperimentalUnits.Count).ToList().Max();
            
            foreach(BlockOfExperimentalUnits BlockOfExperimentalUnits in BlockSets[setNumber].BlocksOfExperimentalUnits)
                BlockOfExperimentalUnits.SortExperimentalUnitsByName();

            for (int i = 0; i < largestBlockSize; i++)
            {
                output.Append(newline);

                for (int j = 0; j < BlockSets[setNumber].BlocksOfExperimentalUnits.Count(); j++)
                {
                    if (i < BlockSets[setNumber].BlocksOfExperimentalUnits[j].ExperimentalUnits.Count)
                    {
                        output.Append(BlockSets[setNumber].BlocksOfExperimentalUnits[j].ExperimentalUnits[i].Name);

                        if (j < BlockSets[setNumber].BlocksOfExperimentalUnits.Count() - 1)
                            output.Append(delimiter);
                    }
                    else
                        output.Append(delimiter);
                }
            }

            if (copyToClipboard)
                Clipboard.SetDataObject(output);

            return output.ToString();
        }

        /// <summary>
        /// Gets a copyable string seperated by tabs and newlines,
        /// containing the names and Markers of all ExperimentalUnits
        /// within each block and subgroup of a desired BlockSet.
        /// </summary>
        /// <param name="setNumber">The index of the desired BlockSet.</param>
        /// <param name="copyToClipboard">Optional bool that must be true if the output should be
        /// copied to the clipboard. Default is false.</param>
        /// <returns>A string seperated by tabs and newlines, containing
        /// all names and Markers of ExperimentalUnits within each block
        /// and subgroup of a desired BlockSet.
        /// Each line contains information of one ExperimentalUnit.</returns>
        /// <remarks>Example of a return string
        /// Block\tSubgroup\tName of experimental unit\tMarker of experimental unit\r\n
        /// Block 1\t1\t2\tL\r\n\
        /// t1\t4\tR\r\n
        /// \t2\t1\tR\r\n\
        /// t2\t8\tRR\r\n
        /// Block 2\t1\t5\tRL\r\n
        /// \t1\t6\tLL\r\n
        /// \t2\t3\tR\r\n
        /// \t2\t7\tRL"
        ///</remarks>
        public string GetExperimentalUnitNamesPerSubgroupAsString(int setNumber, bool copyToClipboard = false)
        {
            string newline = "\r\n";
            string delimiter = "\t";
            StringBuilder output = new StringBuilder(null);

            output.Append("Block" + delimiter);
            output.Append("Subgroup" + delimiter);
            output.Append("Name of experimental unit");
            if (OnRequestExperimentalUnitsHaveMarker())
                output.Append(delimiter + "Marker of experimental unit");

            for (int i = 0; i < BlockSets[setNumber].BlocksOfExperimentalUnits.Count(); i++)
            {
                for (int j = 0; j < BlockSets[setNumber].BlocksOfExperimentalUnits[i].Subgroups.Count(); j++)
                {
                    for (int k = 0; k < BlockSets[setNumber].BlocksOfExperimentalUnits[i].Subgroups[j].ExperimentalUnits.Count(); k++)
                    {
                        BlockSets[setNumber].BlocksOfExperimentalUnits[i].Subgroups[j].SortExperimentalUnitsByName();
                        output.Append(newline);
                        
                        if (j == 0 && k == 0)
                        {
                            output.Append(BlockSets[setNumber].Groups[i].Name);
                            output.Append(delimiter);
                        }
                        else
                            output.Append(delimiter);

                        output.Append((j + 1).ToString() + delimiter);
                        output.Append(BlockSets[setNumber].BlocksOfExperimentalUnits[i].Subgroups[j].ExperimentalUnits[k].Name);
                        
                        if (OnRequestExperimentalUnitsHaveMarker())
                            output.Append(delimiter + BlockSets[setNumber].BlocksOfExperimentalUnits[i].Subgroups[j].ExperimentalUnits[k].Marker.Name);
                    }
                }

                foreach (ExperimentalUnit ExperimentalUnit in BlockSets[setNumber].BlocksOfExperimentalUnits[i].ExperimentalUnits)
                {
                    if (BlockSets[setNumber].NonDistributableExperimentalUnits.Contains(ExperimentalUnit))
                    {
                        output.Append(newline);
                        output.Append(delimiter);
                        output.Append("no subgroup available" + delimiter);
                        output.Append(ExperimentalUnit.Name + delimiter);
                        output.Append(ExperimentalUnit.Marker.Name);
                    }
                }
            }

            if (copyToClipboard)
                Clipboard.SetDataObject(output);

            return output.ToString();
        }

        /// <summary>
        /// Gets a copyable string seperated by tabs and newlines,
        /// containing the descriptives of all Variables of a BlockOfExperimentalUnits
        /// within a desired BlockSet.
        /// </summary>
        /// <param name="setNumber">The index of the desired BlockSet.</param>
        /// <param name="descriptiveCheckboxes">A list of CheckBoxes of which the Tag represents the exact name of a Descriptive.</param>
        /// <param name="copyToClipboard">Optional bool that must be true if the output should be
        /// copied to the clipboard. Default is false.</param>
        /// <returns>A string seperated by tabs and newlines, containing
        /// all user-selected descriptives of all Variables of each BlockOfExperimentalUnits
        /// within a desired BlockSet.
        /// Each line contains one descriptive of each Variable.</returns>
        /// <remarks>Example of a return string:
        /// Block\tDescriptive\tVariable1\r\n
        /// Block 1\tMean\t0,47\r\n
        ///	\tSD\t0,26\r\n
        /// Block 2\tMean\t0,47\r\n
        ///	\tSD\t0,26</remarks>
        public string GetDescriptivesOfExperimentalUnitsAsString(int setNumber, List<CheckBox> descriptiveCheckboxes, bool copyToClipboard = false)
        {
            string newline = "\r\n";
            string delimiter = "\t";
            StringBuilder output = new StringBuilder(null);

            CalculateDescriptivesOfBlocksInBlockSet(setNumber, calculateAllDescriptives: true);
            output.Append("Block" + delimiter + "Descriptive");

            for (int i = 0; i < OnRequestVariables().Count; i++)
                output.Append(delimiter + OnRequestVariables()[i].Name);

            List<string> descriptives = new List<string>(descriptiveCheckboxes.Where(checkbox => checkbox.Checked == true).Select(checkbox => checkbox.Tag.ToString()).ToArray());
            
            if (descriptives.Count != 0)
            {
                for (int i = 0; i < BlockSets[setNumber].BlocksOfExperimentalUnits.Count; i++)
                {
                    for (int j = 0; j < descriptives.Count; j++)
                    {
                        output.Append(newline);

                        //for each block: add the group name in the first column in the row of the first descriptive.
                        //note: if blocks have not yet been assigned group names, the GroupNames should be "Block 1", "Block 2", etc.
                        if (j == 0)
                        {
                            output.Append(BlockSets[setNumber].Groups[i].Name);
                            output.Append(delimiter);
                        }
                        else
                            output.Append(delimiter);

                        //place the descriptive name in the second column
                        output.Append(descriptives[j].Replace("Rounded", "")); //remove text "Rounded" to get a readable descriptive name
                        output.Append(delimiter);

                        //foreach descriptive, get the corresponding value
                        for (int k = 0; k < RequestVariables().Count(); k++)
                        {
                            //call a value by its name that is set in the checkboxes
                            output.Append(BlockSets[setNumber].BlocksOfExperimentalUnits[i].Descriptives[k][descriptives[j]].ToString());
                            output.Append(delimiter);
                        }
                    }
                }
            }

            if (copyToClipboard)
                Clipboard.SetDataObject(output);

            return output.ToString();
        }

        public bool CheckForUnicity { get; set; } = true;

        /// <summary>
        /// The total number of Sets that have been considered when BlockSets were created
        /// This value therefore also includes any Sets that were omitted because they were not unique.
        /// </summary>
        public long TotalNumberOfSetsConsidered { get; set; } = 0;

        /// <summary>
        /// The total number of unique Sets that have been considered when BlockSets were created
        /// </summary>
        public long UniqueSetsCreated { get; set; } = 0;

        /// <summary>
        /// A list of BlockSets that should contain the best x number of Sets, based on their .Rank
        /// </summary>
        public List<BlockSet> BlockSets { get; set; } = new List<BlockSet>();

        public TimeSpan TotalTimeElapsedForCreatingBlockSets { get; set; } = new TimeSpan();

        private TimeSpan TimeRemaining { get; set; } = new TimeSpan();

        public List<TimeSpan> CalculatedTimesRemaining { get; set; } = new List<TimeSpan>();

        public SortedSet<long> BlockSetHashes { get; set; } = new SortedSet<long>();
    }
}
