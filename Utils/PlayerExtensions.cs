using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using RaycastInfo = RFRocketLibrary.Models.RaycastInfo;

namespace RFRocketLibrary.Utils
{
    public static class PlayerExtensions
    {
        #region Methods

        public static bool GetRaycastHit(this Player player, float distance, int masks, out RaycastHit hit) => UnityEngine.Physics.Raycast(player.look.aim.position, player.look.aim.forward, out hit, distance, masks);
        //PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward),
        //        out hit, distance, masks);

        public static RaycastInfo GetRaycastInfo(this Player player, int masks, float distance = float.MaxValue) =>
            RaycastInfo.FromPlayerLook(player, masks, distance);

        public static SteamPending? GetSteamPending(this CSteamID cSteamID) =>
            Provider.pending.FirstOrDefault(x => x.playerID.steamID == cSteamID);

        #endregion
    }
}