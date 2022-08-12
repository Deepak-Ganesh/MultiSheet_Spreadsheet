using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
    public partial class Form2 : Form
    {
        // the controller that communicates between the client (this) and the server. controller communicates via events
        private ClientController clientController;

        private SpreadsheetListPanel sheetPanel;

        // methodinvoker that tells the client that a connection request has been received and accepted by the server
        private MethodInvoker connectionAccepted;
        // methodinvoker that tells the client to update its size when the spreadsheet list is received
        private MethodInvoker updateClientSize;
        // methodinvoker that tells the client to update as soon as possible
        private MethodInvoker invalidateForm;
        // methodinvoker to create radio buttons
        private MethodInvoker updateSSButtons;
        // methodinvoker to close the form when a spreadsheet has successfully been received
        private MethodInvoker closeForm;
        // methodinvoker to hide the form when a spreadsheet is given to the client
        private MethodInvoker hideForm;

        private MethodInvoker enableButtons;

        public delegate void CreateSpreadsheetGUIHandler(FullSendMessage FSM);
        public event CreateSpreadsheetGUIHandler cssgh;

        public delegate void CloseHandler();
        public event CloseHandler closeHandler;

        private System.Timers.Timer connectionTimer;
        
        private string[] sheets;

        public bool shouldClose;
        private bool hasRecursed;

        public string sheetName;

        // used as a lock for all gui elements. nothing special about the item.
        private object locker;

        public Form2(ClientController ctl)
        {
            shouldClose = false;
            hasRecursed = false;
            locker = new object();

            connectionTimer = new System.Timers.Timer();
            connectionTimer.Interval = 5000;
            connectionTimer.Elapsed += ConnectFailed;

            // initialize form, and pass the controller reference to the form
            clientController = ctl;
            sheetPanel = new SpreadsheetListPanel(locker);
            InitializeComponent();

            // set the methodinvokers to let the form know to update
            connectionAccepted = new MethodInvoker(ConnectionAccepted);
            updateClientSize = new MethodInvoker(UpdateClientSize);
            updateSSButtons = new MethodInvoker(UpdateSSButtons);
            closeForm = new MethodInvoker(Close);
            hideForm = new MethodInvoker(Hide);
            enableButtons = new MethodInvoker(EnableButtons);

            invalidateForm = new MethodInvoker(() => this.Invalidate(true));

            // register all the events needed to properly display updates from the server
            clientController.RegisterHandlers(ConnectionHandler, SpreadsheetHandler, SpreadsheetListHandler, ErrorHandler);

            sheetPanel.Location = new Point(0, 70);
            sheetPanel.Size = new Size(800, 1000);

            this.Controls.Add(sheetPanel);

            // clean out the test values of every box, disable the username and password text boxes
            // testcode stubbed in just for easier testing
            IPText.Text = "lab1-34.eng.utah.edu";
            UsernameText.Text = "";
            PasswordText.Text = "";
            UsernameText.Enabled = false;
            PasswordText.Enabled = false;

            newSheetText.Enabled = false;
            newButton.Enabled = false;
        }

        /// <summary>
        /// Register the event to create a spreadsheet for the main function's use
        /// </summary>
        /// <param name="e"></param>
        public void RegisterCSSGH(CreateSpreadsheetGUIHandler e)
        {
            cssgh += e;
        }

        public void RegisterCloseHandler(CloseHandler ch)
        {
            closeHandler += ch;
        }

        /// <summary>
        /// it dont do nothin
        /// </summary>
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButtonHandler();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            NewSheetHandler();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            BtnClickHandler(sender);
        }

        private void ConnectButtonHandler()
        {
            if (clientController.IsConnected())
            {
                if (!ReconnectBox())
                    return;
            }

            if (IPText.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                return;
            }

            // Disable the controls and try to connect
            IPText.Enabled = false;
            UsernameText.Enabled = false;
            PasswordText.Enabled = false;
            ConnectButton.Enabled = false;
            newSheetText.Enabled = false;
            newButton.Enabled = false;
            
            try
            {
                clientController.ConnectToServer(IPText.Text);
                connectionTimer.Start();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("IP Address invalid. Please enter a valid address.");
                IPText.Enabled = true;
                ConnectButton.Enabled = true;
                return;
            }
        }

        private void ConnectionHandler()
        {
            this.Invoke(connectionAccepted);
        }

        private void SpreadsheetHandler(FullSendMessage FSM)
        {
            SheetMessageHandler(FSM);
        }

        private void ErrorHandler(ErrorMessage EM)
        {
            MessageBox.Show("Incorrect password.");
        }

        private void SheetMessageHandler(FullSendMessage FSM)
        {
            this.Invoke(hideForm);

            clientController.ClearHandlers();

            cssgh?.Invoke(FSM);
        }

        private void SpreadsheetListHandler(SpreadsheetListMessage SSLM)
        {
            sheets = SSLM.spreadsheets;
            sheetPanel.SetSheets(sheets);

            this.Invoke(updateClientSize);
            this.Invoke(updateSSButtons);

            try { this.Invoke(invalidateForm); }
            catch (Exception) { }
        }

        private void BtnClickHandler(object sender)
        {
            if (UsernameText.Text == "" || PasswordText.Text == "")
            {
                MessageBox.Show("Please enter a username and password.");
                return;
            }

            Button clickedButton = (Button)sender;

            if (clickedButton == null)
                return;

            for (int i = 0; i < sheets.Length; i++)
            {
                if (clickedButton.Name == "btn" + i)
                {
                    sheetName = sheets[i];
                    clientController.SendData(new OpenMessage(sheets[i], UsernameText.Text, PasswordText.Text).ToString());
                }
            }
        }

        private void NewSheetHandler()
        {
            string message = "Create a new spreadsheet?";
            string title = "Create new Spreadsheet";

            DialogResult result;

            result = MessageBox.Show(
                     message,
                     title,
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (newSheetText.Text == "")
                {
                    MessageBox.Show("Please enter a new spreadsheet name.");
                    return;
                }
                if (UsernameText.Text == "" || PasswordText.Text == "")
                {
                    MessageBox.Show("Please enter a username and password.");
                    return;
                }

                sheetName = newSheetText.Text;
                clientController.SendData(new OpenMessage(newSheetText.Text, UsernameText.Text, PasswordText.Text).ToString());
            }
        }

        private bool ReconnectBox()
        {
            bool ret;

            string message = "Client is already connected to a server. Reconnect?";
            string title = "Reconnect to server";

            DialogResult result;

            result = MessageBox.Show(
                     message,
                     title,
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
                ret = true;
            else
                ret = false;
            return ret;
        }

        private void UpdateSSButtons()
        {
            pnl.Controls.Clear();

            Button btn;
            for (int i = 0; i < sheets.Length; i++)
            {
                btn = new Button() { Text = "Open", Name = "btn" + i };
                btn.Click += new System.EventHandler(btn_Click);
                this.Controls.Add(btn);
                pnl.Controls.Add(btn);
            }

            this.Controls.Add(pnl);
        }

        private void ConnectionAccepted()
        {
            connectionTimer.Stop();

            this.Invoke(enableButtons);
        }

        protected void ConnectFailed(object sender, EventArgs e)
        {
            connectionTimer.Stop();
            
            MessageBox.Show("Connection timed out",
                            "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.Invoke(enableButtons);
        }

        private void EnableButtons()
        {
            IPText.Enabled = true;
            UsernameText.Enabled = true;
            PasswordText.Enabled = true;
            newSheetText.Enabled = true;

            newButton.Enabled = true;
            ConnectButton.Enabled = true;
        }

        private void UpdateClientSize()
        {
            int oldHeight = 453;
            int sheetHeight = 50 * sheets.Length;
            int sheetAdjustHeight = 5 * sheets.Length;
            int newClientHeight = oldHeight + sheetAdjustHeight;
            int newHeight = oldHeight + sheetHeight;

            // no need to resize on low sheet counts
            if (sheets.Length > 6)
            {
                ClientSize = new Size(ClientSize.Width, newClientHeight);
                newSheetText.Location = new Point(newSheetText.Location.X, newSheetText.Location.Y + sheetAdjustHeight);
                newButton.Location = new Point(newButton.Location.X, newButton.Location.Y + sheetAdjustHeight);
            }

            sheetPanel.Size = new Size(sheetPanel.Size.Width, sheetHeight);

            try { this.Invoke(invalidateForm); }
            catch (Exception) { }
        }

        public void CloseClient()
        {
            if (shouldClose)
                this.Invoke(closeForm);
        }

        public void HideClient()
        {
            this.Invoke(hideForm);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!hasRecursed)
            {
                hasRecursed = true;
                HideClient();
                closeHandler?.Invoke();
                if (!shouldClose)
                    e.Cancel = true;
            }
            else
            {
                hasRecursed = false;
            }
            hasRecursed = false;
        }
    }
}
