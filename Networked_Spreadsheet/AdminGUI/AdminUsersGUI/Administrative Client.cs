using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Backend;

namespace AdminGUI
{
    public partial class Form1 : Form
    {
        AdminController controller;
        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectTab(3);
            connectBox.Text = "lab1-34.eng.utah.edu";
        }

        private void activityToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CreateSpreadsheetButton_Click(object sender, EventArgs e)
        {
            controller.createSpreadsheet(ssNameTextBox.Text);
        }

        private void deleteSpreadsheetButton_Click(object sender, EventArgs e)
        {
            foreach (object item in spreadsheetList.SelectedItems)
                controller.removeSpreadsheet(item.ToString());
        }

        private void createUserButton_Click(object sender, EventArgs e)
        {
            controller.createUser(usernameTextBox.Text, passwordTextBox.Text);
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            foreach (object item in userList.SelectedItems)
                controller.deleteUser(item.ToString());
        }

        private void changepwButton_Click(object sender, EventArgs e)
        {
            foreach (object item in userList.SelectedItems)
                controller.changePassword(item.ToString(), passwordTextBox.Text);
        }

        public void updateSpreadsheetList(string[] list)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                spreadsheetList.Items.Clear();
                foreach (string ss in list)
                {
                    spreadsheetList.Items.Add(ss);
                }
            }));
        }

        public void updateUsersList(Dictionary<string, bool> list)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                userList.Items.Clear();
                connectionList.Items.Clear();
                foreach (string user in list.Keys)
                {
                    userList.Items.Add(user);
                    if (list[user])
                    {
                        connectionList.Items.Add(user);
                    }
                }
            }));
        }

        public void updateEditList(string owner, string edit, string name)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                string[] row = { owner, name, edit };
                ListViewItem item = new ListViewItem(row);
                updateList.Items.Insert(0, item);
            }));
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                controller = new AdminController(connectBox.Text);
                controller.registerHandlers(updateSpreadsheetList, updateEditList, updateUsersList);
                tabControl1.SelectTab(0);
            }
            catch (ArgumentException)
            {
                connectBox.Text = "Invalid address";
            }
        }

        private void shutDownButton_Click(object sender, EventArgs e)
        {
            controller.shutDownServer();
        }
    }
}
