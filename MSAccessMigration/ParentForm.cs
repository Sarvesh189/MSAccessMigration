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
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        private void migrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmMigration = new frmMSAccessMigration();
            frmMigration.MdiParent = this;
            frmMigration.WindowState = FormWindowState.Maximized;
            frmMigration.Dock = DockStyle.Fill;
            frmMigration.Show();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmLog = new LogForm();
            frmLog.MdiParent = this;
            frmLog.Dock = DockStyle.Fill;
            frmLog.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmAbout = new MSAbout();
            frmAbout.MdiParent = this;
            frmAbout.Show();
        }
    }
}
