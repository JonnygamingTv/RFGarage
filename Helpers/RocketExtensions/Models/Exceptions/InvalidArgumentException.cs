using System;

namespace RocketExtensions.Models.Exceptions
{
    public sealed class InvalidArgumentException : Exception
    {
        public override string Message { get; }

        public InvalidArgumentException(string? name)
        {
            Message = $"Invalid value for field {name}";
        }

        public InvalidArgumentException(int index)
        {
            Message = $"Invalid value at command position {index + 1}";
        }
    }
}