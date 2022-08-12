namespace AdminGUI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.activityTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.connectionList = new System.Windows.Forms.ListView();
            this.ActiveUserCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateList = new System.Windows.Forms.ListView();
            this.UserCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SpreadsheetCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EditCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ssTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ssNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteSpreadsheetButton = new System.Windows.Forms.Button();
            this.CreateSpreadsheetButton = new System.Windows.Forms.Button();
            this.spreadsheetList = new System.Windows.Forms.ListBox();
            this.usersTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.userList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteUserButton = new System.Windows.Forms.Button();
            this.createUserButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.changepwButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionTab = new System.Windows.Forms.TabPage();
            this.shutDownButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.connectBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.activityTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ssTab.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.usersTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.connectionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.activityTab);
            this.tabControl1.Controls.Add(this.ssTab);
            this.tabControl1.Controls.Add(this.usersTab);
            this.tabControl1.Controls.Add(this.connectionTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1065, 567);
            this.tabControl1.TabIndex = 1;
            // 
            // activityTab
            // 
            this.activityTab.Controls.Add(this.splitContainer1);
            this.activityTab.Location = new System.Drawing.Point(4, 25);
            this.activityTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.activityTab.Name = "activityTab";
            this.activityTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.activityTab.Size = new System.Drawing.Size(1057, 538);
            this.activityTab.TabIndex = 1;
            this.activityTab.Text = "Activity";
            this.activityTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.connectionList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.updateList);
            this.splitContainer1.Size = new System.Drawing.Size(1051, 534);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 0;
            // 
            // connectionList
            // 
            this.connectionList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.connectionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ActiveUserCol});
            this.connectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionList.Location = new System.Drawing.Point(0, 0);
            this.connectionList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connectionList.Name = "connectionList";
            this.connectionList.Size = new System.Drawing.Size(1051, 216);
            this.connectionList.TabIndex = 0;
            this.connectionList.UseCompatibleStateImageBehavior = false;
            this.connectionList.View = System.Windows.Forms.View.Details;
            // 
            // ActiveUserCol
            // 
            this.ActiveUserCol.Text = "Active Users";
            this.ActiveUserCol.Width = 223;
            // 
            // updateList
            // 
            this.updateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserCol,
            this.SpreadsheetCol,
            this.EditCol});
            this.updateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateList.GridLines = true;
            this.updateList.Location = new System.Drawing.Point(0, 0);
            this.updateList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.updateList.Name = "updateList";
            this.updateList.Size = new System.Drawing.Size(1051, 314);
            this.updateList.TabIndex = 0;
            this.updateList.UseCompatibleStateImageBehavior = false;
            this.updateList.View = System.Windows.Forms.View.Details;
            // 
            // UserCol
            // 
            this.UserCol.Text = "User";
            this.UserCol.Width = 190;
            // 
            // SpreadsheetCol
            // 
            this.SpreadsheetCol.Text = "Spreadsheet";
            this.SpreadsheetCol.Width = 330;
            // 
            // EditCol
            // 
            this.EditCol.Text = "Edit";
            this.EditCol.Width = 262;
            // 
            // ssTab
            // 
            this.ssTab.Controls.Add(this.tableLayoutPanel2);
            this.ssTab.Location = new System.Drawing.Point(4, 25);
            this.ssTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ssTab.Name = "ssTab";
            this.ssTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ssTab.Size = new System.Drawing.Size(1057, 538);
            this.ssTab.TabIndex = 0;
            this.ssTab.Text = "Spreadsheets";
            this.ssTab.UseVisualStyleBackColor = true;
            this.ssTab.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.spreadsheetList, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1051, 534);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ssNameTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.deleteSpreadsheetButton);
            this.groupBox2.Controls.Add(this.CreateSpreadsheetButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(894, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(154, 530);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // ssNameTextBox
            // 
            this.ssNameTextBox.Location = new System.Drawing.Point(11, 448);
            this.ssNameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ssNameTextBox.Name = "ssNameTextBox";
            this.ssNameTextBox.Size = new System.Drawing.Size(100, 22);
            this.ssNameTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 418);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name:";
            // 
            // deleteSpreadsheetButton
            // 
            this.deleteSpreadsheetButton.Location = new System.Drawing.Point(80, 489);
            this.deleteSpreadsheetButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteSpreadsheetButton.Name = "deleteSpreadsheetButton";
            this.deleteSpreadsheetButton.Size = new System.Drawing.Size(75, 23);
            this.deleteSpreadsheetButton.TabIndex = 2;
            this.deleteSpreadsheetButton.Text = "Delete";
            this.deleteSpreadsheetButton.UseVisualStyleBackColor = true;
            this.deleteSpreadsheetButton.Click += new System.EventHandler(this.deleteSpreadsheetButton_Click);
            // 
            // CreateSpreadsheetButton
            // 
            this.CreateSpreadsheetButton.Location = new System.Drawing.Point(1, 489);
            this.CreateSpreadsheetButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CreateSpreadsheetButton.Name = "CreateSpreadsheetButton";
            this.CreateSpreadsheetButton.Size = new System.Drawing.Size(75, 23);
            this.CreateSpreadsheetButton.TabIndex = 1;
            this.CreateSpreadsheetButton.Text = "Create";
            this.CreateSpreadsheetButton.UseVisualStyleBackColor = true;
            this.CreateSpreadsheetButton.Click += new System.EventHandler(this.CreateSpreadsheetButton_Click);
            // 
            // spreadsheetList
            // 
            this.spreadsheetList.BackColor = System.Drawing.Color.White;
            this.spreadsheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetList.FormattingEnabled = true;
            this.spreadsheetList.ItemHeight = 16;
            this.spreadsheetList.Location = new System.Drawing.Point(3, 2);
            this.spreadsheetList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spreadsheetList.Name = "spreadsheetList";
            this.spreadsheetList.Size = new System.Drawing.Size(885, 530);
            this.spreadsheetList.TabIndex = 0;
            // 
            // usersTab
            // 
            this.usersTab.Controls.Add(this.tableLayoutPanel1);
            this.usersTab.Location = new System.Drawing.Point(4, 25);
            this.usersTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usersTab.Name = "usersTab";
            this.usersTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usersTab.Size = new System.Drawing.Size(1057, 538);
            this.usersTab.TabIndex = 2;
            this.usersTab.Text = "Users";
            this.usersTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.Controls.Add(this.userList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1051, 534);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // userList
            // 
            this.userList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userList.FormattingEnabled = true;
            this.userList.ItemHeight = 16;
            this.userList.Location = new System.Drawing.Point(3, 2);
            this.userList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(885, 530);
            this.userList.TabIndex = 0;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deleteUserButton);
            this.groupBox1.Controls.Add(this.createUserButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.changepwButton);
            this.groupBox1.Controls.Add(this.usernameTextBox);
            this.groupBox1.Controls.Add(this.passwordTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(894, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(154, 530);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // deleteUserButton
            // 
            this.deleteUserButton.Location = new System.Drawing.Point(80, 455);
            this.deleteUserButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteUserButton.Name = "deleteUserButton";
            this.deleteUserButton.Size = new System.Drawing.Size(75, 23);
            this.deleteUserButton.TabIndex = 6;
            this.deleteUserButton.Text = "Delete";
            this.deleteUserButton.UseVisualStyleBackColor = true;
            this.deleteUserButton.Click += new System.EventHandler(this.deleteUserButton_Click);
            // 
            // createUserButton
            // 
            this.createUserButton.Location = new System.Drawing.Point(1, 455);
            this.createUserButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.createUserButton.Name = "createUserButton";
            this.createUserButton.Size = new System.Drawing.Size(75, 23);
            this.createUserButton.TabIndex = 5;
            this.createUserButton.Text = "Create";
            this.createUserButton.UseVisualStyleBackColor = true;
            this.createUserButton.Click += new System.EventHandler(this.createUserButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username:";
            // 
            // changepwButton
            // 
            this.changepwButton.Location = new System.Drawing.Point(24, 484);
            this.changepwButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.changepwButton.Name = "changepwButton";
            this.changepwButton.Size = new System.Drawing.Size(116, 23);
            this.changepwButton.TabIndex = 7;
            this.changepwButton.Text = "Set Password";
            this.changepwButton.UseVisualStyleBackColor = true;
            this.changepwButton.Click += new System.EventHandler(this.changepwButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(15, 358);
            this.usernameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(76, 22);
            this.usernameTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(15, 417);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(76, 22);
            this.passwordTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 383);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label2.Size = new System.Drawing.Size(73, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // connectionTab
            // 
            this.connectionTab.Controls.Add(this.shutDownButton);
            this.connectionTab.Controls.Add(this.connectButton);
            this.connectionTab.Controls.Add(this.label4);
            this.connectionTab.Controls.Add(this.connectBox);
            this.connectionTab.Location = new System.Drawing.Point(4, 25);
            this.connectionTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectionTab.Name = "connectionTab";
            this.connectionTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectionTab.Size = new System.Drawing.Size(1057, 538);
            this.connectionTab.TabIndex = 3;
            this.connectionTab.Text = "Connect";
            this.connectionTab.UseVisualStyleBackColor = true;
            // 
            // shutDownButton
            // 
            this.shutDownButton.Location = new System.Drawing.Point(485, 305);
            this.shutDownButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.shutDownButton.Name = "shutDownButton";
            this.shutDownButton.Size = new System.Drawing.Size(100, 23);
            this.shutDownButton.TabIndex = 3;
            this.shutDownButton.Text = "Shut Down";
            this.shutDownButton.UseVisualStyleBackColor = true;
            this.shutDownButton.Click += new System.EventHandler(this.shutDownButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(485, 258);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(100, 26);
            this.connectButton.TabIndex = 2;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(507, 187);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Address";
            // 
            // connectBox
            // 
            this.connectBox.Location = new System.Drawing.Point(447, 219);
            this.connectBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectBox.Name = "connectBox";
            this.connectBox.Size = new System.Drawing.Size(176, 22);
            this.connectBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 567);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Admin GUI";
            this.tabControl1.ResumeLayout(false);
            this.activityTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ssTab.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.usersTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.connectionTab.ResumeLayout(false);
            this.connectionTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ssTab;
        private System.Windows.Forms.TabPage activityTab;
        private System.Windows.Forms.TabPage usersTab;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ssNameTextBox;
        private System.Windows.Forms.Button deleteSpreadsheetButton;
        private System.Windows.Forms.Button CreateSpreadsheetButton;
        private System.Windows.Forms.ListBox spreadsheetList;
        private System.Windows.Forms.Button changepwButton;
        private System.Windows.Forms.Button deleteUserButton;
        private System.Windows.Forms.Button createUserButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView connectionList;
        private System.Windows.Forms.ListView updateList;
        private System.Windows.Forms.TabPage connectionTab;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox connectBox;
        private System.Windows.Forms.Button shutDownButton;
        private System.Windows.Forms.ColumnHeader UserCol;
        private System.Windows.Forms.ColumnHeader SpreadsheetCol;
        private System.Windows.Forms.ColumnHeader EditCol;
        private System.Windows.Forms.ColumnHeader ActiveUserCol;
    }
}

