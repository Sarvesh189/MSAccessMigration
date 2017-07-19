using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSAccessMigrationLibrary;
namespace MSAccessMigration
{
    static class Program
    {

       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                RegisterComponent.Register();
                //    Application.Run(new frmMSAccessMigration());
                Application.Run(new ParentForm());
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
          
        }
    }
}
