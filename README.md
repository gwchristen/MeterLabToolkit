# MeterLabToolkit

An Avalonia-based cross-platform data entry application for utility meter and device tracking that replaces traditional spreadsheet workflows.

## Overview

MeterLabToolkit provides a modern desktop application for managing:
- **Created Histories** - Meter and device creation records with PO information
- **Cellular/uAP Registration** - Device registration with cellular network details
- **RMA Entries** - Return merchandise authorization tracking
- **Reference Tables** - Lookup codes, manufacturer codes, and device codes
- **Tools** - CSV date updater, serial range generator, range splitter, and AEP barcode generator

## Technology Stack

- **Avalonia UI 11.3.9** - Cross-platform desktop UI framework
- **CommunityToolkit.Mvvm 8.2.1** - MVVM framework with source generators
- **Entity Framework Core 8.0** with **SQLite** - Data persistence
- **.NET 8.0** - Target framework

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Building and Running

### Clone and Build

```bash
# Clone the repository
git clone https://github.com/gwchristen/MeterLabToolkit.git
cd MeterLabToolkit

# Restore dependencies and build
dotnet build

# Run the application
dotnet run --project src/MeterLabToolkit/MeterLabToolkit.csproj
```

### Development

```bash
# Build in debug mode
dotnet build

# Build in release mode
dotnet build --configuration Release

# Run with hot reload (for development)
dotnet watch run --project src/MeterLabToolkit/MeterLabToolkit.csproj
```

## Data Storage

The application uses SQLite for data persistence. The database file is stored at:
- **Windows**: `%LOCALAPPDATA%\MeterLabToolkit\meterlab.db`
- **macOS**: `~/Library/Application Support/MeterLabToolkit/meterlab.db`
- **Linux**: `~/.local/share/MeterLabToolkit/meterlab.db`

The database is automatically created on first run.

## Features

### Data Entry
- **Created Histories**: Track device procurement and establishment
- **Cellular Registration**: Manage cellular device registrations with IMEI, ICCID, and network details
- **RMA Entries**: Log returned devices and defects

### Tools
- **CSV Date Updater**: Batch update dates in CSV files
- **Serial Range Generator**: Generate serial number ranges
- **Range Splitter**: Split serial number ranges
- **AEP Barcode Generator**: Generate AEP barcodes for meters and devices

### User Interface
- Modern, clean design using Avalonia Fluent theme
- Navigation panel with expandable sections
- Collapsible bottom panel for quick tool access
- Responsive layout suitable for daily business use

## Project Structure

```
MeterLabToolkit/
├── MeterLabToolkit.sln
├── src/
│   └── MeterLabToolkit/
│       ├── MeterLabToolkit.csproj
│       ├── Models/              # Data models
│       ├── ViewModels/          # MVVM view models
│       ├── Views/               # Avalonia views
│       ├── Services/            # Data services
│       ├── Data/                # EF Core DbContext
│       └── Assets/              # Application resources
└── README.md
```

## License

[Add your license information here]

## Contributing

[Add contribution guidelines here]
