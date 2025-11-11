# Tracom Frames Parser Application

A Windows Forms application for parsing and analyzing log files from Microcontroller Units (MCUs) that contain multiple frame types, specifically focusing on Main Frames (0x00) and OBD2 Frames (0x0D).

## Features

### 🎯 Core Functionality
- **Log File Parsing**: Automatically detects and parses MCU log files with custom frame formats
- **Dual Frame Support**: Handles both Main Frames (0x00) and OBD2 Frames (0x0D)
- **Real-time Progress**: Visual progress bar and status updates during file parsing
- **Frame Browsing**: Navigate through parsed frames with detailed view

### 📊 Data Display
- **List View**: Overview of all parsed frames with timestamps and types
- **Detailed Inspection**: Complete frame details displayed in readable format
- **Raw Data Preservation**: Original hexadecimal data available for reference

### 💾 Export Capabilities
- **CSV Export**: Export parsed frames to semicolon-delimited CSV files
- **Separate Files**: Main frames export to `main.csv`, OBD2 frames to `obd2.csv`
- **Progress Tracking**: Export progress shown for large datasets

## Supported Frame Types

### Main Frame (0x00)
Contains comprehensive sensor and system data:
- Timers and digital inputs
- Analog inputs (4 channels)
- Accelerometer data (X, Y, Z)
- Gyroscope data (X, Y, Z)
- One Wire sensor data
- Battery voltages (vehicle and internal)
- Temperature readings (board and battery)
- Tamper sensor status

### OBD2 Frame (0x0D)
Contains vehicle diagnostic data:
- Vehicle speed (km/h)
- Engine RPM (with actual RPM calculation)
- Engine temperature
- Fuel tank level percentage
- Engine total operating hours
- Odometer reading
- Diagnostic Trouble Codes (DTC)

## Installation

### Prerequisites
- .NET Framework 4.7.2 or later
- Windows 7 or later
- 10 MB free disk space

### Setup
1. Download the latest release from the releases page
2. Extract the ZIP file to your desired location
3. Run `MCULogParser.exe`

## Usage

### Basic Operation
1. **Load Log File**
   - Click "Browse" to select your MCU log file
   - Supported formats: `.log`, `.txt`

2. **Parse Frames**
   - Click "Parse" to start processing the log file
   - Monitor progress with the progress bar and status text

3. **Browse Frames**
   - Select any frame from the list to view its detailed information
   - Switch between Main and OBD2 frames seamlessly

4. **Export Data**
   - Use "Export Main Frames" to save main frame data to CSV
   - Use "Export OBD2 Frames" to save OBD2 data to CSV
   - Choose destination folder when prompted

### File Format Specification

#### Log File Structure
```
data: YYYY-MM-DD_HH-mm-ss.fff: $$[FrameType][PayloadLength][Payload][Checksum]<$>
```

#### Frame Structure
- **Preamble**: `$$` (2 bytes)
- **Frame Type**: 1 byte (hexadecimal)
- **Payload Length**: 2 bytes (unsigned, hexadecimal)
- **Payload**: Variable length (hexadecimal)
- **Checksum**: 1 byte (hexadecimal)
- **Trailer**: `<$>` (3 bytes)

## Project Structure

```
Tracom-Frame-Parser/
├── Form1.cs                 # Main application form
├── Form1.Designer.cs        # UI layout and controls
├── FrameClasses.cs          # Frame data structures and parser
├── Program.cs              # Application entry point
└── README.md              # This file
```

### Key Classes

- **`Form1`**: Main application window with UI controls
- **`LogParser`**: Core parsing logic for log files
- **`Frame`**: Base class for all frame types
- **`MainFrame`**: Specific implementation for Main Frames (0x00)
- **`OBD2Frame`**: Specific implementation for OBD2 Frames (0x0D)
- **`ProgressReport`**: Progress tracking structure

## Building from Source

### Requirements
- Visual Studio 2019 or later
- .NET Framework 4.7.2 SDK

### Steps
1. Clone or download the source code
2. Open `MCULogParser.sln` in Visual Studio
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

## Technical Details

### Parsing Algorithm
1. Read log file line by line
2. Extract timestamp and frame data
3. Validate frame structure (preamble, trailer, checksum)
4. Parse frame type and payload length
5. Extract and convert payload data based on frame type
6. Store parsed frames in memory for display and export

### Data Conversion
- Hexadecimal to numeric conversion for all fields
- Proper handling of signed/unsigned values
- Big-endian format support
- Custom timestamp parsing

### Performance Considerations
- Background worker for non-blocking UI
- Progress reporting for large files
- Efficient memory management
- Error handling for malformed data

## Error Handling

The application includes comprehensive error handling for:
- File access issues
- Malformed frame data
- Invalid hexadecimal values
- Missing or corrupted timestamps
- Memory allocation errors

## Troubleshooting

### Common Issues

1. **"No valid frames found"**
   - Verify log file format matches expected structure
   - Check that frames start with `$$` and end with `<$>`

2. **Parsing errors**
   - Ensure hexadecimal values are properly formatted
   - Verify payload lengths match frame specifications

3. **Export failures**
   - Check write permissions in destination folder
   - Ensure sufficient disk space

### Log File Examples

#### Valid Main Frame
```
data: 2025-11-03_14-01-40.235: $$00002C0000000080000000000000000000024CC2CAE23CFEDC0036FF94FFFFFFFFFFFFFFFF06D3026406D805D7000066<$>
```

#### Valid OBD2 Frame
```
data: 2025-11-03_14-01-39.707: $$0D001A0000000000000000000000000000000000000000000000000000C0<$>
```

## Support

For issues and feature requests, please contact the development team or create an issue in the project repository.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Version History

- **v1.0.0** (2024-01-XX)
  - Initial release
  - Basic frame parsing and display
  - CSV export functionality
  - Progress tracking

---

**Note**: This application is designed for specific MCU log formats. Ensure your log files match the specified structure before use.
