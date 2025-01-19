using System.Xml.Serialization;
using CrypticLauncherBeautify.Theme;
using log4net;

namespace CrypticLauncherBeautify.Generic;

public class ThemeManager
{
    private const string ThemeFolder = "Theme";
    private const string ThemeFolderDefault = "Theme/Default";
    public static readonly List<Theme.Theme> Themes = new List<Theme.Theme>();
    private static readonly ILog Log = LogManager.GetLogger(typeof(ThemeManager));

    public static void CreatDefaultTheme()
    {
        if (!Directory.Exists(ThemeFolderDefault))
        {
            Directory.CreateDirectory(ThemeFolderDefault);
        }

        var loginPagePath = Path.Combine(ThemeFolderDefault, "LoginPageTheme.xml");
        var engagePagePath = Path.Combine(ThemeFolderDefault, "EngagePageTheme.xml");
        
        File.Create(loginPagePath).Close();
        File.Create(engagePagePath).Close();
        
        LoginPage loginPage = new LoginPage();
        EngagePage engagePage = new EngagePage();
        
        SerializeObjectToXml(loginPage, loginPagePath);
        SerializeObjectToXml(engagePage, engagePagePath);
    }
    
    private static void SerializeObjectToXml(object obj, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(obj.GetType());

        using FileStream fs = new FileStream(filePath, FileMode.Create) ;
        serializer.Serialize(fs, obj);
    }
    
    private static T DeserializeObjectFromXml<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file at {filePath} does not exist.");

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using FileStream fs = new FileStream(filePath, FileMode.Open);
        return (T)serializer.Deserialize(fs);
    }

    public static void ParseAllTheme()
    {
        var directories = Directory.GetDirectories(ThemeFolder);

        foreach (var dir in directories)
        {
            var loginPagePath = Path.Combine(dir, "LoginPageTheme.xml");
            var engagePagePath = Path.Combine(dir, "EngagePageTheme.xml");

            Theme.Theme theme = new Theme.Theme();
            
            if (File.Exists(loginPagePath))
            {
                theme.LoginPage = DeserializeObjectFromXml<LoginPage>(loginPagePath);
            }
            
            if (File.Exists(engagePagePath))
            {
                theme.EngagePage = DeserializeObjectFromXml<EngagePage>(engagePagePath);
            }
            
            if (theme.LoginPage != null || theme.EngagePage != null)
            {
                theme.ThemeName = theme.LoginPage?.ThemeName ?? theme.EngagePage?.ThemeName;
                if (!Themes.Contains(theme))
                {
                    Themes.Add(theme);
                }
                else
                {
                    Log.Warn($"Theme {theme.ThemeName} already exists.");
                }
            }
        }
    }
}