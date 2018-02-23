using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;

namespace IntelligentHack.Pages
{
    public partial class LanguageSettingsPage : BasePage
    {
        public LanguageSettingsPage()
        {
            InitializeComponent();
            BindingContext = new LanguageSettingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Language Settings");
        }
    }
}
