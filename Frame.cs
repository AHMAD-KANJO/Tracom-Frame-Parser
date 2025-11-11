using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Frame_Parser
{

    public enum FrameType
    {
        MainFrame = 0x00,
        OBD2Frame = 0x0D
    }

    public abstract class Frame
    {
        public DateTime Timestamp { get; set; }
        public FrameType Type { get; set; }
        public ushort PayloadLength { get; set; }
        public byte Checksum { get; set; }
        public string RawData { get; set; }
    }

    public class MainFrame : Frame
    {
        public ushort Timer1 { get; set; }
        public ushort Timer2 { get; set; }
        public ushort Inputs { get; set; }
        public ushort AnalogInput1 { get; set; }
        public ushort AnalogInput2 { get; set; }
        public ushort AnalogInput3 { get; set; }
        public ushort AnalogInput4 { get; set; }
        public short AccX { get; set; }
        public short AccY { get; set; }
        public short AccZ { get; set; }
        public short GyroX { get; set; }
        public short GyroY { get; set; }
        public short GyroZ { get; set; }
        public ulong OneWire { get; set; }
        public ushort VehicleBatteryVoltage { get; set; }
        public ushort InternalBatteryVoltage { get; set; }
        public ushort BoardTemperature { get; set; }
        public ushort BatteryTemperature { get; set; }
        public ushort TamperSensor { get; set; }
    }

    public class OBD2Frame : Frame
    {
        public byte Speed { get; set; }
        public ushort RPM { get; set; }
        public double ActualRPM => RPM / 4.0;
        public byte Temperature { get; set; }
        public byte FuelTankLevel { get; set; }
        public ulong EngineTotalHours { get; set; }
        public uint Odometer { get; set; }
        public uint DTC { get; set; }
    }

    public class LogParser
    {
        public List<Frame> ParseLogFile(string filePath)
        {
            var frames = new List<Frame>();

            try
            {
                var lines = File.ReadAllLines(filePath);
                Program.MainForm.progressBar1.Show();
                Program.MainForm.progressBar1.Maximum = lines.Length;
                foreach (var line in lines)
                {
                    try
                    {
                        var frame = ParseLine(line);
                        if (frame != null)
                        {
                            frames.Add(frame);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Continue parsing other lines if one fails
                        System.Diagnostics.Debug.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                    }
                    Program.MainForm.progressBar1.Value = frames.Count;
                    
                    Application.DoEvents(); // Allow UI to update
                }
                Program.MainForm.progressBar1.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading file: {ex.Message}", ex);
            }

            return frames;
        }

        private Frame ParseLine(string line)
        {
            var parts = line.Split(':');
            if (parts.Length < 3) return null;

            var timestampStr = parts[1].Trim();
            var frameData = parts[2].Trim();

            var timestamp = ParseTimestamp(timestampStr);

            if (!frameData.StartsWith("$$") || !frameData.EndsWith("<$>"))
                return null;

            var frameContent = frameData.Substring(2, frameData.Length - 5);

            var frameType = (FrameType)HexToByte(frameContent.Substring(0, 2));
            var payloadLength = HexToUInt16(frameContent.Substring(2, 4));

            var payloadHex = frameContent.Substring(6, payloadLength * 2);
            var checksumHex = frameContent.Substring(6 + payloadLength * 2, 2);
            var checksum = HexToByte(checksumHex);

            Frame frame;

            switch (frameType)
            {
                case FrameType.MainFrame:
                    frame = ParseMainFrame(payloadHex);
                    break;
                case FrameType.OBD2Frame:
                    frame = ParseOBD2Frame(payloadHex);
                    break;
                default:
                    return null;
            }

            frame.Timestamp = timestamp;
            frame.Type = frameType;
            frame.PayloadLength = payloadLength;
            frame.Checksum = checksum;
            frame.RawData = frameData;

            return frame;
        }

        private DateTime ParseTimestamp(string timestampStr)
        {
            var format = "yyyy-MM-dd_HH-mm-ss.fff";
            return DateTime.ParseExact(timestampStr, format, CultureInfo.InvariantCulture);
        }

        private MainFrame ParseMainFrame(string payloadHex)
        {
            var frame = new MainFrame();
            var index = 0;

            frame.Timer1 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.Timer2 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.Inputs = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AnalogInput1 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AnalogInput2 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AnalogInput3 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AnalogInput4 = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AccX = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AccY = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.AccZ = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.GyroX = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.GyroY = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.GyroZ = HexToInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.OneWire = HexToUInt64(GetHexSubstring(payloadHex, ref index, 8));
            frame.VehicleBatteryVoltage = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.InternalBatteryVoltage = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.BoardTemperature = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.BatteryTemperature = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.TamperSensor = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));

            return frame;
        }

        private OBD2Frame ParseOBD2Frame(string payloadHex)
        {
            var frame = new OBD2Frame();
            var index = 0;

            frame.Speed = HexToByte(GetHexSubstring(payloadHex, ref index, 1));
            frame.RPM = HexToUInt16(GetHexSubstring(payloadHex, ref index, 2));
            frame.Temperature = HexToByte(GetHexSubstring(payloadHex, ref index, 1));
            frame.FuelTankLevel = HexToByte(GetHexSubstring(payloadHex, ref index, 1));
            frame.EngineTotalHours = HexToUInt64(GetHexSubstring(payloadHex, ref index, 8));
            frame.Odometer = HexToUInt32(GetHexSubstring(payloadHex, ref index, 4));
            frame.DTC = HexToUInt32(GetHexSubstring(payloadHex, ref index, 4));

            return frame;
        }

        private string GetHexSubstring(string hexString, ref int index, int bytes)
        {
            var substring = hexString.Substring(index, bytes * 2);
            index += bytes * 2;
            return substring;
        }

        private byte HexToByte(string hex) => byte.Parse(hex, NumberStyles.HexNumber);
        private ushort HexToUInt16(string hex) => ushort.Parse(hex, NumberStyles.HexNumber);
        private short HexToInt16(string hex) => short.Parse(hex, NumberStyles.HexNumber);
        private uint HexToUInt32(string hex) => uint.Parse(hex, NumberStyles.HexNumber);
        private ulong HexToUInt64(string hex) => ulong.Parse(hex, NumberStyles.HexNumber);
    }


}
