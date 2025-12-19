using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rocket.API;
using Rocket.Core.Assets;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Storages
{
    public class XmlDataStore<T> : Asset<T> where T : class, IDefaultable
    {
        #region Methods

        public override void Load(AssetLoaded<T>? callback = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(DataPath) && File.Exists(DataPath))
                {
                    using var stream = File.OpenText(DataPath);
                    instance = (T) Serializer.Deserialize(stream);
                }
                
                if (callback == null)
                    return;
                
                callback(this);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] XmlDataStore Load: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
            }
        }

        public Task LoadAsync(AssetLoaded<T>? callback = null)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Load();
                }
                catch (Exception e)
                {
                    var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] XmlDataStore LoadAsync: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return Task.CompletedTask;
        }

        public override T Save()
        {
            try
            {
                var directoryName = Path.GetDirectoryName(DataPath) ?? string.Empty;
                if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                using var stream = new FileStream(DataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using var streamWriter = new StreamWriter(stream);
                if (instance == null)
                {
                    if (DefaultInstance == null)
                    {
                        instance = Activator.CreateInstance<T>();
                        instance.LoadDefaults();
                    }
                    else
                        instance = DefaultInstance;
                }
                Serializer.Serialize(streamWriter, instance);
                streamWriter.Close();
                stream.Close();
                return instance;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] XmlDataStore Save: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public Task<T> SaveAsync()
        {
            var source = new TaskCompletionSource<T>();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    source.SetResult(Save());
                }
                catch (Exception e)
                {
                    source.SetException(e);
                    var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] XmlDataStore SaveAsync: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return source.Task;
        }

        #endregion

        public XmlDataStore(string dir, string fileName, Type[]? extraTypes = null, T? defaultInstance = null)
        {
            DataPath = Path.Combine(dir, fileName);
            Serializer = new XmlSerializer(typeof (T), extraTypes ?? Type.EmptyTypes);
            DefaultInstance = defaultInstance;
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            if (!File.Exists(DataPath))
                File.CreateText(DataPath).Dispose();
        }

        private string DataPath { get; }
        private XmlSerializer Serializer { get; }
        private T? DefaultInstance { get; }
    }
}