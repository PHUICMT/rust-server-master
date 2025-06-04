# RustServerMaster (RSM)

**RustServerMaster (RSM)** is a Windows GUI application (built with WPF and .NET 9) designed to simplify the installation, configuration, and management of a Rust Dedicated Server. RSM provides a unified interface for:

1. **Server Installation & Update Module**  
   - Invoke SteamCMD to download or update Rust Dedicated Server  
   - Check for the latest server version and apply updates automatically  
   - Choose or change the installation directory  

2. **Mod Management Module**  
   - Connect to uMod (Oxide) API (if available) to retrieve a list of Rust mods  
   - Display available mods and allow users to install, update, or remove them  
   - Download mod files (`.cs`, `.dll`) into `<server_root>/oxide/plugins/`  

3. **Configuration Panel Module**  
   - Load, edit, and save `server.cfg` located in `<server_root>/cfg/`  
   - If Oxide/uMod is installed, edit plugin-specific configuration files  
   - Option to select a custom map or procedural map seed/size  
   - Configure basic server settings: server name, password, max players, world size, seed, ports, RCON credentials, etc.  

4. **Server Control & Monitoring Module**  
   - Start, stop, and restart `RustDedicated.exe` with a single click  
   - Tail and display real-time server output and error logs  
   - Show the current server status (Running / Stopped / Error)  
   - (Optional) Run predefined console commands via buttons (no typing required), with an input field for manual commands  
   - (Optional) Detect installed plugins and expose their commands through the GUI  
   - (Optional) Backup and restore world data (`<server_root>/user/` and related files)  

RSM is intended to expand over time; future versions will introduce additional features such as automated port forwarding checks, built-in RCON console, plugin dependency management, and more.

---

## Table of Contents

