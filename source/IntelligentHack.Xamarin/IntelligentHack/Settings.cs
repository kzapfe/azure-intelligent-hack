using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace IntelligentHack
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private static readonly string SettingsDefault = string.Empty;

        private const string LanguageKey = "LanguageKey";
        private const string FunctionURLKey = "FunctionURLKey";
        private const string CryptographyKey = "CryptographyKey";
        private const string AzureWebJobsStorageKey = "AzureWebJobsStorageKey";
        private const string AppCenterID_AndroidKey = "AppCenterID_AndroidKey";
        private const string AppCenterID_iOSKey = "AppCenterID_iOSKey";
        private const string ImageStorageUrlKey = "ImageStorageUrlKey";

        #endregion

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageKey, value);
            }
        }

        public static string FunctionURL
        {
            get
            {
                return AppSettings.GetValueOrDefault(FunctionURLKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FunctionURLKey, value);
            }
        }

        public static string Cryptography
        {
            get
            {
                return AppSettings.GetValueOrDefault(CryptographyKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CryptographyKey, value);
            }
        }

        public static string AzureWebJobsStorage
        {
            get
            {
                return AppSettings.GetValueOrDefault(AzureWebJobsStorageKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AzureWebJobsStorageKey, value);
            }
        }

        public static string AppCenterID_Android
        {
            get
            {
                return AppSettings.GetValueOrDefault(AppCenterID_AndroidKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AppCenterID_AndroidKey, value);
            }
        }

        public static string AppCenterID_iOS
        {
            get
            {
                return AppSettings.GetValueOrDefault(AppCenterID_iOSKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AppCenterID_iOSKey, value);
            }
        }

        public static string ImageStorageUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault(ImageStorageUrlKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ImageStorageUrlKey, value);
            }
        }
    }
}