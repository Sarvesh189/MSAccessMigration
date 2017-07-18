﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSAccessMigrationLibrary;

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
            _migrationManager.MSAccessTransfer.ItemTransferred += MSAccessTransfer_ItemTransferred;
        }

        private void MSAccessTransfer_ItemTransferred(object sender, ItemTransferEventArg e)
        {
            foreach (DataGridViewRow row in gvAnalysis.Rows)
            {
                if (row.Cells["AccessObject"].Value.ToString() == e.ItemName)
                {
                    row.Cells["IsMigrated"].Value = true;
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        private async void btnMigration_Click(object sender, EventArgs e)
        {         
           var result =  await Progress();
             await AnalyseResult(txtDestinationFile.Text,destinationText);
            if (result)
            {
                MessageBox.Show("Done!");
            }
          
        }

        private async Task AnalyseResult(string file,RichTextBox ctrl)
        {
          var formattedText =  await Task.Run(() => Utility.FormatAnalysis(_migrationManager.AnalyseAccessDB(file)));
            var destinationAnalysis = Utility.GVAccessAnalysisInfo;

            ctrl.AppendText("Destination DBEngine Analysis");

            ctrl.AppendText(Environment.NewLine + formattedText);

            ctrl.AppendText(Environment.NewLine + Utility.GetFormattedException());

            gvDestination.DataSource = destinationAnalysis;
            gvDestination.Columns[2].Visible = false;
            gvDestination.Columns[3].Visible = false;
          
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

        private async Task<bool> Progress()
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

            return resultTask;
           
         /*   if (resultTask)
            {
                MessageBox.Show("Done!");
            }
            */

        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;        
            _dbEngineObject = _migrationManager.AnalyseAccessDB(txtSourceFile.Text);

            var strg =   Utility.FormatAnalysis(_dbEngineObject);

            sourceText.AppendText("Source DBEngine Analysis"+Environment.NewLine);
            sourceText.AppendText(strg);
            var analysisList =  Utility.DeepCopy(Utility.GVAccessAnalysisInfo);
            gvAnalysis.DataSource = analysisList;
            gvAnalysis.Refresh();
         
           
            btnMigrate.Enabled = true;
            chkSQLMigration.Enabled = true;
            Cursor.Current = Cursors.Arrow;
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtDestinationFile.Text = string.Empty;
            txtSourceFile.Text = string.Empty;
            gvAnalysis.DataSource = null;
            sourceText.Text = string.Empty;
            destinationText.Text = string.Empty;
            gvDestination.DataSource = null;
        }
    }
}
