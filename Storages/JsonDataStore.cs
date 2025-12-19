using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Storages
{
    public class JsonDataStore<T> where T : class
    {
        #region Methods

        public T? Load()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return null;

                string dataText;
                using (var stream = File.OpenText(DataPath))
                    dataText = stream.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(dataText);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore Load: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public async Task<T?> LoadAsync()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return null;
                string dataText;
                using (var stream = File.OpenText(DataPath))
                    dataText = await stream.ReadToEndAsync();

                return JsonConvert.DeserializeObject<T>(dataText);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore LoadAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public bool Save(T obj)
        {
            return Save(obj, Formatting.Indented);
        }

        public bool Save(T obj, Formatting format = Formatting.Indented)
        {
            try
            {
                var objData = JsonConvert.SerializeObject(obj, format);

                using var stream = new FileStream(DataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using var streamWriter = new StreamWriter(stream);
                streamWriter.Write(objData);

                return true;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore SaveAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }
        public async Task<bool> SaveAsync(T obj)
        {
            return await SaveAsync(obj, Formatting.Indented);
        }
        public async Task<bool> SaveAsync(T obj, Formatting format = Formatting.Indented)
        {
            try
            {
                var objData = JsonConvert.SerializeObject(obj, format);

                using var stream = new FileStream(DataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using var streamWriter = new StreamWriter(stream);
                await streamWriter.WriteAsync(objData);

                return true;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore SaveAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        #endregion

        public JsonDataStore(string dir, string fileName)
        {
            DataPath = Path.Combine(dir, fileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(DataPath))
                File.Create(DataPath).Dispose();
        }

        private string DataPath { get; set; }
    }
}