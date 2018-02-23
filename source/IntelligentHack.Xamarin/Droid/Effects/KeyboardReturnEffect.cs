using System;
using IntelligentHack.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(KeyboardReturnEffect), "KeyboardReturnEffect")]
namespace IntelligentHack.Droid.Effects
{
    public class KeyboardReturnEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control == null)
                return;

            // var editText = Control as Android.Widget.EditText;
        }

        protected override void OnDetached()
        {
        }
    }
}