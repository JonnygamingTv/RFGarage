using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookField
    {
        [JsonProperty("name")] public string Name = "";

        [JsonProperty("value")] public string Value = "";

        [JsonProperty("inline", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Inline;
    }
}