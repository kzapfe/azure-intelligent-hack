using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
using IntelligentHack.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IntelligentHack.Bot.Dialogs
{
    [Serializable]
    public class SearchDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            TraceManager.SendTrace(context, "SearchDialog", "Begin");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string QuestionPrompt = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_Question}", Settings.SpecificLanguage);
            string NotValid = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_NotValid}", Settings.SpecificLanguage);
            string TooManyAttempts = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.TooManyAttempts}", Settings.SpecificLanguage);

            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, NotValid, TooManyAttempts, await Collections.Search.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnSearchModeSelected, options);
        }

        private async Task OnSearchModeSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                string photo = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_Photo}", Settings.SpecificLanguage);
                string namelastname = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_NameLastname}", Settings.SpecificLanguage);


                if (selected.ToLower().Contains(photo.ToLower()))
                {
                    string waiting = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_WaitingForImage}", Settings.SpecificLanguage);

                    await context.PostAsync(waiting);
                    context.Wait(ImageReceivedAsync);
                }
                else if (selected.ToLower().Contains(namelastname.ToLower()))
                {
                    var registrationFormDialog = FormDialog.FromForm(this.BuildSearchForm, FormOptions.PromptFieldsWithValues);
                    await context.Forward(registrationFormDialog, AfterSearchAsync, context.Activity, CancellationToken.None);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task<SearchQueryAllText> BuildAsync()
        {
            SearchQueryAllText result = new SearchQueryAllText();
            result.Name = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Registration_Name}", Settings.SpecificLanguage);
            result.Lastname = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Registration_Lastname}", Settings.SpecificLanguage);
            result.CountryText = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Registration_Country}", Settings.SpecificLanguage);

            return result;
        }

        private IForm<SearchQuery> BuildSearchForm()
        {
            OnCompletionAsyncDelegate<SearchQuery> processSearch = async (context, state) =>
            {
            };

            SearchQueryAllText res = null;
            Task.Run(BuildAsync).ContinueWith((b) => { res = b.Result; }).Wait();

            return new FormBuilder<SearchQuery>()
                .Field(nameof(SearchQuery.Name), res.Name)
                .Field(nameof(SearchQuery.Lastname), res.Lastname)
                .Field(nameof(SearchQuery.Country), res.CountryText)
                .OnCompletion(processSearch)
                .Build();
        }

        private async Task AfterSearchAsync(IDialogContext context, IAwaitable<SearchQuery> result)
        {
            var state = await result;

            MetadataVerification metadata = new MetadataVerification();
            metadata.Name = state.Name;
            metadata.Lastname = state.Lastname;
            metadata.Country = state.Country.ToString();

            var list = new List<Person>();
            list = await RestHelper.MetadataVerification(metadata);

            if (!list.Any())
            {
                var noitems = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_NoItems}", Settings.SpecificLanguage);

                await context.PostAsync(noitems);
                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }
            else
            {
                var reply = context.MakeMessage();

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsAttachments(list);

                await context.PostAsync(reply);

                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }
        }

        private async Task ImageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var inprogress = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_InProgress}", Settings.SpecificLanguage);

            await context.PostAsync(inprogress);

            var message = await result;

            byte[] imageBytes = null;

            if (message.Attachments.Count > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    imageBytes = await httpClient.GetByteArrayAsync(message.Attachments[0].ContentUrl);
                }
            }

            Stream stream = new MemoryStream(imageBytes);

            var pid = Guid.NewGuid().ToString();
            var extension = "jpg";
            var list = new List<Person>();

            if (await StorageHelper.UploadPhoto(pid, stream, true))
            {
                list = await RestHelper.ImageVerification($"{pid}.{extension}");
            }
            else
            {
                var verification = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_VerificationError}", Settings.SpecificLanguage);

                await context.PostAsync(verification);
                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }

            if (!list.Any())
            {
                var noitems = await TranslatorHelper.TranslateSentenceAsync($"{Resources.Resource.Search_NoItems}", Settings.SpecificLanguage);

                await context.PostAsync(noitems);
                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }
            else
            {
                var reply = context.MakeMessage();

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsAttachments(list);

                await context.PostAsync(reply);

                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }
        }

        private static IList<Attachment> GetCardsAttachments(List<Person> list)
        {
            List<Attachment> result = new List<Attachment>();
            foreach(Person p in list)
            {
                var element = GetThumbnailCard($"{p.Name} {p.Lastname}", $"{p.Country} {p.LocationOfLost}", $"", new CardImage(url: $"{Settings.ImageStorageUrl}{p.Picture}"));
                result.Add(element);
            }

            return result;
        }

        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}