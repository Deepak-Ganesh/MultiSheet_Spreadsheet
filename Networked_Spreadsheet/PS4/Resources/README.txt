Project by Bridger Holt for CS 3500 Fall 2018. Initially complete October 5.

The implementation of Spreadsheet is pretty straightforward. It contains two fields,
a DependencyGraph from PS2 and a Dictionary<string, Cell>. Cell is a simple type with
two string fields: Contents and Value. Contents is the behind-the-scenes data that the
user actually typed in. Value is what should be displayed.

One method of Spreadsheet that is used a lot is SetOrCreateCell. It either sets the
contents of a cell in the dictionary or creates a new one. Additionally, if the previous
contents was a Formula, it removes all of that cell's dependents in the DependencyGraph.
If the new contents is a Formula, then the Formula overload of SetCellContents will add
in dependents based on the variables in the Formula.

The Resources project contains DLLs of PS2 and PS3 of commit 9ae2410.
This includes DependencyGraph and Formula, which are used in Spreadsheet.

There are two sets of tests: GradingTests and CoreTests. GradingTests are the grading tests from
PS4 adapted for the new spec. CoreTests are my tests from PS4, adapted for the new spec, with a
lot of additions.