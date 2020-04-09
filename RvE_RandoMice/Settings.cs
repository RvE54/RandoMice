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
    public class Settings
    {
        public Settings(int defaultDesiredUniqueSets,
                        int maximalDesiredUniqueBlockSets,
                        int defaultRememberSets,
                        bool defaultCheckForBlockSetUnicityCheckBoxChecked, 
                        string defaultTheoreticalUniqueBlockSetsLabelText,
                        string defaultTimeElapsedLabelText,
                        string defaultTimeRemainingLabelText,
                        string defaultProgressLabelText,
                        string defaultProgressPercentageLabelText)
        {
            this.Load(defaultDesiredUniqueSets,
                      maximalDesiredUniqueBlockSets,
                      defaultRememberSets,
                      defaultCheckForBlockSetUnicityCheckBoxChecked,
                      defaultTheoreticalUniqueBlockSetsLabelText,
                      defaultTimeElapsedLabelText,
                      defaultTimeRemainingLabelText,
                      defaultProgressLabelText,
                      defaultProgressPercentageLabelText);
        }

        public void Load(int defaultDesiredUniqueSets,
                         int maximalDesiredUniqueBlockSets,
                         int defaultRememberSets,
                         bool defaultCheckForBlockSetUnicityCheckBoxChecked,
                         string defaultTheoreticalUniqueBlockSetsLabelText,
                         string defaultTimeElapsedLabelText,
                         string defaultTimeRemainingLabelText,
                         string defaultProgressLabelText,
                         string defaultProgressPercentageLabelText)
        {
            //default values from controls in MainFrom are passed as Variable
            DefaultDesiredUniqueBlockSets = defaultDesiredUniqueSets;
            MaximalDesiredUniqueBlockSets = maximalDesiredUniqueBlockSets;
            DefaultRememberSets = defaultRememberSets;
            DefaultCheckForBlockSetUnicityCheckBoxChecked = defaultCheckForBlockSetUnicityCheckBoxChecked;
            DefaultTheoreticalUniqueBlockSetsLabelText = defaultTheoreticalUniqueBlockSetsLabelText;
            DefaultTimeElapsedLabelText = defaultTimeElapsedLabelText;
            DefaultTimeRemainingLabelText = defaultTimeRemainingLabelText;
            DefaultProgressLabelText = defaultProgressLabelText;
            DefaultProgressPercentageLabelText = defaultProgressPercentageLabelText;
        }

        public double MissingValue { get; private set; } = -999;
        public short MissingBlockSize { get; private set; } = -999;
        public string TimeSpanStringFormat { get; private set; } = @"hh\:mm\:ss";
        public int CreateBlockSetsTimerInterval { get; private set; } = 100; //milliseconds
        public int BackgroundWorkerReportProgressPeriod { get; private set; } = 50; //the background worker reports every x milliseconds
        public decimal MaxVariableWeight { get; private set; } = 256;
        public (byte DefaultValue, byte MaxValue) VariableDecimalPlaces { get; private set; } = (2, 8);
        public int MaximalNumberOfBlocks { get; private set; } = 200;
        public (int Horizontal, int Vertical) MaximalSubgroupSizeControls { get; private set; } = (8, 10);


        //Names of TabPages that will be visible to the user
        public string BlockSizesTabPageName { get; private set; } = "Block sizes";
        public string SubgroupSizesTabPageName { get; private set; } = "Subgroup sizes";


        //Default texts of labels, ToolTip texts and buttons that may be presented to the user
        public string NoBlocksExistYetLabelText { get; private set; } = "The number of blocks to create has not been defined yet. Please define this number first.";
        public string SubgroupsCannotBeFormedExplanatoryLabelText { get; private set; } = "Make sure to first provide:\n" +
                                                                                          "1) the number of blocks to be created,\n" +
                                                                                          "2) the block sizes, and\n" +
                                                                                          "3) the subgroup sizes\n" +
                                                                                          "before changing the individual subgroup sizes.";
        public string SubgroupsHelpPictureBoxToolTipText { get; private set; } = "In this tab page, the subgroup sizes of each block can be modified.\n\n" +
                                                                                 "Subgroups can be added to a block using its \" + \" button.\n" +
                                                                                 "Subgroups can be removed using the right-mouse button";
        public string SubgroupSizesTemplateLabelText { get; private set; } = "Many subgroups exist with the current blocks and experimental units.\n" +
                                                                             "Should you desire to alter one or more individual subgroup sizes,\n" +
                                                                             "please import the subgroup sizes in a .txt file, separated by semicolons\n" +
                                                                             "A template containing the current subgroup sizes may be downloaded via\n" +
                                                                             "the buttons below.";
        public string ExportSubgroupSizesTemplateButtonText { get; private set; } = "Export template";
        public string ImportSubgroupSizesFromTemplateButtonText { get; private set; } = "Import subgroup sizes";

        //Default control name bases
        //Note: controls using these names will be create on run-time.
        //These names are defined once, and will never be visible to the user.
        //The names of controls using the nameBases below will often be followed by a block number,
        //or by a block number + dott + subgroup number
        //using the same name basis for all controls of the same time, allows for distinguishing between the various controls
        //and allows for easily determining to which block and/or subgroup the control belongs by evaluating its Name property only.
        public string BlockNumberLabelNameBasis { get; private set; } = "BlockNumberLabel";
        public string SubgroupNumberLabelNameBasis { get; private set; } = "SubgroupNumberLabel";
        public string SubgroupSizeNumericUpDownNameBasis { get; private set; } = "SubgroupSizeNumericUpDown";
        public string AddSubgroupControlsButtonNameBasis { get; private set; } = "AddSubgroupComboboxButton";
        public string NoBlocksExistYetLabelName { get; private set; } = "noBlocksExistYetLabel";
        public string BlockSizeNumericUpDownNameBasis { get; private set; } = "BlockSizeNumericUpDown";
        public string BlockSizeLabelNameBasis { get; private set; } = "BlockSizeLabel";
        public string VariableWeightNumericUpDownNameBasis { get; private set; } = "VariableWeightNumericUpDown";
        public string VariableNameTextBoxNameBasis { get; private set; } = "VariableName";
        public string VariableDecimalPlacesNumericUpDownNameBasis { get; private set; } = "VariableDecimalPlacesNumericUpDown";
        public string GroupNameLabelNameBasis { get; private set; } = "GroupNameLabel";
        public string GroupNameTextBoxNameBasis { get; private set; } = "GroupNameTextBox";
        public string BlockSizeLabel { get; private set; } = "BlockSizesLabel";


        //Names of controls that are created on run-time. These names are not visible to the user.
        public string VariablesTabPageName { get; private set; } = "VariablesTabPage";
        public string SubgroupsCannotBeFormedExplanatoryLabelName { get; private set; } = "SubgroupsCannotBeFormedExplanatoryLabel";
        public string SubgroupsHelpPictureBoxName { get; private set; } = "SubgroupsHelpPictureBox";
        public string SubgroupSizesTemplateLabelName { get; private set; } = "SubgroupSizesTemplateLabel";
        public string exportSubgroupSizesTemplateButtonName { get; private set; } = "ExportSubgroupSizesTemplateButton";
        public string ImportSubgroupSizesFromTemplateButtonName { get; private set; } = "ImportSubgroupSizesFromTemplateButton";


        //Default control positions and sizes
        public (int Top, int Left, int Height) DefaultVariableControl { get; private set; } = (18, 10, 21);
        public int DefaultSubgroupBlockLabelWidth { get; private set; } = 80;
        public int DefaultSubgroupNumericUpDownWidth { get; private set; } = 50;
        public (int Height, int Width) DefaultControl { get; private set; } = (21, 50);
        public int DefaultVariableDecimalPlacesNumericUpDownWidth { get; private set; } = 50;
        public int DefaultVariableNameTextBoxWidth { get; private set; } = 150;
        public int DefaultVariableWeightNumericUpDownWidth { get; private set; } = 80;
        public int SubgroupsCannotBeFormedExplanatoryLabelTop { get; private set; } = 30;
        public (int Top, int Width) NoBlocksExistYetLabel { get; private set; } = (50, 500);
        public int DefaultTabPageMargin { get; private set; } = 5;
        public (int Top, int Left, int Height) DefaultGroupNameControl { get; private set; } = (21, 0, 21);
        public int DefaultGroupNameLabelWidth { get; private set; } = 60;
        public (int Height, int Width) DefaultBlockNumberLabel { get; private set; } = (13, 60);
        public int DefaultGroupNameTextBoxWidth { get; private set; } = 200;
        public int MaxGroupNamesPanelHeight { get; private set; } = 200;
        public (int Top, int Left) SubgroupSizesTemplateLabel { get; private set; } = (20, 20);
        public int DownloadSubgroupSizesTemplateButtonWidth { get; private set; } = 120;
        public int ImportSubgroupSizesFromTemplateButtonWidth { get; private set; } = 120;
        public (int Height, int Width) MainFormWithResultsSize { get; private set; } = (705, 840);


        //Misc default values that are set via the designer of MainForm.cs
        //Therefore, these values need to be passed as Variables on initialization of Settings.
        public string DefaultTheoreticalUniqueBlockSetsLabelText { get; private set; }
        public string DefaultTimeElapsedLabelText { get; private set; }
        public string DefaultTimeRemainingLabelText { get; private set; }
        public string DefaultProgressLabelText { get; private set; }
        public string DefaultProgressPercentageLabelText { get; private set; }
        public bool DefaultCheckForBlockSetUnicityCheckBoxChecked { get; private set; }
        public int DefaultDesiredUniqueBlockSets { get; private set; }
        public int MaximalDesiredUniqueBlockSets { get; private set; }
        public int DefaultRememberSets { get; private set; }
    }
}
