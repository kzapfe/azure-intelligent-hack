using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IntelligentHack.Bot.Dialogs
{
    [Serializable]
    public class TranslateDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            TraceManager.SendTrace(context, "TranslateDialog", "Begin");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //force to always request the language
            var reply = context.MakeMessage();
            reply.Text = $"{Resources.Resource.Welcome_TranslationMessage}";
            await context.PostAsync(reply);
            context.Wait(TranslationReceivedAsync);
        }

        private async Task TranslationReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            Settings.SpecificLanguage = await TranslatorHelper.GetDesiredLanguageAsync(activity.Text);
            TraceManager.SendTrace(context, "TranslateDialog", "End");
            context.Done("done");
        }
    }
}