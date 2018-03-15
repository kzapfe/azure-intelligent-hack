using Autofac;
using IntelligentHack.Bot.Classes;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;

namespace IntelligentHack.Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Bot Storage: This is a great spot to register the private state storage for your bot.
            // We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
            // For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure

            //load application settings.
            Settings.DataStorage = ConfigurationManager.AppSettings["DataStorage"];
            Settings.EnableCustomLog = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCustomLog"]);
            Settings.EnableVerboseLog = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableVerboseLog"]);
            Settings.FunctionURL = ConfigurationManager.AppSettings["FunctionURL"];
            Settings.Cryptography = ConfigurationManager.AppSettings["Cryptography"];
            Settings.ImageStorageUrl = ConfigurationManager.AppSettings["ImageStorageUrl"];

            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    // Using Azure Table Storage
                    var store = new TableBotDataStore(Settings.DataStorage); // requires Microsoft.BotBuilder.Azure Nuget package

                    // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
                    // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package
                    // var store = new InMemoryDataStore(); // volatile in-memory store

                    builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();

                    builder.RegisterType<TraceManager>().AsImplementedInterfaces().InstancePerDependency();
                });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}