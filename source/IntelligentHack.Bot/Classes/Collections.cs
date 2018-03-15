using System;
using System.Collections.Generic;

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
            public static List<string> CreateList()
            {
                List<string> list = new List<string>();
                list.Add($"{Resources.Resource.Search_Photo}");
                list.Add($"{Resources.Resource.Search_NameLastname}");
                return list;
            }
        }

        [Serializable]
        public class Registration
        {
            public static List<string> CreateList()
            {
                List<string> list = new List<string>();
                list.Add($"{Resources.Resource.Registration_Yes}");
                list.Add($"{Resources.Resource.Registration_No}");
                return list;
            }
        }

        [Serializable]
        public class Continue
        {
            public static List<string> CreateList()
            {
                List<string> list = new List<string>();
                list.Add("Continue");
                return list;
            }
        }

        [Serializable]
        public class ReportSearch
        {
            public static List<string> CreateList()
            {
                List<string> list = new List<string>();
                list.Add($"{Resources.Resource.MenuReportSearch_Report}");
                list.Add($"{Resources.Resource.MenuReportSearch_Search}");
                return list;
            }
        }

        [Serializable]
        public class Option
        {
            public string Locale { get; set; }
            public string Text { get; set; }

            public Option()
            {
                Locale = "";
                Text = "";
            }

            public override string ToString()
            {
                return this.Text;
            }

            public static List<Option> CreateList()
            {
                List<Option> list = new List<Option>();

                var english = new Option();
                english.Locale = "en-US";
                english.Text = "English";

                var spanishMexico = new Option();
                spanishMexico.Locale = "es-MX";
                spanishMexico.Text = "Spanish";

                list.Add(english);
                list.Add(spanishMexico);
                return list;
            }
        }
    }
}