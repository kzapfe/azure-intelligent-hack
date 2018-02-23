using IntelligentHack.iOS.Services;
using IntelligentHack.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(MobileAppService))]
namespace IntelligentHack.iOS.Services
{
    public class MobileAppService : IMobileAppService
    {
        public void Initialize()
        {
            //create the client instance, using the mobile app backend URL.
            AppDelegate.MobileClient = new MobileServiceClient(Settings.FunctionURL);
        }
    }
}