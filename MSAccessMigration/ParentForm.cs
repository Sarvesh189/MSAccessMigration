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

        private Form CheckFormInstance(string frmname)
        {
          return  this.MdiChildren.FirstOrDefault(f => f.Name == frmname);
        }

        private void migrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmMigration = CheckFormInstance("frmMSAccessMigration");
            if (frmMigration != null)
            {
                frmMigration.Activate();
            }
            else
            {
                frmMigration = new frmMSAccessMigration();
                frmMigration.MdiParent = this;
                frmMigration.WindowState = FormWindowState.Maximized;
                frmMigration.Dock = DockStyle.Fill;
                frmMigration.Show();
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmlog = CheckFormInstance("LogForm");
            if (frmlog != null)
            {
                frmlog.Activate();
            }
            else
            {
                frmlog = new LogForm();
                frmlog.MdiParent = this;
                frmlog.Dock = DockStyle.Fill;
                frmlog.Show();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmAbout = new MSAbout();
            frmAbout.MdiParent = this;
            frmAbout.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Helpdoc.pdf");
            //Form frmHelp = CheckFormInstance("HelpForm");
            //if (frmHelp != null)
            //{
            //    frmHelp.Activate();
            //}
            //else
            //{
            //    frmHelp = new HelpForm();
            //    frmHelp.MdiParent = this;
            //    frmHelp.Dock = DockStyle.Fill;
            //    frmHelp.Show();
            //}
        }
    }
}
