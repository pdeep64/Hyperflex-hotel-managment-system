using HYFLEX_HMS.CLASS;
using HYFLEX_HMS.FORMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomWindowsForm
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
            bool CurrentStatus = CLS_REGISTER.GetProgramCurrentStatus();
            if (CurrentStatus==true)
            {
                Application.Run(new BLOCK());
            }
            else
            {
                Application.Run(new LOGIN());
            }
           
        }
    }
}
