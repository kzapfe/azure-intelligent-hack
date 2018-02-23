using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace IntelligentHack.Pages
{
    public partial class CreateReportConfirmationPage : BasePage
    {
        public CreateReportConfirmationPage()
        {
            InitializeComponent();
            BindingContext = new CreateReportConfirmationViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Report Confirmation");
        }
    }
}