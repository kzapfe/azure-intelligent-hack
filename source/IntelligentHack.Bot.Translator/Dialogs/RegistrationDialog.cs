using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
using IntelligentHack.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IntelligentHack.Bot.Dialogs
{
    [Serializable]
    public class RegistrationDialog : IDialog<object>
    {
        public const string REGISTRATIONDATA = "REGISTRATIONDATA";

        public Task StartAsync(IDialogContext context)
        {
            TraceManager.SendTrace(context, "RegistrationDialog", "Begin");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string QuestionPrompt = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration}", Settings.SpecificLanguage);
            string NotValid = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_NotValid}", Settings.SpecificLanguage);
            string TooManyAttempts = await Translator.TranslateSentenceAsync($"{Resources.Resource.TooManyAttempts}", Settings.SpecificLanguage);

            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, NotValid, TooManyAttempts, await Collections.Registration.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnRegistrationSelected, options);
        }

        private async Task OnRegistrationSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                string yes = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Yes}", Settings.SpecificLanguage);
                string no = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_No}", Settings.SpecificLanguage);

                if (selected.ToLower().Contains(yes.ToLower()))
                {
                }
                else if (selected.ToLower().Contains(no.ToLower()))
                {
                    string advice = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_AdviseNoIdentification}", Settings.SpecificLanguage);

                    await context.PostAsync(advice);
                    var registrationFormDialog = FormDialog.FromForm(this.BuildRegistrationForm, FormOptions.PromptFieldsWithValues);
                    await context.Forward(registrationFormDialog, AfterRegistrationAsync, context.Activity, CancellationToken.None);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task<RegistrationQueryAllText> BuildAsync()
        {
            RegistrationQueryAllText result = new RegistrationQueryAllText();
            result.Name = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Name}", Settings.SpecificLanguage);
            result.Lastname = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Lastname}", Settings.SpecificLanguage);
            result.CountryText = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Country}", Settings.SpecificLanguage);
            result.LocationOfLost = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_LocationOfLost}", Settings.SpecificLanguage);
            result.DateOfLost = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_DateOfLost}", Settings.SpecificLanguage);
            result.ReportId = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_ReportId}", Settings.SpecificLanguage);
            result.ReportedBy = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_ReportedBy}", Settings.SpecificLanguage);
            result.GenreText = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Genre}", Settings.SpecificLanguage);

            return result;
        }

        private IForm<RegistrationQuery> BuildRegistrationForm()
        {
            OnCompletionAsyncDelegate<RegistrationQuery> processRegistration = async (context, state) =>
            {
                string waiting = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_WaitingForImage}", Settings.SpecificLanguage);

                await context.PostAsync(waiting);
                context.PrivateConversationData.SetValue(REGISTRATIONDATA, state);
            };

            RegistrationQueryAllText res = null;
            Task.Run(BuildAsync).ContinueWith((b)=> { res = b.Result; }).Wait();

            return new FormBuilder<RegistrationQuery>()
                .Field(nameof(RegistrationQuery.Name), res.Name)
                .Field(nameof(RegistrationQuery.Lastname), res.Lastname)
                .Field(nameof(RegistrationQuery.Country), res.CountryText)
                .Field(nameof(RegistrationQuery.LocationOfLost), res.LocationOfLost)
                .Field(nameof(RegistrationQuery.DateOfLost), res.DateOfLost)
                .Field(nameof(RegistrationQuery.ReportId), res.ReportId)
                .Field(nameof(RegistrationQuery.ReportedBy), res.ReportedBy)
                .Field(nameof(RegistrationQuery.Genre), res.GenreText)
                .OnCompletion(processRegistration)
                .Build();
        }

        private async Task AfterRegistrationAsync(IDialogContext context, IAwaitable<RegistrationQuery> result)
        {
            context.Wait(ImageReceivedAsync);
        }

        private async Task ImageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string inprogress = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_InProgress}", Settings.SpecificLanguage);

            await context.PostAsync(inprogress);

            var message = await result;

            var data = context.PrivateConversationData.GetValueOrDefault<RegistrationQuery>(REGISTRATIONDATA);

            var person = new Person
            {
                Country = data.Country.ToString(),
                ReportedBy = data.ReportedBy,
                Name = data.Name,
                Lastname = data.Lastname,
                LocationOfLost = data.LocationOfLost,
                DateOfLost = data.DateOfLost,
                ReportId = data.ReportId,
                Genre = data.Genre.ToString(),
                Complexion = string.Empty,
                Skin = string.Empty,
                Front = string.Empty,
                Mouth = string.Empty,
                Eyebrows = string.Empty,
                Age = string.Empty,
                Height = string.Empty,
                Face = string.Empty,
                Nose = string.Empty,
                Lips = string.Empty,
                Chin = string.Empty,
                TypeColorEyes = string.Empty,
                TypeColorHair = string.Empty,
                ParticularSigns = string.Empty,
                Clothes = string.Empty
            };

            byte[] imageBytes = null;

            if (message.Attachments.Count > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    imageBytes = await httpClient.GetByteArrayAsync(message.Attachments[0].ContentUrl);
                }
            }

            var pid = Guid.NewGuid().ToString();

            if (await StorageHelper.UploadMetadata(pid, person))
            {
                try
                {
                    Stream stream = new MemoryStream(imageBytes);
                    if (await StorageHelper.UploadPhoto(pid, stream))
                    {
                        string success = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Success}", Settings.SpecificLanguage);

                        await context.PostAsync(success);
                        TraceManager.SendTrace(context, "RegistrationDialog", "End");
                        context.Done("done");
                    }
                    else
                    {
                        string fail = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Fail}", Settings.SpecificLanguage);

                        await context.PostAsync(fail);
                        TraceManager.SendTrace(context, "RegistrationDialog", "End");
                        context.Done("done");
                    }
                }
                catch (Exception)
                {
                    string fail = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Fail}", Settings.SpecificLanguage);

                    await context.PostAsync(fail);
                    TraceManager.SendTrace(context, "RegistrationDialog", "End");
                    context.Done("done");
                }
               
            }
            else
            {
                string fail = await Translator.TranslateSentenceAsync($"{Resources.Resource.Registration_Fail}", Settings.SpecificLanguage);

                await context.PostAsync(fail);
                TraceManager.SendTrace(context, "RegistrationDialog", "End");
                context.Done("done");
            }
        }
    }
}