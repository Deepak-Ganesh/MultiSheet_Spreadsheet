using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var context = BasicContext.getAppContext();
            context.RunContainer(new SpreadsheetContainer());
        }
    }

    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    internal class BasicContext : ApplicationContext
    {
        private int containerCount = 0;
        private bool recursed = false;

        // Singleton Context Object
        private static BasicContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private BasicContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static BasicContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new BasicContext();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunContainer(SpreadsheetContainer sc)
        {
            containerCount++;
            
            sc.RegisterHandlers(ClientClosed, SpreadOpened);

            // run the form
            if (containerCount > 1)
                sc.RunNew();
            else
                sc.Run();
        }

        void ClientClosed(SpreadsheetContainer sc)
        {
            if (!recursed)
            {
                recursed = true;
                containerCount--;
                if (containerCount <= 0)
                {
                    try
                    {
                        Application.Exit();
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                recursed = false;
            }
            recursed = false;
        }

        void SpreadOpened(SpreadsheetContainer sc)
        {
            RunContainer(sc);
        }
    }




    public class SpreadsheetContainer
    {
        public SpreadsheetGUI.Form1 SS;
        public Form2 client;
        ClientController cc;

        public delegate void ClientCloseHandler(SpreadsheetContainer sc);
        public event ClientCloseHandler clientCloseHandler;
        public delegate void ClientOpenHandler(SpreadsheetContainer sc);
        public event ClientOpenHandler clientOpenHandler;

        private bool clientClosed = false;

        public SpreadsheetContainer()
        {
        }

        public void RegisterHandlers(ClientCloseHandler cch, ClientOpenHandler coh)
        {
            clientCloseHandler += cch;
            clientOpenHandler += coh;
        }

        public void Run()
        {
            cc = new ClientController();

            client = new Form2(cc);
            client.RegisterCSSGH(CreateSpreadsheetGUI);
            client.RegisterCloseHandler(ClientClosed);
            
            Application.Run(client);
        }

        public void RunNew()
        {
            cc = new ClientController();

            client = new Form2(cc);
            client.RegisterCSSGH(CreateSpreadsheetGUI);
            client.RegisterCloseHandler(ClientClosed);

            // show instead of run, since show runs a new thread
            Thread t = new Thread(() => Application.Run(client));
            t.Start();
        }

        /// <summary>
        /// creates a spreadsheet when the clientgui requests one to be made
        /// </summary>
        void CreateSpreadsheetGUI(FullSendMessage FSM)
        {
            SS = new SpreadsheetGUI.Form1(cc, FSM, client.sheetName);
            SS.RegisterContainerHandlers(SSCloseHandler, CreateClientGUI);
            Thread t = new Thread(() => Application.Run(SS));
            t.Start();
        }

        /// <summary>
        /// Creates a separate clientgui when the spreadsheetgui wants to create a new clientgui
        ///  Does so by creating a new container to keep track of references
        /// </summary>
        void CreateClientGUI()
        {
            SpreadsheetContainer sc = new SpreadsheetContainer();
            clientOpenHandler?.Invoke(sc);
        }

        /// <summary>
        /// Closes the clientgui connected to the spreadsheet when the spreadsheet is closed.
        /// </summary>
        void SSCloseHandler()
        {
            ClientClosed();
        }

        /// <summary>
        /// Handles the closing of the clientgui
        /// </summary>
        void ClientClosed()
        {
            if (!clientClosed)
            {
                clientClosed = true;
                client.shouldClose = true;
                clientCloseHandler?.Invoke(this);
            }
        }
    }
}
