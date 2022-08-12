namespace SpreadsheetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxCellInput = new System.Windows.Forms.TextBox();
            this.buttonEnter = new System.Windows.Forms.Button();
            this.textBoxCellName = new System.Windows.Forms.TextBox();
            this.textBoxCellValue = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.labelCell = new System.Windows.Forms.Label();
            this.labelCellIs = new System.Windows.Forms.Label();
            this.spreadsheetPanelPrimary = new SS.SpreadsheetPanel();
            this.selectedCellInput = new SpreadsheetGUI.SelectedCellInput();
            this.Revert_Button = new System.Windows.Forms.Button();
            this.Undo_Button = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxCellInput
            // 
            this.textBoxCellInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCellInput.Location = new System.Drawing.Point(8, 83);
            this.textBoxCellInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCellInput.Name = "textBoxCellInput";
            this.textBoxCellInput.Size = new System.Drawing.Size(1090, 26);
            this.textBoxCellInput.TabIndex = 1;
            // 
            // buttonEnter
            // 
            this.buttonEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnter.Location = new System.Drawing.Point(1108, 80);
            this.buttonEnter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(80, 35);
            this.buttonEnter.TabIndex = 2;
            this.buttonEnter.Text = "Enter";
            this.buttonEnter.UseVisualStyleBackColor = true;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            // 
            // textBoxCellName
            // 
            this.textBoxCellName.Location = new System.Drawing.Point(48, 43);
            this.textBoxCellName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCellName.Name = "textBoxCellName";
            this.textBoxCellName.ReadOnly = true;
            this.textBoxCellName.Size = new System.Drawing.Size(54, 26);
            this.textBoxCellName.TabIndex = 3;
            // 
            // textBoxCellValue
            // 
            this.textBoxCellValue.Location = new System.Drawing.Point(142, 43);
            this.textBoxCellValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCellValue.Name = "textBoxCellValue";
            this.textBoxCellValue.ReadOnly = true;
            this.textBoxCellValue.Size = new System.Drawing.Size(148, 26);
            this.textBoxCellValue.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1200, 35);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(243, 30);
            this.openFileToolStripMenuItem.Text = "Open Spreadsheet";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(243, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basicHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // basicHelpToolStripMenuItem
            // 
            this.basicHelpToolStripMenuItem.Name = "basicHelpToolStripMenuItem";
            this.basicHelpToolStripMenuItem.Size = new System.Drawing.Size(174, 30);
            this.basicHelpToolStripMenuItem.Text = "Basic help";
            this.basicHelpToolStripMenuItem.Click += new System.EventHandler(this.basicHelpToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "XML files|*.xml|All files|*.*";
            this.openFileDialog.InitialDirectory = ".";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            this.saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
            this.saveFileDialog.InitialDirectory = ".";
            // 
            // labelCell
            // 
            this.labelCell.AutoSize = true;
            this.labelCell.Location = new System.Drawing.Point(9, 48);
            this.labelCell.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCell.Name = "labelCell";
            this.labelCell.Size = new System.Drawing.Size(35, 20);
            this.labelCell.TabIndex = 6;
            this.labelCell.Text = "Cell";
            // 
            // labelCellIs
            // 
            this.labelCellIs.AutoSize = true;
            this.labelCellIs.Location = new System.Drawing.Point(112, 48);
            this.labelCellIs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCellIs.Name = "labelCellIs";
            this.labelCellIs.Size = new System.Drawing.Size(20, 20);
            this.labelCellIs.TabIndex = 7;
            this.labelCellIs.Text = "is";
            // 
            // spreadsheetPanelPrimary
            // 
            this.spreadsheetPanelPrimary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanelPrimary.Location = new System.Drawing.Point(8, 126);
            this.spreadsheetPanelPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spreadsheetPanelPrimary.Name = "spreadsheetPanelPrimary";
            this.spreadsheetPanelPrimary.Size = new System.Drawing.Size(1191, 565);
            this.spreadsheetPanelPrimary.TabIndex = 0;
            // 
            // selectedCellInput
            // 
            this.selectedCellInput.Enabled = false;
            this.selectedCellInput.Location = new System.Drawing.Point(654, 297);
            this.selectedCellInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectedCellInput.Name = "selectedCellInput";
            this.selectedCellInput.Size = new System.Drawing.Size(120, 26);
            this.selectedCellInput.TabIndex = 9;
            this.selectedCellInput.Visible = false;
            // 
            // Revert_Button
            // 
            this.Revert_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Revert_Button.Location = new System.Drawing.Point(995, 43);
            this.Revert_Button.Name = "Revert_Button";
            this.Revert_Button.Size = new System.Drawing.Size(94, 32);
            this.Revert_Button.TabIndex = 10;
            this.Revert_Button.Text = "Revert";
            this.Revert_Button.UseVisualStyleBackColor = true;
            this.Revert_Button.Click += new System.EventHandler(this.Revert_Button_Click);
            // 
            // Undo_Button
            // 
            this.Undo_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Undo_Button.Location = new System.Drawing.Point(1095, 43);
            this.Undo_Button.Name = "Undo_Button";
            this.Undo_Button.Size = new System.Drawing.Size(93, 32);
            this.Undo_Button.TabIndex = 11;
            this.Undo_Button.Text = "Undo";
            this.Undo_Button.UseVisualStyleBackColor = true;
            this.Undo_Button.Click += new System.EventHandler(this.Undo_Button_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.Undo_Button);
            this.Controls.Add(this.Revert_Button);
            this.Controls.Add(this.selectedCellInput);
            this.Controls.Add(this.labelCellIs);
            this.Controls.Add(this.labelCell);
            this.Controls.Add(this.textBoxCellValue);
            this.Controls.Add(this.textBoxCellName);
            this.Controls.Add(this.buttonEnter);
            this.Controls.Add(this.textBoxCellInput);
            this.Controls.Add(this.spreadsheetPanelPrimary);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(550, 258);
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanelPrimary;
        private System.Windows.Forms.TextBox textBoxCellInput;
        private System.Windows.Forms.Button buttonEnter;
        private System.Windows.Forms.TextBox textBoxCellName;
        private System.Windows.Forms.TextBox textBoxCellValue;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem basicHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label labelCell;
        private System.Windows.Forms.Label labelCellIs;
        private SpreadsheetGUI.SelectedCellInput selectedCellInput;
        private System.Windows.Forms.Button Revert_Button;
        private System.Windows.Forms.Button Undo_Button;
    }
}

