using System.Globalization;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Frame_Parser
{
    public partial class Form1 : Form
    {
        private List<Frame> parsedFrames = new List<Frame>();

        public Form1()
        {
            InitializeComponent();
            UpdateExportButtons();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid log file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnParse.Enabled = false;
            parsedFrames.Clear();
            lstFrames.Items.Clear();
            txtFrameDetails.Clear();
            UpdateExportButtons();

            try
            {
                var parser = new LogParser();
                parsedFrames = parser.ParseLogFile(txtFilePath.Text);

                // Display frames in listbox
                lstFrames.Items.Clear();
                Program.MainForm.progressBar1.Show();
                Program.MainForm.progressBar1.Maximum = parsedFrames.Count;
                foreach (var frame in parsedFrames)
                {
                    string frameType = frame.Type == FrameType.MainFrame ? "Main" : "OBD2";
                    lstFrames.Items.Add($"{frame.Timestamp:HH:mm:ss.fff} - {frameType} Frame");
                    Program.MainForm.progressBar1.Value = lstFrames.Items.Count;
                    Application.DoEvents(); // Allow UI to update
                }
                Program.MainForm.progressBar1.Hide();
                

                if (parsedFrames.Count > 0)
                {
                    lstFrames.SelectedIndex = 0;
                    MessageBox.Show($"Successfully parsed {parsedFrames.Count} frames.", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No valid frames found in the log file.", "Warning",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                UpdateExportButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing file: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnParse.Enabled = true;
        }

        private void UpdateExportButtons()
        {
            bool hasFrames = parsedFrames.Count > 0;
            bool hasMainFrames = parsedFrames.OfType<MainFrame>().Any();
            bool hasOBD2Frames = parsedFrames.OfType<OBD2Frame>().Any();

            btnExportMainFrames.Enabled = hasFrames && hasMainFrames;
            btnExportOBD2Frames.Enabled = hasFrames && hasOBD2Frames;
            btnDifTimerCSV.Enabled = btnExportMainFrames.Enabled;
        }

        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFrames.SelectedIndex >= 0 && lstFrames.SelectedIndex < parsedFrames.Count)
            {
                var frame = parsedFrames[lstFrames.SelectedIndex];
                txtFrameDetails.Text = GetFrameDetails(frame);
            }
        }

        private void btnExportMainFrames_Click(object sender, EventArgs e)
        {
            ExportFrames<MainFrame>("main.csv", "Main Frames");
        }

        private void btnExportOBD2Frames_Click(object sender, EventArgs e)
        {
            ExportFrames<OBD2Frame>("obd2.csv", "OBD2 Frames");
        }

        private void ExportFrames<T>(string defaultFileName, string frameTypeName) where T : Frame
        {
            var frames = parsedFrames.OfType<T>().ToList();

            if (frames.Count == 0)
            {
                MessageBox.Show($"No {frameTypeName} found to export.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.Title = $"Export {frameTypeName} to CSV";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (typeof(T) == typeof(MainFrame))
                    {
                        ExportMainFramesToCsv(frames as List<MainFrame>, saveFileDialog.FileName);
                    }
                    else if (typeof(T) == typeof(OBD2Frame))
                    {
                        ExportOBD2FramesToCsv(frames as List<OBD2Frame>, saveFileDialog.FileName);
                    }

                    MessageBox.Show($"Successfully exported {frames.Count} {frameTypeName} to {saveFileDialog.FileName}",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting {frameTypeName}: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportMainFramesToCsv(List<MainFrame> frames, string filePath)
        {
            var csv = new StringBuilder();

            // Header
            csv.AppendLine("Timestamp;FrameType;PayloadLength;Checksum;Timer1;Timer2;Inputs;AnalogInput1;AnalogInput2;AnalogInput3;AnalogInput4;AccX;AccY;AccZ;GyroX;GyroY;GyroZ;OneWire;VehicleBatteryVoltage;InternalBatteryVoltage;BoardTemperature;BatteryTemperature;TamperSensor;RawData");

            // Data rows
            foreach (var frame in frames)
            {
                var line = string.Join(";",
                    frame.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    ((byte)frame.Type).ToString("X2"),
                    frame.PayloadLength,
                    frame.Checksum.ToString("X2"),
                    frame.Timer1.ToString("X4"),
                    frame.Timer2.ToString("X4"),
                    frame.Inputs.ToString("X4"),
                    frame.AnalogInput1.ToString("X4"),
                    frame.AnalogInput2.ToString("X4"),
                    frame.AnalogInput3.ToString("X4"),
                    frame.AnalogInput4.ToString("X4"),
                    frame.AccX,
                    frame.AccY,
                    frame.AccZ,
                    frame.GyroX,
                    frame.GyroY,
                    frame.GyroZ,
                    frame.OneWire.ToString("X16"),
                    frame.VehicleBatteryVoltage.ToString("X4"),
                    frame.InternalBatteryVoltage.ToString("X4"),
                    frame.BoardTemperature.ToString("X4"),
                    frame.BatteryTemperature.ToString("X4"),
                    frame.TamperSensor.ToString("X4"),
                    EscapeCsvField(frame.RawData)
                );
                csv.AppendLine(line);
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        private void ExportDifTimerToCsv(List<MainFrame> frames, string filePath)
        {
            var csv = new StringBuilder();

            // Header
            csv.AppendLine("Timestamp;Timer1;Timer2;Dif1;Dif2");
                var LastT1 = frames[0].Timer1;
                var LastT2 = frames[0].Timer2;

            // Data rows
            foreach (var frame in frames)
            {
                var line = string.Join(";",
                    frame.Timestamp.ToString("HH:mm:ss.fff"),

                    frame.Timer1.ToString(),
                    frame.Timer2.ToString(),
                    (frame.Timer1 - LastT1).ToString(),
                    (frame.Timer2 - LastT2).ToString()
                );
                csv.AppendLine(line);
                LastT1 = frame.Timer1;
                LastT2 = frame.Timer2;
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        private void ExportOBD2FramesToCsv(List<OBD2Frame> frames, string filePath)
        {
            var csv = new StringBuilder();

            // Header
            csv.AppendLine("Timestamp;FrameType;PayloadLength;Checksum;Speed;RPM;ActualRPM;Temperature;FuelTankLevel;EngineTotalHours;Odometer;DTC;RawData");

            // Data rows
            foreach (var frame in frames)
            {
                var line = string.Join(";",
                    frame.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    ((byte)frame.Type).ToString("X2"),
                    frame.PayloadLength,
                    frame.Checksum.ToString("X2"),
                    frame.Speed,
                    frame.RPM,
                    frame.ActualRPM.ToString("F1", CultureInfo.InvariantCulture),
                    frame.Temperature,
                    frame.FuelTankLevel,
                    frame.EngineTotalHours,
                    frame.Odometer,
                    frame.DTC.ToString("X8"),
                    EscapeCsvField(frame.RawData)
                );
                csv.AppendLine(line);
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        private string EscapeCsvField(string field)
        {
            if (field == null) return string.Empty;

            // If field contains delimiter (;) or quotes, wrap in quotes and escape existing quotes
            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }

        private string GetFrameDetails(Frame frame)
        {
            if (frame is MainFrame mainFrame)
            {
                return GetMainFrameDetails(mainFrame);
            }
            else if (frame is OBD2Frame obd2Frame)
            {
                return GetOBD2FrameDetails(obd2Frame);
            }

            return "Unknown frame type";
        }

        private string GetMainFrameDetails(MainFrame frame)
        {
            return $@"MAIN FRAME DETAILS
--------------------
Timestamp: {frame.Timestamp:yyyy-MM-dd HH:mm:ss.fff}
Frame Type: 0x{((byte)frame.Type):X2} (Main Frame)
Payload Length: {frame.PayloadLength} bytes
Checksum: 0x{frame.Checksum:X2}

Raw Data: {frame.RawData}

FIELD DATA:
----------
Timer 1: 0x{frame.Timer1:X4}
Timer 2: 0x{frame.Timer2:X4}
Digital Inputs: 0x{frame.Inputs:X4}
Analog Input 1: 0x{frame.AnalogInput1:X4}
Analog Input 2: 0x{frame.AnalogInput2:X4}
Analog Input 3: 0x{frame.AnalogInput3:X4}
Analog Input 4: 0x{frame.AnalogInput4:X4}
Accelerometer X: 0x{frame.AccX:X4} ({frame.AccX})
Accelerometer Y: 0x{frame.AccY:X4} ({frame.AccY})
Accelerometer Z: 0x{frame.AccZ:X4} ({frame.AccZ})
Gyroscope X: 0x{frame.GyroX:X4} ({frame.GyroX})
Gyroscope Y: 0x{frame.GyroY:X4} ({frame.GyroY})
Gyroscope Z: 0x{frame.GyroZ:X4} ({frame.GyroZ})
One Wire: 0x{frame.OneWire:X16}
Vehicle Battery Voltage: 0x{frame.VehicleBatteryVoltage:X4}
Internal Battery Voltage: 0x{frame.InternalBatteryVoltage:X4}
Board Temperature: 0x{frame.BoardTemperature:X4}
Battery Temperature: 0x{frame.BatteryTemperature:X4}
Tamper Sensor: 0x{frame.TamperSensor:X4}";
        }

        private string GetOBD2FrameDetails(OBD2Frame frame)
        {
            return $@"OBD2 FRAME DETAILS
--------------------
Timestamp: {frame.Timestamp:yyyy-MM-dd HH:mm:ss.fff}
Frame Type: 0x{((byte)frame.Type):X2} (OBD2 Frame)
Payload Length: {frame.PayloadLength} bytes
Checksum: 0x{frame.Checksum:X2}

Raw Data: {frame.RawData}

VEHICLE DATA:
------------
Speed: {frame.Speed} km/h
RPM: {frame.RPM} (Actual: {frame.ActualRPM:F1} RPM)
Engine Temperature: {frame.Temperature}°C
Fuel Tank Level: {frame.FuelTankLevel}%
Engine Total Hours: {frame.EngineTotalHours} seconds
Odometer: {frame.Odometer:N0} km
Diagnostic Trouble Code: 0x{frame.DTC:X8}";
        }

        private void btnDifTimerCSV_Click(object sender, EventArgs e)
        {
            var frames = parsedFrames.OfType<MainFrame>().ToList();
            string frameTypeName = "Timer_Dif";
            saveFileDialog.FileName = "TimeDif.csv";
            saveFileDialog.Title = $"Export {frameTypeName} to CSV";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportDifTimerToCsv(frames as List<MainFrame>, saveFileDialog.FileName);

                    MessageBox.Show($"Successfully exported {frames.Count} {frameTypeName} to {saveFileDialog.FileName}",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting {frameTypeName}: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

}
