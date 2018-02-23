using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace IntelligentHack.Pages
{
    public partial class HomePage : BasePage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new HomeViewModel();
            Analytics.TrackEvent("View: Home");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}