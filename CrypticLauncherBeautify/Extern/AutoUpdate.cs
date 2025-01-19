using System.Diagnostics;
using CrypticLauncherBeautify.Core;
using log4net;

namespace CrypticLauncherBeautify.Extern
{
    public class AutoUpdate
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketManager));
        
        private static readonly string Author = "XKaguya";
        private static readonly string Project = "CrypticLauncherBeautify";
        private static readonly string ExeName = "CrypticLauncherBeautify.exe";
        private static readonly string CurrentExePath = Path.Combine(Environment.CurrentDirectory, ExeName);
        private static readonly string NewExePath = Path.Combine(Environment.CurrentDirectory, "CrypticLauncherBeautify-New.exe");
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static void StartAutoUpdateTask()
        {
            Task.Run(async () => await AutoUpdateTask(CancellationTokenSource.Token));
        }

        private static async Task AutoUpdateTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                CheckAndUpdate();
                await Task.Delay(TimeSpan.FromHours(1), token);
            }
        }

        private static void CheckAndUpdate()
        {
            if (!GlobalVariables.AutoUpdate)
            {
                return;
            }
            
            try
            {
                string commonUpdaterPath = Path.Combine(Directory.GetCurrentDirectory(), "CommonUpdater.exe");

                if (!File.Exists(commonUpdaterPath))
                {
                    Log.Info("There's no CommonUpdater in the folder. Failed to update.");
                    return;
                }
                
                string arguments = $"{Project} {ExeName} {Author} {Program.Version} \"{CurrentExePath}\" \"{NewExePath}\"";
                
                var startInfo = new ProcessStartInfo
                {
                    FileName = commonUpdaterPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = true
                };

                Log.Debug($"Starting CommonUpdater with arguments: {arguments}");
                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    Log.Error("Failed to start CommonUpdater: Process.Start returned null.");
                    return;
                }
                
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                
                if (!string.IsNullOrEmpty(error))
                {
                    Log.Error($"CommonUpdater error: {error}");
                }
                    
                if (process.ExitCode != 0)
                {
                    Log.Error($"CommonUpdater exited with code {process.ExitCode}");
                }
                else
                {
                    Log.Debug("CommonUpdater started successfully.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to start CommonUpdater: {ex.Message}");
            }
        }
    }
}