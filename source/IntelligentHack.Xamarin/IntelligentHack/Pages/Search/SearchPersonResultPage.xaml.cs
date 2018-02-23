using IntelligentHack.Domain;
using IntelligentHack.ViewModels;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace IntelligentHack.Pages
{
    public partial class SearchPersonResultPage : BasePage
    {
        public SearchPersonResultPage(bool isByPicture, byte[] picture, Person person)
        {
            InitializeComponent();
            BindingContext = new SearchPersonResultViewModel(isByPicture, picture, person);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent("View: Search Person Results");
        }

        private void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var vm = BindingContext as SearchPersonResultViewModel;
            if (e.SelectedItem != null)
            {
                var person = e.SelectedItem as Person;

                if (vm.OnSelectedItemCommand.CanExecute(person))
                {
                    vm.OnSelectedItemCommand.Execute(person);

                    ((ListView)sender).SelectedItem = null;
                }
            }
        }
    }
}