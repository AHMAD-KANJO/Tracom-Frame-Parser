using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Frame_Parser
{

    public partial class SpikeDetectionForm : Form
    {
        private List<Frame> allFrames;
        private List<SpikeDetectionResult> detectedSpikes;

        public SpikeDetectionForm(List<Frame> frames)
        {
            InitializeComponent();
            allFrames = frames ?? new List<Frame>();
            detectedSpikes = new List<SpikeDetectionResult>();
        }

        private void SpikeDetectionForm_Load(object sender, EventArgs e)
        {
            InitializeFrameTypeComboBox();
            InitializeDataGrid();
            UpdateExportButton();
        }

        private void InitializeFrameTypeComboBox()
        {
            cmbFrameType.Items.Clear();

            if (allFrames.OfType<MainFrame>().Any())
                cmbFrameType.Items.Add(new ComboBoxItem("Main Frames", FrameType.MainFrame));

            if (allFrames.OfType<OBD2Frame>().Any())
                cmbFrameType.Items.Add(new ComboBoxItem("OBD2 Frames", FrameType.OBD2Frame));

            if (cmbFrameType.Items.Count > 0)
                cmbFrameType.SelectedIndex = 0;
        }

        private void cmbFrameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeFieldComboBox();
        }

        private void InitializeFieldComboBox()
        {
            cmbField.Items.Clear();

            if (cmbFrameType.SelectedItem is ComboBoxItem selectedItem)
            {
                var frameType = (FrameType)selectedItem.Value;

                if (frameType == FrameType.MainFrame)
                {
                    AddMainFrameFields();
                }
                else if (frameType == FrameType.OBD2Frame)
                {
                    AddOBD2FrameFields();
                }

                if (cmbField.Items.Count > 0)
                    cmbField.SelectedIndex = 0;
            }
        }

        private void AddMainFrameFields()
        {
            cmbField.Items.Add(new ComboBoxItem("Timer 1", "Timer1"));
            cmbField.Items.Add(new ComboBoxItem("Timer 2", "Timer2"));
            cmbField.Items.Add(new ComboBoxItem("Digital Inputs", "Inputs"));
            cmbField.Items.Add(new ComboBoxItem("Analog Input 1", "AnalogInput1"));
            cmbField.Items.Add(new ComboBoxItem("Analog Input 2", "AnalogInput2"));
            cmbField.Items.Add(new ComboBoxItem("Analog Input 3", "AnalogInput3"));
            cmbField.Items.Add(new ComboBoxItem("Analog Input 4", "AnalogInput4"));
            cmbField.Items.Add(new ComboBoxItem("Accelerometer X", "AccX"));
            cmbField.Items.Add(new ComboBoxItem("Accelerometer Y", "AccY"));
            cmbField.Items.Add(new ComboBoxItem("Accelerometer Z", "AccZ"));
            cmbField.Items.Add(new ComboBoxItem("Gyroscope X", "GyroX"));
            cmbField.Items.Add(new ComboBoxItem("Gyroscope Y", "GyroY"));
            cmbField.Items.Add(new ComboBoxItem("Gyroscope Z", "GyroZ"));
            cmbField.Items.Add(new ComboBoxItem("Vehicle Battery Voltage", "VehicleBatteryVoltage"));
            cmbField.Items.Add(new ComboBoxItem("Internal Battery Voltage", "InternalBatteryVoltage"));
            cmbField.Items.Add(new ComboBoxItem("Board Temperature", "BoardTemperature"));
            cmbField.Items.Add(new ComboBoxItem("Battery Temperature", "BatteryTemperature"));
            cmbField.Items.Add(new ComboBoxItem("Tamper Sensor", "TamperSensor"));
        }

        private void AddOBD2FrameFields()
        {
            cmbField.Items.Add(new ComboBoxItem("Speed", "Speed"));
            cmbField.Items.Add(new ComboBoxItem("RPM", "RPM"));
            cmbField.Items.Add(new ComboBoxItem("Actual RPM", "ActualRPM"));
            cmbField.Items.Add(new ComboBoxItem("Engine Temperature", "Temperature"));
            cmbField.Items.Add(new ComboBoxItem("Fuel Tank Level", "FuelTankLevel"));
            cmbField.Items.Add(new ComboBoxItem("Odometer", "Odometer"));
        }

        private void InitializeDataGrid()
        {
            gridSpikes.Columns.Clear();

            gridSpikes.Columns.Add("Timestamp", "Timestamp");
            gridSpikes.Columns.Add("FrameType", "Frame Type");
            gridSpikes.Columns.Add("FieldName", "Field");
            gridSpikes.Columns.Add("CurrentValue", "Current Value");
            gridSpikes.Columns.Add("PreviousValue", "Previous Value");
            gridSpikes.Columns.Add("NextValue", "Next Value");
            gridSpikes.Columns.Add("Change", "Change");
            gridSpikes.Columns.Add("Threshold", "Threshold");
            gridSpikes.Columns.Add("TimeDifference", "Time Difference");

            // Format numeric columns
            gridSpikes.Columns["Change"].DefaultCellStyle.Format = "F1";
            gridSpikes.Columns["Change"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridSpikes.Columns["Threshold"].DefaultCellStyle.Format = "F1";
            gridSpikes.Columns["Threshold"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridSpikes.Columns["CurrentValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridSpikes.Columns["PreviousValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void btnDetectSpikes_Click(object sender, EventArgs e)
        {
            if (cmbFrameType.SelectedItem == null || cmbField.SelectedItem == null)
            {
                MessageBox.Show("Please select both frame type and field.", "Selection Required",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ShowProgress(true, "Detecting spikes...");
                detectedSpikes.Clear();
                gridSpikes.Rows.Clear();

                var frameType = (FrameType)((ComboBoxItem)cmbFrameType.SelectedItem).Value;
                var fieldName = ((ComboBoxItem)cmbField.SelectedItem).Value.ToString();
                var threshold = (double)numThreshold.Value;

                var frames = allFrames.Where(f => f.Type == frameType).ToList();

                if (frames.Count < 2)
                {
                    MessageBox.Show("Not enough frames of selected type for spike detection.", "Insufficient Data",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Sort frames by timestamp
                frames = frames.OrderBy(f => f.Timestamp).ToList();

                DetectSpikesInFrames(frames, fieldName, threshold);
                DisplayDetectedSpikes();

                MessageBox.Show($"Detected {detectedSpikes.Count} spikes in {frames.Count} frames.", "Spike Detection Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detecting spikes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowProgress(false);
                UpdateExportButton();
            }
        }

        private void DetectSpikesInFrames(List<Frame> frames, string fieldName, double threshold)
        {
            var previousFrame = frames[0];
            double previousValue = GetFieldValue(previousFrame, fieldName);
            for (int i = 1; i < frames.Count; i++)
            {
                var currentFrame = frames[i];
                

                double currentValue = GetFieldValue(currentFrame, fieldName);
                

                // Skip if previous value is zero to avoid division by zero and infinite percentages
                if (previousValue == 0) continue;

                double change = Math.Abs((currentValue - previousValue));
                double changePercent = change / previousValue * 100;
                var timeDifference = currentFrame.Timestamp - previousFrame.Timestamp;

                if (change >= threshold)
                {
                    var NextFrame = frames[i+1];
                    double NextValue = GetFieldValue(NextFrame, fieldName);

                    if(Math.Abs((NextValue - currentValue)) >= threshold)
                    detectedSpikes.Add(new SpikeDetectionResult
                    {
                        Frame = currentFrame,
                        FieldName = fieldName,
                        CurrentValue = currentValue,
                        PreviousValue = previousValue,
                        NextValue = NextValue,
                        Change = change, // changePercent,
                        Threshold = threshold,
                        TimeDifference = timeDifference,
                        PreviousFrame = previousFrame
                    });
                }

                previousFrame = currentFrame;
                previousValue = currentValue;

                // Update progress
                if (i % 10 == 0 || i == frames.Count - 1)
                {
                    int progress = (int)((double)i / frames.Count * 100);
                    UpdateProgress(progress, $"Processing frame {i + 1} of {frames.Count}");
                }
            }
        }

        private double GetFieldValue(Frame frame, string fieldName)
        {
            if (frame is MainFrame mainFrame)
            {
                return fieldName switch
                {
                    "Timer1" => mainFrame.Timer1,
                    "Timer2" => mainFrame.Timer2,
                    "Inputs" => mainFrame.Inputs,
                    "AnalogInput1" => mainFrame.AnalogInput1,
                    "AnalogInput2" => mainFrame.AnalogInput2,
                    "AnalogInput3" => mainFrame.AnalogInput3,
                    "AnalogInput4" => mainFrame.AnalogInput4,
                    "AccX" => mainFrame.AccX,
                    "AccY" => mainFrame.AccY,
                    "AccZ" => mainFrame.AccZ,
                    "GyroX" => mainFrame.GyroX,
                    "GyroY" => mainFrame.GyroY,
                    "GyroZ" => mainFrame.GyroZ,
                    "VehicleBatteryVoltage" => mainFrame.VehicleBatteryVoltage,
                    "InternalBatteryVoltage" => mainFrame.InternalBatteryVoltage,
                    "BoardTemperature" => mainFrame.BoardTemperature,
                    "BatteryTemperature" => mainFrame.BatteryTemperature,
                    "TamperSensor" => mainFrame.TamperSensor,
                    _ => 0
                };
            }
            else if (frame is OBD2Frame obd2Frame)
            {
                return fieldName switch
                {
                    "Speed" => obd2Frame.Speed,
                    "RPM" => obd2Frame.RPM,
                    "ActualRPM" => obd2Frame.ActualRPM,
                    "Temperature" => obd2Frame.Temperature,
                    "FuelTankLevel" => obd2Frame.FuelTankLevel,
                    "Odometer" => obd2Frame.Odometer,
                    _ => 0
                };
            }

            return 0;
        }

        private void DisplayDetectedSpikes()
        {
            gridSpikes.Rows.Clear();

            foreach (var spike in detectedSpikes.OrderByDescending(s => s.Change))
            {
                gridSpikes.Rows.Add(
                    spike.Frame.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    spike.Frame.Type.ToString(),
                    GetDisplayFieldName(spike.FieldName),
                    spike.CurrentValue.ToString("F2"),
                    spike.PreviousValue.ToString("F2"),
                    spike.NextValue.ToString("F2"),
                    spike.Change,
                    spike.Threshold,
                    spike.TimeDifference.ToString(@"hh\:mm\:ss\.fff")
                );
            }

            lblResults.Text = $"Detected Spikes: {detectedSpikes.Count}";
        }

        private string GetDisplayFieldName(string fieldName)
        {
            // Convert property names to display names
            return fieldName switch
            {
                "Timer1" => "Timer 1",
                "Timer2" => "Timer 2",
                "Inputs" => "Digital Inputs",
                "AnalogInput1" => "Analog Input 1",
                "AnalogInput2" => "Analog Input 2",
                "AnalogInput3" => "Analog Input 3",
                "AnalogInput4" => "Analog Input 4",
                "AccX" => "Accelerometer X",
                "AccY" => "Accelerometer Y",
                "AccZ" => "Accelerometer Z",
                "GyroX" => "Gyroscope X",
                "GyroY" => "Gyroscope Y",
                "GyroZ" => "Gyroscope Z",
                "VehicleBatteryVoltage" => "Vehicle Battery Voltage",
                "InternalBatteryVoltage" => "Internal Battery Voltage",
                "BoardTemperature" => "Board Temperature",
                "BatteryTemperature" => "Battery Temperature",
                "TamperSensor" => "Tamper Sensor",
                "ActualRPM" => "Actual RPM",
                "FuelTankLevel" => "Fuel Tank Level",
                _ => fieldName
            };
        }

        private void gridSpikes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < detectedSpikes.Count)
            {
                var spike = detectedSpikes[e.RowIndex];
                ShowSpikeDetails(spike);
            }
        }

        private void ShowSpikeDetails(SpikeDetectionResult spike)
        {
            var details = new StringBuilder();
            details.AppendLine($"SPIKE DETECTION DETAILS");
            details.AppendLine($"=======================");
            details.AppendLine($"Field: {GetDisplayFieldName(spike.FieldName)}");
            details.AppendLine($"Frame Type: {spike.Frame.Type}");
            details.AppendLine($"Timestamp: {spike.Frame.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            details.AppendLine($"Previous Timestamp: {spike.PreviousFrame.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            details.AppendLine($"Time Difference: {spike.TimeDifference:hh\\:mm\\:ss\\.fff}");
            details.AppendLine();
            details.AppendLine($"VALUES:");
            details.AppendLine($"Previous Value: {spike.PreviousValue:F2}");
            details.AppendLine($"Current Value: {spike.CurrentValue:F2}");
            details.AppendLine($"Absolute Change: {Math.Abs(spike.CurrentValue - spike.PreviousValue):F2}");
            details.AppendLine($"Change Percentage: {spike.Change:F1}%");
            details.AppendLine($"Threshold: {spike.Threshold:F1}%");
            details.AppendLine();
            details.AppendLine($"RAW DATA:");
            details.AppendLine($"Current Frame: {spike.Frame.RawData}");
            details.AppendLine($"Previous Frame: {spike.PreviousFrame.RawData}");

            MessageBox.Show(details.ToString(), "Spike Details",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportSpikes_Click(object sender, EventArgs e)
        {
            if (detectedSpikes.Count == 0)
            {
                MessageBox.Show("No spikes to export.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            saveFileDialog.FileName = $"spikes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ShowProgress(true, "Exporting spikes...");
                    ExportSpikesToCsv(saveFileDialog.FileName);
                    MessageBox.Show($"Successfully exported {detectedSpikes.Count} spikes to {saveFileDialog.FileName}",
                                  "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting spikes: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ShowProgress(false);
                }
            }
        }

        private void ExportSpikesToCsv(string filePath)
        {
            var csv = new StringBuilder();

            // Header
            csv.AppendLine("Timestamp;FrameType;FieldName;CurrentValue;PreviousValue;NextValue;ChangePercent;Threshold;TimeDifference;CurrentFrameData;PreviousFrameData");

            // Data rows
            for (int i = 0; i < detectedSpikes.Count; i++)
            {
                var spike = detectedSpikes[i];
                var line = string.Join(";",
                    spike.Frame.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    spike.Frame.Type.ToString(),
                    GetDisplayFieldName(spike.FieldName),
                    spike.CurrentValue.ToString("F2", CultureInfo.InvariantCulture),
                    spike.PreviousValue.ToString("F2", CultureInfo.InvariantCulture),
                    spike.NextValue.ToString("F2", CultureInfo.InvariantCulture),
                    spike.Change.ToString("F1", CultureInfo.InvariantCulture),
                    spike.Threshold.ToString("F1", CultureInfo.InvariantCulture),
                    spike.TimeDifference.ToString(@"hh\:mm\:ss\.fff"),
                    EscapeCsvField(spike.Frame.RawData),
                    EscapeCsvField(spike.PreviousFrame.RawData)
                );
                csv.AppendLine(line);

                // Update progress
                if (detectedSpikes.Count > 100 && i % 10 == 0)
                {
                    int progress = (int)((i + 1) / (double)detectedSpikes.Count * 100);
                    UpdateProgress(progress, $"Exporting {i + 1} of {detectedSpikes.Count}");
                }
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        private string EscapeCsvField(string field)
        {
            if (field == null) return string.Empty;

            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }

        private void ShowProgress(bool show, string message = "")
        {
            progressBar.Visible = show;
            lblProgress.Visible = show;
            lblProgress.Text = message;

            if (show)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }

            btnDetectSpikes.Enabled = !show;
            btnExportSpikes.Enabled = !show && detectedSpikes.Count > 0;
            Application.DoEvents();
        }

        private void UpdateProgress(int percent, string message)
        {
            if (progressBar.Visible)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = percent;
                lblProgress.Text = message;
                Application.DoEvents();
            }
        }

        private void UpdateExportButton()
        {
            btnExportSpikes.Enabled = detectedSpikes.Count > 0;
        }
    }

    // Helper class for spike detection results
    public class SpikeDetectionResult
    {
        public Frame Frame { get; set; }
        public Frame PreviousFrame { get; set; }
        public string FieldName { get; set; }
        public double CurrentValue { get; set; }
        public double PreviousValue { get; set; }
        public double NextValue { get; set; }
        public double Change { get; set; }
        public double Threshold { get; set; }
        public TimeSpan TimeDifference { get; set; }
    }

    // Helper class for ComboBox items
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboBoxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}


