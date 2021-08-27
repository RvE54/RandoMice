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
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RvE_RandoMice
{
    public enum SaveState { Success, Failed, Cancelled, NotNeeded };
    public enum LoadState { Success, Failed, Cancelled };
    public enum ValidOrInvalid { Valid, Invalid };
    public enum EnableOrDisable { Disable = 0, Enable = 1 };

    static class Global
    {
        public static string CurrentAssemblyVersion { get; } = "v" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.')); //do not show revision number
        public static int DisplayAboutFormOnStartDuration { get; } = 2000; //in milliseconds
        public static Experiment CurrentExperiment { get; set; }
        public static Run CurrentRun { get; set; }
        public static Experiment FinishedExperiment { get; set; }
        public static Settings Settings { get; set; }
        public static NumberFormatInfo CultureNumberFormatInfo { get; } = CultureInfo.CurrentCulture.NumberFormat; // Gets a NumberFormatInfo associated with the current culture.
        public static List<(Control Control, bool OriginalEnabledState)> ControlsAndEnabledStates { get; set; } = new List<(Control Control, bool OriginalEnabledState)>();


        private static int groupID = 0;
        public static int GroupID
        {
            get { groupID++; return groupID; } //An integer that returns a unique value each time, to be used as unique Group.ID value

            private set { groupID = value; }
        }

        //variables that are set directly by buttons etc
        public static int DesiredUniqueSets { get; set; } //must be set on MainForm start
        public static int RememberSets { get; set; } //must be set at MainForm start
        public static int NamesOfExperimentalUnitsInputColumnNumber { get; set; } = 0;
        public static int ExperimentalUnitsMarkerInputColumnNumber { get; set; } = -1;
        public static bool CheckForBlockSetUnicity { get; set; } = false;
        public static int SelectedBlockSetIndex { get; set; } = 0;
        public static int SetsExported { get; set; } = 0;


        //variables that are set in functions etc
        public static double TheoreticalUniqueBlockSets { get; set; } = 0;
        public static bool DoNotInterruptBlockSetCreation { get; set; } = true;


        //control lists that need to be filled during form initialization
        public static List<BackgroundWorker> BackgroundWorkers { get; set; }

        public static bool AnyBackgroundWorkerIsBusy 
        { 
            get 
            {
                foreach (BackgroundWorker backgroundWorker in BackgroundWorkers)
                {
                    if (backgroundWorker.IsBusy)
                        return true;
                }

                return false;
            } 
        }
        public static List<CheckBox> DescriptiveCheckboxes { get; set; }
    }
}
