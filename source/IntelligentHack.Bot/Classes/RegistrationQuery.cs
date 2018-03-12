﻿using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Drawing;

namespace IntelligentHack.Bot.Classes
{
    [Serializable]
    public class RegistrationQuery
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public Country? Country { get; set; }

        public string LocationOfLost { get; set; }

        public string DateOfLost { get; set; }

        public string ReportId { get; set; }

        [Pattern(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")]
        public string ReportedBy { get; set; }
    }

    public enum Country
    {
        US,
        Mexico
    }
}