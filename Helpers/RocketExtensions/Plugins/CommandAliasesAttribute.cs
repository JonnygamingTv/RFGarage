using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class CommandAliasesAttribute : Attribute
    {
        public List<string>? Aliases { get; }

        public CommandAliasesAttribute(params string[] aliases)
        {
            Aliases = aliases.ToList();
        }
    }
}