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
            string QuestionPrompt = $"{Resources.Resource.Search_Question}";
            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, $"{Resources.Resource.Search_NotValid}", $"{Resources.Resource.TooManyAttempts}", Collections.Search.CreateList(), 1);
            PromptDialog.Choice<string>(context, OnSearchModeSelected, options);
        }

        private async Task OnSearchModeSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selected = await result;

                if (selected.ToLower().Contains($"{Resources.Resource.Search_Photo.ToLower()}"))
                {
                    await context.PostAsync($"{Resources.Resource.Search_WaitingForImage}");
                    context.Wait(ImageReceivedAsync);
                }
                else if (selected.ToLower().Contains($"{Resources.Resource.Search_NameLastname.ToLower()}"))
                {
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ImageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync($"{Resources.Resource.Search_InProgress}");

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
                await context.PostAsync($"{Resources.Resource.Search_VerificationError}");
                TraceManager.SendTrace(context, "SearchDialog", "End");
                context.Done("done");
            }

            if (!list.Any())
            {
                await context.PostAsync($"{Resources.Resource.Search_NoItems}");
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