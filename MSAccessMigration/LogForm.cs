using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSAccessMigrationLibrary;

namespace MSAccessMigration
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

      
        private async void btnLogFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppLogManager.LogFolder();
            var result = openFileDialog1.ShowDialog();
            if (result==DialogResult.OK)
            {
                txtLogFile.Text = openFileDialog1.FileName;
                txtLog.Text =await AppLogManager.ReadLog(txtLogFile.Text.Trim());
            }
        }
    }
}
