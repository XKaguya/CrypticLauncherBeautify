using System.ComponentModel;
using System.Diagnostics;
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
    [SettingManager.IgnoreSetting]
    public static Process? SavedProcess { get; set; } = null;
}