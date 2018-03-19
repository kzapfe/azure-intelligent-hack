using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IntelligentHack.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            TraceManager.SendTrace(context, "RootDialog", "Begin");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            await context.Forward(new TranslateDialog(), AfterTranslateAsync, activity, CancellationToken.None);
        }

        private async Task AfterTranslateAsync(IDialogContext context, IAwaitable<object> result)
        {
            string welcome = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Welcome}", Settings.SpecificLanguage);

            await context.PostAsync(welcome);
            
            string QuestionPrompt = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch}", Settings.SpecificLanguage);
            string NotValid = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch_NotValid}", Settings.SpecificLanguage);
            string TooManyAttempts = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.TooManyAttempts}", Settings.SpecificLanguage);

            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, NotValid, TooManyAttempts, await Collections.ReportSearch.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnMenuReportSearchSelected, options);
        }

        private async Task OnMenuReportSearchSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                string report = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch_Report}", Settings.SpecificLanguage);
                string search = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.MenuReportSearch_Search}", Settings.SpecificLanguage);

                if (selected.ToLower().Contains(report.ToLower()))
                {
                    await context.Forward(new RegistrationDialog(), AfterRegistrationAsync, context.Activity, CancellationToken.None);
                }
                else if (selected.ToLower().Contains(search.ToLower()))
                {
                    await context.Forward(new SearchDialog(), AfterSearchAsync, context.Activity, CancellationToken.None);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task AfterRegistrationAsync(IDialogContext context, IAwaitable<object> result)
        {
            ClearConversation(context);
        }

        private async Task AfterSearchAsync(IDialogContext context, IAwaitable<object> result)
        {
            ClearConversation(context);
        }

        private void ClearConversation(IDialogContext context)
        {
            TraceManager.SendTrace(context, "RootDialog", "End");
            context.Wait(MessageReceivedAsync);
            context.PrivateConversationData.Clear();
        }
    }
}