using System;

namespace RocketExtensions.Models.Exceptions
{
    public sealed class ArgumentMissingException : Exception
    {
        public override string Message { get; }

        public ArgumentMissingException(string? name)
        {
            Message = $"Missing Argument {name}";
        }

        public ArgumentMissingException(int index)
        {
            Message = $"Missing Argument at position {index + 1}";
        }
    }
}