using Rocket.API;

namespace RocketExtensions.Models
{
    public class RunningCommand
    {
        public IRocketPlayer? Caller { get; set; }
        public uint Instances { get; set; }
    }
}