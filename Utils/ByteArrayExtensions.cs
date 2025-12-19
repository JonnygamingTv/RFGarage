using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Utils
{
    public static class ByteArrayExtensions
    {
        #region Methods

        public static T? Deserialize<T>(this byte[] byteArray)
        {
            try
            {
                using var ms = new MemoryStream(byteArray);
                return (T)new BinaryFormatter().Deserialize(ms);
            }
            catch (Exception e)
            {
                var name = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{name}] [ERROR] ByteArray Serialize: {e.Message}");
                Logger.LogError($"[{name}] [ERROR] Details: {e}");
                return default;
            }
        }

        public static byte[] Serialize<T>(this T m)
        {
            try
            {
                using var ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, m);
                return ms.ToArray();
            }
            catch (Exception e)
            {
                var name = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{name}] [ERROR] ByteArray Serialize: {e.Message}");
                Logger.LogError($"[{name}] [ERROR] Details: {e}");
                return Array.Empty<byte>();
            }
        }

        #endregion
    }
}