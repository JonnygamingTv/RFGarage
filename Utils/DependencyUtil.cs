using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RFRocketLibrary.Constants;
using RFRocketLibrary.Enum;

namespace RFRocketLibrary.Utils
{
    public static class DependencyUtil
    {
        #region Methods

        public static bool CanBeLoaded(EDependency dependency)
        {
            if(Rocket.Core.Plugins.RocketPlugin.IsDependencyLoaded(dependency.ToString()))return false;
            return AppDomain.CurrentDomain.GetAssemblies().All(x =>
                x.FullName != typeof(DependencyFullName).GetField(dependency.ToString()).GetValue(null).ToString());
        }

        public static string GetCachePath(EDependency dependency)
        {
            var rocketDir = Environment.CurrentDirectory;
            var librariesDir = Path.Combine(rocketDir, "Libraries");
            var cacheDir = Path.Combine(librariesDir, "Cache");
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            return Path.Combine(cacheDir,
                typeof(DependencyIntegrity).GetField(dependency.ToString()).GetValue(null).ToString());
        }

        public static string GetIntegrity(string filePath)
        {
            using var md = MD5.Create();
            using var fileStream = File.OpenRead(filePath);
            var value = md.ComputeHash(fileStream);
            var result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            fileStream.Close();

            return result;
        }

        public static string GetIntegrity(byte[] buffer)
        {
            using var md = MD5.Create();
            var value = md.ComputeHash(buffer);
            var result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            return result;
        }

        public static bool IsCacheExists(EDependency dependency)
        {
            var cachePath = GetCachePath(dependency);
            return File.Exists(cachePath);
        }

        public static void Load(EDependency dependency, bool mirror = false)
        {
            if (!CanBeLoaded(dependency))
                return;

            using var wc = new WebClient();
            wc.Proxy = null;
            byte[] assembly = null;
            var cachePath = GetCachePath(dependency);
            if (dependency != EDependency.RFRocketLibrary && IsCacheExists(dependency))
            {
                try
                {
                    using var fs = File.OpenRead(cachePath);
                    assembly = StreamToByteArray(fs);
                    fs.Close();
                }
                catch
                {
                    try
                    {
                        assembly = wc.DownloadData(typeof(DependencyLink)
                            .GetField(dependency + $"{(mirror ? "Mirror" : string.Empty)}").GetValue(null)
                            .ToString());
                    }
                    catch
                    {
                        try
                        {
                            assembly = wc.DownloadData(typeof(DependencyLink).GetField(dependency + "Mirror").GetValue(null)
                            .ToString());
                        }
                        catch { }
                    }
                }
            }
            else
            {
                try
                {
                    assembly = wc.DownloadData(typeof(DependencyLink)
                        .GetField(dependency + $"{(mirror ? "Mirror" : string.Empty)}").GetValue(null)
                        .ToString());
                }
                catch
                {
                    try
                    {
                        assembly = wc.DownloadData(typeof(DependencyLink).GetField(dependency + "Mirror").GetValue(null)
                            .ToString());
                    }
                    catch { }
                }

                try
                {
                    if (dependency != EDependency.RFRocketLibrary)
                        File.WriteAllBytes(cachePath, assembly);
                }
                catch (Exception)
                {
                    //
                }
            }

            Assembly.Load(assembly);
        }

        public static async Task LoadAsync(EDependency dependency, bool mirror = false)
        {
            if (!CanBeLoaded(dependency))
                return;

            using var wc = new WebClient();
            wc.Proxy = null;
            byte[] assembly;
            var cachePath = GetCachePath(dependency);
            if (IsCacheExists(dependency))
            {
                try
                {
                    using var fs = File.OpenRead(cachePath);
                    assembly = StreamToByteArray(fs);
                    fs.Close();
                }
                catch (Exception)
                {
                    try
                    {
                        assembly = await wc.DownloadDataTaskAsync(typeof(DependencyLink)
                            .GetField(dependency + $"{(mirror ? "Mirror" : string.Empty)}").GetValue(null)
                            .ToString());
                    }
                    catch
                    {
                        assembly = await wc.DownloadDataTaskAsync(typeof(DependencyLink).GetField(dependency + "Mirror")
                            .GetValue(null)
                            .ToString());
                    }
                }
            }
            else
            {
                try
                {
                    assembly = await wc.DownloadDataTaskAsync(typeof(DependencyLink)
                        .GetField(dependency + $"{(mirror ? "Mirror" : string.Empty)}").GetValue(null)
                        .ToString());
                }
                catch
                {
                    assembly = await wc.DownloadDataTaskAsync(typeof(DependencyLink).GetField(dependency + "Mirror")
                        .GetValue(null)
                        .ToString());
                }

                try
                {
                    File.WriteAllBytes(cachePath, assembly);
                }
                catch (Exception)
                {
                    //
                }
            }

            Assembly.Load(assembly);
        }

        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        #endregion
    }
}