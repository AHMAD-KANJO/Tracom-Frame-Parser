namespace Frame_Parser
{
    partial class SpikeDetectionForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbFrameType;
        private Label lblFrameType;
        private ComboBox cmbField;
        private Label lblField;
        private NumericUpDown numThreshold;
        private Label lblThreshold;
        private Button btnDetectSpikes;
        private DataGridView gridSpikes;
        private Label lblResults;
        private Button btnExportSpikes;
        private SaveFileDialog saveFileDialog;
        private ProgressBar progressBar;
        private Label lblProgress;

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
            cmbFrameType = new ComboBox();
            lblFrameType = new Label();
            cmbField = new ComboBox();
            lblField = new Label();
            numThreshold = new NumericUpDown();
            lblThreshold = new Label();
            btnDetectSpikes = new Button();
            gridSpikes = new DataGridView();
            lblResults = new Label();
            btnExportSpikes = new Button();
            saveFileDialog = new SaveFileDialog();
            progressBar = new ProgressBar();
            lblProgress = new Label();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridSpikes).BeginInit();
            SuspendLayout();
            // 
            // cmbFrameType
            // 
            cmbFrameType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFrameType.FormattingEnabled = true;
            cmbFrameType.Location = new Point(97, 16);
            cmbFrameType.Margin = new Padding(3, 4, 3, 4);
            cmbFrameType.Name = "cmbFrameType";
            cmbFrameType.Size = new Size(171, 28);
            cmbFrameType.TabIndex = 1;
            cmbFrameType.SelectedIndexChanged += cmbFrameType_SelectedIndexChanged;
            // 
            // lblFrameType
            // 
            lblFrameType.AutoSize = true;
            lblFrameType.Location = new Point(14, 20);
            lblFrameType.Name = "lblFrameType";
            lblFrameType.Size = new Size(88, 20);
            lblFrameType.TabIndex = 12;
            lblFrameType.Text = "Frame Type:";
            // 
            // cmbField
            // 
            cmbField.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbField.FormattingEnabled = true;
            cmbField.Location = new Point(97, 56);
            cmbField.Margin = new Padding(3, 4, 3, 4);
            cmbField.Name = "cmbField";
            cmbField.Size = new Size(228, 28);
            cmbField.TabIndex = 2;
            // 
            // lblField
            // 
            lblField.AutoSize = true;
            lblField.Location = new Point(14, 60);
            lblField.Name = "lblField";
            lblField.Size = new Size(44, 20);
            lblField.TabIndex = 11;
            lblField.Text = "Field:";
            // 
            // numThreshold
            // 
            numThreshold.Location = new Point(153, 97);
            numThreshold.Margin = new Padding(3, 4, 3, 4);
            numThreshold.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(91, 27);
            numThreshold.TabIndex = 3;
            numThreshold.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.Location = new Point(14, 100);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(143, 20);
            lblThreshold.TabIndex = 10;
            lblThreshold.Text = "Spike Threshold (%):";
            // 
            // btnDetectSpikes
            // 
            btnDetectSpikes.Location = new Point(251, 96);
            btnDetectSpikes.Margin = new Padding(3, 4, 3, 4);
            btnDetectSpikes.Name = "btnDetectSpikes";
            btnDetectSpikes.Size = new Size(114, 33);
            btnDetectSpikes.TabIndex = 4;
            btnDetectSpikes.Text = "Detect Spikes";
            btnDetectSpikes.UseVisualStyleBackColor = true;
            btnDetectSpikes.Click += btnDetectSpikes_Click;
            // 
            // gridSpikes
            // 
            gridSpikes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridSpikes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridSpikes.ColumnHeadersHeight = 29;
            gridSpikes.Location = new Point(14, 173);
            gridSpikes.Margin = new Padding(3, 4, 3, 4);
            gridSpikes.Name = "gridSpikes";
            gridSpikes.ReadOnly = true;
            gridSpikes.RowHeadersWidth = 51;
            gridSpikes.Size = new Size(869, 467);
            gridSpikes.TabIndex = 5;
            gridSpikes.CellDoubleClick += gridSpikes_CellDoubleClick;
            // 
            // lblResults
            // 
            lblResults.AutoSize = true;
            lblResults.Location = new Point(14, 149);
            lblResults.Name = "lblResults";
            lblResults.Size = new Size(119, 20);
            lblResults.TabIndex = 9;
            lblResults.Text = "Detected Spikes:";
            // 
            // btnExportSpikes
            // 
            btnExportSpikes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportSpikes.Location = new Point(768, 653);
            btnExportSpikes.Margin = new Padding(3, 4, 3, 4);
            btnExportSpikes.Name = "btnExportSpikes";
            btnExportSpikes.Size = new Size(114, 33);
            btnExportSpikes.TabIndex = 6;
            btnExportSpikes.Text = "Export to CSV";
            btnExportSpikes.UseVisualStyleBackColor = true;
            btnExportSpikes.Click += btnExportSpikes_Click;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.Title = "Export Spikes to CSV";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(14, 653);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(747, 27);
            progressBar.TabIndex = 7;
            progressBar.Visible = false;
            // 
            // lblProgress
            // 
            lblProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(768, 656);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(0, 20);
            lblProgress.TabIndex = 8;
            lblProgress.Visible = false;
            // 
            // SpikeDetectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 703);
            Controls.Add(lblProgress);
            Controls.Add(progressBar);
            Controls.Add(btnExportSpikes);
            Controls.Add(lblResults);
            Controls.Add(gridSpikes);
            Controls.Add(btnDetectSpikes);
            Controls.Add(numThreshold);
            Controls.Add(lblThreshold);
            Controls.Add(cmbField);
            Controls.Add(lblField);
            Controls.Add(cmbFrameType);
            Controls.Add(lblFrameType);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(912, 738);
            Name = "SpikeDetectionForm";
            Text = "Spike Detection";
            Load += SpikeDetectionForm_Load;
            ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridSpikes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}