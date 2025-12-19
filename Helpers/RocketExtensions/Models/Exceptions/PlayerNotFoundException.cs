using System;

namespace RocketExtensions.Models.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        public override string Message { get; }

        public PlayerNotFoundException(string handle)
        {
            Message = $"Failed to find player by Name/ID '{handle}'";
        }
    }
}