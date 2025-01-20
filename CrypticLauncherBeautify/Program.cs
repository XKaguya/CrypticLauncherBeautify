using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;
using CrypticLauncherBeautify.Core;
using CrypticLauncherBeautify.Extern;
using CrypticLauncherBeautify.Generic;
using log4net;
using log4net.Config;
using WebSocketSharp;

namespace CrypticLauncherBeautify;

public class Program
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
    public const string Version = "1.0.1";
    
    public static async Task Main(string[] args)
    {
        ThemeManager.CreatDefaultTheme();
        
        var themeOption = new Option<string>(
            new[] { "-t", "--Theme" },
            description: "The name of the theme to load.");

        var rootCommand = new RootCommand
        {
            themeOption
        };

        rootCommand.Description = "Select theme.";
        
        rootCommand.SetHandler(async (InvocationContext context) =>
        {
            var theme = context.ParseResult.GetValueForOption(themeOption);

            await Init(theme);
        });

        await rootCommand.InvokeAsync(args);
    }

    private static async Task Init(string? theme)
    {
        if (!await ConfigureLogging())
        {
            Log.Error("Failed to configure logging. Exiting.");
            return;
        }

        SettingManager.ParseConfig();
        
        AutoUpdate.StartAutoUpdateTask();

        if (theme == null || theme == "Default")
        {
            Log.Info("Default theme, Exiting.");
            Environment.Exit(0);
        }
        
        ThemeManager.ParseAllTheme();
        
        DisplayThemes();
        
        await TheRing(theme);
    }

    private static async Task TheRing(string theme)
    {
        try
        {
            await Api.InitApi();
            
            while (true)
            {
                while (true)
                {
                    if (GlobalVariables.SavedProcess == null || GlobalVariables.SavedProcess.Id == -1 || !GlobalVariables.SavedProcess.Responding || WebSocketManager.WebSocket == null || !WebSocketManager.WebSocket.IsAlive)
                    {
                        GlobalVariables.ReceivedValue = String.Empty;
                        GlobalVariables.PreviousUrl = String.Empty;
                        GlobalVariables.SavedProcess = null;
                        WebSocketManager.WebSocket = null;
                        
                        break;
                    }
                    
                    if (await Api.IsUrlChanged())
                    {
                        if (await Api.IsLoaded())
                        {
                            await HandlePageThemeChange(theme);
                        }
                    }
                
                    await Task.Delay(500);
                }

                GlobalVariables.ReceivedValue = String.Empty;
                GlobalVariables.PreviousUrl = String.Empty;
                GlobalVariables.SavedProcess = null;
                WebSocketManager.WebSocket = null;
                break;
            }
            
            await TheRing(theme);
        }
        catch (WebSocketException e)
        {
            Console.WriteLine(e);
            await TheRing(theme);
        }
    }

    private static async Task<bool> ConfigureLogging()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "CrypticLauncherBeautify.log4net.config";
        
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            XmlConfigurator.Configure(stream);
            return true;
        }

        Log.Error($"Failed to find embedded resource: {resourceName}");
        return false;
    }

    private static void DisplayThemes()
    {
        foreach (var theme in ThemeManager.Themes)
        {
            Console.WriteLine($"Loaded Theme: {theme.ThemeName}");
        }
    }

    private static async Task HandlePageThemeChange(string themeName)
    {
        if (string.IsNullOrEmpty(themeName) || themeName == "Default")
        {
            return;
        }
        
        if (await Api.IsLoginPage())
        {
            if (ThemeManager.Themes.Any(x => x.ThemeName == themeName))
            {
                var theme = ThemeManager.Themes.First(x => x.ThemeName == themeName);
                
                if (theme.EngagePage != null && theme.LoginPage != null)
                {
                    await Api.ChangeLoginPageThemeAsync(theme.LoginPage);
                }
            }
            else
            {
                Log.Error($"Failed to find Theme with name: {themeName}");
            }
        }
        else
        {
            if (ThemeManager.Themes.Any(x => x.ThemeName == themeName))
            {
                var theme = ThemeManager.Themes.First(x => x.ThemeName == themeName);
                
                if (theme.EngagePage != null && theme.EngagePage != null)
                {
                    await Api.ChangeEngagePageThemeAsync(theme.EngagePage);
                }
            }
            else
            {
                Log.Error($"Failed to find Theme with name: {themeName}");
            }
        }
    }
}
