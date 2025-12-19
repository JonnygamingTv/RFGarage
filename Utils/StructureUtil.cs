using HarmonyLib;
using SDG.Unturned;
using UnityEngine;

namespace RFRocketLibrary.Utils
{
    public static class StructureUtil
    {
        public static StructureDrop FindDropFast(Transform structureTransform)
        {
            var fastBarricade = Traverse.Create<StructureDrop>().Method("FindByRootFast", new[] {typeof(Transform)});
            return fastBarricade.GetValue<StructureDrop>(structureTransform);
        }
    }
}