using System.Threading.Tasks;
using Xamarin.Forms;

namespace IntelligentHack.Interfaces
{
    public interface INavigationService
    {
        Task PopAsync();

        Task PopModalAsync();

        Task PushModalAsync(Page page);

        Task PopToRootAsync();

        Task PushAsync(Page page);
    }
}