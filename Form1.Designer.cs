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
            this.components = new System.ComponentModel.Container();
            this.btnLoadFile = new Button();
            this.txtFilePath = new TextBox();
            this.lblFilePath = new Label();
            this.lstFrames = new ListBox();
            this.lblFrames = new Label();
            this.txtFrameDetails = new TextBox();
            this.lblDetails = new Label();
            this.btnParse = new Button();
            this.openFileDialog = new OpenFileDialog();

            SuspendLayout();

            // lblFilePath
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new Point(12, 15);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new Size(51, 15);
            this.lblFilePath.Text = "Log File:";

            // txtFilePath
            this.txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtFilePath.Location = new Point(69, 12);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new Size(450, 23);
            this.txtFilePath.TabIndex = 1;

            // btnLoadFile
            this.btnLoadFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLoadFile.Location = new Point(525, 12);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new Size(75, 23);
            this.btnLoadFile.TabIndex = 2;
            this.btnLoadFile.Text = "Browse";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new EventHandler(this.btnLoadFile_Click);

            // lstFrames
            this.lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.lstFrames.FormattingEnabled = true;
            this.lstFrames.ItemHeight = 15;
            this.lstFrames.Location = new Point(12, 70);
            this.lstFrames.Name = "lstFrames";
            this.lstFrames.Size = new Size(250, 364);
            this.lstFrames.TabIndex = 3;
            this.lstFrames.SelectedIndexChanged += new EventHandler(this.lstFrames_SelectedIndexChanged);

            // lblFrames
            this.lblFrames.AutoSize = true;
            this.lblFrames.Location = new Point(12, 52);
            this.lblFrames.Name = "lblFrames";
            this.lblFrames.Size = new Size(81, 15);
            this.lblFrames.Text = "Parsed Frames:";

            // txtFrameDetails
            this.txtFrameDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtFrameDetails.Location = new Point(268, 70);
            this.txtFrameDetails.Multiline = true;
            this.txtFrameDetails.Name = "txtFrameDetails";
            this.txtFrameDetails.ReadOnly = true;
            this.txtFrameDetails.ScrollBars = ScrollBars.Vertical;
            this.txtFrameDetails.Size = new Size(332, 364);
            this.txtFrameDetails.TabIndex = 4;

            // lblDetails
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new Point(268, 52);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new Size(85, 15);
            this.lblDetails.Text = "Frame Details:";

            // btnParse
            this.btnParse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnParse.Location = new Point(525, 41);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new Size(75, 23);
            this.btnParse.TabIndex = 5;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new EventHandler(this.btnParse_Click);

            // openFileDialog
            this.openFileDialog.Filter = "Log files (*.log;*.txt)|*.log;*.txt|All files (*.*)|*.*";
            this.openFileDialog.Title = "Select Log File";

            // Form1
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(612, 450);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.txtFrameDetails);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lstFrames);
            this.Controls.Add(this.lblFrames);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFilePath);
            this.Name = "Form1";
            this.Text = "MCU Log Parser";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
