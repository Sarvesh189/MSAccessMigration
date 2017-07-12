using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MSAccessMigration
{
    public partial class frmMSAccessMigration : Form
    {
        IMigrationManager _migrationManager;
        DBEngineObject _dbEngineObject;
        List<string> _sqlmigrationTables;
        
        public bool ChildDialogResult { get; set; }
        public frmMSAccessMigration()
        {
            InitializeComponent();

            _migrationManager = RegisterComponent.Container.Resolve<IMigrationManager>();
            _sqlmigrationTables = new List<string>();
        }

        private async void btnMigration_Click(object sender, EventArgs e)
        {         
             await Progress();
             await AnalyseResult(txtDestinationFile.Text,destinationText);  
          
        }

        private async Task AnalyseResult(string file,RichTextBox ctrl)
        {
            _dbEngineObject =await Task.Run(()=> _migrationManager.AnalyseAccessDB(file));

            ctrl.AppendText("Destination DBEngine Analysis");

            ctrl.AppendText(Environment.NewLine + "--------------Tables---------------");

            foreach (var tableInfo in _dbEngineObject.Tables.FindAll(t=>t.TableType=="Internal"))
            {
                ctrl.AppendText(Environment.NewLine + tableInfo.TableName);
            }

            

            ctrl.AppendText(Environment.NewLine + "--------------Forms---------------");

            foreach (string str in _dbEngineObject.Forms)
            {
                ctrl.AppendText(Environment.NewLine + str);
            }

            ctrl.AppendText(Environment.NewLine + "--------------Reports---------------");

            foreach (string str in _dbEngineObject.Reports)
            {
                ctrl.AppendText(Environment.NewLine + str);
            }

            ctrl.AppendText(Environment.NewLine + "--------------Queries---------------");

            foreach (string str in _dbEngineObject.Queries)
            {
                ctrl.AppendText(Environment.NewLine + str);
            }
            ctrl.AppendText(Environment.NewLine + "--------------Macros---------------");

            foreach (string str in _dbEngineObject.Macros)
            {
                ctrl.AppendText(Environment.NewLine + str);
            }
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
          var dialogResult =  fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtSourceFile.Text = fileDialog.FileName;
            }
        }

        private void btnDestinationFile_Click(object sender, EventArgs e)
        {
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtDestinationFile.Text = fileDialog.FileName;
            }
        }

        private async Task Progress()
        {
            migrationProgressBar.Maximum = 100;
            migrationProgressBar.Minimum = 0;
            migrationProgressBar.Step = 1;
            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                migrationProgressBar.Value = v;
            });

            // Run operation in another thread
            //  await Task.Run(() => DoWork(progress));
            var resultTask = await Task.Run(() => _migrationManager.TransferAccessDB(txtSourceFile.Text, txtDestinationFile.Text, _sqlmigrationTables, progress));
                            
  
           
            if (resultTask)
            {
                MessageBox.Show("Done!");
            }


        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {

            
            //   CreateDestinationDB(txtDestinationFile.Text);
            _dbEngineObject = _migrationManager.AnalyseAccessDB(txtSourceFile.Text);

            sourceText.AppendText("Source DBEngine Analysis");

            sourceText.AppendText(Environment.NewLine + "--------------Local Tables---------------");
            if (_dbEngineObject.Tables.Exists(t=>t.TableType=="Internal"))
                       {
                foreach (TableInfo tbl in _dbEngineObject.Tables.FindAll(t=>t.TableType=="Internal"))
                {
                    sourceText.AppendText(Environment.NewLine + tbl.TableName);
                }
            }

            sourceText.AppendText(Environment.NewLine + "--------------Linked Tables---------------");
            if (_dbEngineObject.Tables.Exists(t => t.TableType == "External"))
            {
                foreach (TableInfo tbl in _dbEngineObject.Tables.FindAll(t => t.TableType == "External"))
                {
                    sourceText.AppendText(Environment.NewLine + tbl.TableName);
                    _sqlmigrationTables.Add(tbl.TableName);
                }
            }
            sourceText.AppendText(Environment.NewLine + "--------------Forms---------------");
            if (_dbEngineObject.Forms != null)
            {
                foreach (string str in _dbEngineObject.Forms)
                {
                    sourceText.AppendText(Environment.NewLine + str);
                }
            }

            sourceText.AppendText(Environment.NewLine + "--------------Reports---------------");
            if (_dbEngineObject.Reports != null)
            {
                foreach (string str in _dbEngineObject.Reports)
                {
                    sourceText.AppendText(Environment.NewLine + str);
                }
            }

            sourceText.AppendText(Environment.NewLine + "--------------Macros---------------");
            if (_dbEngineObject.Macros != null)
            {
                foreach (string str in _dbEngineObject.Macros)
                {
                    sourceText.AppendText(Environment.NewLine + str);
                }
            }

            btnMigrate.Enabled = true;
            chkSQLMigration.Enabled = true;
        }

      

        private void chkSQLMigration_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSQLMigration.Checked)
            {
                SQLMigrationDialog sqlMigrationDialog = new SQLMigrationDialog();
                List<string> tables = new List<string>();
                _dbEngineObject.Tables.FindAll(t => t.TableType == "Internal").ForEach(t => tables.Add(t.TableName));
                sqlMigrationDialog.TableList = tables;
                sqlMigrationDialog.Show();
                // var selectedtables = sqlMigrationDialog.SelectedTables;
                sqlMigrationDialog.FormClosing += SqlMigrationDialog_FormClosing;
            }

           
        }

        private void SqlMigrationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Utility.IsSqlMigrationSelected)
            {
                _sqlmigrationTables.AddRange(((SQLMigrationDialog)sender).SelectedTables);
            }
            else
            {
                chkSQLMigration.Checked = false;
            }
        }
        private void KillProcess()
        {
            foreach (var process in Process.GetProcessesByName("MSAccess"))
            {
                process.Kill();
            }
        }

        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void ClearProcess_Click(object sender, EventArgs e)
        {
            KillProcess();
        }
    }
}
