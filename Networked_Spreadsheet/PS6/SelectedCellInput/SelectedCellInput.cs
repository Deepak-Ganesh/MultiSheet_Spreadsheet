/// Written by Bridger Holt for Fall 2018 CS 3500.
/// Not to be shared with anyone.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SS;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Meant to be executed when a textbox's input has been finished, meaning the user pressed
    /// enter or clicked elsewhere.
    /// Used by SelectedCellInput's InputFinished event.
    /// </summary>
    /// <param name="cellInput">The object calling the method.</param>
    /// <param name="text">The text in the box at that moment.</param>
    public delegate void InputFinishHandler(SelectedCellInput cellInput, string text);

    public partial class SelectedCellInput : TextBox
    {
        // -- Public fields -- \\

        /// <summary>
        /// Gets invoked when the user's focus leaves this object.
        /// </summary>
        public event InputFinishHandler InputFinished;

        /// <summary>
        /// The column of the cell that the user is modifying.
        /// </summary>
        public int col { get; private set; }

        /// <summary>
        /// The row of the cell that the user is modifying.
        /// </summary>
        public int row { get; private set; }



        // -- Private fields -- \\

        /// <summary>
        /// The controller used by the SpreadsheetGUI Form.
        /// </summary>
        private SpreadsheetController ssController;

        /// <summary>
        /// The input bar at the top of the screen.
        /// </summary>
        private TextBox inputBar;


        
        // -- Public methods -- \\

        /// <summary>
        /// Default constructor. Begins disabled.
        /// </summary>
        public SelectedCellInput()
        {
            Disable();
        }


        /// <summary>
        /// Must be called by the Form. Sets up references.
        /// </summary>
        /// <param name="ssController">The controller used by the SpreadsheetGUI Form.</param>
        /// <param name="inputBar">The input bar at the top of the screen.</param>
        /// <param name="ssPanel">The SpreadsheetPanel, used for setting up events.</param>
        public void Initialize(
            SpreadsheetController ssController,
            TextBox inputBar,
            SpreadsheetPanel ssPanel)
        {
            this.inputBar = inputBar;
            this.ssController = ssController;

            ssPanel.Scrolled += OnScroll;
            ssPanel.CellDoubleClicked += OnSelectionChanged;
        }



        // -- Protected methods -- \\

        /// <summary>
        /// Changes the input bar's text to match this box's.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (Enabled && Focused)
            {
                inputBar.Text = Text;
            }
        }


        /// <summary>
        /// Goes invisible if disabled.
        /// </summary>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!Enabled)
            {
                Visible = false;
                Text = "";
            }
        }


        /// <summary>
        /// Invokes the InputFinished event and disables the box.
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            InputFinished?.Invoke(this, Text);

            Disable();
        }



        // -- Private methods -- \\

        /// <summary>
        /// Abstraction from the Forms API. Doesn't current serve a purpose.
        /// </summary>
        private void Disable()
        {
            Enabled = false;
        }


        /// <summary>
        /// Bound to the SpreadsheetPanel Scrolled event. Updates the position of the textbox.
        /// Disables if out of bounds.
        /// </summary>
        /// <param name="ssPanel">The panel that was scrolled.</param>
        private void OnScroll(SpreadsheetPanel ssPanel)
        {
            if (Enabled)
            {
                var inBounds = ssPanel.GetCellTopLeft(
                    col, row, out Point pos);

                if (inBounds)
                {
                    Location = pos;
                }
                else
                {
                    Disable();
                }
            }
        }


        /// <summary>
        /// Bound to the SpreadsheetPanel SelectionChanged event.
        /// </summary>
        /// <param name="ssPanel">The panel whose selection changed.</param>
        /// <param name="col">The new selection's column.</param>
        /// <param name="row">The new selection's row.</param>
        private void OnSelectionChanged(SpreadsheetPanel ssPanel, int col, int row)
        {
            var inBounds = ssPanel.GetCellTopLeft(col, row, out Point pos);

            if (inBounds)
            {
                this.col = col;
                this.row = row;

                Location = pos;
                Visible = true;
                Enabled = true;
                Text = ssController.GetCellContents(col, row);
                Focus();
            }
        }
    }
}
