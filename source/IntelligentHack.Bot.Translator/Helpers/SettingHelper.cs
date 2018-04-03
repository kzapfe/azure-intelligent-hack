
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IntelligentHack.Bot.Helpers
{
    public class SettingHelper
    {
        public static string GetSetting(string setting)
        {
            string result = string.Empty;
            result = (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"))) ? ConfigurationManager.AppSettings[setting] : Environment.GetEnvironmentVariable(setting);
            return result;
        }
    }
}