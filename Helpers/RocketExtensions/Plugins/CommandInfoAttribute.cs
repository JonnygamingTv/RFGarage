using System;

namespace RocketExtensions.Plugins
{
    public sealed class CommandInfoAttribute : Attribute
    {
        public string? Syntax { get; }
        public string? Help { get; }
        public bool AllowSimultaneousCalls { get; set; } = true;

        public CommandInfoAttribute(string? help = "", string? syntax = "")
        {
            Syntax = syntax;
            Help = help;
        }
    }
}
