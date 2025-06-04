using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using RustServerMaster.UI.Helpers;
using RustServerMaster.UI.Services;

namespace RustServerMaster.UI.ViewModel
{
  /// <summary>
  /// ViewModel for the Installation & Update screen.
  /// </summary>
  public class InstallationViewModel : BaseViewModel
  {
    private string _steamCmdPath = string.Empty;
    private string _installFolder = string.Empty;
    private string _logText = string.Empty;
    private bool _isInstalling;

    public string SteamCmdPath
    {
      get => _steamCmdPath;
      set => SetField(ref _steamCmdPath, value);
    }

    public string InstallFolder
    {
      get => _installFolder;
      set => SetField(ref _installFolder, value);
    }

    public string LogText
    {
      get => _logText;
      set => SetField(ref _logText, value);
    }

    public bool IsInstalling
    {
      get => _isInstalling;
      set
      {
        SetField(ref _isInstalling, value);
        ((RelayCommand)BrowseSteamCmdCommand).RaiseCanExecuteChanged();
        ((RelayCommand)BrowseInstallFolderCommand).RaiseCanExecuteChanged();
        ((AsyncRelayCommand)StartInstallCommand).RaiseCanExecuteChanged();
      }
    }

    public ICommand BrowseSteamCmdCommand { get; }
    public ICommand BrowseInstallFolderCommand { get; }
    public ICommand StartInstallCommand { get; }

    public InstallationViewModel()
    {
      SteamCmdPath = @"C:\SteamCMD\steamcmd.exe";
      InstallFolder = @"C:\RustServer";

      BrowseSteamCmdCommand = new RelayCommand(_ => BrowseSteamCmd(), _ => !IsInstalling);
      BrowseInstallFolderCommand = new RelayCommand(_ => BrowseInstallFolder(), _ => !IsInstalling);
      StartInstallCommand = new AsyncRelayCommand(_ => StartInstallAsync(), _ => !IsInstalling);
    }

    private void BrowseSteamCmd()
    {
      // Explicitly use WPF's OpenFileDialog
      var dlg = new Microsoft.Win32.OpenFileDialog
      {
        Title = "Select steamcmd.exe",
        Filter = "SteamCMD executable (steamcmd.exe)|steamcmd.exe|All Files (*.*)|*.*"
      };
      bool? result = dlg.ShowDialog();
      if (result == true)
      {
        SteamCmdPath = dlg.FileName;
      }
    }

    private void BrowseInstallFolder()
    {
      // Fully qualify WinForms FolderBrowserDialog here
      using var dlg = new System.Windows.Forms.FolderBrowserDialog
      {
        Description = "Select installation folder for Rust Server"
      };
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        InstallFolder = dlg.SelectedPath;
      }
    }

    private async Task StartInstallAsync()
    {
      if (IsInstalling) return;

      if (string.IsNullOrWhiteSpace(SteamCmdPath) || !File.Exists(SteamCmdPath))
      {
        LogText = "[Error] Invalid SteamCMD path." + Environment.NewLine;
        return;
      }
      if (string.IsNullOrWhiteSpace(InstallFolder))
      {
        LogText = "[Error] Installation folder is required." + Environment.NewLine;
        return;
      }
      try
      {
        Directory.CreateDirectory(InstallFolder);
      }
      catch (Exception ex)
      {
        LogText = $"[Error] Could not create install folder: {ex.Message}" + Environment.NewLine;
        return;
      }

      IsInstalling = true;
      LogText = "[Info] Starting SteamCMD ..." + Environment.NewLine;

      var service = new SteamCmdService(SteamCmdPath, InstallFolder);
      bool success = await service.InstallOrUpdateRustServerAsync(log =>
      {
        App.Current.Dispatcher.Invoke(() =>
              {
            LogText += log + Environment.NewLine;
          });
      });

      if (success)
      {
        App.Current.Dispatcher.Invoke(() =>
        {
          LogText += "[Info] Rust Dedicated installation/update complete." + Environment.NewLine;
        });
      }
      else
      {
        App.Current.Dispatcher.Invoke(() =>
        {
          LogText += "[Error] SteamCMD failed to install/update Rust Server." + Environment.NewLine;
        });
      }

      IsInstalling = false;
    }
  }
}
