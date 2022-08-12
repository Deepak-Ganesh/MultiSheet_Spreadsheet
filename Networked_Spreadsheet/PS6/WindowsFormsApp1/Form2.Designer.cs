using System;

namespace SS
{
    partial class Form2
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.IPText = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameText = new System.Windows.Forms.TextBox();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.pnl = new System.Windows.Forms.FlowLayoutPanel();
            this.newButton = new System.Windows.Forms.Button();
            this.newSheetText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(915, 54);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.BackColor = System.Drawing.SystemColors.Control;
            this.IPLabel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPLabel.Location = new System.Drawing.Point(15, 15);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(76, 18);
            this.IPLabel.TabIndex = 1;
            this.IPLabel.Text = "IPAddress:";
            // 
            // IPText
            // 
            this.IPText.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPText.Location = new System.Drawing.Point(97, 13);
            this.IPText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.IPText.Name = "IPText";
            this.IPText.Size = new System.Drawing.Size(116, 24);
            this.IPText.TabIndex = 2;
            this.IPText.Text = "test text";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.BackColor = System.Drawing.SystemColors.Control;
            this.UsernameLabel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(264, 15);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(71, 18);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "Username:";
            // 
            // UsernameText
            // 
            this.UsernameText.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameText.Location = new System.Drawing.Point(341, 13);
            this.UsernameText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(116, 24);
            this.UsernameText.TabIndex = 4;
            this.UsernameText.Text = "test text";
            // 
            // PasswordText
            // 
            this.PasswordText.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordText.Location = new System.Drawing.Point(579, 13);
            this.PasswordText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new System.Drawing.Size(116, 24);
            this.PasswordText.TabIndex = 6;
            this.PasswordText.Text = "test text";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.BackColor = System.Drawing.SystemColors.Control;
            this.PasswordLabel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(508, 15);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(65, 18);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Password";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(776, 13);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(114, 24);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(850, 77);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(14, 13);
            this.radioButton1.TabIndex = 8;
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // pnl
            // 
            this.pnl.BackColor = System.Drawing.SystemColors.Window;
            this.pnl.Location = new System.Drawing.Point(813, 70);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(119, 1000);
            this.pnl.TabIndex = 9;
            // 
            // newButton
            // 
            this.newButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.newButton.Location = new System.Drawing.Point(138, 377);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(75, 23);
            this.newButton.TabIndex = 10;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // newSheetText
            // 
            this.newSheetText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.newSheetText.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSheetText.Location = new System.Drawing.Point(12, 377);
            this.newSheetText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.newSheetText.Name = "newSheetText";
            this.newSheetText.Size = new System.Drawing.Size(116, 24);
            this.newSheetText.TabIndex = 11;
            this.newSheetText.Text = "Spreadsheet Name";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(915, 415);
            this.Controls.Add(this.newSheetText);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameText);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.IPText);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "Form2";
            this.Text = "FUBAR Spreadsheet Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox IPText;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox UsernameText;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.FlowLayoutPanel pnl;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.TextBox newSheetText;
    }
}

