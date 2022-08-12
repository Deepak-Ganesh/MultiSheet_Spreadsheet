using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
// remove this using later when testclient is no longer needed
using SS;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    internal class BasicContext : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
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
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }

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

            // Start an application context and run one form inside it
            var context = BasicContext.getAppContext();
            context.RunForm(new Form1());
            Application.Run(context);
        }
    }
}
