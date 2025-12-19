using System.Collections.Generic;
using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookAllowedMention
    {
        [JsonIgnore] 
        private WebhookMessage? _parent;
        [JsonProperty("parse", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Parse;
        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Roles;
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Users;
        [JsonProperty("replied_user", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RepliedUser;

        internal WebhookAllowedMention(WebhookMessage? parent)
        {
            _parent = parent;
        }

        public WebhookAllowedMention()
        {
        }

        public WebhookMessage Build()
        {
            return _parent ??= new WebhookMessage {AllowedMentions = new List<WebhookAllowedMention> {this}};
        }

        public WebhookAllowedMention WithParse(List<string>? parse)
        {
            Parse = parse;
            return this;
        }

        public WebhookAllowedMention WithRoles(List<string>? roles)
        {
            Roles = roles;
            return this;
        }

        public WebhookAllowedMention WithUsers(List<string>? users)
        {
            Users = users;
            return this;
        }

        public WebhookAllowedMention WithRepliedUser()
        {
            RepliedUser = false;
            return this;
        }
    }
}