using Autofac;
using IntelligentHack.Bot.Classes;
using IntelligentHack.Bot.Helpers;
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
            Settings.CosmosDBUri = SettingHelper.GetSetting("CosmosDBUri");
            Settings.CosmosDBKey = SettingHelper.GetSetting("CosmosDBKey");
            Settings.EnableCustomLog = Convert.ToBoolean(SettingHelper.GetSetting("EnableCustomLog"));
            Settings.EnableVerboseLog = Convert.ToBoolean(SettingHelper.GetSetting("EnableVerboseLog"));
            Settings.FunctionURL = SettingHelper.GetSetting("FunctionURL");
            Settings.Cryptography = SettingHelper.GetSetting("Cryptography");
            Settings.ImageStorageUrl = SettingHelper.GetSetting("ImageStorageUrl");
            Settings.TranslatorKey = SettingHelper.GetSetting("TranslatorKey");
            Settings.SpecificLanguage = SettingHelper.GetSetting("SpecificLanguage");

            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    // Using Azure Table Storage
                    //var store = new TableBotDataStore(Settings.DataStorage); // requires Microsoft.BotBuilder.Azure Nuget package

                    // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
                    var store = new DocumentDbBotDataStore(new Uri(Settings.CosmosDBUri), Settings.CosmosDBKey, "AzureIntelligentHack", "Bot"); // requires Microsoft.BotBuilder.Azure Nuget package
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