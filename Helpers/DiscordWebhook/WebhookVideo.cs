using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookVideo
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)] 
        public string? URL;

        [JsonProperty("proxy_url", NullValueHandling = NullValueHandling.Ignore)]
        public string? ProxyURL;

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public int? Height;

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public int? Width;
    }
}