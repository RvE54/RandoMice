# RandoMice
RandoMice has been developed as a user-friendly tool that allows users to create well-balanced blocks of experimental units and randomly assign those blocks to experimental intervention groups.

## Getting started
These instructions will get you a copy of the software up and running on your local machine.

### Prerequisites
RandoMice makes use of .NET Framework 4.7.2, which will be downloaded and installed automatically when installing RandoMice. Alternatively, you may download the Framework here: http://go.microsoft.com/fwlink/?LinkId=863262

### Installation
A .zip file containing the latest installable version of RandoMice for Microsoft Windows can be downloaded from [the releases page](https://github.com/RvE54/RandoMice/releases).

After downloading, installation begins by opening the .zip file and executing the setup.msi file. Doing so will open a guide that asks you to accept the [license agreement](./LICENSE), to provide an installation folder, and to specify if RandoMice should be installed just for yourself or for all users on your computer.

## User guide
These instructions will describe the software's user interface and workflow.

RandoMice's interface may be roughly divided into two sections: the left section allows users to import data and define, amongst others, the block-sizes and subgroup sizes; the right section will display progress and results, see the screenshot below.

![RandoMice's user interface may be roughly divided into two sections](/images/Screenshot_UI_v.1.0.9.png "Screenshot of RandoMice v1.0.9")

**Figure 1. Screenshot of RandoMice v1.0.9.**

### Importing data of experimental units
Data of experimental units can be copied from any spreadsheet editor (*e.g.* Microsoft Excel) and pasted from the clipboard into the top-left data grid by pressing the corresponding button. Each row of data should contain data of one experimental unit; each experimental unit must contain a unique name or number, and may contain one or more variable(s) and covariate(s) in separate columns; each column must contain a descriptive header. An example of such data:

Name|Covariate_1|Covariate_2|Covariate_3
---|---|---|---
1|6.87|17.91|0.91
2|3.92|15.96|1.09
3|5.59|15.07|1.24
4|2.28|13.49|1.42
5|2.10|12.30|1.36
6|5.24|9.90|0.70
7|5.96|14.67|1.82
8|2.22|16.36|1.52

Please note that decimal symbols should match your local machine's regional settings.\
After importing data of experimental units, the user should define which data column contains the unique name or number of the experimental units.

### Defining block sizes
In the bottom-left section of the software, several tab pages are visible. In the first tab page named "Blocks", the user should indicate the number of blocks that the software has to create and define whether or not all block sizes are equal. If all block sizes are equal, the block sizes can directly be specified in the current tab page. Else, a new tab page will appear in which the individual block sizes can be set.

### Defining run parameters
Next, the desired number of unique block sets that RandoMice creates should be provided. RandoMice will show the total number of unique block sets in the upper right corner. The default setting is 8,000,000 unique block sets, which will typically be created in a matter of minutes. In addition, the user should indicate the number of best-balanced block sets to remember and whether newly created block sets should be tested for unicity.

Considerations:
* Increasing the desired number of unique block sets will increase the likelihood that a block set is created that is well-balanced for the provided variable(s) and covariate(s), at the cost of time. The top-right progress section displays the theoretical number of unique sets that can be created with currently defined block sizes. Pressing the button "max" and "99%" will set the desired number of unique block sets to create 100% or 99% of the theoretical maximum, respectively, with a maximum of 1 billion.
* By default, RandoMice will check newly created block sets for unicity to avoid creating duplicates. Unchecking this box will significantly increase RandoMice's speed. However, you risk identifying duplicate block sets.

When all abovementioned details are provided, the user may directly [start block set creation](#Starting-a-run). Alternatively, the user may (optionally) first define weights of variable(s) and covariate(s) and, if desired, define if subgroups should be created, see the following two sections.

### Defining weights of variable(s) and covariate(s)
In the second tab page named "Variables" in the bottom-left section of the software, the user may optionally define weights for each variable and covariate, which will reflect their relative importance when scoring a set of blocks. Weights are linear, with larger values representing greater "importance". In this tab page the user may also specify the number of digits of each variable: this number will not affect how RandoMice performs its calculations, but will determine how final results are rounded and shown.

### Defining subgroup sizes
In the third tab page named "Subgroups" in the bottom-left section of the software, the user may optionally instruct the software to divide the experimental units in each block into subgroups. After ticking the corresponding checkbox, the user must define which input data column contains (physical) markers. If "None" is selected, the software assumes that subgroups must be created randomly. Else, the software will divide experimental units of each block into subgroups based on the markers as defined in the data column in such way that there is as little overlap between markers within subgroups as possible. The user must also define the subgroup sizes; a default subgroup size may be set in the current tab page and subgroup sizes may be further specified on the appeared tab page named "Subgroup sizes".

### Starting a run
Block set creation can be started by pressing the "Run" button. The top-right progress section will display the time elapsed, the estimated time remaining and the number of created block sets. The run can always be interrupted by pressing the button "Abort". Alternatively, one may press the button "Show results" which will stop the run and display the current results.

### Evaluating results
Once finished, a list of best-balanced block sets and their ranks are displayed in the bottom-right section. If blocks were to be divided into subgroups based on a (physical) marker, the total number of overlapping markers will also be displayed here.

An extra section will become visible at the bottom, in which details of the selected block set can be viewed: the leftmost data grid displays the block composition; the middle data grid displays the subgroup composition; the rightmost data grid displays variable descriptives for each block.

### Randomly assigning blocks to groups
Finally, once the user has chosen his/her favorite block set, blocks may be randomly allocated to groups/interventions. This can be done by pressing the corresponding button; if group names have not yet been defined, the user will be prompted to do so.

### Saving and exporting results
Details of the resulting block sets may be exported into various file formats via *>File>Export...*
Alternatively, results may be saved to disk via *>File>Save as...* which will create a .rndm file that can be re-opened in the software later.

## Authors
Randomice was originally written by:
* R. van Eenige - [RvE54](https://github.com/RvE54)

The following people have contributed code/logics to RandoMice:
* P.J. Koemans
* S. Kooijman

The following people have contributed otherwise to RandoMice:
* P.S. Verhave
* I.A.C.W. Tiebosch
* S.T. Bate
* P.C.N. Rensen

## License
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version - see the [LICENSE](./LICENSE) file for details.