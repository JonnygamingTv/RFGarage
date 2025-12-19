using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RFRocketLibrary.API.Interfaces;
using RFRocketLibrary.Models;
// using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Assets;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using RocketExtensions.Utilities;

namespace RocketExtensions.Plugins
{
    public class ExtendedRocketPlugin<TPluginConfig> : ExtendedRocketPlugin, IRocketPlugin<TPluginConfig>
        where TPluginConfig : class, IRocketPluginConfiguration
    {
        public IAsset<TPluginConfig> Configuration { get; }

        public ExtendedRocketPlugin()
        {
            var configurationFile = Path.Combine(Directory,
                string.Format(Rocket.Core.Environment.PluginConfigurationFileTemplate, Name, "xml"));
            var url = "";
            if (R.Settings.Instance.WebConfigurations.Enabled)
                url = string.Format(Rocket.Core.Environment.WebConfigurationTemplate,
                    R.Settings.Instance.WebConfigurations.Url, Name, R.Implementation.InstanceId);
            else if (File.Exists(configurationFile))
                url = File.ReadAllLines(configurationFile).First().Trim();

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                Configuration = new WebXMLFileAsset<TPluginConfig>(uri, null, _ => { base.LoadPlugin(); });
            else
                Configuration = new XMLFileAsset<TPluginConfig>(configurationFile);
        }

        public override void LoadPlugin()
        {
            Configuration.Load(_ => base.LoadPlugin() );
        }
    }

    public class ExtendedRocketPlugin : RocketPlugin
    {
        public ISerialQueue? CommandQueue { get; private set; }

        public override void LoadPlugin()
        {
            base.LoadPlugin();
            CommandQueue = new SerialQueue();
        }
    }
}