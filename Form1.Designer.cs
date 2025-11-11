namespace Frame_Parser
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnLoadFile;
        private TextBox txtFilePath;
        private Label lblFilePath;
        private ListBox lstFrames;
        private Label lblFrames;
        private TextBox txtFrameDetails;
        private Label lblDetails;
        private Button btnParse;
        private OpenFileDialog openFileDialog;
        private Button btnExportMainFrames;
        private Button btnExportOBD2Frames;
        private SaveFileDialog saveFileDialog;
        private Button btnDifTimerCSV;
        public ProgressBar progressBar1;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnLoadFile = new Button();
            txtFilePath = new TextBox();
            lblFilePath = new Label();
            lstFrames = new ListBox();
            lblFrames = new Label();
            txtFrameDetails = new TextBox();
            lblDetails = new Label();
            btnParse = new Button();
            openFileDialog = new OpenFileDialog();
            btnExportMainFrames = new Button();
            btnExportOBD2Frames = new Button();
            saveFileDialog = new SaveFileDialog();
            btnDifTimerCSV = new Button();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // btnLoadFile
            // 
            btnLoadFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadFile.Location = new Point(600, 16);
            btnLoadFile.Margin = new Padding(3, 4, 3, 4);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(86, 31);
            btnLoadFile.TabIndex = 2;
            btnLoadFile.Text = "Browse";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click;
            // 
            // txtFilePath
            // 
            txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFilePath.Location = new Point(79, 16);
            txtFilePath.Margin = new Padding(3, 4, 3, 4);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(514, 27);
            txtFilePath.TabIndex = 1;
            // 
            // lblFilePath
            // 
            lblFilePath.AutoSize = true;
            lblFilePath.Location = new Point(14, 20);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(64, 20);
            lblFilePath.TabIndex = 23;
            lblFilePath.Text = "Log File:";
            // 
            // lstFrames
            // 
            lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(14, 93);
            lstFrames.Margin = new Padding(3, 4, 3, 4);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(285, 444);
            lstFrames.TabIndex = 3;
            lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged;
            // 
            // lblFrames
            // 
            lblFrames.AutoSize = true;
            lblFrames.Location = new Point(14, 69);
            lblFrames.Name = "lblFrames";
            lblFrames.Size = new Size(106, 20);
            lblFrames.TabIndex = 22;
            lblFrames.Text = "Parsed Frames:";
            // 
            // txtFrameDetails
            // 
            txtFrameDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtFrameDetails.Location = new Point(306, 93);
            txtFrameDetails.Margin = new Padding(3, 4, 3, 4);
            txtFrameDetails.Multiline = true;
            txtFrameDetails.Name = "txtFrameDetails";
            txtFrameDetails.ReadOnly = true;
            txtFrameDetails.ScrollBars = ScrollBars.Vertical;
            txtFrameDetails.Size = new Size(379, 444);
            txtFrameDetails.TabIndex = 4;
            // 
            // lblDetails
            // 
            lblDetails.AutoSize = true;
            lblDetails.Location = new Point(306, 69);
            lblDetails.Name = "lblDetails";
            lblDetails.Size = new Size(103, 20);
            lblDetails.TabIndex = 21;
            lblDetails.Text = "Frame Details:";
            // 
            // btnParse
            // 
            btnParse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnParse.Location = new Point(600, 55);
            btnParse.Margin = new Padding(3, 4, 3, 4);
            btnParse.Name = "btnParse";
            btnParse.Size = new Size(86, 31);
            btnParse.TabIndex = 5;
            btnParse.Text = "Parse";
            btnParse.UseVisualStyleBackColor = true;
            btnParse.Click += btnParse_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Log files (*.log;*.txt)|*.log.*;*.log;*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Select Log File";
            // 
            // btnExportMainFrames
            // 
            btnExportMainFrames.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportMainFrames.Location = new Point(14, 553);
            btnExportMainFrames.Margin = new Padding(3, 4, 3, 4);
            btnExportMainFrames.Name = "btnExportMainFrames";
            btnExportMainFrames.Size = new Size(137, 31);
            btnExportMainFrames.TabIndex = 6;
            btnExportMainFrames.Text = "Export Main Frames";
            btnExportMainFrames.UseVisualStyleBackColor = true;
            btnExportMainFrames.Click += btnExportMainFrames_Click;
            // 
            // btnExportOBD2Frames
            // 
            btnExportOBD2Frames.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportOBD2Frames.Location = new Point(162, 553);
            btnExportOBD2Frames.Margin = new Padding(3, 4, 3, 4);
            btnExportOBD2Frames.Name = "btnExportOBD2Frames";
            btnExportOBD2Frames.Size = new Size(137, 31);
            btnExportOBD2Frames.TabIndex = 7;
            btnExportOBD2Frames.Text = "Export OBD2 Frames";
            btnExportOBD2Frames.UseVisualStyleBackColor = true;
            btnExportOBD2Frames.Click += btnExportOBD2Frames_Click;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            // 
            // btnDifTimerCSV
            // 
            btnDifTimerCSV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDifTimerCSV.Enabled = false;
            btnDifTimerCSV.Location = new Point(528, 553);
            btnDifTimerCSV.Margin = new Padding(3, 5, 3, 5);
            btnDifTimerCSV.Name = "btnDifTimerCSV";
            btnDifTimerCSV.Size = new Size(157, 31);
            btnDifTimerCSV.TabIndex = 20;
            btnDifTimerCSV.Text = "Export Dif Timer";
            btnDifTimerCSV.UseVisualStyleBackColor = true;
            btnDifTimerCSV.Click += btnDifTimerCSV_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(16, 50);
            progressBar1.Margin = new Padding(3, 4, 3, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(577, 8);
            progressBar1.TabIndex = 20;
            progressBar1.Value = 20;
            progressBar1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(699, 600);
            Controls.Add(btnExportOBD2Frames);
            Controls.Add(btnExportMainFrames);
            Controls.Add(btnParse);
            Controls.Add(btnDifTimerCSV);
            Controls.Add(txtFrameDetails);
            Controls.Add(lblDetails);
            Controls.Add(lstFrames);
            Controls.Add(lblFrames);
            Controls.Add(btnLoadFile);
            Controls.Add(txtFilePath);
            Controls.Add(lblFilePath);
            Controls.Add(progressBar1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "MCU Log Parser";
            ResumeLayout(false);
            PerformLayout();
        }

    }
}
