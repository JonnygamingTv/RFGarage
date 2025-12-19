using System;
using RFRocketLibrary.Events;
using RFRocketLibrary.Patches;

namespace RFRocketLibrary
{
    public static class Library
    {
        private static string HarmonyId => "RFRocketLibrary.Patches";
        private static bool Initialized { get; set; }
        private static uint AttachedAssembly { get; set; }
        private static uint Event_AttachedAssembly { get; set; }
        private static uint Event_AttachedAssemblyWithHarmony { get; set; }

        public static void Initialize()
        {
            AttachedAssembly++;
            if (Initialized)
                return;

            // var harmony = new HarmonyLib.Harmony(HarmonyId);
            // var processor = new HarmonyLib.PatchClassProcessor(harmony, typeof(UnturnedPatch));
            // processor.Patch();
            PrintRFArt();
            Initialized = true;
        }

        public static void Uninitialize()
        {
            AttachedAssembly--;
            if (!Initialized)
                return;

            if (AttachedAssembly > 0)
                return;

            // var harmony = new HarmonyLib.Harmony(HarmonyId);
            // harmony.UnpatchAll();
            Initialized = false;
        }

        public static void AttachEvent(bool withHarmony = false)
        {
            EventBus.Load();
            Event_AttachedAssembly++;
            if (withHarmony)
            {
                EventBus.LoadHarmony();
                Event_AttachedAssemblyWithHarmony++;
            }
        }

        public static void DetachEvent(bool withHarmony = false)
        {
            Event_AttachedAssembly--;
            if (Event_AttachedAssembly == 0)
                EventBus.Unload();

            if (withHarmony)
            {
                Event_AttachedAssemblyWithHarmony--;
                if (Event_AttachedAssemblyWithHarmony == 0)
                    EventBus.UnloadHarmony();
            }
        }
        
        public static void PrintRFArt()
        {
            Console.WriteLine("                                        ...");
            Console.WriteLine("                                        ...");
            Console.WriteLine("                                       .:.      ..");
            Console.WriteLine("                                      .:.     .:..");
            Console.WriteLine("                                     .:.     .:.");
            Console.WriteLine("                                    .:.     .:.");
            Console.WriteLine("                                   ...    ....");
            Console.WriteLine("                                  ..:.   .:..");
            Console.WriteLine("                     ..........  ....   .:.");
            Console.WriteLine("               ...:::::----=-:.:::......:.");
            Console.WriteLine("           ..:::-=***#%@@@@@@%*=:.:::....");
            Console.WriteLine("       ..:.:-+#%%%@@@@@@@@@@@@#=::++:.::::...");
            Console.WriteLine("     ..:-+*#%@@@@@@@@@@@@@@@@@%##*=::+%%#*+:...");
            Console.WriteLine("   ...:-+%%%%%%%%%%%%%%%%%%%%%%%@%*++#%%%@%+-::..");
            Console.WriteLine("......::-:::--------------------:------:::--::......");
            Console.WriteLine(".::------------------------------------------===-::.");
            Console.WriteLine(".::=++++++++++++++++++++++++++++++++++++++++*%@%+:..");
            Console.WriteLine("..:-=+========+++++++++++++++++++++++++++++++**+-...");
            Console.WriteLine("...:===========++++++++++++++++++++++++++++*%@#=:..");
            Console.WriteLine("  .:-===========++++++++++++++++++++++++++*%@@*:..");
            Console.WriteLine("  ...-==========+++++++++++++++++++++++++#@@%+-..");
            Console.WriteLine("   ..::===========+++++++++++++++++++++*%@@%+:..");
            Console.WriteLine("     .::-==========++++++++++++++++++*%@@%*-...");
            Console.WriteLine("      ..::-==++=====++++++++++++++*#@@@%*-....");
            Console.WriteLine("       ...::--====================+*#*+-:::..");
            Console.WriteLine("    .... ............................. .... ....");
            Console.WriteLine("   .:.:+*******#############################+:..");
            Console.WriteLine("     .::=+**++****************************+=::.");
            Console.WriteLine("       ..:::::::::::::::::::::::::::::::::...");
        }
    }
}