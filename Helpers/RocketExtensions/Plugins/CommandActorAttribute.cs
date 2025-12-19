using System;

namespace RocketExtensions.Plugins
{
    public class CommandActorAttribute : Attribute
    {
        public Rocket.API.AllowedCaller Actor { get; private set; }

        public const Rocket.API.AllowedCaller Player = Rocket.API.AllowedCaller.Player;
        public const Rocket.API.AllowedCaller Console = Rocket.API.AllowedCaller.Console;
        public const Rocket.API.AllowedCaller Both = Rocket.API.AllowedCaller.Both;

        public CommandActorAttribute(Rocket.API.AllowedCaller allowedActor)
        {
            Actor = allowedActor;
        }
    }
}