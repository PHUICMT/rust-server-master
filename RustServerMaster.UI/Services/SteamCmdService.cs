using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RustServerMaster.UI.Services
{
  /// <summary>
  /// Wraps SteamCMD calls to install or update a Rust Dedicated Server.
  /// </summary>
  public class SteamCmdService
  {
    private readonly string _steamCmdPath;
    private readonly string _installFolder;

    /// <summary>
    /// Constructor takes the path to steamcmd.exe and the target install folder.
    /// </summary>
    public SteamCmdService(string steamCmdPath, string installFolder)
    {
      _steamCmdPath = steamCmdPath ?? throw new ArgumentNullException(nameof(steamCmdPath));
      _installFolder = installFolder ?? throw new ArgumentNullException(nameof(installFolder));
    }

    /// <summary>
    /// Invokes SteamCMD to download or update RustDedicated into the install folder.
    /// </summary>
    /// <param name="logCallback">
    /// Optional callback that receives SteamCMD stdout/stderr lines in real time.
    /// </param>
    /// <returns>True if SteamCMD exited with code 0; false otherwise.</returns>
    public async Task<bool> InstallOrUpdateRustServerAsync(Action<string>? logCallback = null)
    {
      // Check that steamcmd.exe exists
      if (!System.IO.File.Exists(_steamCmdPath))
      {
        logCallback?.Invoke($"[Error] steamcmd.exe not found at '{_steamCmdPath}'");
        return false;
      }

      // Build arguments: force install directory, login anonymously, app_update for Rust (AppID 258550), then quit.
      string args = $"+force_install_dir \"{_installFolder}\" +login anonymous +app_update 258550 validate +quit";

      var startInfo = new ProcessStartInfo
      {
        FileName = _steamCmdPath,
        Arguments = args,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
      };

      try
      {
        using (var proc = new Process { StartInfo = startInfo, EnableRaisingEvents = true })
        {
          proc.OutputDataReceived += (_, e) =>
          {
            if (!string.IsNullOrEmpty(e.Data))
              logCallback?.Invoke(e.Data);
          };
          proc.ErrorDataReceived += (_, e) =>
          {
            if (!string.IsNullOrEmpty(e.Data))
              logCallback?.Invoke("[ERROR] " + e.Data);
          };

          proc.Start();
          proc.BeginOutputReadLine();
          proc.BeginErrorReadLine();

          await proc.WaitForExitAsync();
          int exitCode = proc.ExitCode;
          if (exitCode == 0)
          {
            logCallback?.Invoke("[Info] SteamCMD completed successfully.");
            return true;
          }
          else
          {
            logCallback?.Invoke($"[Error] SteamCMD exited with code {exitCode}.");
            return false;
          }
        }
      }
      catch (Exception ex)
      {
        logCallback?.Invoke($"[Exception] {ex.Message}");
        return false;
      }
    }
  }
}
