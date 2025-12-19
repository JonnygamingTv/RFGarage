using RocketExtensions.Models.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Models
{
    public class ArgumentList : IEnumerable<string>
    {
        private readonly List<string> _items;

        public ArgumentList(IEnumerable<string> arguments)
        {
            _items = arguments.ToList();
        }

        // Backward Compatibility

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, T? defaultValue) => Get<T>(index, defaultValue, null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index) => Get<T>(index, null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, T? defaultValue, string? paramName = null)
        {
            if (index >= _items.Count)
            {
                return defaultValue;
            }

            var value = _items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            if (result == EParseResult.InvalidType)
            {
                throw new InvalidCastException($"Type {typeof(T).Name} is not valid for automatic string parsing");
            }

            if (result == EParseResult.PlayerNotFound)
            {
                return defaultValue;
            }
            if (result == EParseResult.ParseFailed)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new InvalidArgumentException(paramName);
                }

                throw new InvalidArgumentException(index);
            }
            return res;
        }

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T? Get<T>(int index, string? paramName = null)
        {
            if (index >= _items.Count)
            {
                if (!string.IsNullOrEmpty(paramName))
                    throw new ArgumentMissingException(paramName);

                throw new ArgumentMissingException(index);
            }

            var value = _items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            if (result == EParseResult.InvalidType)
                throw new InvalidCastException($"Type {typeof(T).Name} is not valid for automatic string parsing");

            if (result == EParseResult.PlayerNotFound)
                throw new PlayerNotFoundException(value);
            
            if (result == EParseResult.ParseFailed)
            {
                if (!string.IsNullOrEmpty(paramName))
                    throw new InvalidArgumentException(paramName);

                throw new InvalidArgumentException(index);
            }
            return res;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}