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
    public partial class SQLMigrationDialog : Form
    {
        public SQLMigrationDialog()
        {
            InitializeComponent();
           
        }

        public List<string> TableList
        {
            set {
                var sqllist = (ListBox)chkListTables;
                sqllist.DataSource = value;           
                }
        }

        public List<string> SelectedTables
        {
            get
            {
                var selectedTables = new List<string>();
                foreach (var item in chkListTables.CheckedItems)
                {
                    selectedTables.Add(item.ToString());
                };
                return selectedTables;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Utility.IsSqlMigrationSelected = true;
            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            Utility.IsSqlMigrationSelected = false;
            
            this.Close();
        }
    }
}
