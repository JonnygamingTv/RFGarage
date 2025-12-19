using System;

namespace RocketExtensions.Plugins
{
    public sealed class CommandNameAttribute : Attribute
    {
        public string? Name { get; }

        public CommandNameAttribute(string? name)
        {
            Name = name;
        }

    }
}
