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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    [Serializable]
    public class Experiment
    {
        private bool hasChangedSinceLastSave = true;

        public Experiment(List<ExperimentalUnit> allExperimentalUnits, List<short> blockSizes, List<Variable> activeVariables, List<Variable> allVariables, bool experimentalUnitsHaveMarkers, List<List<short>> subgroupSizesOFEachBlock = null)
        {
            AllExperimentalUnits = allExperimentalUnits;
            BlockSizes = blockSizes;
            ActiveVariables = activeVariables;
            AllVariables = allVariables;
            ExperimentalUnitsHaveMarkers = experimentalUnitsHaveMarkers;

            if (subgroupSizesOFEachBlock != null)
                SubgroupSizesOfEachBlock = subgroupSizesOFEachBlock;
        }

        public Experiment()
        {
            //creates an empty new Experiment();
        }

        /// <summary>
        /// Creates a new Run and sets the OnRequest handlers.
        /// </summary>
        /// <returns>A new instance of Run with OnRequest handlers set.</returns>
        public Run CreateNewRun(bool checkForBlockSetUnicity)
        {
            Run run = new Run(checkForBlockSetUnicity);

            run.OnRequestVariables += new Func<List<Variable>>(delegate { return ActiveVariables; });
            run.OnRequestCreateSubgroups += new Func<bool>(delegate { return CreateSubgroups; });
            run.OnRequestExperimentalUnitsHaveMarker += new Func<bool>(delegate { return ExperimentalUnitsHaveMarkers; });
            run.OnRequestSubgroupSizesPerBlock += new Func<List<List<short>>>(delegate { return SubgroupSizesOfEachBlock; });
            run.OnRequestBlockSizes += new Func<List<short>>(delegate { return BlockSizes; });
            run.OnRequestAllExperimentalUnits += new Func<List<ExperimentalUnit>>(delegate { return AllExperimentalUnits; });
            run.OnRequestGroups += new Func<List<Group>>(delegate { return Groups; });

            return run;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Experiment);
        }

        public bool Equals(Experiment experiment)
        {
            // If Variable is null, return false.
            if (Object.ReferenceEquals(experiment, null))
                return false;

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != experiment.GetType())
                return false;

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.

            //First compare the two-dimensional lists SubgroupSizesOfEachBlock
            bool SubgroupSizesOfEachBlockAreEqual = (SubgroupSizesOfEachBlock.Count == experiment.SubgroupSizesOfEachBlock.Count); //check if counts are equal

            for (int i = 0; SubgroupSizesOfEachBlockAreEqual && i < SubgroupSizesOfEachBlock.Count; i++)
                SubgroupSizesOfEachBlockAreEqual = SubgroupSizesOfEachBlock[i].SequenceEqual(experiment.SubgroupSizesOfEachBlock[i]);

            return (AllExperimentalUnits.SequenceEqual(experiment.AllExperimentalUnits))
                && (BlockSizes.SequenceEqual(experiment.BlockSizes))
                && (ActiveVariables.SequenceEqual(experiment.ActiveVariables))
                && (AllVariables.SequenceEqual(experiment.AllVariables))
                && (Groups.SequenceEqual(experiment.Groups))
                && (CreateSubgroups == experiment.CreateSubgroups)
                && (ExperimentalUnitsHaveMarkers == experiment.ExperimentalUnitsHaveMarkers)
                && (SubgroupSizesOfEachBlockAreEqual)
                && (InputData == experiment.InputData);
        }

        public static bool operator ==(Experiment leftHandSide, Experiment rightHandSide)
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

        public static bool operator !=(Experiment leftHandSide, Experiment rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }

        public Experiment Clone()
        {
            //clone relevant data
            List<ExperimentalUnit> CloneOfAllExperimentalUnits = AllExperimentalUnits.Select(ExperimentalUnit => ExperimentalUnit.Clone()).ToList();
            List<short> CloneOfBlockSizes = BlockSizes.Select(blockSize => blockSize).ToList();
            List<Variable> CloneOfAllVariables = AllVariables.Select(Variable => Variable.Clone()).ToList();
            List<Variable> CloneOfActiveVariables = ActiveVariables.Select(Variable => Variable.Clone()).ToList();
            List<List<short>> CloneOfSubgroupSizesOfEachBlock = SubgroupSizesOfEachBlock.Select(SubgroupSizesOfBlock => SubgroupSizesOfBlock.Select(subgroupSize => subgroupSize).ToList()).ToList();
            List<Group> CloneOfGroups = Groups.Select(group => group.Clone()).ToList();

            //create new experiment
            Experiment newExperiment = new Experiment(CloneOfAllExperimentalUnits, CloneOfBlockSizes, CloneOfActiveVariables, CloneOfAllVariables, ExperimentalUnitsHaveMarkers, CloneOfSubgroupSizesOfEachBlock)
            {
                //fill new experiment with clones of all remaining relevant data
                CreateSubgroups = CreateSubgroups,
                InputData = (string)InputData.Clone(),
                Groups = CloneOfGroups,
                SubgroupSizesAreDefinedViaFormControls = SubgroupSizesAreDefinedViaFormControls
            };

            return newExperiment;
        }

        public override string ToString()
        {
            string[] subgroupSizesOfEachBlock = new string[SubgroupSizesOfEachBlock.Count];

            for (int i = 0; i < SubgroupSizesOfEachBlock.Count; i++)
            {
                //fill array with joined subgroup sizes
                subgroupSizesOfEachBlock[i] = "{" + string.Join(";", SubgroupSizesOfEachBlock[i].Select(subgroupSizes => subgroupSizes.ToString()).ToArray()) + "}";
            }

            string ExperimentString = "Number of runs: " + Runs.Count.ToString()
                + "\nExperimentalUnits: {" + string.Join(";", AllExperimentalUnits.Select(ExperimentalUnit => ExperimentalUnit.Name).ToArray()) + "}"
                + "\nBlock sizes: {" + string.Join(";", BlockSizes.Select(i => i.ToString()).ToArray()) + "}"
                + "\nVariables: " + string.Join(";", ActiveVariables.Select(Variable => Variable.Name).ToArray())
                + "\nCreate subgroups: " + CreateSubgroups.ToString()
                + "\nExperimentalUnits have markers: " + ExperimentalUnitsHaveMarkers.ToString()
                + "\nSubgroup sizes of each block: \n------\n" + string.Join("\n", subgroupSizesOfEachBlock) + "\n------"
                + "\nInput data: \n------\n" + InputData + "\n------";

            return ExperimentString;
        }

        /// <summary>
        /// Saves the Experiment if the user selects a valid file location.
        /// </summary>
        /// <returns>An enum SaveState which reflects if the save was successful or not.</returns>
        public SaveState Save(bool forceAskUserForFilePath = false)
        {
            SaveState saveState = SaveState.Cancelled; //default value

            if (forceAskUserForFilePath || !File.Exists(SaveFilePath))
            {
                //ask user where to save the finished experiment
                SaveFileDialog SaveExperimentSaveFileDialog = new SaveFileDialog
                {
                    Filter = "RandoMice files (*.rndm)|*.rndm",
                    FileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString("d2") + "_" + DateTime.Now.Day.ToString("d2") + " " + "RandoMice data.rndm"
                };

                if (SaveExperimentSaveFileDialog.ShowDialog() == DialogResult.OK)
                    saveState = Serialize(SaveExperimentSaveFileDialog.FileName);
            }
            else
                saveState = Serialize(SaveFilePath);

            return saveState;
        }

        /// <summary>
        /// Serializes the Experiment to a file.
        /// </summary>
        /// <param name="fileName">A string containing the full file path.</param>
        /// <returns>A SaveState value.</returns>
        private SaveState Serialize(string fileName)
        {
            SaveState saveState = SaveState.Cancelled; //default value;

            try
            {
                //try to serialize finishedExperiment to stream
                IFormatter formatter = new BinaryFormatter();

                using (Stream stream = new FileStream(fileName,
                                            FileMode.Create,
                                        FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                    stream.Close();
                }

                saveState = SaveState.Success;

                RunCountAtLastSave = Runs.Count(); //save the number of runs currently present
                HasChangedSinceLastSave = false;
                SaveFilePath = fileName;
            }
            catch
            {
                saveState = SaveState.Failed;
            }

            return saveState;
        }

        /// <summary>
        /// Resets the properties that are responsible for determining if the Experiment has changed since last save.
        /// This function should always be called after deserializing an Experiment from a file.
        /// </summary>
        public void ResetExperimentHasChangedProperties()
        {
            HasChangedSinceLastSave = false;
            RunCountAtLastSave = Runs.Count();
        }

        public List<Variable> AllVariables { get; set; } = new List<Variable>();

        public List<Variable> ActiveVariables { get; set; } = new List<Variable>(); //All variables except those that contain names or markers of experimental units

        public List<ExperimentalUnit> AllExperimentalUnits { get; set; } = new List<ExperimentalUnit>();

        public List<short> BlockSizes { get; set; } = new List<short>();

        public bool BlockSizesAreTooLarge { get { return BlockSizes.Sum(blockSize => blockSize) > AllExperimentalUnits.Count; } }

        /// <summary>
        /// The original string that is copied by the user in the InputDataGridView
        /// and contains all information of the ExperimentalUnits.
        /// </summary>
        public string InputData { get; set; } = string.Empty;

        public bool SubgroupSizesAreDefinedViaFormControls { get; set; } = true;

        public List<List<short>> SubgroupSizesOfEachBlock { get; set; } = new List<List<short>>();

        public ValidOrInvalid SubgroupSizesAreValid
        {
            get
            {
                ValidOrInvalid subgroupSizesAreValid = ValidOrInvalid.Valid;

                if (SubgroupSizesOfEachBlock.Count != BlockSizes.Count)
                    subgroupSizesAreValid = ValidOrInvalid.Invalid;
                else
                {
                    for (int i = 0; i < Global.CurrentExperiment.BlockSizes.Count; i++)
                    {
                        if (SubgroupSizesOfEachBlock[i].Sum(subgroupSize => subgroupSize) != BlockSizes[i])
                            subgroupSizesAreValid = ValidOrInvalid.Invalid;
                    }
                }

                return subgroupSizesAreValid;
            } 
        }

        public bool CreateSubgroups { get; set; } = false;

        public bool ExperimentalUnitsHaveMarkers { get; set; } = false;

        public List<Run> Runs { get; private set; } = new List<Run>();

        public bool HasChangedSinceLastSave
        {
            get
            {
                if (Runs.Count() > RunCountAtLastSave)
                    hasChangedSinceLastSave = true;

                return hasChangedSinceLastSave;
            }
            private set
            { 
                hasChangedSinceLastSave = value;
            }
        }

        /// <summary>
        /// Contains the full file path to which the Experiment was last saved to.
        /// </summary>
        public string SaveFilePath { get; set; } = string.Empty;

        /// <summary>
        /// The number of runs at last save (i.e. serialization).
        /// This is used to track if experiment has changed since last save.
        /// </summary>
        private int RunCountAtLastSave { get; set; } = 0;

        public bool GroupNamesHaveBeenDefinedByUser { get { return Groups.Count != 0; } }

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
