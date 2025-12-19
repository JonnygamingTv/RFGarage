using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookFooter
    {
        [JsonProperty("text")] public string Text = "";

        [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string? IconURL;

        [JsonProperty("proxy_icon_url", NullValueHandling = NullValueHandling.Ignore)]
        public string? ProxyIconURL;
    }
}