using System.Windows.Forms;

namespace Frame_Parser
{

        public partial class Form1 : Form
        {
            private List<Frame> parsedFrames = new List<Frame>();

            public Form1()
            {
                InitializeComponent();
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

                try
                {
                    var parser = new LogParser();
                    parsedFrames = parser.ParseLogFile(txtFilePath.Text);

                    // Display frames in listbox
                    lstFrames.Items.Clear();
                    foreach (var frame in parsedFrames)
                    {
                        string frameType = frame.Type == FrameType.MainFrame ? "Main" : "OBD2";
                        lstFrames.Items.Add($"{frame.Timestamp:HH:mm:ss.fff} - {frameType} Frame");
                    }

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing file: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void lstFrames_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (lstFrames.SelectedIndex >= 0 && lstFrames.SelectedIndex < parsedFrames.Count)
                {
                    var frame = parsedFrames[lstFrames.SelectedIndex];
                    txtFrameDetails.Text = GetFrameDetails(frame);
                }
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
        }
   
}
