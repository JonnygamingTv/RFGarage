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
    public class RaycastInfo
    {
        public const int Masks = RayMasks.PLAYER | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE |
                                 RayMasks.AGENT | RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL;
        public Player? Player { get; set; }
        public InteractableVehicle? Vehicle { get; set; }
        public BarricadeDrop? Barricade { get; set; }
        public StructureDrop? Structure { get; set; }
        public Animal? Animal { get; set; }
        public Zombie? Zombie { get; set; }
        public ResourceSpawnpoint? Resource { get; set; }
        public LevelObject? Object { get; set; }

        public static RaycastInfo FromPlayerLook(Player player, int masks = Masks, float distance = float.MaxValue)
        {
            var result = new RaycastInfo();
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
                    goto Result;
                }
            }

            // Vehicle
            if (transform.CompareTag("Vehicle"))
            {
                var root = transform.root;
                var vehicle = root.GetComponent<InteractableVehicle>();
                var vehicleRef = root.GetComponent<VehicleRef>();
                if (vehicle != null)
                {
                    result.Vehicle = vehicle;
                    goto Result;
                }

                if (vehicleRef != null)
                {
                    result.Vehicle = vehicleRef.vehicle;
                    goto Result;
                }
            }

            // Barricade
            if (transform.root.CompareTag("Barricade"))
            {
                result.Barricade = BarricadeUtil.FindDropFast(transform.root);
                goto Result;
            }

            // Structure
            if (transform.CompareTag("Structure"))
            {
                result.Structure = StructureUtil.FindDropFast(transform);
                goto Result;
            }

            if (transform.CompareTag("Agent"))
            {
                // Animal
                var animal = transform.GetComponentInParent<Animal>();
                if (animal != null)
                {
                    result.Animal = animal;
                    goto Result;
                }
                
                // Zombie
                var zombie = transform.GetComponentInParent<Zombie>();
                if (zombie != null)
                {
                    result.Zombie = zombie;
                    goto Result;
                }
            }

            // Resource
            if (transform.CompareTag("Resource"))
            {
                if (ResourceManager.tryGetRegion(transform, out var x, out var y, out var index))
                    result.Resource = ResourceManager.getResourceSpawnpoint(x, y, index);
                goto Result;
            }

            // Object
            if (transform.CompareTag("Large") || transform.CompareTag("Medium") || transform.CompareTag("Small"))
            {
                flag = ObjectManager.tryGetRegion(transform, out var x, out var y, out var index);
                if (flag)
                {
                    result.Object = ObjectManager.getObject(x, y, index);
                    goto Result;
                }
            }

            Result:
            return result;
        }
    }
}