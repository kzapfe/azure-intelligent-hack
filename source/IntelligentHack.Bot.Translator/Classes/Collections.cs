using IntelligentHack.Bot.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntelligentHack.Bot.Classes
{
    public class Collections
    {
        public enum Genre
        {
            M,
            F
        }

        public enum Country
        {
            US,
            MX
        }

        [Serializable]
        public class Search
        {
            public static async Task<List<string>> CreateList()
            {
                string photo = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_Photo}", Settings.SpecificLanguage);
                string namelastname = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_NameLastname}", Settings.SpecificLanguage);

                List<string> list = new List<string>();
                list.Add(photo);
                list.Add(namelastname);
                return list;
            }
        }

        [Serializable]
        public class Registration
        {
            public static async Task<List<string>> CreateList()
            {
                string yes = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Registration_Yes}", Settings.SpecificLanguage);
                string no = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Registration_No}", Settings.SpecificLanguage);

                List<string> list = new List<string>();
                list.Add(yes);
                list.Add(no);
                return list;
            }
        }

        [Serializable]
        public class ReportSearch
        {
            public static async Task<List<string>> CreateList()
            {
                string report = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch_Report}", Settings.SpecificLanguage);
                string search = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch_Search}", Settings.SpecificLanguage);

                List<string> list = new List<string>();
                list.Add(report);
                list.Add(search);
                return list;
            }
        }
    }
}