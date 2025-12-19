using HarmonyLib;
using SDG.Unturned;

namespace RFRocketLibrary.Utils
{
    public static class ZombieUtil
    {
        public static void Reset(Zombie zombie)
        {
            Traverse.Create(zombie).Method("reset").GetValue();
        }
    }
}