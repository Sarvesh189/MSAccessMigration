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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                txtLog.SelectionStart = 0;
                txtLog.SelectionLength = txtLog.TextLength;
                txtLog.SelectionBackColor = Color.White;
                int j = 0, i = 0;
                var searchText = textBox1.Text.Trim();

                do
                {
                    j = txtLog.Text.IndexOf(searchText, i, StringComparison.InvariantCultureIgnoreCase);
                    if (j >= 0)
                    {
                        txtLog.SelectionStart = j;
                        txtLog.SelectionLength = searchText.Length;
                        txtLog.SelectionBackColor = Color.Yellow;
                        // txtLog.Refresh();
                        i = j + 1;

                    }

                } while (j >= 0);
            }
            else { MessageBox.Show("Please enter text to search"); }
            Cursor.Current = Cursors.Arrow;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var starttime = dtfilter.Text.Trim();
            var endtime = dtEndTime.Text.Trim();
            var lines = txtLog.Lines.ToList();
            int startIndex = 0,endIndex=0;

            for(int index=0; index<lines.Count;index++)
            {
                if (lines[index].Contains(starttime)) {
                    startIndex = index;
                    break;
                }
            }

            for (int index = lines.Count-1; index > 0; index--)
            {
                if (lines[index].Contains(endtime))
                {
                    endIndex = index;
                    break;
                }
            }




            var linefiltered = lines.Skip(startIndex).Take(endIndex - startIndex);
            txtLog.Clear();
            txtLog.Lines = linefiltered.ToArray();
            Cursor.Current = Cursors.Arrow;
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            dtfilter.CustomFormat = "HH:mm";
            dtfilter.Format = DateTimePickerFormat.Custom;
            dtEndTime.CustomFormat = "HH:mm";
            dtEndTime.Format = DateTimePickerFormat.Custom;
        }

        private async void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = await AppLogManager.ReadLog(txtLogFile.Text.Trim());
        }

        private void dtfilter_ValueChanged(object sender, EventArgs e)
        {
            dtEndTime.Text = dtfilter.Text;
        }
    }
}
