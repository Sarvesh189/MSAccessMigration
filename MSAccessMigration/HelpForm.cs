using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSAccessMigration
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            DisplayInstructions();
        }
        private void DisplayInstructions()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            webBrowserHelp.Url = new Uri(filePath+"Helpdoc.html");
        }
    }
}
