using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookAttachment
    {
        [JsonIgnore] private WebhookMessage? _parent;

        [JsonProperty("id")] public string Id = "";

        [JsonProperty("filename")] public string FileName = "";

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description;

        [JsonProperty("content_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? ContentType;

        [JsonProperty("size")] public int Size;

        [JsonProperty("url")] public string URL = "";

        [JsonProperty("proxy_url")] public string ProxyURL = "";

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public int? Height;

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public int? Width;

        [JsonProperty("ephemeral", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ephemeral;

        internal WebhookAttachment(WebhookMessage? parent)
        {
            _parent = parent;
        }

        public WebhookAttachment()
        {
        }

        // public WebhookMessage Build()
        // {
        //     return _parent ??= new WebhookMessage {Attachment = new WebhookAttachment {this}};
        // }

        public WebhookAttachment WithId(string id)
        {
            Id = id;
            return this;
        }

        public WebhookAttachment WithFileName(string fileName)
        {
            FileName = fileName;
            return this;
        }

        public WebhookAttachment WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public WebhookAttachment WithContentType(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public WebhookAttachment WithSize(int size)
        {
            Size = size;
            return this;
        }

        public WebhookAttachment WithURL(string url)
        {
            URL = url;
            return this;
        }

        public WebhookAttachment WithProxyURL(string proxyURL)
        {
            ProxyURL = proxyURL;
            return this;
        }

        public WebhookAttachment WithHeight(int height)
        {
            Height = height;
            return this;
        }

        public WebhookAttachment WithWidth(int width)
        {
            Width = width;
            return this;
        }

        public WebhookAttachment WithEphemeral()
        {
            Ephemeral = true;
            return this;
        }
    }
}