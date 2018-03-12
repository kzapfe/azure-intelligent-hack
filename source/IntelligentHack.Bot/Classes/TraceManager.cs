using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IntelligentHack.Bot.Classes
{
    public class TraceManager : IActivityLogger
    {
        public static void SendTrace(IDialogContext context, string dialog, string message)
        {
            if (Settings.EnableCustomLog)
                context.PostAsync($"*** Dialog: {dialog}. *** Message: {message}");
        }

        public async Task LogAsync(IActivity activity)
        {
            if (Settings.EnableVerboseLog)
                Debug.WriteLine($"From:{activity.From.Id} - To:{activity.Recipient.Id} - Message:{activity.AsMessageActivity()?.Text}");
        }
    }
}