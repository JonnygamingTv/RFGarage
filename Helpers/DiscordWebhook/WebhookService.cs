using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public static class WebhookService
    {
        public static void PostMessage(this WebhookMessage webhookMessage, string webhookURL, int? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "POST";
            request.ContentType = "application/json";

            var payload = JsonConvert.SerializeObject(webhookMessage);
            var buffer = Encoding.UTF8.GetBytes(payload);

            request.ContentLength = buffer.Length;
            using (var write = request.GetRequestStream())
            {
                write.Write(buffer, 0, buffer.Length);
                write.Flush();
            }

            // _ = (HttpWebResponse)request.GetResponse();
            request.GetResponse();
        }

        public static async void PostMessageAsync(this WebhookMessage webhookMessage, string webhookURL, string? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "POST";
            request.ContentType = "application/json";

            var payload = JsonConvert.SerializeObject(webhookMessage);
            var buffer = Encoding.UTF8.GetBytes(payload);

            request.ContentLength = buffer.Length;
            using (var write = (await request.GetRequestStreamAsync()))
            {
                await write.WriteAsync(buffer, 0, buffer.Length);
                await write.FlushAsync();
            }

            // _ = (HttpWebResponse)await request.GetResponseAsync();
            await request.GetResponseAsync();
        }
        
        // public static void GetMessage(this WebhookMessage webhookMessage, string webhookURL, string? threadId = null, string? messageId = null)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public static async void GetMessageAsync(this WebhookMessage webhookMessage, string webhookURL, string? threadId = null, string? messageId = null)
        // {
        //     throw new NotImplementedException();
        // }
        
        public static void EditMessage(this WebhookMessage webhookMessage, string webhookURL, string messageId, int? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageId}" + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "PATCH";
            request.ContentType = "application/json";

            var payload = JsonConvert.SerializeObject(webhookMessage);
            var buffer = Encoding.UTF8.GetBytes(payload);

            request.ContentLength = buffer.Length;
            using (var write = request.GetRequestStream())
            {
                write.Write(buffer, 0, buffer.Length);
                write.Flush();
            }

            // _ = (HttpWebResponse)request.GetResponse();
            request.GetResponse();
        }

        public static async void EditMessageAsync(this WebhookMessage webhookMessage, string webhookURL, string messageId, string? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageId}" + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "PATCH";
            request.ContentType = "application/json";

            var payload = JsonConvert.SerializeObject(webhookMessage);
            var buffer = Encoding.UTF8.GetBytes(payload);

            request.ContentLength = buffer.Length;
            using (var write = (await request.GetRequestStreamAsync()))
            {
                await write.WriteAsync(buffer, 0, buffer.Length);
                await write.FlushAsync();
            }

            // _ = (HttpWebResponse)await request.GetResponseAsync();
            await request.GetResponseAsync();
        }
        
        public static void DeleteMessage(this WebhookMessage webhookMessage, string webhookURL, string messageId, string? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageId}" + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "DELETE";
            request.GetResponse();
        }
        
        public static async void DeleteMessageAsync(this WebhookMessage webhookMessage, string webhookURL, string messageId, string? threadId = null)
        {
            var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageId}" + (threadId == null ? string.Empty : $"?thread_id={threadId}"));
            request.Proxy = new WebProxy();
            request.Method = "DELETE";
            await request.GetResponseAsync();
        }
    }
}