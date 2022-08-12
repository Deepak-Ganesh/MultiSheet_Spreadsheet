/// Written by Bridger Holt for Fall 2018 CS 3500.
/// Not to be shared with anyone.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using SS;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        public delegate void SpreadsheetClosingHandler();
        public SpreadsheetClosingHandler spreadsheetClosing;

        public delegate void RequestNewSSHandler();
        public RequestNewSSHandler requestNewSS;

        // -- Private fields -- \\

        private ClientController clientController;


        private MethodInvoker closeForm;
        private MethodInvoker hideForm;

        private object locker = new object();
        bool canEdit;
        string editedCell;

        /// <summary>
        /// The interface to the spreadsheet.
        /// </summary>
        private SpreadsheetController ssController;

        /// <summary>
        /// The name of the current file.
        /// Begins as unnamed spreadsheet.
        /// </summary>
        private string name;

        /// <summary>
        /// The version for saving and loading the XML.
        /// </summary>
        private static readonly string ssVersion = "ps6";


        /// <summary>
        /// Convenience property for getting the underlying spreadsheet.
        /// </summary>
        private Spreadsheet spreadsheet { get { return ssController.Spreadsheet; } }



        // -- Public methods -- \\

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Form1() : this(CreateSpreadsheet(), "<unnamed spreadsheet>")
        {
        }


        /// <summary>
        /// Loads a spreadsheet from server-given updates.
        ///  Note that we treat the spreadsheet like a new, unsaved spreadsheet here.
        ///  Lots of the saving spreadsheet code is not touched.
        /// </summary>
        public Form1(ClientController ctl, FullSendMessage FSM, string sheetName) : this(CreateSpreadsheet(), sheetName)
        {
            canEdit = true;

            closeForm = new MethodInvoker(Close);
            hideForm = new MethodInvoker(Hide);

            // register the needed spreadsheet events to update the gui
            clientController = ctl;
            clientController.RegisterSheetHandlers(UpdateHandler, ErrorHandler);

            // update the gui with the initial update message the server gave the
            //  connection gui
            UpdateHandler(FSM);
        }

        public void RegisterContainerHandlers(SpreadsheetClosingHandler csh, RequestNewSSHandler rss)
        {
            spreadsheetClosing += csh;
            requestNewSS += rss;
        }

        // -- Protected methods -- \\

        /// <summary>
        /// Occurs when the window is closed. Warns of unsaved changes.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            spreadsheetClosing?.Invoke();
            base.OnClosed(e);
        }



        // -- Private methods -- \\

        /// <summary>
        /// Constructs using a pre-constructed Spreadsheet and a file name.
        /// </summary>
        /// <param name="ss">The pre-constructed Spreadsheet.</param>
        /// <param name="name">The name to set to the window.</param>
        private Form1(Spreadsheet ss, string name)
        {
            InitializeComponent();

            ssController = new SpreadsheetController(ss);

            selectedCellInput.Initialize(
                ssController, textBoxCellInput, spreadsheetPanelPrimary);

            selectedCellInput.InputFinished += OnSelectedCellInputFinished;

            spreadsheetPanelPrimary.SelectionChanged += OnSelectionChanged;

            OnSelectionChanged(spreadsheetPanelPrimary);

            UpdateAllCellValues();

            this.name = name;
        }

        private void UpdateHandler(FullSendMessage FSM)
        {
            // list of edits that needs to be made to the spreadsheet
            Dictionary<string, dynamic> edits = FSM.MockSheet;

            // no error checking--everything should have been cleaned in the server
            foreach (KeyValuePair<string, dynamic> edit in edits)
            {
                string cellName = edit.Key;

                if (cellName == editedCell)
                    canEdit = true;

                // treat this as a string, let the spreadsheet handle it
                string value = edit.Value as string;

                // update the spreadsheet per edit received

                // get the column and row from the cellname
                // convert the ascii a-z char back to an int
                int col = char.ToUpper(cellName[0]) - 65;
                int row = int.Parse(cellName.Substring(1)) - 1;

                SetInputToCell(col, row, value);

                // update every relevant cell
                OnSelectionChangedNoFocus(spreadsheetPanelPrimary);
            }

            if (canEdit)
                UnlockButtons();
        }

        private void ErrorHandler(ErrorMessage EM)
        {
            Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("Server gave a circular dependency error", "Circular dependency",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            });

            canEdit = true;
            UnlockButtons();
        }

        /// <summary>
        /// Serves as the normalizer for the Spreadsheet.
        /// </summary>
        /// <param name="s">The cell's un-normalized name.</param>
        /// <returns>The cell's normalized name.</returns>
        private static string NormalizeCellName(string s)
        {
            return s.ToUpper();
        }


        /// <summary>
        /// Serves as the verifier for the Spreadsheet.
        /// </summary>
        /// <param name="s">The normalized cell name.</param>
        /// <returns>Whether or not it's a valid cell name.</returns>
        private static bool VerifyCellName(string s)
        {
            if (s.Length < 2 || s.Length > 3)
                return false;

            char firstChar = s[0];

            if (firstChar < 'A' || firstChar > 'Z')
                return false;

            if (int.TryParse(s.Substring(1), out int num))
            {
                if (num >= 1 && num <= 99)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Convenience method for constructing a Spreadsheet.
        /// Uses the VeriftCellName and NormalizeCellName delegates.
        /// </summary>
        /// <returns>The constructed Spreadsheet.</returns>
        private static Spreadsheet CreateSpreadsheet()
        {
            return new Spreadsheet(VerifyCellName, NormalizeCellName, ssVersion);
        }


        /// <summary>
        /// Convenience method for loading a file into a Spreadsheet.
        /// </summary>
        /// <param name="fileName">The name of the XML file.</param>
        /// <returns>The constructed Spreadsheet.</returns>
        private static Spreadsheet CreateSpreadsheet(string fileName)
        {
            return new Spreadsheet(fileName, VerifyCellName, NormalizeCellName, ssVersion);
        }

        /// <summary>
        /// Updates all cells.
        /// </summary>
        private void UpdateAllCellValues()
        {
            foreach (var name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                UpdateCell(name);
            }
        }


        /// <summary>
        /// Updates a specific cell, meaning it retrieves its current value.
        /// </summary>
        /// <param name="cellName">The name of the cell being updated.</param>
        private void UpdateCell(string cellName)
        {
            int col, row;
            SpreadsheetController.CellNameToColRow(cellName, out col, out row);

            string value = ssController.GetCellValue(cellName);
            spreadsheetPanelPrimary.SetValue(col, row, value);
        }


        /// <summary>
        /// Called by the SelectedCellInput object.
        /// </summary>
        /// <param name="cellInput">The SelectedCellInput object calling it.</param>
        /// <param name="text">The SelectedCellInput's current text.</param>
        private void OnSelectedCellInputFinished(SelectedCellInput cellInput, string text)
        {
        }


        /// <summary>
        /// Updates the text boxes and focuses on the panel.
        /// </summary>
        private void OnSelectionChanged(SpreadsheetPanel ssPanel)
        {
            OnSelectionChangedNoFocus(ssPanel);

            if (ssPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    ssPanel.Focus();
                });
            }
            else
            {
                ssPanel.Focus();
            }
        }

        private void OnSelectionChangedNoFocus(SpreadsheetPanel ssPanel)
        {
            int col, row;
            ssPanel.GetSelection(out col, out row);

            string cellName, cellContents, cellValue;
            lock (locker)
            {
                ssController.GetCellData(col, row,
                    out cellName, out cellContents, out cellValue);
            }

            if (textBoxCellValue.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    textBoxCellName.Text = cellName;
                    textBoxCellValue.Text = cellValue.ToString();
                    textBoxCellInput.Text = cellContents.ToString();
                });
            }
            else
            {
                textBoxCellName.Text = cellName;
                textBoxCellValue.Text = cellValue.ToString();
                textBoxCellInput.Text = cellContents.ToString();
            }
        }


        /// <summary>
        /// Saves the current input to a specific cell.
        /// Displays a message if there is an exception.
        /// </summary>
        /// <param name="col">The column of the cell.</param>
        /// <param name="row">The row of the cell.</param>
        /// <param name="text">The input's text.</param>
        private bool SetInputToCell(int col, int row, string text)
        {
            // implicit types need to be initialized, so initialize it with a stub
            var result = new object();

            // get what result actually should be in a locked function call
            lock (locker)
            {
                result = ssController.SetCellContents(col, row, text);
            }

            // use result like normal
            if (result is ISet<string>)
            {
                var resultSet = (ISet<string>)result;
                foreach (var s in resultSet)
                {
                    UpdateCell(s);
                }
            }

            else
            {
                var resultException = (Exception)result;
                MessageBox.Show(resultException.Message, "Error setting cell contents",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                canEdit = true;
                UnlockButtons();

                return false;
            }

            return true;
        }


        private void SetInputAndSendEdit(int col, int row, string text)
        {
            // column is the letter, so convert it to an ascii a-z char
            char column = (char)((true ? 65 : 97) + (col));
            string cellName = column + (row + 1).ToString();

            editedCell = cellName;
            canEdit = false;
            LockButtons();

            bool setCellWorked = SetInputToCell(col, row, text);

            if (setCellWorked)
            {
                List<string> dependees = new List<string>();
                if (text.Length > 0)
                {
                    if (text[0] == '=')
                    {
                        dependees = new List<string>(GetTokens(text.ToUpper()));
                    }
                }
                clientController.SendData(new EditMessage(cellName, text, dependees).ToString());
            }

            // Clear any focus.
            if (buttonEnter.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    buttonEnter.Focus();
                });
            }
            else
            {
                buttonEnter.Focus();
            }

        }


        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (!canEdit)
                return;
            if (textBoxCellInput.Text.Length > 255)
            {
                MessageBox.Show("Input greater than 255 characters. Please trim your input.",
                                "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int col, row;
            spreadsheetPanelPrimary.GetSelection(out col, out row);

            bool inputBarFocused = textBoxCellInput.Focused;

            char column = (char)((true ? 65 : 97) + (col));
            string cellName = column + (row + 1).ToString();

            SetInputAndSendEdit(col, row, textBoxCellInput.Text);

            OnSelectionChanged(spreadsheetPanelPrimary);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OnClosed(e);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            requestNewSS?.Invoke();
        }

        private void basicHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string caption = "Basic Help";
            string message = "Left click on a cell to select it. You can then enter its " +
                "contents into the input bar.\n\n" +
                "You can press enter to save the contents.\n\n" +
                "Additionally, instead of using the input bar, you can double-click the " +
                "cell and an input box will appear. Type your input into there and type enter to save it.\n\n" +
                "Begin a cell with '=' to make a formula which can reference other cells.\n\n" +
                "The top of the window will show the file name, with an asterisk if you have unsaved changes. " +
                "Use Revert to undo the most recent change to a specific cell. \n\n" +
                "Use Undo to undo the most recent change to the entire spreadsheet. \n\n";

            MessageBoxButtons buttons = MessageBoxButtons.OK;

            MessageBox.Show(message, caption, buttons);
        }

        /// <summary>
        /// oops didnt mean to make this but might as well keep it here
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Revert_Button_Click(object sender, EventArgs e)
        {
            if (!canEdit)
                return;

            //get the cellname
            int col, row;
            string value;
            spreadsheetPanelPrimary.GetSelection(out col, out row);
            spreadsheetPanelPrimary.GetValue(col, row, out value);
            if (value == "")
                return;

            char column = (char)((true ? 65 : 97) + (col));
            string cellName = column + (row + 1).ToString();

            //send the revert message with the specific cellname
            clientController.SendData(new RevertMessage(cellName).ToString());

            editedCell = cellName;
            canEdit = false;
            LockButtons();
        }

        private void Undo_Button_Click(object sender, EventArgs e)
        {
            if (!canEdit)
                return;

            //just send undo to the server
            clientController.SendData(new UndoMessage().ToString());
        }

        private void UnlockButtons()
        {
            if (Revert_Button.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Revert_Button.Enabled = true;
                    Undo_Button.Enabled = true;
                    buttonEnter.Enabled = true;
                });
            }
            else
            {
                Revert_Button.Enabled = true;
                Undo_Button.Enabled = true;
                buttonEnter.Enabled = true;
            }
        }

        private void LockButtons()
        {
            Invoke((MethodInvoker)delegate
            {
                Revert_Button.Enabled = false;
                Undo_Button.Enabled = false;
                buttonEnter.Enabled = false;
            });
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    if (VerifyCellName(s))
                        yield return NormalizeCellName(s);
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(clientController.IsConnected().ToString());
        }

        /// <summary>
        /// A helper method to figure out whether a given token is a "legal" number or not.
        ///  A legal number starts with a digit, and may only contain a '.', or an 'e.'
        /// </summary>
        /// <param name="token">The token to be found as a number or not.</param>
        /// <returns>Returns whether the token was a legal number or not.</returns>
        private static bool IsLegalNumber(string token)
        {
            bool ret = true;
            double dubTest;
            if (!Double.TryParse(token.ToString(), out dubTest))
                ret = false;
            return ret;
        }

        /// <summary>
        /// A helper method to verify whether a character is an alphabetical letter---or an underscore---or not.
        /// </summary>
        /// <param name="c">The char to be checked.</param>
        /// <returns>Returns whether the char is a letter or not.</returns>
        private static bool isLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z' || c == '_');
        }
    }
}