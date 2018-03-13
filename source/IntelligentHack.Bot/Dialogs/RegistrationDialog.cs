using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
using IntelligentHack.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
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
            string QuestionPrompt = $"{Resources.Resource.Registration}";
            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, $"{Resources.Resource.Registration_NotValid}", $"{Resources.Resource.TooManyAttempts}", Collections.Registration.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnRegistrationSelected, options);
        }

        private async Task OnRegistrationSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                if (selected.ToLower().Contains($"{Resources.Resource.Registration_Yes.ToLower()}"))
                {
                }
                else if (selected.ToLower().Contains($"{Resources.Resource.Registration_No.ToLower()}"))
                {
                    await context.PostAsync($"{Resources.Resource.Registration_AdviseNoIdentification}");
                    var registrationFormDialog = FormDialog.FromForm(this.BuildRegistrationForm, FormOptions.PromptFieldsWithValues);
                    await context.Forward(registrationFormDialog, AfterRegistrationAsync, context.Activity, CancellationToken.None);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private IForm<RegistrationQuery> BuildRegistrationForm()
        {
            OnCompletionAsyncDelegate<RegistrationQuery> processRegistration = async (context, state) =>
            {
                await context.PostAsync($"{Resources.Resource.Registration_WaitingForImage}");
                context.PrivateConversationData.SetValue(REGISTRATIONDATA, state);
            };

            return new FormBuilder<RegistrationQuery>()
                .Field(nameof(RegistrationQuery.Name), $"{Resources.Resource.Registration_Name}")
                .Field(nameof(RegistrationQuery.Lastname), $"{Resources.Resource.Registration_Lastname}")
                .Field(nameof(RegistrationQuery.Country), $"{Resources.Resource.Registration_Country}")
                .Field(nameof(RegistrationQuery.LocationOfLost), $"{Resources.Resource.Registration_LocationOfLost}")
                .Field(nameof(RegistrationQuery.DateOfLost), $"{Resources.Resource.Registration_DateOfLost}")
                .Field(nameof(RegistrationQuery.ReportId), $"{Resources.Resource.Registration_ReportId}")
                .Field(nameof(RegistrationQuery.ReportedBy), $"{Resources.Resource.Registration_ReportedBy}")
                .OnCompletion(processRegistration)
                .Build();
        }

        private async Task AfterRegistrationAsync(IDialogContext context, IAwaitable<RegistrationQuery> result)
        {
            context.Wait(ImageReceivedAsync);
        }

        private async Task ImageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync($"{Resources.Resource.Registration_InProgress}");

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
                Genre = string.Empty,
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
                        await context.PostAsync($"{Resources.Resource.Registration_Success}");
                        TraceManager.SendTrace(context, "RegistrationDialog", "End");
                        context.Done("done");
                    }
                    else
                    {
                        await context.PostAsync($"{Resources.Resource.Registration_Fail}");
                        TraceManager.SendTrace(context, "RegistrationDialog", "End");
                        context.Done("done");
                    }
                }
                catch (Exception)
                {
                    await context.PostAsync($"{Resources.Resource.Registration_Fail}");
                    TraceManager.SendTrace(context, "RegistrationDialog", "End");
                    context.Done("done");
                }
               
            }
            else
            {
                await context.PostAsync($"{Resources.Resource.Registration_Fail}");
                TraceManager.SendTrace(context, "RegistrationDialog", "End");
                context.Done("done");
            }
        }
    }
}