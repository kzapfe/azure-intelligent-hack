using IntelligentHack.Bot.Classes;
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
            if (!context.PrivateConversationData.ContainsKey(LanguageChoiceDialog.LCID))
            {
                await context.Forward(new LanguageChoiceDialog(), AfterLanguageChoiceAsync, activity, CancellationToken.None);
            }
        }

        private async Task AfterLanguageChoiceAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync($"{Resources.Resource.Welcome}");

            string QuestionPrompt = $"{Resources.Resource.MenuReportSearch}";
            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, $"{Resources.Resource.MenuReportSearch_NotValid}", $"{Resources.Resource.TooManyAttempts}", Collections.ReportSearch.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnMenuReportSearchSelected, options);
        }

        private async Task OnMenuReportSearchSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                if (selected.ToLower().Contains($"{Resources.Resource.MenuReportSearch_Report.ToLower()}"))
                {
                    await context.Forward(new RegistrationDialog(), AfterRegistrationAsync, context.Activity, CancellationToken.None);
                }
                else if (selected.ToLower().Contains($"{Resources.Resource.MenuReportSearch_Search.ToLower()}"))
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