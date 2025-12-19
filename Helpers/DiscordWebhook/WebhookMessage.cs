using System.Collections.Generic;
using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookMessage
    {
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)] 
        public string? Username;
        [JsonProperty("avatar_url", NullValueHandling = NullValueHandling.Ignore)] 
        public string? AvatarURL;
        [JsonProperty("content")] public string Content = "";

        [JsonProperty("embeds")]
        public List<WebhookEmbed> Embeds = new();

        [JsonProperty("allowed_mentions", NullValueHandling = NullValueHandling.Ignore)]
        public List<WebhookAllowedMention>? AllowedMentions;

        [JsonProperty("tts", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tts;

        public WebhookMessage WithEmbed(WebhookEmbed webhookEmbed)
        {
            Embeds.Add(webhookEmbed);
            return this;
        }

        public WebhookMessage WithAllowedMention(WebhookAllowedMention webhookAllowedMention)
        {
            if (AllowedMentions != null)
                AllowedMentions.Add(webhookAllowedMention);
            else
                AllowedMentions = new List<WebhookAllowedMention> {webhookAllowedMention};
            return this;
        }

        public WebhookEmbed PassEmbed()
        {
            var embed = new WebhookEmbed(this);
            Embeds.Add(embed);
            return embed;
        }

        public WebhookMessage WithUsername(string username)
        {
            Username = username;
            return this;
        }

        public WebhookMessage WithAvatar(string avatar)
        {
            AvatarURL = avatar;
            return this;
        }

        public WebhookMessage WithContent(string content)
        {
            Content = content;
            return this;
        }

        public WebhookMessage WithTts()
        {
            Tts = true;
            return this;
        }
    }
}