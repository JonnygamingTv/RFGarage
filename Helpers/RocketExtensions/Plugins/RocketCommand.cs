// using Cysharp.Threading.Tasks;

using Rocket.API;
using Rocket.Core;
using Rocket.Core.Plugins;
using RocketExtensions.Models;
using RocketExtensions.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers;
using RocketExtensions.Utilities;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using RocketCaller = Rocket.API.AllowedCaller;

namespace RocketExtensions.Plugins
{
    public abstract class RocketCommand : IRocketCommand
    {
        private CommandActorAttribute? _actor;
        private IRocketPlugin? _plugin;
        private bool _pluginInit;

        public IRocketPlugin? Plugin
        {
            get
            {
                if (!_pluginInit)
                {
                    _plugin = R.Plugins.GetPlugin(GetType().Assembly);
                    _pluginInit = true;
                }

                return _plugin;
            }
        }

        public RocketCaller AllowedCaller
        {
            get
            {
                var typ = GetType();
                _actor ??= typ.GetCustomAttribute<CommandActorAttribute>() ??
                           new CommandActorAttribute(RocketCaller.Both);
                return _actor.Actor;
            }
        }

        private string? _name;

        public string? Name
        {
            get
            {
                if (_name == null)
                {
                    var typ = GetType();

                    var nm = typ.GetCustomAttribute<CommandNameAttribute>();
                    if (nm != null)
                        _name = nm.Name;
                    else
                    {
                        var className = typ.Name;
                        var cmdIndex = className.IndexOf("Command", 0, StringComparison.OrdinalIgnoreCase);
                        if (cmdIndex != -1)
                            className = className.Remove(cmdIndex, 7);

                        _name = className;
                    }
                }

                return _name;
            }
        }

        private string? _help;
        private string? _syntax;
        private bool? _allowSimultaneousCall;

        private void m_InitInfo()
        {
            var typ = GetType();
            var info = typ.GetCustomAttribute<CommandInfoAttribute>();
            if (info != null)
            {
                _help = info.Help;
                _syntax = !string.IsNullOrEmpty(info.Syntax) ? info.Syntax : Name;
                _allowSimultaneousCall = info.AllowSimultaneousCalls;
            }
            else
            {
                _help = "";
                _syntax = Name;
                _allowSimultaneousCall = false;
            }
        }

        public string Help
        {
            get
            {
                if (_help == null)
                    m_InitInfo();

                return _help;
            }
        }

        public string Syntax
        {
            get
            {
                if (_syntax == null)
                    m_InitInfo();

                return _syntax;
            }
        }

        public bool AllowSimultaneousCalls
        {
            get
            {
                if (_allowSimultaneousCall == null)
                    m_InitInfo();

                return _allowSimultaneousCall.Value;
            }
        }

        private List<string>? _aliases;

        public List<string>? Aliases
        {
            get
            {
                if (_aliases == null)
                {
                    var info = GetType().GetCustomAttribute<CommandAliasesAttribute>();
                    _aliases = info != null ? info.Aliases : new List<string>();
                }

                return _aliases;
            }
        }

        private List<string> _permissions = new();

        public List<string> Permissions
        {
            get
            {
                var typ = GetType();
                var inst = typ.GetCustomAttribute<CommandPermissionsAttribute>();
                if (inst != null)
                    _permissions = inst.Permissions;
                else
                {
                    var asmName = typ.Assembly.GetName().Name;
                    _permissions = new List<string> {$"{asmName}.{Name}"};
                }

                return _permissions;
            }
        }

        private List<RunningCommand> RunningCommands { get; } = new();
        private ExtendedRocketPlugin? _extendedPlugin;

        public ExtendedRocketPlugin? ExtendedPlugin
        {
            get
            {
                if (_extendedPlugin != null)
                    return _extendedPlugin;

                if (Plugin is ExtendedRocketPlugin extendedRocketPlugin)
                {
                    _extendedPlugin = extendedRocketPlugin;
                    return extendedRocketPlugin;
                }

                return null;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var context = new CommandContext(caller, this, command);

            var runningCommand = RunningCommands.FirstOrDefault(x =>
                x.Caller.Id.Equals(context.Player.Id, StringComparison.OrdinalIgnoreCase));
            if (runningCommand == null)
            {
                runningCommand = new RunningCommand {Caller = context.Player, Instances = 1};
                RunningCommands.Add(runningCommand);
            }
            else
                runningCommand.Instances++;

            if (ExtendedPlugin == null)
                Task.Run(async () => await Run(context));
            else
                ExtendedPlugin.CommandQueue.Enqueue(async () => await Run(context));
        }

        private async Task Run(CommandContext context)
        {
            var runningCommand = RunningCommands.First(x =>
                x.Caller.Id.Equals(context.Player.Id, StringComparison.OrdinalIgnoreCase));
            try
            {
                if (!AllowSimultaneousCalls && runningCommand.Instances > 1)
                {
                    await context.ReplyAsync(
                        $"This command does not support simultaneous calls. Please wait for previous command to finish!",
                        Color.yellow);
                    await context.CancelCooldownAsync();
                }

                await Execute(context);
            }
            catch (InvalidArgumentException invalid)
            {
                await context.ReplyAsync(invalid.Message, Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (PlayerNotFoundException player)
            {
                await context.ReplyAsync(player.Message, Color.red);
                await context.CancelCooldownAsync();
            }
            catch (ArgumentMissingException missing)
            {
                await context.ReplyAsync(missing.Message, Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (WrongUsageOfCommandException usage)
            {
                await context.CancelCooldownAsync();
                throw usage;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while executing /{Name}");
                Logger.LogError($"[{Plugin.Name}] Message: {ex.Message}");
                Logger.LogError($"[{Plugin.Name}] Details: {ex}");
                await context.ReplyAsync("An error occurred during the execution of this command", Color.red);
            }
            finally
            {
                runningCommand.Instances--;
            }
        }

        public T? GetPluginInstance<T>() where T : IRocketPlugin
        {
            return typeof(T).Assembly.TryGetPlugin<T>();
        }

        public T? GetPluginConfig<T>() where T : IRocketPluginConfiguration
        {
            var cType = typeof(RocketPlugin<>).MakeGenericType(typeof(T));
            dynamic? plugin = typeof(T).Assembly.TryGetPlugin(cType);
            if (plugin == null)
                return default;

            var configInstance = plugin.Configuration.Instance;

            if (configInstance == null)
                return default;

            if (configInstance is T t)
                return t;

            if (configInstance is T instance)
                return instance;

            return default;
        }

        public abstract Task Execute(CommandContext context);

        /// <summary>
        /// Sends a message to the specified player
        /// </summary>
        public async Task SayAsync(IRocketPlayer player, string message, Color? messageColor = null,
            string? iconUrl = null)
        {
            messageColor ??= Color.green;
            await ThreadTool.RunOnGameThreadAsync(ChatHelper.Say, player, message, messageColor, iconUrl);
        }

        /// <summary>
        /// Sends a message to all online players.
        /// </summary>
        public async Task AnnounceAsync(string message, Color? messageColor = null, string? iconUrl = null)
        {
            messageColor ??= Color.green;
            await ThreadTool.RunOnGameThreadAsync(ChatHelper.Broadcast, message, messageColor, iconUrl);
        }
    }
}