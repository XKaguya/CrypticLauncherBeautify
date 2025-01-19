using System.ComponentModel;
using CrypticLauncherBeautify.Generic;

namespace CrypticLauncherBeautify.Core;

public class GlobalVariables
{
    [Description("Set to true to allow program self update. \nDefault value: true")]
    public static bool AutoUpdate { get; set; } = true;
    [SettingManager.IgnoreSetting]
    public static string WebSocketUrl { get; set; } = string.Empty;
    [SettingManager.IgnoreSetting]
    public static string DebugUrl { get; set; } = string.Empty;
    [SettingManager.IgnoreSetting]
    public static int DebugPort { get; set; }
    [Description("Set STO Launcher, The Star Trek Online.exe. \nDefault value: SET_YOUR_STO_LAUNCHER_PATH_HERE")]
    public static string LauncherPath { get; set; } = "SET_YOUR_STO_LAUNCHER_PATH_HERE";
    [SettingManager.IgnoreSetting]
    public static string ObjectId { get; set; } = string.Empty;
    [SettingManager.IgnoreSetting]
    public static string ReceivedValue { get; set; } = string.Empty;
    [SettingManager.IgnoreSetting]
    public static bool IsLoginPage { get; set; }
    [SettingManager.IgnoreSetting]
    public static bool IsLoaded { get; set; }
    [SettingManager.IgnoreSetting]
    public static string PreviousUrl { get; set; } = string.Empty;
}