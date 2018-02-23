using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

[assembly: Dependency(typeof(IntelligentHack.iOS.Services.AppCenterService))]
namespace IntelligentHack.iOS.Services
{
    public class AppCenterService : Interfaces.IAppCenterService
    {
        public void Initialize()
        {
            //telemetry on Mobile Center.
            AppCenter.Start(Settings.MobileCenterID_iOS, typeof(Analytics), typeof(Crashes));
        }
    }
}