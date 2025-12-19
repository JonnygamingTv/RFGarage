using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public class WebhookEmbed
    {
        [JsonIgnore] private WebhookMessage? _parent;

        internal WebhookEmbed(WebhookMessage? parent)
        {
            _parent = parent;
        }

        public WebhookEmbed()
        {
        }

        public WebhookMessage Build()
        {
            return _parent ??= new WebhookMessage {Embeds = new List<WebhookEmbed> {this}};
        }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public int? Color;

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookAuthor? Author;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string? Title;

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string? URL;

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description;

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<WebhookField>? Fields;

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookImage? Image;

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookThumbnail? Thumbnail;

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookFooter? Footer;

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookProvider? Provider;

        [JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
        public WebhookVideo? Video;

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string? Timestamp;

        public WebhookEmbed WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public WebhookEmbed WithURL(string value)
        {
            URL = value;
            return this;
        }

        public WebhookEmbed WithDescription(string value)
        {
            Description = value;
            return this;
        }

        public WebhookEmbed WithTimestamp(DateTime value)
        {
            Timestamp = WebhookUtil.DateTimeToIso(value);
            return this;
        }

        public WebhookEmbed WithField(string name, string value, bool? inline = null)
        {
            if (Fields == null)
                Fields = new List<WebhookField> {new() {Value = value, Inline = inline, Name = name}};
            else
                Fields.Add(new WebhookField {Value = value, Inline = inline, Name = name});
            return this;
        }

        public WebhookEmbed WithImage(string url, int? height = null, int? width = null, string? proxyURL = null)
        {
            Image = new WebhookImage {URL = url, Height = height, Width = width, ProxyURL = proxyURL};
            return this;
        }

        public WebhookEmbed WithThumbnail(string url, int? height = null, int? width = null, string? proxyURL = null)
        {
            Thumbnail = new WebhookThumbnail {URL = url, Height = height, Width = width, ProxyURL = proxyURL};
            return this;
        }

        public WebhookEmbed WithAuthor(string name, string? url = null, string? icon = null,
            string? proxyIconURL = null)
        {
            Author = new WebhookAuthor {Name = name, IconURL = icon, URL = url, ProxyIconURL = proxyIconURL};
            return this;
        }

        public WebhookEmbed WithFooter(string text, string? icon = null, string? proxyIconURL = null)
        {
            Footer = new WebhookFooter {Text = text, IconURL = icon, ProxyIconURL = proxyIconURL};
            return this;
        }

        public WebhookEmbed WithProvider(string? text = null, string? url = null)
        {
            Provider = new WebhookProvider {Name = text, URL = url};
            return this;
        }

        public WebhookEmbed WithVideo(string url, int? height = null, int? width = null, string? proxyURL = null)
        {
            Video = new WebhookVideo {URL = url, Height = height, Width = width, ProxyURL = proxyURL};
            return this;
        }

        public WebhookEmbed WithColor(Color color)
        {
            Color = BitConverter.ToInt32(new byte[] {color.B, color.G, color.R, 0}, 0);
            return this;
        }

        // Unity Color
        public WebhookEmbed WithColor(UnityEngine.Color color)
        {
            var r = Clamp(color.r);
            var g = Clamp(color.g);
            var b = Clamp(color.b);

            var numeric = BitConverter.ToInt32(new byte[] {b, g, r, 0}, 0);
            Color = numeric;
            return this;
        }

        private static byte Clamp(float a)
        {
            return (byte) Math.Round(a * 255, 0);
        }
    }
}