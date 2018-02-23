using System;
using Xamarin.Forms;
using System.Threading;
using System.Globalization;
using IntelligentHack.iOS.Services;
using IntelligentHack.Interfaces;
using Foundation;

[assembly: Dependency(typeof(LocalizeService))]
namespace IntelligentHack.iOS.Services
{
    public class LocalizeService : ILocalizeService
    {
        public void Set(string culture)
        {
            CultureInfo ci = new CultureInfo(culture);
            Resx.AppResources.Culture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("CurrentCulture set: " + ci.DisplayName);
        }
    }
}