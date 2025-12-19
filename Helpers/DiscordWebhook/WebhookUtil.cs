using System;
using System.Globalization;

namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public static class WebhookUtil
    {
        public static string DateTimeToIso(DateTime dateTime)
        {
            return DateTimeToIso(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static string DateTimeToIso(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second, 0, DateTimeKind.Local)
                .ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
        }
    }
}