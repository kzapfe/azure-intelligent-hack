using IntelligentHack.Domain;
using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;

namespace IntelligentHack.Pages
{
    public partial class PersonDetailPage : BasePage
    {
        private PersonDetailViewModel _vm;
        public PersonDetailViewModel ViewModel => _vm ?? (_vm = BindingContext as PersonDetailViewModel);

        public PersonDetailPage(Person person)
        {
            InitializeComponent();
            BindingContext = new PersonDetailViewModel(person);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Person Detail");
        }
    }
}