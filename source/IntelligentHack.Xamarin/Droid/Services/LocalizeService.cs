using System;
using Xamarin.Forms;
using System.Threading;
using System.Globalization;
using IntelligentHack.Droid.Services;
using IntelligentHack.Interfaces;

[assembly:Dependency(typeof(LocalizeService))]
namespace IntelligentHack.Droid.Services
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