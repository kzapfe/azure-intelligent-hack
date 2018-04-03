using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntelligentHack.Bot
{
    public class Settings
    {
        public static string DataStorage { get; set; }

        public static bool EnableCustomLog { get; set; }

        public static bool EnableVerboseLog { get; set; }

        public static string FunctionURL { get; set; }

        public static string Cryptography { get; set; }

        public static string ImageStorageUrl { get; set; }

        public static string TranslatorKey { get; set; }

        public static string SpecificLanguage { get; set; }

        public static string CosmosDBUri { get; set; }

        public static string CosmosDBKey { get; set; }
    }
}