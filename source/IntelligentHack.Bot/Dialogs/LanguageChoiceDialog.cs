using IntelligentHack.Bot.Classes;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using static IntelligentHack.Bot.Classes.Collections;

namespace IntelligentHack.Bot.Dialogs
{
    [Serializable]
    public class LanguageChoiceDialog : IDialog<object>
    {
        public const string LCID = "LCID";
        public const string COMPLETED = "COMPLETED";

        public Task StartAsync(IDialogContext context)
        {
            TraceManager.SendTrace(context, "LanguageChoiceDialog", "Begin");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (!context.PrivateConversationData.ContainsKey(COMPLETED))
            {
                string QuestionPrompt = "Please choose a language.";
                PromptOptions<Option> options = new PromptOptions<Option>(QuestionPrompt, "Not a valid language.", $"{Resources.Resource.TooManyAttempts}", Collections.Option.CreateList(), 1);
                PromptDialog.Choice<Option>(context, this.OnLanguageSelected, options);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task OnLanguageSelected(IDialogContext context, IAwaitable<Option> result)
        {
            try
            {
                Option optionSelected = await result;

                switch (optionSelected.Text)
                {
                    case "English":
                    case "Spanish":
                        context.PrivateConversationData.SetValue(LCID, optionSelected.Locale);

                        string QuestionPrompt = "Settings updated, press 'Continue' to refresh.";
                        PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, "Not a valid option.", $"{Resources.Resource.TooManyAttempts}", Collections.Continue.CreateList(), 1);
                        PromptDialog.Choice<string>(context, OnMenuSelected, options);

                        break;

                    default:
                        context.Wait(MessageReceivedAsync);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task OnMenuSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                if (selected.ToLower().Contains("continue"))
                {
                    if (context.PrivateConversationData.ContainsKey(LCID))
                    {
                        context.PrivateConversationData.SetValue(COMPLETED, true);
                        context.Done("done");
                    }
                    TraceManager.SendTrace(context, "LanguageChoiceDialog", "End");
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}