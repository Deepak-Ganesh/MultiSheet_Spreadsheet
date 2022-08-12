/// Written by Bridger Holt for Fall 2018 CS 3500.
/// Not to be shared with anyone.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using SpreadsheetUtilities;

/// <summary>
/// Stands for Spreadsheet. Contains the abstract type AbstractSpreadsheet and the
/// class Spreadsheet. Also contains various exception types.
/// </summary>
namespace SS
{
    /// <summary>
    /// A working implementation of AbstractSpreadsheet.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }


        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        /// <param name="isValid">Method for validating a variable.</param>
        /// <param name="normalize">Method for normalizing a variable.</param>
        /// <param name="version">The spreadsheet version.</param>
        public Spreadsheet(
            Func<string, bool> isValid,
            Func<string, string> normalize,
            string version) :
                base(isValid, normalize, version)
        {
            Changed = false;

            dependencyGraph = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
        }


        /// <summary>
        /// Loads an XML file describing a spreadsheet, setting all the cells.
        /// </summary>
        /// <param name="filename">The file name with path included.</param>
        /// <param name="isValid">Method for validating a variable.</param>
        /// <param name="normalize">Method for normalizing a variable.</param>
        /// <param name="version">The required spreadsheet version.</param>
        public Spreadsheet(
            string filename,
            Func<string, bool> isValid,
            Func<string, string> normalize,
            string version) :
                this(isValid, normalize, version)
        {
            try
            {
                // There is likely whitespace between the <?xml> node and <spreadsheet> node.
                // There's no reason currently to ignore comments but maybe in the future.
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                using (var reader = XmlReader.Create(filename, settings))
                {
                    var actualVersion = ReadToVersion(reader); // <spreadsheet>

                    if (actualVersion != version)
                    {
                        throw new SpreadsheetReadWriteException(
                            "Invalid version, was expecting \"" + version + '"');
                    }
                    
                    // Look for each element.
                    while (reader.Read() && reader.Name == "cell")  // <cell>
                    {
                        VerifyNextNodeName(reader, "name");  // <name>

                        reader.Read();  // name
                        string name = reader.Value;


                        VerifyNextNodeName(reader, "name");  // </name>
                        VerifyNextNodeName(reader, "contents");  // <contents>

                        reader.Read();  // contents
                        string contents = reader.Value;


                        VerifyNextNodeName(reader, "contents");  // </contents>
                        VerifyNextNodeName(reader, "cell");  // </cell>
                        
                        SetContentsOfCell(name, contents);
                    }

                    VerifyNodeName(reader, "spreadsheet");
                }

                Changed = false;
            }
            
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(
                    "Error reading \"" + filename + "\": " + e);
            }
        }


        /// <summary>
        /// The default constructor.
        /// isValid always returns true, normalize does not change the string,
        /// version is "default".
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {

        }


        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">The file name with path included.</param>
        /// <returns>The version expressed in the file.</returns>
        public override string GetSavedVersion(String filename)
        {
            try
            {
                // There is likely whitespace between the <?xml> node and <spreadsheet> node.
                // There's no reason currently to ignore comments but maybe in the future.
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                using (var reader = XmlReader.Create(filename, settings))
                {
                    return ReadToVersion(reader);
                }
            }

            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(
                    "Error reading \"" + filename + "\": " + e);
            }
        }


        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">The file name with path included.</param>
        public override void Save(String filename)
        {
            try
            {
                // Bsic indentation of 2 chars, with newlines.
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    // <spreadsheet version=Version>
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    // <cell>
                    //   <name>cell.Key</name>
                    //   <contents>cell.Value.Contents</contents>
                    // </cell> ...
                    foreach (var cell in cells)
                    {
                        cell.WriteXml(writer);
                    }

                    // </spreadsheet>
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                // This is the only place where it get reset to false.
                Changed = false;
            }

            // Likely a writing error.
            catch
            {
                throw new SpreadsheetReadWriteException(
                    "Failed to open and write to the file \"" + filename + '"');
            }
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(String name)
        {
            if (TryGetCell(name, out Cell cell))
                return cell.Value;
            else
                return "";
        }


        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// Copies each key in the internal Dictionary.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            var result = new List<String>(cells.Keys);
            return result;
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// Cells by default (i.e. not previously set) are an empty string.
        /// </summary>
        public override object GetCellContents(String name)
        {
            if (TryGetCell(name, out Cell cell))
                return cell.Contents;
            else
                return "";
        }


        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents are being set.</param>
        /// <param name="content">The written contents to set the cell to.</param>
        /// <returns>All recalculated cells.</returns>
        public override ISet<String> SetContentsOfCell(String name, String content)
        {
            VerifyArgumentNotNull(content, "content");
            name = VerifyAndNormalizeName(name);

            ISet<String> result;

            if (double.TryParse(content, out double value))
            {
                result = SetCellContents(name, value);
            }
            else if (content.Length > 0 && content[0] == '=')
            {
                var formula = new Formula(content.Substring(1), Normalize, IsValid);
                result = SetCellContents(name, formula);
            }
            else
            {
                result = SetCellContents(name, content);
            }

            return result;
        }



        // -- Protected -- \\

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents are being set.</param>
        /// <param name="number">The number to set the cell to.</param>
        /// <returns>All recalculated cells.</returns>
        protected override ISet<String> SetCellContents(String name, double number)
        {
            VerifyValidCellName(name);
            
            SetOrCreateCell(name, number);
            cells[name].Value = number;

            Changed = true;

            return RecalculateCells(name);
        }


        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents are being set.</param>
        /// <param name="text">The text to set the cell to.</param>
        /// <returns>All recalculated cells.</returns>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            VerifyArgumentNotNull(text, "text");
            VerifyValidCellName(name);
            
            SetOrCreateCell(name, text);

            if (text != "")
                cells[name].Value = text;

            Changed = true;

            return RecalculateCellsNoRoot(name);
        }


        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents are being set.</param>
        /// <param name="formula">The Formula to set the cell to.</param>
        /// <returns>All recalculated cells.</returns>
        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            VerifyArgumentNotNull(formula, "formula");
            VerifyValidCellName(name);

            // Shouldn't have any duplicates.
            var formulaVarList = formula.GetVariables();

            VerifyNoCircularDependencies(name, formulaVarList);

            object previousValue = GetCellContents(name);

            // Begin altering the Spreadsheet once it's verified not to be circular.
            SetOrCreateCell(name, formula);

            // This must happen after SetOrCreateCell because that method removes dependents.
            foreach (var s in formulaVarList)
            {
                dependencyGraph.AddDependency(name, s);
            }


            // Rewind in case of CircularException.
            try
            {
                var recalculate = RecalculateCells(name);

                // Only reaches if no CircularException.
                Changed = true;
                return recalculate;
            }
            catch (CircularException e)
            {
                SetOrCreateCell(name, previousValue);

                throw e;
            }
        }


        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name">The name of the cell whose dependents are being returned.</param>
        /// <returns>The cell's direct dependents..</returns>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            VerifyArgumentNotNull(name, "name");
            VerifyValidCellName(name);

            return dependencyGraph.GetDependees(name);
        }

        /// <summary>
        /// Getter for DependencyGraph object added by Lisa Richardson 4/18/19
        /// </summary>
        /// <returns></returns>
        public DependencyGraph GetDependencyGraph()
        {
            return dependencyGraph;
        }


        // -- Private -- \\

        /// <summary>
        /// Verifies cellName is a valid name, normalizes it, and validates it.
        /// </summary>
        /// <param name="cellName">The supposed name of the cell.</param>
        /// <returns>The normalized version of cellName.</returns>
        public string VerifyAndNormalizeName(string cellName)
        {
            VerifyValidCellName(cellName);

            cellName = Normalize(cellName);

            if (!IsValid(cellName))
                throw new InvalidNameException();

            return cellName;
        }


        /// <summary>
        /// Throws InvalidNameException if <paramref name="cellName"/> is null or invalid.
        /// </summary>
        /// <param name="cellName">The name of the cell passed in.</param>
        /// <exception cref="InvalidNameException">Thrown if <paramref name="cellName"/> is null or invalid.</exception>
        private static void VerifyValidCellName(string cellName)
        {
            if (!IsValidCellName(cellName))
            {
                throw new InvalidNameException();
            }
        }


        /// <summary>
        /// Returns true if <paramref name="cellName"/> is not null and is valid.
        /// </summary>
        /// <param name="cellName">The name of the cell passed in.</param>
        /// <returns>Whether <paramref name="cellName"/> is not null and is valid.</returns>
        private static bool IsValidCellName(string cellName)
        {
            if (cellName == null)
                return false;

            return Regex.IsMatch(cellName, @"^[a-zA-Z_]+[\w]*$");
        }


        /// <summary>
        /// Checks if the cell is within the cells dictionary, returning true if it is.
        /// The name argument is normalized and verified and validated.
        /// </summary>
        /// <param name="name">The name of the cell being looked up.</param>
        /// <param name="cell">The cell in the dictionary, if one exists.</param>
        /// <returns>Whether or not the cell exists.</returns>
        private bool TryGetCell(string name, out Cell cell)
        {
            name = VerifyAndNormalizeName(name);

            return cells.TryGetValue(name, out cell);
        }


        /// <summary>
        /// If arg is null, throws ArgumentNullException using argName in the message.
        /// </summary>
        /// <param name="arg">The actual argument object.</param>
        /// <param name="argName">The name of the argument for use in the message.</param>
        private static void VerifyArgumentNotNull(object arg, string argName)
        {
            if (arg is null)
                throw new ArgumentNullException("The argument \"" + argName + "\" is null");
        }
        

        /// <summary>
        /// Either sets the contents of an existent cell or creates one.
        /// If the cell was a Formula (and/or still is), removes its dependents.
        /// </summary>
        /// <param name="cellName">The name of the cell being altered or created.</param>
        /// <param name="cellContents">The new contents for the cell.</param>
        /// <returns>The cell within the dictionary.</returns>
        private Cell SetOrCreateCell(string cellName, object cellContents)
        {
            bool isEmptyString = false;
            if (cellContents is string)
                isEmptyString = (string)cellContents == "";

            if (cells.TryGetValue(cellName, out Cell cell))
            {
                if (cell.Contents is Formula)
                {
                    // Clears the dependents.
                    dependencyGraph.ReplaceDependents(cellName, new List<string>());
                }

                // Remove the cell if the new contents are "".
                if (isEmptyString)
                {
                    cells.Remove(cellName);
                }
                else
                {
                    cell.Contents = cellContents;
                }
            }

            // Create the cell and add to the dictionary.
            else
            {
                if (!isEmptyString)
                {
                    cell = new Cell(cellContents);
                    cells.Add(cellName, cell);
                }
            }

            return cell;
        }


        /// <summary>
        /// A form of RecalculateCells(string) where the cell indicated by changedCellName is
        /// not recalculated.
        /// Used when the cell's contents are set to a string.
        /// </summary>
        /// <param name="changedCellName">The cell that is assumed to have changed.</param>
        /// <returns>The unordered set of all cells returned by GetCellsToRecalculate.
        /// This includes changedCellName.</returns>
        private ISet<string> RecalculateCellsNoRoot(string changedCellName)
        {
            var toRecalculate = GetCellsToRecalculate(changedCellName);

            var toRecalculateList = new List<string>(toRecalculate);

            toRecalculateList.Remove(changedCellName);

            RecalculateCells(toRecalculateList);

            return new HashSet<string>(toRecalculate);
        }


        /// <summary>
        /// Convenience method. Calls RecalculateCells(IEnumerable) by passing in the result
        /// of a call to GetCellsToRecalculate using changedCellName.
        /// The cell indicated by changedCellName is recalculated.
        /// </summary>
        /// <param name="changedCellName">The cell that is assumed to have changed.</param>
        /// <returns>The unordered set of all cells that were recalculated.</returns>
        private ISet<string> RecalculateCells(string changedCellName)
        {
            var toRecalculate = GetCellsToRecalculate(changedCellName);

            RecalculateCells(toRecalculate);

            return new HashSet<string>(toRecalculate);
        }


        /// <summary>
        /// Loops through an enumerable in order, calling Evaluate on each Formula.
        /// Stores the result of Evaluate in the cell's value field.
        /// </summary>
        /// <param name="cellList">An ordered list of cell names.</param>
        private void RecalculateCells(IEnumerable<string> cellList)
        {
            foreach (var cellName in cellList)
            {
                var cell = cells[cellName];
                var contents = cell.Contents;

                if (contents is Formula)
                {
                    cell.Value = ((Formula)contents).Evaluate(Lookup);
                }
            }
        }
        

        /// <summary>
        /// Calls Read twice. Verifies that the second node is spreadsheet and has a version
        /// attribute and returns it. Leaves the reader on the spreadsheet node.
        /// </summary>
        /// <param name="reader">The XML document containing a spreadsheet's data.</param>
        /// <returns>The version expressed in the XML document.</returns>
        private static string ReadToVersion(XmlReader reader)
        {
            // <?xml> node
            reader.Read();

            // <spreadsheet> node
            reader.Read();

            if (reader.Name == "spreadsheet")
            {
                var version = reader.GetAttribute("version");
                if (version != null)
                {
                    return version;
                }

                // Version attribute doesn't exist.
                else
                {
                    throw new SpreadsheetReadWriteException(
                        "No version attribute of root node");
                }
            }

            // Root node must be spreadsheet.
            else
            {
                throw new SpreadsheetReadWriteException("Improper root name");
            }
        }


        /// <summary>
        /// Calls Read on reader once. Verifies the node it advanced to has the appropriate name.
        /// </summary>
        /// <param name="reader">The XML document containing a spreadsheet's data.</param>
        /// <param name="expectedName">The name the next node must have.</param>
        /// <exception cref="SpreadsheetReadWriteException">Thrown if the next node's name is
        /// not expectedName.</exception>
        private static void VerifyNextNodeName(XmlReader reader, string expectedName)
        {
            reader.Read();
            VerifyNodeName(reader, expectedName);
        }


        /// <summary>
        /// Verifies the current node of reader is named expectedName.
        /// </summary>
        /// <param name="reader">The XML document containing a spreadsheet's data.</param>
        /// <param name="expectedName">The name the next node must have.</param>
        /// <exception cref="SpreadsheetReadWriteException">Thrown if the current node's name is
        /// not expectedName.</exception>
        private static void VerifyNodeName(XmlReader reader, string expectedName)
        {
            if (reader.Name != expectedName)
            {
                throw new SpreadsheetReadWriteException(
                    "Was expecting a node named \"" + expectedName +
                    "\", got \"" + reader.Name + '"');
            }
        }
        

        /// <summary>
        /// Throws an exception if there is a circular dependency within the cell
        /// being added.
        /// </summary>
        /// <param name="cellName">The name of the cell being added.</param>
        /// <param name="formulaVarList">All the variables in the cell's formula.</param>
        private void VerifyNoCircularDependencies(
            string cellName, IEnumerable<String> formulaVarList)
        {
            var formulaVars = new HashSet<String>(formulaVarList);
            var dependees = GetCellsToRecalculate(formulaVars);
        }
        

        /// <summary>
        /// Satisfies the delegate required by Formula.Evaluate.
        /// Returns the value of the cell indicated by name, throwing an exception if it does not
        /// exist or is not a number.
        /// </summary>
        /// <param name="name">The name of the cell being looked up.</param>
        /// <returns>The value of the cell indicated by name.</returns>
        /// <exception cref="ArgumentException">Thrown if the name does not exist or is not
        /// a number.</exception>
        private double Lookup(string name)
        {
            if (cells.TryGetValue(name, out Cell cell))
            {
                var value = cell.Value;

                if (value is double)
                {
                    return (double)value;
                }
                else
                {
                    throw new ArgumentException(
                        "The cell named \"" + name + "\" does not have a numeric value");
                }
            }
            else
            {
                throw new ArgumentException("There is no set cell named \"" + name + '"');
            }
        }



        /// <summary>
        /// The contents-value pairs for each non-empty cell.
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// The dependency graph of all non-empty cells.
        /// </summary>
        private DependencyGraph dependencyGraph;
    }
    


    // -- Internal -- \\

    /// <summary>
    /// Contains the non-dependency information about a cell.
    /// This consists of its contents and its value.
    /// Both fields are public properties because Cell should never escape.
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// A string, double, or Formula.
        /// </summary>
        public object Contents { get; set; }

        /// <summary>
        /// A string, double, or FormulaError.
        /// </summary>
        public object Value { get; set; }



        /// <summary>
        /// Creates a cell with the contents set.
        /// Value never gets set immediately.
        /// </summary>
        /// <param name="contents"></param>
        public Cell(object contents)
        {
            Contents = contents;
        }
    }


    /// <summary>
    /// Contains an extension method for <see cref="KeyValuePair{string, Cell}"/>.
    /// </summary>
    internal static class PairExtensions
    {
        /// <summary>
        /// Writes XML data to writer for a single cell (string name and Cell).
        /// </summary>
        /// <param name="pair">The KeyValuePair within a dictionary where the string is the
        /// cell's name and the Cell is the cell's data.</param>
        /// <param name="writer">The XmlWriter for the document being saved.</param>
        public static void WriteXml(
            this KeyValuePair<string, Cell> pair, XmlWriter writer)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("name", pair.Key);

            var cellContents = pair.Value.Contents;
            string cellContentsString;

            if (cellContents is Formula)
            {
                cellContentsString = "=" + (Formula)cellContents;
            }
            else
            {
                cellContentsString = cellContents.ToString();
            }

            writer.WriteElementString("contents", cellContentsString);
            writer.WriteEndElement();
        }
    }
}
