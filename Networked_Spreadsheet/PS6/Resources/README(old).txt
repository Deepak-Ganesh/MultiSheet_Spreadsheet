Project: PS6 (SpreadsheetGUI)
Author: Bridger Holt
Class: CS 3500
Completed: October 21, 2018


The additional feature I added was the ability to double-click a cell to alter its contents. This
is further explained in paragraph 4. I added quality-of-life adjustments as well. This includes
having the filename as the name of the window, with an asterisk if the spreadsheet has been
changed. I also added a "Save as" button for saving the spreadsheet to a different file.

This project is broken up into 4 different assemblies. The first two are GUI components for the VS
Forms environment. This includes SpreadsheetPanel and SelectedCellInput. The third is
SpreadsheetController which acts as an interface to the Spreadsheet class. The final assembly is
the actual Forms project, SpreadsheetGUI.

SpreadsheetPanel is the provided project containing a UserControl class for the A-Z by 1-99 cells.
I made some modifications to it, including adding more events and a mechanism for getting a cell's
(X, Y) position.

SelectedCellInput contains a TextBox class and is used for the feature I added that lets you
double-click on a cell to alter its contents inline where the cell is. This is easier than having
to click on the input bar at the top. It additionally lets the user keep their eyes on the cell
they want to change.

SpreadsheetController acts as the controller part of the MVC design. It interfaces between the
model (the Spreadsheet class) and the view (the SpreadsheetGUI project). SpreadsheetGUI and
SelectedCellInput both reference SpreadsheetController. It also contains methods for converting
between 0-index (col, row) positions to [A-Z][1-99] strings, which is why SelectedCellInput uses
it.

SpreadsheetGUI is the Forms assembly. It references the 3 other assemblies and can be executed.

The Resources folder that this file is contained in is used to reference the DependencyGraph,
Formula, and Spreadsheet DLL and XML files. SpreadsheetController makes the most use of it.