- [RustServerMaster (RSM)](#rustservermaster-rsm)
  - [Table of Contents](#table-of-contents)
  - [Prerequisites](#prerequisites)
  - [Repository Structure](#repository-structure)
  - [Getting Started](#getting-started)
    - [1. Clone the Repository](#1-clone-the-repository)
    - [2. Install .NET 9 SDK](#2-install-net-9-sdk)
    - [3. Install SteamCMD](#3-install-steamcmd)
    - [4. Open the Project in VS Code](#4-open-the-project-in-vs-code)
    - [5. Restore Dependencies](#5-restore-dependencies)
    - [6. Build and Run](#6-build-and-run)
      - [Initial Configuration](#initial-configuration)
  - [How to Contribute](#how-to-contribute)
  - [License](#license)

---

## Prerequisites

Before you begin, ensure that you have the following installed on your Windows machine:

1. **.NET 9 SDK**  
   Download and install from:  
   https://dotnet.microsoft.com/download/dotnet/9.0

2. **Visual Studio Code (or Visual Studio 2022/2019)**  
   - If you use VS Code, install the C# extension  
   - If you use Visual Studio, make sure you include the “.NET Desktop Development” workload  

3. **SteamCMD**  
   RSM relies on SteamCMD to download and update the Rust Dedicated Server files.  
   - Download SteamCMD for Windows: https://developer.valvesoftware.com/wiki/SteamCMD  
   - Extract `steamcmd.exe` to a folder of your choice (e.g., `C:\SteamCMD`)  

4. **Internet Connection**  
   Required to download Rust server files and mods from SteamCMD and uMod.

---

## Repository Structure

```
rust-server-master/
├─ .gitignore
├─ LICENSE
├─ README.md
├─ RustServerMaster.sln
└─ RustServerMaster.UI/
   ├─ RustServerMaster.UI.csproj
   ├─ App.xaml
   ├─ App.xaml.cs
   ├─ MainWindow.xaml
   ├─ MainWindow.xaml.cs
   ├─ Assets/
   │   └─ (icons, logos, images)
   ├─ Model/
   │   ├─ ModInfo.cs
   │   └─ ServerConfig.cs
   ├─ View/
   │   ├─ InstallationView.xaml
   │   ├─ InstallationView.xaml.cs
   │   ├─ ModManagementView.xaml
   │   ├─ ModManagementView.xaml.cs
   │   ├─ ConfigurationView.xaml
   │   ├─ ConfigurationView.xaml.cs
   │   ├─ ServerControlView.xaml
   │   └─ ServerControlView.xaml.cs
   ├─ ViewModel/
   │   ├─ BaseViewModel.cs
   │   ├─ InstallationViewModel.cs
   │   ├─ ModManagementViewModel.cs
   │   ├─ ConfigurationViewModel.cs
   │   └─ ServerControlViewModel.cs
   ├─ Services/
   │   ├─ SteamCmdService.cs
   │   ├─ ModApiService.cs
   │   ├─ FileService.cs
   │   └─ ProcessService.cs
   ├─ Helpers/
   │   ├─ RelayCommand.cs
   │   ├─ AsyncRelayCommand.cs
   │   └─ InverseBoolConverter.cs
   ├─ Resources/
   │   └─ (ResourceDictionary XAML files, e.g., Styles.xaml)
   └─ Assets/
       └─ (application icons, images, etc.)
```

- **RustServerMaster.UI/**  
  The main WPF (.NET 9) project containing all UI, ViewModels, Models, Services, and Helpers.

- **.gitignore**  
  Specifies files and directories that Git should ignore (e.g., `bin/`, `obj/`, user-specific configs).

- **LICENSE**  
  MIT License text.

- **README.md**  
  This file. Provides an overview of RSM, setup instructions, and contribution guidelines.

---

## Getting Started

Follow these steps to set up RSM on your local machine for development or testing.

### 1. Clone the Repository

```bash
git clone https://github.com/PHUICMT/rust-server-master.git
cd rust-server-master
```

### 2. Install .NET 9 SDK

Download and install .NET 9 SDK from:

```
https://dotnet.microsoft.com/download/dotnet/9.0
```

Verify the installation:

```bash
dotnet --version
# Should output 7.x.x
```

### 3. Install SteamCMD

1. Download SteamCMD for Windows:  
   https://developer.valvesoftware.com/wiki/SteamCMD

2. Extract `steamcmd.exe` and its supporting files to a folder, for example:

```
C:\SteamCMD\
```

You will point RSM to this `steamcmd.exe` later.

### 4. Open the Project in VS Code

1. Launch Visual Studio Code.
2. Open the folder `rust-server-master` (the root directory).

   - VS Code should detect the `.sln` file (`RustServerMaster.sln`) and prompt you to install recommended extensions (if not already installed).  
   - Ensure the C# extension is installed (for Intellisense and debugging).

### 5. Restore Dependencies

In VS Code Terminal (or any command prompt), run:

```bash
dotnet restore RustServerMaster.UI/RustServerMaster.UI.csproj
```

This restores all NuGet packages and dependencies.

### 6. Build and Run

From the root directory, run:

```bash
dotnet run --project RustServerMaster.UI/RustServerMaster.UI.csproj
```

This will build the WPF project and launch RustServerMaster. At first launch, you’ll see an empty window with tabs for each module (Installation, Mod Management, Configuration, Server Control).

#### Initial Configuration

1. **Installation Tab**  
   - Click “Browse…” next to “SteamCMD Path” and select your `steamcmd.exe` (e.g., `C:\SteamCMD\steamcmd.exe`).  
   - Click “Browse…” next to “Install Folder” and select or create a folder to install the Rust server (e.g., `C:\RustServer\`).  
   - Click “Start Install / Update” to download the Rust Dedicated Server via SteamCMD. Progress and logs appear in the text area below.

2. **Mod Management Tab**  
   - Enter the path to `<install_folder>\oxide\plugins\` (e.g., `C:\RustServer\oxide\plugins`).  
   - Click “Load Available Mods” to fetch a list of mods from uMod.  
   - Select a mod from the DataGrid and click “Install Selected Mod” to download it to your plugins folder. Logs appear in the text area below.

3. **Configuration Tab**  
   - Set “Install Folder” to the same folder used above (e.g., `C:\RustServer\`).  
   - The application will load (or generate) `server.cfg` from `<install_folder>\cfg\server.cfg`.  
   - Edit fields such as Hostname, Max Players, Ports, RCON credentials, map type, world size, seed, etc.  
   - Click “Save Configuration” to write changes back to `server.cfg`.

4. **Server Control Tab**  
   - Set “Install Folder” to the same folder (e.g., `C:\RustServer\`).  
   - Click “Load Log” to load any existing `output_log.txt`.  
   - Click “Start Server” to launch `RustDedicated.exe` with parameters pulled from `server.cfg`. Logs stream in real time.  
   - Click “Stop Server” to kill the server process.  

---

## How to Contribute

Contributions are welcome! If you’d like to suggest improvements, report issues, or submit pull requests, please follow these guidelines:

1. **Fork the Repository**  
   - Click “Fork” on the upper right of the GitHub page to create your own copy.

2. **Create a Feature Branch**  
   ```bash
   git checkout -b feature/YourFeatureName
   ```

3. **Make Your Changes**  
   - Follow the existing code style (MVVM architecture, English comments, consistent naming conventions).  
   - Test thoroughly before committing.

4. **Commit and Push**  
   ```bash
   git add .
   git commit -m "Add [brief description of your change]"
   git push origin feature/YourFeatureName
   ```

5. **Open a Pull Request**  
   - Navigate to `https://github.com/PHUICMT/rust-server-master` and click “Compare & pull request”.  
   - Provide a clear title and description of your changes.  

6. **Review Process**  
   - The maintainers will review your PR.  
   - Please respond to feedback or requested changes.  
   - Once approved, your PR will be merged into `main`.

---

## License

This project is licensed under the **MIT License**. See [LICENSE](./LICENSE) for details.
