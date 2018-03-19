﻿using IntelligentHack.Bot;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace IntelligentHack.Bot.Classes
{
    public static class Translator
    {
        public async static Task<string> GetDesiredLanguageAsync(string content)
        {
            HttpClient languageClient = new HttpClient();
            languageClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.TranslatorKey);
            var detectLanguageResponse = await languageClient.GetStreamAsync(
                $"http://api.microsofttranslator.com/v2/http.svc/Detect?text={content}");

            return (string)new DataContractSerializer(typeof(string)).ReadObject(detectLanguageResponse);
        }

        public async static Task<string> TranslateSentenceAsync(string originalSentence, string languageCode)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.TranslatorKey);
            var translatedResponse = await client.GetStreamAsync(
                $"http://api.microsofttranslator.com/v2/http.svc/translate?text={originalSentence}&from=en&to={languageCode}&category=general"
                );

            return (string)new DataContractSerializer(typeof(string)).ReadObject(translatedResponse);
        }
    }
}