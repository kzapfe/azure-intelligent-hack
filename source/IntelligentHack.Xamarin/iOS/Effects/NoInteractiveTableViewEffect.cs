using IntelligentHack.iOS.Effects;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("IntelligentHack")]
[assembly: ExportEffect(typeof(NoInteractiveTableViewEffect), "NoInteractiveTableViewEffect")]
namespace IntelligentHack.iOS.Effects
{
    public class NoInteractiveTableViewEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control == null)
                return;
            
            ((UITableView)Control).SeparatorStyle = UITableViewCellSeparatorStyle.None;
            ((UITableView)Control).AllowsSelection = false;
        }

        protected override void OnDetached()
        {
        }
    }
}