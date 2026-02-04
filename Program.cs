using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transparent_Form
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
            // Initialize database here
            DatabaseInitializer.Initialize();

            // Then start your application
            Application.Run(new SplashForm());




        }
    }
}


//exceptions handling

/*
            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show(
                    "Unhandled error:\n" + e.Exception.Message,
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Exception ex = e.ExceptionObject as Exception;
                MessageBox.Show(
                    "Fatal error:\n" + ex.Message,
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };
            */