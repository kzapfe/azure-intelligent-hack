﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntelligentHack.Bot
{
    public class Settings
    {
        public static string AzureWebJobsStorage { get; set; }

        public static bool EnableCustomLog { get; set; }

        public static bool EnableVerboseLog { get; set; }
    }
}