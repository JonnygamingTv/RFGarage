using System;
using System.Linq;
using HarmonyLib;
using RFRocketLibrary.Utils;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFRocketLibrary.Models
{
    public class RaycastLimb
    {
        public const int Masks = RayMasks.PLAYER | RayMasks.AGENT;
        public Player? Player { get; set; }
        public Animal? Animal { get; set; }
        public Zombie? Zombie { get; set; }
        public ELimb Limb { get; set; } = ELimb.SPINE;

        public static RaycastLimb FromPlayerLook(Player player, int masks = Masks, float distance = float.MaxValue)
        {
            var result = new RaycastLimb();
            var flag = UnityEngine.Physics.Raycast(player.look.aim.position, player.look.aim.forward, out var hit, distance, masks);
            //var flag = PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out var hit, distance, masks);
            if (!flag)
                goto Result;
            
            var transform = hit.collider != null ? hit.collider.transform : hit.transform;
            if (transform == null)
                goto Result;

            // var gameObject = transform.gameObject;
            // Logger.LogWarning($"[DEBUG] FromPlayerLook: Tag: {gameObject.tag} Layer: {gameObject.layer} Name: {gameObject.name} Position: {transform.position}");
            
            // Player
            if (transform.CompareTag("Enemy"))
            {
                var resPlayer = transform.GetComponentInParent<Player>();
                if (resPlayer != null)
                {
                    result.Player = resPlayer;
                    result.Limb = DamageTool.getLimb(transform);
                    goto Result;
                }
            }

            if (transform.CompareTag("Agent"))
            {
                // Animal
                var animal = transform.GetComponentInParent<Animal>();
                if (animal != null)
                {
                    result.Animal = animal;
                    result.Limb = DamageTool.getLimb(transform);
                    goto Result;
                }
                
                // Zombie
                var zombie = transform.GetComponentInParent<Zombie>();
                if (zombie != null)
                {
                    result.Zombie = zombie;
                    result.Limb = DamageTool.getLimb(transform);
                    goto Result;
                }
            }

            Result:
            return result;
        }
    }
}