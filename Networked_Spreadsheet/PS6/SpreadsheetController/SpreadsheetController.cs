/// Written by Bridger Holt for Fall 2018 CS 3500.
/// Not to be shared with anyone.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpreadsheetUtilities;
using System.Diagnostics;

namespace SS
{
    /// <summary>
    /// Used for whenever the underlying spreadsheet is altered.
    /// </summary>
    /// <param name="controller">The controller invoking it.</param>
    public delegate void SpreadsheetChangeHandler(SpreadsheetController controller);


    /// <summary>
    /// An abstraction of the core spreadsheet model. Represents the View part of the MVC design,
    /// where Spreadsheet is the model part. Interfaces between the 0-index (col, row) locations
    /// used by SpreadsheetPanel with the alphanumerical strings used by Spreadsheet.
    /// </summary>
    public class SpreadsheetController
    {
        // -- Public fields -- \\

        /// <summary>
        /// The underlying spreadsheet. Public access is used in some areas of the program.
        /// </summary>
        public Spreadsheet Spreadsheet { get; private set; }

        /// <summary>
        /// Is invoked whenever the underlying spreadsheet changes.
        /// </summary>
        public event SpreadsheetChangeHandler ChangedEvent;



        // -- Public methods -- \\

        /// <summary>
        /// Only constructor. Sets the underlying spreadsheet.
        /// </summary>
        /// <param name="ss">The underlying spreadsheet.</param>
        public SpreadsheetController(Spreadsheet ss)
        {
            Spreadsheet = ss;
        }


        /// <summary>
        /// Retrieves all the data of a single cell.
        /// </summary>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The cell of the cell.</param>
        /// <param name="cellName">The name of the cell.</param>
        /// <param name="cellContents">The contents of the cell.</param>
        /// <param name="cellValue">The value of the cell.</param>
        public void GetCellData(
            int col, int row,
            out string cellName,
            out string cellContents,
            out string cellValue)
        {
            cellName = ColRowToCellName(col, row);

            cellContents = GetCellContents(cellName);
            cellValue = GetCellValue(cellName);
        }


        /// <summary>
        /// Retrieves the cell's value using its name.
        /// Returns "ERROR" on FormulaError.
        /// </summary>
        /// <param name="cellName">The name of the cell.</param>
        /// <returns>The cell's value or "ERROR".</returns>
        public string GetCellValue(string cellName)
        {
            string cellValue;

            var value = Spreadsheet.GetCellValue(cellName);

            if (value is FormulaError)
            {
                cellValue = "ERROR";
            }
            else
            {
                cellValue = value.ToString();
            }

            return cellValue;
        }


        /// <summary>
        /// Returns the contents of a cell specified by a column and row.
        /// </summary>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The row of the cell.</param>
        /// <returns>The contents of the cell.</returns>
        public string GetCellContents(int col, int row)
        {
            var cellName = ColRowToCellName(col, row);

            return GetCellContents(cellName);
        }


        /// <summary>
        /// Returns the contents of the cell specified by a name.
        /// Puts '=' before a formula.
        /// </summary>
        /// <param name="cellName">The name of the cell.</param>
        /// <returns>The cell's contents.</returns>
        public string GetCellContents(string cellName)
        {
            var contents = Spreadsheet.GetCellContents(cellName);

            if (contents is Formula)
            {
                return "=" + contents;
            }
            else
            {
                return contents.ToString();
            }
        }


        /// <summary>
        /// Sets the contents of a cell.
        /// </summary>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The row of the cell.</param>
        /// <param name="contents">The new contents to set it to.</param>
        /// <returns>ISet{string} of cells to redraw if successful, otherwise an Exception.</returns>
        public object SetCellContents(int col, int row, string contents)
        {
            var cellName = ColRowToCellName(col, row);

            try
            {
                var result = Spreadsheet.SetContentsOfCell(cellName, contents);
                ChangedEvent?.Invoke(this);
                return result;
            }
            catch (Exception e)
            {
                return e;
            }
        }


        /// <summary>
        /// Converts (col, row) to an alphanumeric name. Both row and col are 0-indexed.
        /// </summary>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The row of the cell.</param>
        /// <returns></returns>
        public static string ColRowToCellName(int col, int row)
        {
            // This works since there are only 26 columns in the current setup.
            Debug.Assert(col >= 0 && col <= 26);
            char colName = (char)('A' + col);
            string cellName = colName + (row + 1).ToString();

            return cellName;
        }


        /// <summary>
        /// Assumes only one character for column. Assumes all caps.
        /// </summary>
        /// <param name="cellName">The name of the cell</param>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The row of the cell.</param>
        public static void CellNameToColRow(string cellName, out int col, out int row)
        {
            Debug.Assert(cellName.Length >= 2 && cellName.Length <= 3);

            char columnChar = cellName[0];
            string rowStr = cellName.Substring(1);

            Debug.Assert(columnChar >= 'A' && columnChar <= 'Z');
            col = columnChar - 'A';

            row = int.Parse(rowStr) - 1;
        }
    }
}
