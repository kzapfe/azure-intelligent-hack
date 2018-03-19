using Autofac;
using IntelligentHack.Bot.Dialogs;
using IntelligentHack.Bot.Resources;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace IntelligentHack.Bot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                // one of these will have an interface and process it
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:

                        await Conversation.SendAsync(activity, () => new RootDialog());

                        break;

                    case ActivityTypes.ConversationUpdate:
                        if (activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id))
                        {
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            Activity reply = activity.CreateReply($"{Resource.Welcome}");
                            connector.Conversations.ReplyToActivity(reply);

                            await Conversation.SendAsync(activity, () => new RootDialog());
                        }
                        break;

                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                        break;
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}