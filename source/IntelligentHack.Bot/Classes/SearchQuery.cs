using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Drawing;
using static IntelligentHack.Bot.Classes.Collections;

namespace IntelligentHack.Bot.Classes
{
    [Serializable]
    public class SearchQuery
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public Country? Country { get; set; }
    }
}