using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using CrypticLauncherBeautify.Generic;
using CrypticLauncherBeautify.Native;
using CrypticLauncherBeautify.Theme;
using log4net;
using Newtonsoft.Json;

namespace CrypticLauncherBeautify.Core;

public class Api
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(Api));
    private const string BaseUrl = "https://startreklauncher.crypticstudios.com";

    public static int GetDebugPort()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.FileName = "cmd.exe";
        processStartInfo.Arguments = "/C wmic process where caption=\"Star Trek Online.exe\" get caption,commandline,processId /value";
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.CreateNoWindow = true;

        Process? process = Process.Start(processStartInfo);

        if (process == null)
        {
            return 0;
        }

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        if (output.Contains("remote-debugging-port"))
        {
            string pattern = @"--remote-debugging-port=(\d+)";
            Match match = Regex.Match(output, pattern);

            if (match.Success)
            {
                string portValue = match.Groups[1].Value;
                int port = int.Parse(portValue);
                int pid = GetPidByPort(port);

                if (pid != -1)
                {
                    GlobalVariables.SavedProcess = Process.GetProcessById(pid);
                }
                
                return port;
            }
        }

        return 0;
    }
    
    private static int GetPidByPort(int port)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C netstat -ano | findstr {port}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        using var reader = process.StandardOutput;
        string output = reader.ReadToEnd();
        string[] lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length > 0)
        {
            string[] columns = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string pid = columns.Last();
            return int.Parse(pid);
        }

        return -1;
    }

    private static async Task<bool> Locate()
    {
        try
        {
            if (HttpClientManager.HttpClient == null)
            {
                Log.Error("HttpClient is null.");
                return false;
            }

            GlobalVariables.DebugUrl = $"http://127.0.0.1:{GlobalVariables.DebugPort}/json/list";

            HttpResponseMessage? response = null;

            try
            {
                response = await HttpClientManager.HttpClient.GetAsync(GlobalVariables.DebugUrl);
            }
            catch (HttpRequestException ex)
            {
                return false;
            }

            Log.Debug($"Attempting accessing {GlobalVariables.DebugUrl}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                List<DebugInfo>? debugInfoList = JsonConvert.DeserializeObject<List<DebugInfo>>(await response.Content.ReadAsStringAsync());
                if (debugInfoList == null || debugInfoList.Count == 0)
                {
                    return false;
                }

                GlobalVariables.DebugUrl = debugInfoList[0].WebSocketDebuggerUrl;
                Log.Debug(GlobalVariables.DebugUrl);
            }

            GlobalVariables.WebSocketUrl = GlobalVariables.DebugUrl;
            Log.Debug($"WebSocket URL: {GlobalVariables.WebSocketUrl}");

            WebSocketManager.InitWebSocket(GlobalVariables.WebSocketUrl);
        
            Log.Info("Api initialized.");

            return true;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return false;
        }
    }
    
    public static async Task InitApi()
    {
        int port = GetDebugPort();
        if (port != 0)
        {
            GlobalVariables.DebugPort = port;
            await HttpClientManager.InitHttpClient();
            await RetryLocate();
        }
    }
        
    private static int GetAvailablePort()
    {
        TcpListener listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();
        IPEndPoint endpoint = (IPEndPoint)listener.LocalEndpoint;
        listener.Stop();
        
        return endpoint.Port;
    }

    private static async Task RetryLocate()
    {
        bool success = false;

        while (!success)
        {
            success = await Locate();

            if (!success)
            {
                await Task.Yield();
            }
        }
    }

    public static async Task SaveToFile(string folder, string fileName, string content)
    {
        string filePath = Path.Combine(folder, fileName);
        
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        
        await File.WriteAllTextAsync(filePath, content);
    }

    public static async Task<string> GetCss()
    {
        string pattern = @"\/static\/css\/[^""]*sto\.css";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(GlobalVariables.ReceivedValue);
        
        if (match.Success)
        {
            HttpClient httpClient = new HttpClient();
            var res = await httpClient.GetStringAsync(BaseUrl + match.Groups[0].Value);
            return res;
        }
        
        return "null";
    }

    /*public static async Task GetLauncherResources()
    {
        await InitApi();
        WebSocketManager.SendEvaluateRequest();
        Console.WriteLine(GlobalVariables.ObjectId);
        WebSocketManager.SendCallFunctionOnRequest(GlobalVariables.ObjectId);

        string folder = "Theme/Default";
        string fileName = "Login.html";
        string cssName = "sto.css";
        
        await SaveToFile(folder, fileName, GlobalVariables.ReceivedValue);
        await SaveToFile(folder, cssName, await GetCss());
    }*/

    public static async Task<bool> IsLoginPage()
    {
        await WebSocketManager.GetLocationAsync();
        await WebSocketManager.GetHrefAsync(GlobalVariables.ObjectId);

        if (GlobalVariables.ReceivedValue.Contains("login"))
        {
            GlobalVariables.IsLoginPage = true;
            return true;
        }
        
        GlobalVariables.IsLoginPage = false;
        return false;
    }

    public static async Task ChangeLoginPageThemeAsync(LoginPage loginPage)
    {
        try
        {
            if (WebSocketManager.WebSocket == null || !WebSocketManager.WebSocket.IsAlive)
            {
                Log.Warn("WebSocket is not connected.");
                return;
            }
            
            await Task.WhenAll(
                WebSocketManager.ChangeArcIcon(loginPage.ArcIcon),
                WebSocketManager.ChangeCssHrefAsync(loginPage.CssLink),
                WebSocketManager.ChangeBackgroundAsync(loginPage.BackgroundString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.ForumUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Forum), loginPage.ForumString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.SupportUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Support), loginPage.SupportString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.AccountGuardUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.AccountGuard), loginPage.AccountGuardString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.OptionsUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Options), loginPage.OptionString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.ReleaseNotesUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.ReleaseNotes), loginPage.ReleaseNoteString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.MyAccountUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.MyAccount), loginPage.MyAccountString),
                WebSocketManager.ChangeLogoAsync(loginPage.LogoString),
                WebSocketManager.ChangeHintAsync(loginPage.HintString),
                WebSocketManager.ChangeAccAndPwdPlaceHolderAsync(loginPage.AccountPlaceholderString, loginPage.PasswordPlaceholderString),
                WebSocketManager.ChangeSubmitContentAsync(loginPage.LoginContentString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.SignUpUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.SignUp), loginPage.SignUpString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.ForgotPasswordUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.ForgotPassword), loginPage.ForgotPasswordString)
            );

            Log.Info("Login page theme changed successfully.");
        }
        catch (Exception ex)
        {
            Log.Error($"Error changing login page theme: {ex.Message}");
        }
    }

    public static async Task ChangeEngagePageThemeAsync(EngagePage engagePage)
    {
        try
        {
            if (WebSocketManager.WebSocket == null || !WebSocketManager.WebSocket.IsAlive)
            {
                Log.Warn("WebSocket is not connected.");
                return;
            }

            await Task.WhenAll(
                WebSocketManager.ChangeArcIcon(engagePage.ArcIcon),
                WebSocketManager.ChangeCssHrefAsync(engagePage.CssLink),
                WebSocketManager.ChangeBackgroundAsync(engagePage.BackgroundString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.ForumUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Forum), engagePage.ForumString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.SupportUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Support), engagePage.SupportString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.AccountGuardUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.AccountGuard), engagePage.AccountGuardString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.OptionsUrlEngagePage, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Options), engagePage.OptionString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.ReleaseNotesUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.ReleaseNotes), engagePage.ReleaseNoteString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.MyAccountUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.MyAccount), engagePage.MyAccountString),
                WebSocketManager.ChangeHrefContentAsync(Native.CrypticNativeUrl.NewsUrl, Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.ViewAll), engagePage.ViewAllNewsContentString),
                WebSocketManager.ChangeSubmitContentAsync(engagePage.EngageContentString),
                WebSocketManager.ChangeH2ContentAsync(Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.Shard), engagePage.ShardContentString),
                WebSocketManager.ChangeH2ContentAsync(Native.CrypticNativeValue.NativeEnumToString(CrypticNativeValue.NativeValue.News), engagePage.NewsContentString),
                WebSocketManager.ChangeServerNameAsync(engagePage.HolodeckContentString, engagePage.TribbleContentString)
            );

            Log.Info("Engage page theme changed successfully.");
        }
        catch (Exception ex)
        {
            Log.Error($"Error changing engage page theme: {ex.Message}");
        }
    }
    
    public static async Task<bool> IsLoaded()
    {
        while (WebSocketManager.WebSocket != null && !WebSocketManager.WebSocket.IsAlive && !GlobalVariables.IsLoaded)
        {
            await WebSocketManager.GetReadyState();
            await Task.Delay(1000);
        }
        
        return true;
    }
    
    public static async Task<bool> IsUrlChanged()
    {
        await WebSocketManager.GetLocationAsync();
        await WebSocketManager.GetHrefAsync(GlobalVariables.ObjectId);

        if (GlobalVariables.PreviousUrl != GlobalVariables.ReceivedValue)
        {
            GlobalVariables.PreviousUrl = GlobalVariables.ReceivedValue;
            return true;
        }

        return false;
    }
}