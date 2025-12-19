using System;
using System.Collections.Concurrent;
using System.Linq;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using RocketExtensions.Interfaces;
using SDG.Unturned;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Used to parse strings
    /// </summary>
    public static class StringTypeConverter
    {
        private static readonly string[] ValueTrue = {"true", "enabled", "on", "active", "t", "1", "yes", "yep"};
        private static readonly string[] ValueFalse = {"false", "disabled", "off", "unactive", "f", "0", "no", "nah"};

        private static readonly ConcurrentDictionary<Type, IStringParser> CustomParsers = new();

        public static EParseResult Parse<T>(string input, out T? result)
        {
            result = default(T);
            var t = typeof(T);
            if (t == typeof(string))
            {
                result = (T) (object) input;
                return EParseResult.Parsed;
            }

            if (t == typeof(sbyte))
            {
                if (sbyte.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(byte))
            {
                if (byte.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(short))
            {
                if (short.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(ushort))
            {
                if (ushort.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(int))
            {
                if (int.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(uint))
            {
                if (uint.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(float))
            {
                if (float.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(double))
            {
                if (double.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(long))
            {
                if (long.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(ulong))
            {
                if (ulong.TryParse(input, out var value))
                {
                    result = (T) (object) value;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(Player))
            {
                var player = ParsePlayer(input);
                if (player != null)
                {
                    result = (T) (object) player;
                    return EParseResult.Parsed;
                }

                return EParseResult.PlayerNotFound;
            }

            if (t == typeof(UnturnedPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl != null)
                {
                    result = (T) (object) UnturnedPlayer.FromPlayer(pl);
                    return EParseResult.Parsed;
                }

                return EParseResult.PlayerNotFound;
            }

            if (t == typeof(SteamPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl != null)
                {
                    result = (T) (object) pl.channel.owner;
                    return EParseResult.Parsed;
                }

                return EParseResult.PlayerNotFound;
            }

            if (t == typeof(bool))
            {
                var lower = input.ToLowerInvariant();

                if (ValueTrue.Contains(lower))
                {
                    result = (T) (object) true;
                    return EParseResult.Parsed;
                }

                if (ValueFalse.Contains(lower))
                {
                    result = (T) (object) false;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(ItemAsset))
            {
                if (ushort.TryParse(input, out var id))
                {
                    var ast = Assets.find(EAssetType.ITEM, id);
                    if (ast is ItemAsset itemAsset)
                    {
                        result = (T) (object) itemAsset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var assets = Assets.find(EAssetType.ITEM)
                        .Where(x => x is ItemAsset {itemName: { }} itemAsset &&
                                    itemAsset.itemName.IndexOf(input, StringComparison.OrdinalIgnoreCase) !=
                                    -1).ToList();
                    if (assets.Any())
                    {
                        result = (T) (object) assets.First();
                        return EParseResult.Parsed;
                    }
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(VehicleAsset))
            {
                if (ushort.TryParse(input, out var id))
                {
                    var ast = Assets.find(EAssetType.VEHICLE, id);
                    if (ast is VehicleAsset vehicleAsset)
                    {
                        result = (T) (object) vehicleAsset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var byName = Assets.find(EAssetType.ITEM)
                        .Where(x => x is VehicleAsset {vehicleName: { }} vehicleAsset && vehicleAsset.vehicleName.IndexOf(input,
                            StringComparison.OrdinalIgnoreCase) != -1).ToList();
                    if (byName.Any())
                    {
                        result = (T) (object) byName.First();
                        return EParseResult.Parsed;
                    }
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(AnimalAsset))
            {
                if (ushort.TryParse(input, out var assetID))
                {
                    var ast = Assets.find(EAssetType.ANIMAL, assetID);
                    if (ast is AnimalAsset instanceAsset)
                    {
                        result = (T) (object) instanceAsset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var byName = Assets.find(EAssetType.ANIMAL)
                        .Where(x => x is AnimalAsset {animalName: { }} animalAsset && animalAsset.animalName.IndexOf(input,
                            StringComparison.OrdinalIgnoreCase) != -1).ToList();
                    if (byName.Any())
                    {
                        result = (T) (object) byName.First();
                        return EParseResult.Parsed;
                    }
                }

                return EParseResult.ParseFailed;
            }

            if (t == typeof(LocationNode))
            {
                var match = LevelNodes.nodes.FirstOrDefault(x =>
                    x is LocationNode ln && !string.IsNullOrEmpty(input) &&
                    ln.name.IndexOf(input, StringComparison.OrdinalIgnoreCase) != -1);
                if (match != null)
                {
                    result = (T) (object) match;
                    return EParseResult.Parsed;
                }

                return EParseResult.ParseFailed;
            }

            var parser = GetParser(t);
            try
            {
                if (parser != null)
                {
                    result = parser.Parse<T>(input, out var epr);
                    return epr;
                }
            }
            catch (Exception ex) // Don't want a broken parser breaking all RocketExtensions commands
            {
                Logger.LogError($"Error occurred while running custom parser of type {parser?.GetType().FullName}");
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
            }

            return EParseResult.InvalidType;
        }

        public static Player ParsePlayer(string handle)
        {
            if (ulong.TryParse(handle, out var steamID))
            {
                var player = PlayerTool.getSteamPlayer(steamID).player;
                return player;
            }

            return PlayerTool.getPlayer(handle);
        }

        public static void RegisterParser(IStringParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            CustomParsers[parser.Type] = parser;
        }

        public static void UnregisterParser(IStringParser parser)
        {
            if (CustomParsers.TryGetValue(parser.Type, out var psr))
            {
                if (psr.GetType() == parser.GetType())
                {
                    CustomParsers.TryRemove(psr.GetType(), out _);
                }
            }
        }

        public static IStringParser? GetParser(Type type)
        {
            if (CustomParsers.TryGetValue(type, out var psr))
            {
                return psr;
            }

            return null;
        }
    }
}