using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookAuthor
    {
        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string? URL;

        [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string? IconURL;

        [JsonProperty("proxy_icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string? ProxyIconURL;
    }
}