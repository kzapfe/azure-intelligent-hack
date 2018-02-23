using IntelligentHack.ViewModels;
using Xamarin.Forms;

namespace IntelligentHack.Pages
{
    public class BasePage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as BaseViewModel;
            vm?.OnViewAppear();
        }

        protected override void OnDisappearing()
        {
            var vm = BindingContext as BaseViewModel;
            vm?.OnViewDissapear();
            base.OnDisappearing();
        }
    }
}