using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;

namespace IntelligentHack.Pages
{
    public partial class CreateReportPreviewPage : BasePage
    {
        public CreateReportPreviewPage()
        {
            InitializeComponent();
        }

        public CreateReportPreviewPage(BaseViewModel reportContext)
        {
            InitializeComponent();
            this.BindingContext = reportContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Preview Page");
        }
    }
}