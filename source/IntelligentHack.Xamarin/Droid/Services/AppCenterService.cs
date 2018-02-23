using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

[assembly: Dependency(typeof(IntelligentHack.Droid.Services.AppCenterService))]
namespace IntelligentHack.Droid.Services
{
    public class AppCenterService : Interfaces.IAppCenterService
    {
        public void Initialize()
        {
            //telemetry on Mobile Center
            AppCenter.Start(Settings.MobileCenterID_Android, typeof(Analytics), typeof(Crashes));
        }
    }
}