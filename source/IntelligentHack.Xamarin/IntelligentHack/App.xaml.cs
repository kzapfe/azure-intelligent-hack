using System.Collections.Generic;
using System.Threading.Tasks;
using IntelligentHack.Interfaces;
using IntelligentHack.Pages;
using Xamarin.Forms;

namespace IntelligentHack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LoadAppConfiguration();
            MainPage = new NavigationPage(new HomePage());
        }

        private void LoadAppConfiguration()
        {
            List<Task> tasks = new List<Task>();
            Task startup = Task.Run(() =>
            {
                //set startup app configuration
                Settings.FunctionURL = "https://{AzureFunctionApp}.azurewebsites.net";
                Settings.Cryptography = "{AzureFunctionAppCryptographyKey}";

                //set startup language configuration
                string language = Settings.Language;

                if (string.IsNullOrEmpty(language))
                {
                    language = "en-US";
                    Settings.Language = language;
                }

                //initialize multi-culture
                DependencyService.Get<ILocalizeService>().Set(language);

                //initialize Azure Mobile App
                DependencyService.Get<IMobileAppService>().Initialize();

            });
            tasks.Add(startup);
            Task.WaitAll(tasks.ToArray());
        }
    }
}