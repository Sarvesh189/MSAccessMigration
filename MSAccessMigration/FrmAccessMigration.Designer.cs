namespace MSAccessMigration
{
    partial class frmMSAccessMigration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.sourceText = new System.Windows.Forms.RichTextBox();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.FileGroup = new System.Windows.Forms.GroupBox();
            this.btnDestinationFile = new System.Windows.Forms.Button();
            this.btnSourceFile = new System.Windows.Forms.Button();
            this.txtDestinationFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.migrationProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnAnalyse = new System.Windows.Forms.Button();
            this.destinationText = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSQLMigration = new System.Windows.Forms.CheckBox();
            this.PopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClearProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.gvAnalysis = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gvDestination = new System.Windows.Forms.DataGridView();
            this.FileGroup.SuspendLayout();
            this.PopupMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDestination)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMigrate
            // 
            this.btnMigrate.Enabled = false;
            this.btnMigrate.Location = new System.Drawing.Point(414, 160);
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Size = new System.Drawing.Size(124, 45);
            this.btnMigrate.TabIndex = 0;
            this.btnMigrate.Text = "Migrate";
            this.btnMigrate.UseVisualStyleBackColor = true;
            this.btnMigrate.Click += new System.EventHandler(this.btnMigration_Click);
            // 
            // sourceText
            // 
            this.sourceText.Location = new System.Drawing.Point(41, 298);
            this.sourceText.Name = "sourceText";
            this.sourceText.Size = new System.Drawing.Size(693, 200);
            this.sourceText.TabIndex = 1;
            this.sourceText.Text = "";
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "fileDialog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source DB File";
            // 
            // FileGroup
            // 
            this.FileGroup.Controls.Add(this.btnDestinationFile);
            this.FileGroup.Controls.Add(this.btnSourceFile);
            this.FileGroup.Controls.Add(this.txtDestinationFile);
            this.FileGroup.Controls.Add(this.label2);
            this.FileGroup.Controls.Add(this.txtSourceFile);
            this.FileGroup.Controls.Add(this.label1);
            this.FileGroup.Location = new System.Drawing.Point(41, 12);
            this.FileGroup.Name = "FileGroup";
            this.FileGroup.Size = new System.Drawing.Size(935, 132);
            this.FileGroup.TabIndex = 3;
            this.FileGroup.TabStop = false;
            this.FileGroup.Text = "MSAccess Files";
            // 
            // btnDestinationFile
            // 
            this.btnDestinationFile.Location = new System.Drawing.Point(798, 81);
            this.btnDestinationFile.Name = "btnDestinationFile";
            this.btnDestinationFile.Size = new System.Drawing.Size(103, 33);
            this.btnDestinationFile.TabIndex = 7;
            this.btnDestinationFile.Text = "Select";
            this.btnDestinationFile.UseVisualStyleBackColor = true;
            this.btnDestinationFile.Click += new System.EventHandler(this.btnDestinationFile_Click);
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Location = new System.Drawing.Point(797, 28);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(103, 34);
            this.btnSourceFile.TabIndex = 6;
            this.btnSourceFile.Text = "Select";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // txtDestinationFile
            // 
            this.txtDestinationFile.Location = new System.Drawing.Point(220, 84);
            this.txtDestinationFile.Name = "txtDestinationFile";
            this.txtDestinationFile.ReadOnly = true;
            this.txtDestinationFile.Size = new System.Drawing.Size(571, 26);
            this.txtDestinationFile.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Destination DB File";
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(220, 32);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.ReadOnly = true;
            this.txtSourceFile.Size = new System.Drawing.Size(571, 26);
            this.txtSourceFile.TabIndex = 3;
            // 
            // migrationProgressBar
            // 
            this.migrationProgressBar.Location = new System.Drawing.Point(45, 229);
            this.migrationProgressBar.Name = "migrationProgressBar";
            this.migrationProgressBar.Size = new System.Drawing.Size(931, 29);
            this.migrationProgressBar.TabIndex = 4;
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Location = new System.Drawing.Point(45, 160);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(154, 45);
            this.btnAnalyse.TabIndex = 5;
            this.btnAnalyse.Text = "Analyse";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // destinationText
            // 
            this.destinationText.Location = new System.Drawing.Point(870, 298);
            this.destinationText.Name = "destinationText";
            this.destinationText.Size = new System.Drawing.Size(684, 200);
            this.destinationText.TabIndex = 6;
            this.destinationText.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 275);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Source";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(866, 275);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Destination/Report";
            // 
            // chkSQLMigration
            // 
            this.chkSQLMigration.AutoSize = true;
            this.chkSQLMigration.Enabled = false;
            this.chkSQLMigration.Location = new System.Drawing.Point(248, 171);
            this.chkSQLMigration.Name = "chkSQLMigration";
            this.chkSQLMigration.Size = new System.Drawing.Size(142, 24);
            this.chkSQLMigration.TabIndex = 9;
            this.chkSQLMigration.Text = "Migrate to SQL";
            this.chkSQLMigration.UseVisualStyleBackColor = true;
            this.chkSQLMigration.CheckedChanged += new System.EventHandler(this.chkSQLMigration_CheckedChanged);
            // 
            // PopupMenu
            // 
            this.PopupMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.PopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearProcess});
            this.PopupMenu.Name = "contextMenuStrip1";
            this.PopupMenu.Size = new System.Drawing.Size(260, 34);
            this.PopupMenu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.contextMenuStrip1_MouseClick);
            // 
            // ClearProcess
            // 
            this.ClearProcess.Name = "ClearProcess";
            this.ClearProcess.Size = new System.Drawing.Size(259, 30);
            this.ClearProcess.Text = "Clear Access Process";
            this.ClearProcess.Click += new System.EventHandler(this.ClearProcess_Click);
            // 
            // gvAnalysis
            // 
            this.gvAnalysis.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAnalysis.Location = new System.Drawing.Point(41, 514);
            this.gvAnalysis.MultiSelect = false;
            this.gvAnalysis.Name = "gvAnalysis";
            this.gvAnalysis.ReadOnly = true;
            this.gvAnalysis.RowTemplate.Height = 28;
            this.gvAnalysis.Size = new System.Drawing.Size(693, 358);
            this.gvAnalysis.TabIndex = 10;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(563, 160);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 45);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gvDestination
            // 
            this.gvDestination.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDestination.Location = new System.Drawing.Point(870, 514);
            this.gvDestination.Name = "gvDestination";
            this.gvDestination.ReadOnly = true;
            this.gvDestination.RowTemplate.Height = 28;
            this.gvDestination.Size = new System.Drawing.Size(684, 358);
            this.gvDestination.TabIndex = 12;
            // 
            // frmMSAccessMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1741, 903);
            this.Controls.Add(this.gvDestination);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.gvAnalysis);
            this.Controls.Add(this.chkSQLMigration);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.destinationText);
            this.Controls.Add(this.btnAnalyse);
            this.Controls.Add(this.migrationProgressBar);
            this.Controls.Add(this.FileGroup);
            this.Controls.Add(this.sourceText);
            this.Controls.Add(this.btnMigrate);
            this.Name = "frmMSAccessMigration";
            this.Text = "MSAccessMigration";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FileGroup.ResumeLayout(false);
            this.FileGroup.PerformLayout();
            this.PopupMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDestination)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.RichTextBox sourceText;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox FileGroup;
        private System.Windows.Forms.TextBox txtDestinationFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Button btnDestinationFile;
        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.ProgressBar migrationProgressBar;
        private System.Windows.Forms.Button btnAnalyse;
        private System.Windows.Forms.RichTextBox destinationText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkSQLMigration;
        private System.Windows.Forms.ContextMenuStrip PopupMenu;
        private System.Windows.Forms.ToolStripMenuItem ClearProcess;
        private System.Windows.Forms.DataGridView gvAnalysis;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView gvDestination;
    }
}

