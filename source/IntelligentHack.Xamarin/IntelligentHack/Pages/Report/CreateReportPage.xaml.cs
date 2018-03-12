using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace IntelligentHack.Pages
{
    public partial class CreateReportPage : BasePage
    {
        private int ReportedByRestrictCount = 200;
        private int NameRestrictCount = 100;
        private int LastNameRestrictCount = 100;
        private int LocationOfLostRestrictCount = 350;
        private int DateOfLostRestrictCount = 100;
        private int ReportIdRestrictCount = 100;

        public CreateReportPage()
        {
            InitializeComponent();

            this.FindByName<Entry>("reportedBy").TextChanged += ReportedByOnTextChanged;
            this.FindByName<Entry>("name").TextChanged += NameOnTextChanged;
            this.FindByName<Entry>("lastname").TextChanged += LastNameOnTextChanged;
            this.FindByName<Entry>("locationOfLost").TextChanged += LocationOfLostOnTextChanged;
            this.FindByName<Entry>("dateOfLost").TextChanged += DateOfLostOnTextChanged;
            this.FindByName<Entry>("reportId").TextChanged += ReportIdOnTextChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Create Report");
        }

        private void ReportedByOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged2("reportedBy", entry.Text, ReportedByRestrictCount);
        }

        private void NameOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged("name", entry.Text, NameRestrictCount);
        }

        private void LastNameOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged("lastname", entry.Text, LastNameRestrictCount);
        }

        private void LocationOfLostOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged("locationOfLost", entry.Text, LocationOfLostRestrictCount);
        }

        private void DateOfLostOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged("dateOfLost", entry.Text, DateOfLostRestrictCount);
        }

        private void ReportIdOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            OnTextChanged("reportId", entry.Text, ReportIdRestrictCount);
        }

        private void OnTextChanged(string entryName, string text, int restrictCount)
        {
            if ((text.Length > restrictCount) || (text.Contains(".")))
            {
                text = text.Remove(text.Length - 1);
                this.FindByName<Entry>(entryName).Text = text;
            }
        }

        private void OnTextChanged2(string entryName, string text, int restrictCount)
        {
            if (text.Length > restrictCount)
            {
                text = text.Remove(text.Length - 1);
                this.FindByName<Entry>(entryName).Text = text;
            }
        }
    }
}