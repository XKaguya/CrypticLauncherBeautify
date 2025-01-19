using System.ComponentModel;
using System.Reflection;
using System.Xml;
using CrypticLauncherBeautify.Core;
using log4net;

namespace CrypticLauncherBeautify.Generic;

public class SettingManager
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreSettingAttribute : Attribute
    {
    }
    
    private static readonly ILog Log = LogManager.GetLogger(typeof(SettingManager));
    private static string ConfigFilePath { get; } = "Config.xml";
    
    public static bool ParseConfig()
    {
        try
        {
            if (!File.Exists(ConfigFilePath))
            {
                Log.Error("Config file not exist.");
                File.WriteAllText(ConfigFilePath, string.Empty);

                SaveToXml(ConfigFilePath);  
            }

            LoadFromXml(ConfigFilePath);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message + ex.StackTrace);
            return false;
        }
    }
    
    private static void LoadFromXml(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        var root = doc.DocumentElement;

        foreach (var prop in typeof(GlobalVariables).GetProperties())
        {
            if (prop.GetCustomAttribute<IgnoreSettingAttribute>() != null)
            {
                continue;
            }

            var element = root.SelectSingleNode(prop.Name);
            if (element == null) continue;

            var value = element.InnerText;
            try
            {
                if (prop.PropertyType == typeof(int))
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        prop.SetValue(null, intValue);
                    }
                    else
                    {
                        Log.Error($"Error converting value '{value}' to int for property '{prop.Name}'");
                    }
                }
                else if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(null, value);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    if (bool.TryParse(value, out bool boolValue))
                    {
                        prop.SetValue(null, boolValue);
                    }
                    else
                    {
                        Log.Error($"Error converting value '{value}' to bool for property '{prop.Name}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error setting property '{prop.Name}'", ex);
            }
        }
    }

    private static void SaveToXml(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("Settings");
        doc.AppendChild(root);

        foreach (var prop in typeof(GlobalVariables).GetProperties())
        {
            var value = prop.GetValue(null);
            var descriptionAttr = (DescriptionAttribute)prop.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            var description = descriptionAttr?.Description ?? string.Empty;

            if (prop.GetCustomAttribute<IgnoreSettingAttribute>() != null)
            {
                continue;
            }

            string valueString = null;

            if (prop.PropertyType == typeof(ushort[]))
            {
                valueString = value != null ? string.Join(",", (ushort[])value) : string.Empty;
            }
            else if (prop.PropertyType == typeof(string))
            {
                valueString = value?.ToString();
            }
            else if (prop.PropertyType == typeof(int))
            {
                valueString = value?.ToString();
            }
            else if (prop.PropertyType == typeof(bool))
            {
                valueString = value?.ToString().ToLower();
            }
            else
            {
                valueString = value?.ToString();
            }

            if (valueString != null)
            {
                AppendElement(doc, root, prop.Name, valueString, description);
            }
        }

        doc.Save(filePath);
    }

    private static void AppendElement(XmlDocument doc, XmlNode root, string name, string value, string description)
    {
        var element = doc.CreateElement(name);
        element.InnerText = value;
        
        foreach (var line in description.Split('\n'))
        {
            var comment = doc.CreateComment(line);
            root.AppendChild(comment);
        }

        root.AppendChild(element);
    }
}