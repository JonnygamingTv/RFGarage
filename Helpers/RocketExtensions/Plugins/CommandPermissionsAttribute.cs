using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class CommandPermissionsAttribute : Attribute
    {
        public List<string> Permissions { get; private set; }

        public CommandPermissionsAttribute(params string[] perms)
        {
            Permissions = perms.ToList();
        }
    }
}