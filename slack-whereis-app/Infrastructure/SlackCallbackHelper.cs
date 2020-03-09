using HenryKam.SlackWhereIs.Infrastructure.Slack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Infrastructure
{
    public static class SlackCallbackHelper
    {

        public async static void PostCallback(Uri endpoint, IHttpClientFactory clientFactory, string message)
        {
            var httpClient = clientFactory.CreateClient();

            var payload = new { Text = message };

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(payload, Settings);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(endpoint, httpContent);
        }

        public async static void PostCallback(Uri endpoint, IHttpClientFactory clientFactory, SimpleSlackBlockResponse blocks)
        {
            var httpClient = clientFactory.CreateClient();

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(blocks);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(endpoint, httpContent);
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TypeEnumConverter.Singleton
            },
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

}
