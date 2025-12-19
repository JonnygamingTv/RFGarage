using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RFRocketLibrary.Utils;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

namespace RFRocketLibrary.Events
{
    public static class UnturnedPatchEvent
    {
        #region Delegates

        public delegate void AnimalDamaged(Animal animal, ushort damage, EPlayerKill kill, uint xp);

        public delegate void AnimalKilled(Animal? animal, ref Vector3 ragdoll, ref ERagdollEffect ragdollEffect);

        public delegate void AnimalMovementChanged(Animal animal, Vector3 lastPosition);

        public delegate void AnimalSpawned(Animal animal, Vector3 position, byte angle);

        public delegate void BarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant);

        public delegate void BarricadeTransformed(byte x, byte y, ushort plant, BarricadeDrop barricade, Vector3 point,
            Quaternion rotation);

        public delegate void ItemSpawned(byte x, byte y, ushort id, byte amount, byte quality,
            byte[] state, Vector3 point, uint instanceID);

        public delegate void ObjectDestroyed(LevelObject levelObject, byte section);

        public delegate void ObjectResourceUpdated(LevelObject levelObject, ushort amount);

        public delegate void ObjectSpawned(LevelObject levelObject, byte section);

        public delegate void OilTankBurned(BarricadeDrop drop, ushort fuel);

        public delegate void PlayerAttack(Player player);

        public delegate void PlayerClimbed(Player player, Vector3 direction);

        public delegate void PlayerDamaged(Player player, byte amount, EDeathCause newCause, ELimb newLimb,
            CSteamID newKiller, EPlayerKill kill);

        public delegate void PlayerFiremodeChanged(Player player, EFiremode newFiremode);

        public delegate void PlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint);

        public delegate void PlayerInteractedBed(Player player, InteractableBed bed, bool claim);

        public delegate void PlayerInteractedDisplay(Player player, InteractableStorage display, byte rot);

        public delegate void PlayerInteractedDoor(Player player, InteractableDoor door, bool open);

        public delegate void PlayerInteractedFire(Player player, InteractableFire fire, bool lit);

        public delegate void PlayerInteractedGenerator(Player player, InteractableGenerator generator, bool powered);

        public delegate void PlayerInteractedLibrary(Player player, InteractableLibrary library, byte transaction,
            uint delta);

        public delegate void PlayerInteractedMannequinPose(Player player, InteractableMannequin mannequin, byte pose);

        public delegate void PlayerInteractedMannequinUpdate(Player player, InteractableMannequin mannequin,
            EMannequinUpdateMode updateMode);

        public delegate void PlayerInteractedOven(Player player, InteractableOven oven, bool lit);

        public delegate void PlayerInteractedOxygenator(Player player, InteractableOxygenator oxygenator, bool powered);

        public delegate void PlayerInteractedSafezone(Player player, InteractableSafezone safezone, bool powered);

        public delegate void PlayerInteractedSign(Player player, InteractableSign sign, string text);

        public delegate void PlayerInteractedSpot(Player player, InteractableSpot spot, bool powered);

        public delegate void PlayerInteractedStereoTrack(Player player, InteractableStereo stereo, Guid track);

        public delegate void PlayerInteractedStereoVolume(Player player, InteractableStereo stereo, byte volume);

        public delegate void PlayerInteractedStorage(Player player, InteractableStorage storage, bool quickGrab);

        public delegate void PlayerMovementChanged(Player player, Vector3 lastPosition);

        public delegate void PlayerStealVehicleBattery(Player player, InteractableVehicle vehicle);

        public delegate void PlayerSuicided(Player player);

        public delegate void PlayerTeleported(Player player, Vector3 lastPosition, Vector3 teleportPosition,
            float rotation);

        public delegate void PreAnimalKilled(Animal animal, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreAnimalMovementChanged(Animal animal, Vector3 lastPosition, ref bool shouldAllow);

        public delegate void PreAnimalSpawned(Animal animal, ref Vector3 position, ref byte angle,
            ref bool shouldAllow);

        public delegate void PreBarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant,
            ref bool shouldAllow);

        public delegate void PreGeneratorBurned(InteractableGenerator generator, ushort amount, ref bool shouldAllow);

        public delegate void PreItemSpawned(byte x, byte y, ref ushort id, ref byte amount, ref byte quality,
            ref byte[] state, ref Vector3 point, uint instanceID, ref bool shouldAllow);

        public delegate void PreObjectResourceUpdated(LevelObject levelObject, ref ushort amount, ref bool shouldAllow);

        public delegate void PreOilTankBurned(BarricadeDrop drop, ref ushort fuel, ref bool shouldAllow);

        public delegate void PrePlayerCaughtFish(Player player, ref bool giveFishReward);

        public delegate void PrePlayerChangedEquipment(Player player, byte page, byte x, byte y, ref Guid newAssetGuid,
            ref byte newQuality, ref byte[] newState, ref NetId useableNetId, ref bool shouldAllow);

        public delegate void PrePlayerChangedGesture(Player player, ref EPlayerGesture gesture, ref bool shouldAllow);

        public delegate void PrePlayerChangedStance(Player player, EPlayerStance lastStance,
            ref EPlayerStance newStance, ref bool all, ref bool shouldAllow);

        public delegate void PrePlayerChatted(Player player, ref EChatMode chatMode, ref string text,
            ref bool shouldAllow);

        public delegate void PrePlayerClimbed(Player player, Vector3 direction, ref bool shouldAllow);

        public delegate void PrePlayerConnected(SteamPlayerID playerID, ref Vector3 point, ref byte angle, bool isPro,
            bool isAdmin, ref byte face, ref byte hair, ref byte beard, ref Color skin, ref Color color, bool hand,
            ref int shirtItem, ref int pantsItem, ref int hatItem, ref int backpackItem, ref int vestItem,
            ref int maskItem,
            ref int glassesItem, ref int[] skinItems, ref string[] skinTags, ref string[] skinDynamicProps,
            EPlayerSkillset skillset, string language);

        public delegate void PrePlayerDehydrated(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerDraggedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PrePlayerDroppedItem(Player inventory, byte page, byte x, byte y, ref bool shouldAllow);

        public delegate void PrePlayerFiremodeChanged(Player player, EFiremode newFiremode, ref bool shouldAllow);

        public delegate void PrePlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            ref bool shouldAllow);

        public delegate void PrePlayerHallucinationBlinded(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerInfected(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerInteractedBed(Player player, InteractableBed bed, bool claim,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedDisplay(Player player, InteractableStorage display, byte rot,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedDoor(Player player, InteractableDoor door, bool open,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedFire(Player player, InteractableFire fire, bool lit,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedGenerator(Player player, InteractableGenerator generator, bool powered,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedLibrary(Player player, InteractableLibrary library, byte transaction,
            uint delta, ref bool shouldAllow);

        public delegate void PrePlayerInteractedMannequinPose(Player player, InteractableMannequin mannequin, byte pose,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedMannequinUpdate(Player player, InteractableMannequin mannequin,
            EMannequinUpdateMode updateMode, ref bool shouldAllow);

        public delegate void PrePlayerInteractedOven(Player player, InteractableOven oven, bool lit,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedOxygenator(Player player, InteractableOxygenator oxygenator,
            bool powered, ref bool shouldAllow);

        public delegate void PrePlayerInteractedSafezone(Player player, InteractableSafezone safezone, bool powered,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedSign(Player player, InteractableSign sign, string text,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedSpot(Player player, InteractableSpot spot, bool powered,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedStereoTrack(Player player, InteractableStereo stereo, Guid track,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedStereoVolume(Player player, InteractableStereo stereo, byte volume,
            ref bool shouldAllow);

        public delegate void PrePlayerInteractedStorage(Player player, InteractableStorage storage, bool quickGrab,
            ref bool shouldAllow);

        public delegate void PrePlayerKilled(Player player, ref Vector3 newRagdoll, ref EDeathCause newCause,
            ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill, ref bool trackKill,
            ref ERagdollEffect newRagdollEffect,
            ref bool shouldAllow);

        public delegate void PrePlayerMovementChanged(Player player, Vector3 lastPosition, Vector3 newPosition,
            ref bool shouldAllow);

        public delegate void PrePlayerRadiated(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerStarved(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerStealVehicleBattery(Player player, InteractableVehicle vehicle,
            ref bool shouldAllow);

        public delegate void PrePlayerSuffocated(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerSuicided(Player player, ref bool shouldAllow);

        public delegate void PrePlayerSwappedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PrePlayerTeleported(Player player, ref Vector3 teleportPosition, ref float rotation,
            ref bool shouldAllow);

        public delegate void PrePlayerTired(Player player, ref byte amount, ref bool shouldAllow);

        public delegate void PrePlayerWarmUpdated(Player player, ref short delta, ref bool shouldAllow);

        public delegate void PreRainBarrelUpdated(BarricadeDrop drop, ref bool isFull, ref bool shouldSend,
            ref bool shouldAllow);

        public delegate void PreResourceSpawned(ResourceSpawnpoint resourceSpawnpoint, ref bool shouldAllow);

        public delegate void PreServerSentMessage(ref string text, ref Color color, ref SteamPlayer fromPlayer,
            ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL, ref bool useRichTextFormatting,
            ref bool shouldAllow);

        public delegate void PreStructureDestroyed(StructureDrop drop, byte x, byte y, ref Vector3 ragdoll,
            ref bool shouldAllow);

        public delegate void PreTankUpdated(BarricadeDrop drop, ref ushort amount, ref bool shouldAllow);

        public delegate void PreVehicleDestroyed(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PreVehicleExploded(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PreVehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition,
            ref bool shouldAllow);

        public delegate void PreVehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition, ref bool shouldAllow);

        public delegate void PreVehicleSpawned(ref VehicleAsset asset, ref ushort skinID,
            ref ushort mythicID, ref float roadPosition, ref Vector3 point, ref Quaternion angle, ref bool sirens,
            ref bool blimp,
            ref bool headlights, ref bool taillights, ref ushort fuel, ref ushort health, ref ushort batteryCharge,
            ref CSteamID owner,
            ref CSteamID group, ref bool locked, ref byte[][] turrets, ref byte tireAliveMask, ref bool shouldAllow);

        public delegate void PreVehicleSpawnedFromSpawnpoint(VehicleSpawnpoint vehicleSpawnpoint, ref bool shouldAllow);

        public delegate void PreZombieKilled(Zombie zombie, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref EZombieStunOverride zombieStunOverride,
            ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreZombieMovementChanged(Zombie zombie, Vector3 lastPosition, ref bool shouldAllow);

        public delegate void PreZombieSpawned(Zombie zombie, ref byte newType, ref byte newSpeciality,
            ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear, ref Vector3 newPosition,
            ref byte newAngle, ref bool shouldAllow);

        public delegate void RainBarrelUpdated(BarricadeDrop drop, bool isFull, bool shouldSend);

        public delegate void ResourceDead(ResourceSpawnpoint resourceSpawnpoint, ref Vector3 ragdoll);

        public delegate void ResourceSpawned(ResourceSpawnpoint resourceSpawnpoint);

        public delegate void StructureDestroyed(StructureDrop drop, byte x, byte y, Vector3 ragdoll);

        public delegate void StructureTransformed(byte x, byte y, StructureDrop drop, Vector3 point, Quaternion rotation);

        public delegate void TankUpdated(BarricadeDrop drop, ushort amount);

        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);

        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition);

        public delegate void VehicleSpawned(InteractableVehicle vehicle);

        public delegate void VehicleSpawnedFromSpawnpoint(VehicleSpawnpoint vehicleSpawnpoint,
            InteractableVehicle vehicle);

        public delegate void ZombieDamaged(Zombie zombie, ushort damage, EPlayerKill kill, uint xp);

        public delegate void ZombieKilled(Zombie? zombie, ZombieRegion? zombieRegion, ref Vector3 ragdoll,
            ref ERagdollEffect ragdollEffect);

        public delegate void ZombieMovementChanged(Zombie zombie, Vector3 lastPosition);

        public delegate void ZombieSpawned(Zombie zombie, byte newType, byte newSpeciality,
            byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition,
            byte newAngle);

        #endregion

        #region Events

        public static event AnimalDamaged? OnAnimalDamaged;
        public static event AnimalKilled? OnAnimalKilled;

        public static event AnimalMovementChanged? OnAnimalMovementChanged;
        public static event AnimalSpawned? OnAnimalSpawned;

        // public static event PreAnimalMovementChanged? OnPreAnimalMovementChanged;
        public static event BarricadeDestroyed? OnBarricadeDestroyed;
        public static event BarricadeTransformed? OnBarricadeTransformed;
        public static event ItemSpawned? OnItemSpawned;
        public static event ObjectDestroyed? OnObjectDestroyed;
        public static event ObjectResourceUpdated? OnObjectResourceUpdated;
        public static event ObjectSpawned? OnObjectSpawned;
        public static event OilTankBurned? OnOilTankBurned;

        public static event PlayerAttack? OnPlayerAttack;
        public static event PlayerClimbed? OnPlayerClimbed;
        public static event PlayerDamaged? OnPlayerDamaged;
        public static event PlayerFiremodeChanged? OnPlayerFiremodeChanged;
        public static event PlayerForagedResource? OnPlayerForagedResource;
        public static event PlayerInteractedBed? OnPlayerInteractedBed;
        public static event PlayerInteractedDisplay? OnPlayerInteractedDisplay;
        public static event PlayerInteractedDoor? OnPlayerInteractedDoor;
        public static event PlayerInteractedFire? OnPlayerInteractedFire;
        public static event PlayerInteractedGenerator? OnPlayerInteractedGenerator;
        public static event PlayerInteractedLibrary? OnPlayerInteractedLibrary;
        public static event PlayerInteractedMannequinPose? OnPlayerInteractedMannequinPose;
        public static event PlayerInteractedMannequinUpdate? OnPlayerInteractedMannequinUpdate;
        public static event PlayerInteractedOven? OnPlayerInteractedOven;
        public static event PlayerInteractedOxygenator? OnPlayerInteractedOxygenator;
        public static event PlayerInteractedSafezone? OnPlayerInteractedSafezone;
        public static event PlayerInteractedSign? OnPlayerInteractedSign;
        public static event PlayerInteractedSpot? OnPlayerInteractedSpot;
        public static event PlayerInteractedStereoTrack? OnPlayerInteractedStereoTrack;
        public static event PlayerInteractedStereoVolume? OnPlayerInteractedStereoVolume;
        public static event PlayerInteractedStorage? OnPlayerInteractedStorage;

        public static event PlayerMovementChanged? OnPlayerMovementChanged;
        public static event PlayerStealVehicleBattery? OnPlayerStealVehicleBattery;
        public static event PlayerSuicided? OnPlayerSuicided;
        public static event PlayerTeleported? OnPlayerTeleported;

        // public static event PrePlayerMovementChanged? OnPrePlayerMovementChanged;
        public static event PreAnimalKilled? OnPreAnimalKilled;
        public static event PreAnimalSpawned? OnPreAnimalSpawned;
        public static event PreBarricadeDestroyed? OnPreBarricadeDestroyed;
        public static event PreGeneratorBurned? OnPreGeneratorBurned;
        public static event PreItemSpawned? OnPreItemSpawned;
        public static event PreObjectResourceUpdated? OnPreObjectResourceUpdated;
        public static event PreOilTankBurned? OnPreOilTankBurned;

        public static event PrePlayerCaughtFish? OnPrePlayerCaughtFish;
        public static event PrePlayerChangedEquipment? OnPrePlayerChangedEquipment;
        public static event PrePlayerChangedGesture? OnPrePlayerChangedGesture;
        public static event PrePlayerChangedStance? OnPrePlayerChangedStance;
        public static event PrePlayerChatted? OnPrePlayerChatted;
        public static event PrePlayerClimbed? OnPrePlayerClimbed;
        public static event PrePlayerConnected? OnPrePlayerConnected;
        public static event PrePlayerDehydrated? OnPrePlayerDehydrated;
        public static event PrePlayerDraggedItem? OnPrePlayerDraggedItem;
        public static event PrePlayerDroppedItem? OnPrePlayerDroppedItem;
        public static event PrePlayerFiremodeChanged? OnPrePlayerFiremodeChanged;
        public static event PrePlayerForagedResource? OnPrePlayerForagedResource;
        public static event PrePlayerHallucinationBlinded? OnPrePlayerHallucinationBlinded;
        public static event PrePlayerInfected? OnPrePlayerInfected;
        public static event PrePlayerInteractedBed? OnPrePlayerInteractedBed;
        public static event PrePlayerInteractedDisplay? OnPrePlayerInteractedDisplay;
        public static event PrePlayerInteractedDoor? OnPrePlayerInteractedDoor;
        public static event PrePlayerInteractedFire? OnPrePlayerInteractedFire;
        public static event PrePlayerInteractedGenerator? OnPrePlayerInteractedGenerator;
        public static event PrePlayerInteractedLibrary? OnPrePlayerInteractedLibrary;
        public static event PrePlayerInteractedMannequinPose? OnPrePlayerInteractedMannequinPose;
        public static event PrePlayerInteractedMannequinUpdate? OnPrePlayerInteractedMannequinUpdate;
        public static event PrePlayerInteractedOven? OnPrePlayerInteractedOven;
        public static event PrePlayerInteractedOxygenator? OnPrePlayerInteractedOxygenator;
        public static event PrePlayerInteractedSafezone? OnPrePlayerInteractedSafezone;
        public static event PrePlayerInteractedSign? OnPrePlayerInteractedSign;
        public static event PrePlayerInteractedSpot? OnPrePlayerInteractedSpot;
        public static event PrePlayerInteractedStereoTrack? OnPrePlayerInteractedStereoTrack;
        public static event PrePlayerInteractedStereoVolume? OnPrePlayerInteractedStereoVolume;
        public static event PrePlayerInteractedStorage? OnPrePlayerInteractedStorage;
        public static event PrePlayerKilled? OnPrePlayerKilled;
        public static event PrePlayerRadiated? OnPrePlayerRadiated;
        public static event PrePlayerStarved? OnPrePlayerStarved;
        public static event PrePlayerStealVehicleBattery? OnPrePlayerStealVehicleBattery;
        public static event PrePlayerSuffocated? OnPrePlayerSuffocated;
        public static event PrePlayerSuicided? OnPrePlayerSuicided;
        public static event PrePlayerSwappedItem? OnPrePlayerSwappedItem;
        public static event PrePlayerTeleported? OnPrePlayerTeleported;
        public static event PrePlayerTired? OnPrePlayerTired;
        public static event PrePlayerWarmUpdated? OnPrePlayerWarmUpdated;
        public static event PreRainBarrelUpdated? OnPreRainBarrelUpdated;
        public static event PreResourceSpawned? OnPreResourceSpawned;
        public static event PreServerSentMessage? OnPreServerMessageSent;
        public static event PreStructureDestroyed? OnPreStructureDestroyed;
        public static event PreTankUpdated? OnPreTankUpdated;
        public static event PreVehicleDestroyed? OnPreVehicleDestroyed;
        public static event PreVehicleExploded? OnPreVehicleExploded;
        public static event PreVehicleMovementChangedByPlayer? OnPreVehicleMovementChangedByPlayer;
        public static event PreVehicleSpawned? OnPreVehicleSpawned;
        public static event PreVehicleSpawnedFromSpawnpoint? OnPreVehicleSpawnedFromSpawnpoint;
        public static event PreZombieKilled? OnPreZombieKilled;
        public static event PreZombieMovementChanged? OnPreZombieMovementChanged;
        public static event PreZombieSpawned? OnPreZombieSpawned;
        public static event RainBarrelUpdated? OnRainBarrelUpdated;
        public static event ResourceDead? OnResourceChopped;
        public static event ResourceDead? OnResourceDestroyed;
        public static event ResourceDead? OnResourceForaged;
        public static event ResourceDead? OnResourceMined;
        public static event ResourceSpawned? OnResourceSpawned;

        public static event StructureDestroyed? OnStructureDestroyed;
        public static event StructureTransformed? OnStructureTransformed;
        public static event TankUpdated? OnTankUpdated;

        // public static event PreVehicleMovementChanged? OnPreVehicleMovementChanged;
        public static event VehicleMovementChanged? OnVehicleMovementChanged;
        public static event VehicleMovementChangedByPlayer? OnVehicleMovementChangedByPlayer;
        public static event VehicleSpawned? OnVehicleSpawned;
        public static event VehicleSpawnedFromSpawnpoint? OnVehicleSpawnedFromSpawnpoint;
        public static event ZombieDamaged? OnZombieDamaged;
        public static event ZombieKilled? OnZombieKilled;
        public static event ZombieMovementChanged? OnZombieMovementChanged;
        public static event ZombieSpawned? OnZombieSpawned;

        #endregion

        #region Nested type: InternalPatches

        [HarmonyPatch]
        internal static class InternalPatches
        {
            [HarmonyCleanup]
            internal static Exception? Cleanup(Exception? ex, MethodBase original)
            {
                if (ex == null)
                    return null;

                Logger.LogError($"[RFRocketLibrary] [ERROR] InternalPatches Exception: {ex}");
                Logger.LogError(
                    $"[RFRocketLibrary] [ERROR] InternalPatches Exception: Failed to patch original method {original.FullDescription()} from patching type {typeof(InternalPatches).FullDescription()}");
                return null;
            }

            [HarmonyPatch(typeof(Provider), "addPlayer")]
            [HarmonyPrefix]
            internal static void AddPlayer(ITransportConnection transportConnection, NetId netId,
                SteamPlayerID playerID, ref Vector3 point, ref byte angle, bool isPro, bool isAdmin, int channel,
                ref byte face, ref byte hair, ref byte beard, ref Color skin, ref Color color, Color markerColor,
                bool hand, ref int shirtItem,
                ref int pantsItem, ref int hatItem, ref int backpackItem, ref int vestItem, ref int maskItem,
                ref int glassesItem, ref int[] skinItems, ref string[] skinTags, ref string[] skinDynamicProps,
                EPlayerSkillset skillset, string language, CSteamID lobbyID)
            {
                OnPrePlayerConnected?.Invoke(playerID, ref point, ref angle, isPro, isAdmin, ref face, ref hair,
                    ref beard, ref skin, ref color, hand, ref shirtItem, ref pantsItem, ref hatItem, ref backpackItem,
                    ref vestItem, ref maskItem, ref glassesItem, ref skinItems, ref skinTags, ref skinDynamicProps,
                    skillset, language);
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveStealVehicleBattery")]
            [HarmonyPrefix]
            internal static bool OnPlayerStealVehicleBatteryInvoker(out bool __state, in ServerInvocationContext context)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var vehicle = player.movement.getVehicle();
                if (vehicle == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerStealVehicleBattery?.Invoke(player, vehicle, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveStealVehicleBattery")]
            [HarmonyPostfix]
            internal static void OnPlayerStealVehicleBatteryInvoker(bool __state, in ServerInvocationContext context)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                var vehicle = player.movement.getVehicle();
                if (vehicle == null)
                    return;

                OnPlayerStealVehicleBattery?.Invoke(player, vehicle);
            }

            [HarmonyPatch(typeof(PlayerStance), "ReceiveClimbRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerClimbedInvoker(out bool __state, PlayerStance __instance, Vector3 direction)
            {
                var shouldAllow = true;
                OnPrePlayerClimbed?.Invoke(__instance.player, direction, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerStance), "ReceiveClimbRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerClimbedInvoker(bool __state, PlayerStance __instance, Vector3 direction)
            {
                if (!__state)
                    return;

                OnPlayerClimbed?.Invoke(__instance.player, direction);
            }

            [HarmonyPatch(typeof(InteractableGenerator), "askBurn")]
            [HarmonyPrefix]
            internal static bool OnGeneratorBurnedInvoker(InteractableGenerator __instance, ushort amount)
            {
                var shouldAllow = true;
                OnPreGeneratorBurned?.Invoke(__instance, amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "ReceiveSuicideRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerSuicideInvoker(out bool __state, PlayerLife __instance)
            {
                __state = false;
                if (__instance.health > 100)
                    return true;

                var shouldAllow = true;
                OnPrePlayerSuicided?.Invoke(__instance.player, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "ReceiveSuicideRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerSuicideInvoker(bool __state, PlayerLife __instance)
            {
                if (!__state)
                    return;
                
                if (!__instance.isDead)
                    return;

                OnPlayerSuicided?.Invoke(__instance.player);
            }

            [HarmonyPatch(typeof(InteractableBed), "ReceiveClaimRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedBedInvoker(out bool __state, InteractableBed __instance,
                in ServerInvocationContext context)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var claim = !__instance.isClaimed;
                var shouldAllow = true;
                OnPrePlayerInteractedBed?.Invoke(player, __instance, claim, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableBed), "ReceiveClaimRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedBedInvoker(bool __state, InteractableBed __instance,
                in ServerInvocationContext context)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                var claim = !__instance.isClaimed;
                OnPlayerInteractedBed?.Invoke(player, __instance, claim);
            }

            [HarmonyPatch(typeof(InteractableMannequin), "ReceivePoseRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedMannequinInvoker(out bool __state, InteractableMannequin __instance,
                in ServerInvocationContext context, byte poseComp)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedMannequinPose?.Invoke(player, __instance, poseComp, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableMannequin), "ReceivePoseRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedMannequinInvoker(bool __state,
                InteractableMannequin __instance, in ServerInvocationContext context, byte poseComp)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedMannequinPose?.Invoke(player, __instance, poseComp);
            }

            [HarmonyPatch(typeof(InteractableMannequin), "ReceiveUpdateRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedMannequinInvoker(out bool __state, InteractableMannequin __instance,
                in ServerInvocationContext context, EMannequinUpdateMode updateMode)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedMannequinUpdate?.Invoke(player, __instance, updateMode, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableMannequin), "ReceiveUpdateRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedMannequinInvoker(bool __state,
                InteractableMannequin __instance,
                in ServerInvocationContext context, EMannequinUpdateMode updateMode)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedMannequinUpdate?.Invoke(player, __instance, updateMode);
            }

            [HarmonyPatch(typeof(InteractableStorage), "ReceiveRotDisplayRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedDisplayInvoker(out bool __state, InteractableStorage __instance,
                in ServerInvocationContext context, byte rotComp)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedDisplay?.Invoke(player, __instance, rotComp, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableStorage), "ReceiveRotDisplayRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedDisplayInvoker(bool __state, InteractableStorage __instance,
                in ServerInvocationContext context, byte rotComp)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedDisplay?.Invoke(player, __instance, rotComp);
            }

            [HarmonyPatch(typeof(InteractableStorage), "ReceiveInteractRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedStorageInvoker(out bool __state, InteractableStorage __instance,
                in ServerInvocationContext context, bool quickGrab)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedStorage?.Invoke(player, __instance, quickGrab, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableStorage), "ReceiveInteractRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedStorageInvoker(bool __state, InteractableStorage __instance,
                in ServerInvocationContext context, bool quickGrab)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedStorage?.Invoke(player, __instance, quickGrab);
            }

            [HarmonyPatch(typeof(InteractableDoor), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedDoorInvoker(out bool __state, InteractableDoor __instance,
                in ServerInvocationContext context, bool desiredOpen)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedDoor?.Invoke(player, __instance, desiredOpen, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableDoor), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedDoorInvoker(bool __state, InteractableDoor __instance,
                in ServerInvocationContext context, bool desiredOpen)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedDoor?.Invoke(player, __instance, desiredOpen);
            }

            [HarmonyPatch(typeof(InteractableFire), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedFireInvoker(out bool __state, InteractableFire __instance,
                in ServerInvocationContext context, bool desiredLit)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedFire?.Invoke(player, __instance, desiredLit, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableFire), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedFireInvoker(bool __state, InteractableFire __instance,
                in ServerInvocationContext context, bool desiredLit)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedFire?.Invoke(player, __instance, desiredLit);
            }

            [HarmonyPatch(typeof(InteractableGenerator), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedGeneratorInvoker(out bool __state, InteractableGenerator __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedGenerator?.Invoke(player, __instance, desiredPowered, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableGenerator), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedGeneratorInvoker(bool __state,
                InteractableGenerator __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedGenerator?.Invoke(player, __instance, desiredPowered);
            }

            [HarmonyPatch(typeof(InteractableOven), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedOvenInvoker(out bool __state, InteractableOven __instance,
                in ServerInvocationContext context, bool desiredLit)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedOven?.Invoke(player, __instance, desiredLit, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableOven), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedOvenInvoker(bool __state, InteractableOven __instance,
                in ServerInvocationContext context, bool desiredLit)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedOven?.Invoke(player, __instance, desiredLit);
            }

            [HarmonyPatch(typeof(InteractableOxygenator), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedOxygenatorInvoker(out bool __state, InteractableOxygenator __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedOxygenator?.Invoke(player, __instance, desiredPowered, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableOxygenator), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedOxygenatorInvoker(bool __state,
                InteractableOxygenator __instance, in ServerInvocationContext context, bool desiredPowered)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedOxygenator?.Invoke(player, __instance, desiredPowered);
            }

            [HarmonyPatch(typeof(InteractableSafezone), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedSafezoneInvoker(out bool __state, InteractableSafezone __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedSafezone?.Invoke(player, __instance, desiredPowered, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableSafezone), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedSafezoneInvoker(bool __state, InteractableSafezone __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedSafezone?.Invoke(player, __instance, desiredPowered);
            }

            [HarmonyPatch(typeof(InteractableSign), "ReceiveChangeTextRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedSignInvoker(out bool __state, InteractableSign __instance,
                in ServerInvocationContext context, string newText)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedSign?.Invoke(player, __instance, newText, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableSign), "ReceiveChangeTextRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedSignInvoker(bool __state, InteractableSign __instance,
                in ServerInvocationContext context, string newText)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedSign?.Invoke(player, __instance, newText);
            }

            [HarmonyPatch(typeof(InteractableSpot), "ReceiveToggleRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedSpotInvoker(out bool __state, InteractableSpot __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedSpot?.Invoke(player, __instance, desiredPowered, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableSpot), "ReceiveToggleRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedSpotInvoker(bool __state, InteractableSpot __instance,
                in ServerInvocationContext context, bool desiredPowered)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedSpot?.Invoke(player, __instance, desiredPowered);
            }

            [HarmonyPatch(typeof(InteractableStereo), "ReceiveTrackRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedStereoInvoker(out bool __state, InteractableStereo __instance,
                in ServerInvocationContext context, Guid newTrack)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedStereoTrack?.Invoke(player, __instance, newTrack, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableStereo), "ReceiveTrackRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedStereoInvoker(bool __state, InteractableStereo __instance,
                in ServerInvocationContext context, Guid newTrack)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedStereoTrack?.Invoke(player, __instance, newTrack);
            }

            [HarmonyPatch(typeof(InteractableStereo), "ReceiveChangeVolumeRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedStereoInvoker(out bool __state, InteractableStereo __instance,
                in ServerInvocationContext context, byte newVolume)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedStereoVolume?.Invoke(player, __instance, newVolume, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableStereo), "ReceiveChangeVolumeRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedStereoInvoker(bool __state, InteractableStereo __instance,
                in ServerInvocationContext context, byte newVolume)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedStereoVolume?.Invoke(player, __instance, newVolume);
            }

            [HarmonyPatch(typeof(InteractableLibrary), "ReceiveTransferLibraryRequest")]
            [HarmonyPrefix]
            internal static bool OnPlayerInteractedLibraryInvoker(out bool __state, InteractableLibrary __instance,
                in ServerInvocationContext context, byte transaction, uint delta)
            {
                __state = false;
                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerInteractedLibrary?.Invoke(player, __instance, transaction, delta, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableLibrary), "ReceiveTransferLibraryRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerInteractedLibraryInvoker(bool __state, InteractableLibrary __instance,
                in ServerInvocationContext context, byte transaction, uint delta)
            {
                if (!__state)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerInteractedLibrary?.Invoke(player, __instance, transaction, delta);
            }

            [HarmonyPatch(typeof(InteractableTank), "ServerSetAmount")]
            [HarmonyPrefix]
            internal static bool OnTankUpdatedInvoker(out bool __state, InteractableTank __instance, ref ushort newAmount)
            {
                var shouldAllow = true;
                OnPreTankUpdated?.Invoke(BarricadeUtil.FindDropFast(__instance.transform), ref newAmount,
                    ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableTank), "ServerSetAmount")]
            [HarmonyPostfix]
            internal static void OnTankUpdatedInvoker(bool __state, InteractableTank __instance,
                ref ushort newAmount)
            {
                if (!__state)
                    return;

                OnTankUpdated?.Invoke(BarricadeUtil.FindDropFast(__instance.transform), newAmount);
            }

            [HarmonyPatch(typeof(ObjectManager), "ReceiveObjectResourceState")]
            [HarmonyPrefix]
            internal static bool ReceiveObjectResourceState(out bool __state, byte x, byte y, ushort index, ref ushort amount)
            {
                var shouldAllow = true;
                OnPreObjectResourceUpdated?.Invoke(LevelObjects.objects[x, y][index], ref amount, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ObjectManager), "ReceiveObjectResourceState")]
            [HarmonyPostfix]
            internal static void ReceiveObjectResourceState(bool __state, byte x, byte y, ushort index,
                ushort amount)
            {
                if (!__state)
                    return;

                OnObjectResourceUpdated?.Invoke(LevelObjects.objects[x, y][index], amount);
            }

            [HarmonyPatch(typeof(Animal), "tick")]
            [HarmonyPrefix]
            internal static void OnAnimalMovementChangedInvoker(out Vector3 __state, Animal __instance,
                Vector3 ___lastUpdatePos)
            {
                // var shouldAllow = true;
                // OnPreAnimalMovementChanged?.Invoke(__instance, ___lastUpdatePos, ref shouldAllow);
                // return shouldAllow;
                __state = ___lastUpdatePos;
            }

            [HarmonyPatch(typeof(Animal), "tick")]
            [HarmonyPostfix]
            internal static void OnAnimalMovementChangedInvoker(Vector3 __state, Animal __instance)
            {
                if (__instance.transform.position == __state)
                    return;

                OnAnimalMovementChanged?.Invoke(__instance, __state);
            }

            [HarmonyPatch(typeof(InteractableOil), "askBurn")]
            [HarmonyPrefix]
            internal static bool OnOilTankBurnedInvoker(out bool __state, InteractableOil __instance, ref ushort amount)
            {
                var shouldAllow = true;
                OnPreOilTankBurned?.Invoke(BarricadeUtil.FindDropFast(__instance.transform), ref amount,
                    ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableOil), "askBurn")]
            [HarmonyPostfix]
            internal static void OnOilTankBurnedInvoker(bool __state, InteractableOil __instance, ushort amount)
            {
                if (!__state)
                    return;

                OnOilTankBurned?.Invoke(BarricadeUtil.FindDropFast(__instance.transform), amount);
            }

            [HarmonyPatch(typeof(BarricadeManager), "updateRainBarrel")]
            [HarmonyPrefix]
            internal static bool OnRainBarrelUpdatedInvoker(out bool __state, Transform transform, ref bool isFull, ref bool shouldSend)
            {
                var shouldAllow = true;
                OnPreRainBarrelUpdated?.Invoke(BarricadeUtil.FindDropFast(transform), ref isFull, ref shouldSend,
                    ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(BarricadeManager), "updateRainBarrel")]
            [HarmonyPostfix]
            internal static void OnRainBarrelUpdatedInvoker(bool __state, Transform transform, bool isFull,
                bool shouldSend)
            {
                if (!__state)
                    return;

                OnRainBarrelUpdated?.Invoke(BarricadeUtil.FindDropFast(transform), isFull, shouldSend);
            }

            [HarmonyPatch(typeof(PlayerMovement), "simulate", typeof(uint), typeof(int), typeof(int), typeof(int),
                typeof(float), typeof(float), typeof(bool), typeof(bool), typeof(float))]
            [HarmonyPrefix]
            internal static void OnPlayerMovementChangedInvoker(PlayerMovement __instance, out Vector3 __state, Vector3 ___velocity, string ___materialName,
                uint simulation, int recov, int input_x, int input_y, float look_x, float look_y,
                bool inputJump, bool inputSprint, float deltaTime)
            {
                // var go = new GameObject
                // {
                //     transform =
                //     {
                //         position = __instance.transform.position,
                //         rotation = __instance.transform.rotation
                //     }
                // };
                // // Logger.LogWarning($"[DEBUG] Prefix Player Last: {__instance.transform.position}");
                // // Logger.LogWarning($"[DEBUG] Prefix Temp Last: {go.transform.position}");
                // var controller = go.AddComponent<CharacterController>();
                // controller.center = controller.center;
                // controller.height = controller.height;
                // controller.radius = controller.radius;
                // controller.detectCollisions = controller.detectCollisions;
                // controller.skinWidth = controller.skinWidth;
                // controller.slopeLimit = controller.slopeLimit;
                // controller.stepOffset = controller.stepOffset;
                // controller.enableOverlapRecovery = controller.enableOverlapRecovery;
                // controller.minMoveDistance = controller.minMoveDistance;
                //
                // var move = new Vector3(input_x, 0f, input_y);
                // if (__instance.player.stance.stance == EPlayerStance.CLIMB)
                // {
                //     controller.CheckedMove(
                //         new Vector3(0f, input_y * __instance.speed * 0.5f, 0f) * deltaTime,
                //         ___overlappingHoleVolumes is {Count: > 0});
                // }
                // else if (__instance.player.stance.stance == EPlayerStance.SWIM)
                // {
                //     if (__instance.player.stance.isSubmerged || (__instance.player.look.pitch > 110f && move.z > 0.1))
                //     {
                //         var velocity = __instance.player.look.aim.rotation * move.normalized * __instance.speed * 5f;
                //         if (inputJump)
                //             velocity.y = 3f * __instance.pluginJumpMultiplier;
                //
                //         controller.CheckedMove(velocity * deltaTime,
                //             ___overlappingHoleVolumes is {Count: > 0});
                //     }
                //     else
                //     {
                //         WaterUtility.getUnderwaterInfo(__instance.transform.position, out _, out var num);
                //         var velocity = __instance.transform.rotation * move.normalized * __instance.speed * 5f;
                //         velocity.y = (num - 1.275f - __instance.transform.position.y) / 8f;
                //         controller.CheckedMove(velocity * deltaTime,
                //             ___overlappingHoleVolumes is {Count: > 0});
                //     }
                // }
                // else
                // {
                //     var flag3 = false;
                //     if (__instance.isGrounded && __instance.ground.normal.y > 0f)
                //     {
                //         var num2 = Vector3.Angle(Vector3.up, __instance.ground.normal);
                //         var num3 = 59f;
                //         if (Level.info != null && Level.info.configData != null &&
                //             Level.info.configData.Max_Walkable_Slope > -0.5f)
                //         {
                //             num3 = Level.info.configData.Max_Walkable_Slope;
                //         }
                //
                //         if (num2 > num3)
                //         {
                //             flag3 = true;
                //             var a = Vector3.Cross(Vector3.Cross(Vector3.up, __instance.ground.normal),
                //                 __instance.ground.normal);
                //             ___velocity += a * 16f * deltaTime;
                //         }
                //     }
                //
                //     if (!flag3)
                //     {
                //         var vector = __instance.transform.rotation * move.normalized * __instance.speed * 5f;
                //         if (__instance.isGrounded)
                //         {
                //             vector = Vector3.Cross(Vector3.Cross(Vector3.up, vector), __instance.ground.normal);
                //             vector.y = Mathf.Min(vector.y, 0f);
                //             // var ewipDoNotUseTemp_SlipMode = Traverse
                //             //     .Create(Type.GetType("SDG.Unturned.PhysicMaterialCustomData"))
                //             //     .Method("WipDoNotUseTemp_GetSlipMode")
                //             //     .GetValue(___materialName) is EWipDoNotUseTemp_SlipMode ? (EWipDoNotUseTemp_SlipMode) Traverse
                //             //     .Create(Type.GetType("SDG.Unturned.PhysicMaterialCustomData"))
                //             //     .Method("WipDoNotUseTemp_GetSlipMode")
                //             //     .GetValue(___materialName) : EWipDoNotUseTemp_SlipMode.None;
                //             var ewipDoNotUseTemp_SlipMode = EWipDoNotUseTemp_SlipMode.None;
                //             switch (ewipDoNotUseTemp_SlipMode)
                //             {
                //                 case EWipDoNotUseTemp_SlipMode.LegacyIce:
                //                     ___velocity = Vector3.Lerp(___velocity, vector, deltaTime);
                //                     break;
                //                 case EWipDoNotUseTemp_SlipMode.LegacyMetal:
                //                 {
                //                     var num4 = __instance.ground.normal.y < 0.75f
                //                         ? 0f
                //                         : Mathf.Lerp(0f, 1f, (__instance.ground.normal.y - 0.75f) * 4f);
                //                     ___velocity = Vector3.Lerp(___velocity, vector * 2f,
                //                         __instance.isMoving ? (2f * deltaTime) : (0.5f * num4 * deltaTime));
                //                     break;
                //                 }
                //                 default:
                //                     ___velocity = vector;
                //                     break;
                //             }
                //         }
                //         else
                //         {
                //             ___velocity.y += Physics.gravity.y *
                //                              (__instance.fall <= 0f ? __instance.totalGravityMultiplier : 1f) * deltaTime * 3f;
                //             var a2 = __instance.totalGravityMultiplier < 0.99f
                //                 ? Physics.gravity.y * 2f * __instance.totalGravityMultiplier
                //                 : -100f;
                //             ___velocity.y = Mathf.Max(a2, ___velocity.y);
                //             var horizontalMagnitude = vector.GetHorizontalMagnitude();
                //             var horizontal = ___velocity.GetHorizontal();
                //             var horizontalMagnitude2 = ___velocity.GetHorizontalMagnitude();
                //             float maxMagnitude;
                //             if (horizontalMagnitude2 > horizontalMagnitude)
                //             {
                //                 var num5 = 2f * Provider.modeConfigData.Gameplay.AirStrafing_Deceleration_Multiplier;
                //                 maxMagnitude = Mathf.Max(horizontalMagnitude, horizontalMagnitude2 - num5 * deltaTime);
                //             }
                //             else
                //                 maxMagnitude = horizontalMagnitude;
                //
                //             var a3 = vector *
                //                      (4f * Provider.modeConfigData.Gameplay.AirStrafing_Acceleration_Multiplier);
                //             var vector2 = horizontal + a3 * deltaTime;
                //             vector2 = vector2.ClampHorizontalMagnitude(maxMagnitude);
                //             ___velocity.x = vector2.x;
                //             ___velocity.z = vector2.z;
                //         }
                //     }
                //
                //     if (inputJump && __instance.isGrounded && !__instance.player.life.isBroken &&
                //         __instance.player.life.stamina >= 10f * (1f - __instance.player.skills.mastery(0, 6) * 0.5f) &&
                //         __instance.player.stance.stance is EPlayerStance.STAND or EPlayerStance.SPRINT &&
                //         !MathfEx.IsNearlyZero(__instance.pluginJumpMultiplier, 0.001f))
                //     {
                //         ___velocity.y = 7f * (1f + __instance.player.skills.mastery(0, 6) * 0.25f) *
                //                         __instance.pluginJumpMultiplier;
                //     }
                //
                //     ___velocity += __instance.pendingLaunchVelocity;
                //     __instance.pendingLaunchVelocity = Vector3.zero;
                //     controller.CheckedMove(___velocity * deltaTime, ___overlappingHoleVolumes is {Count: > 0});
                // }
                //
                // __state = go.transform.position;
                // Object.Destroy(go);
                //
                // var shouldAllow = true;
                // OnPrePlayerMovementChanged?.Invoke(__instance.player, __instance.transform.position, __state, ref shouldAllow);
                // return shouldAllow;
                __state = __instance.transform.position;
            }

            [HarmonyPatch(typeof(PlayerMovement), "simulate", typeof(uint), typeof(int), typeof(int), typeof(int),
                typeof(float), typeof(float), typeof(bool), typeof(bool), typeof(float))]
            [HarmonyPostfix]
            internal static void OnPlayerMovementChangedInvoker(PlayerMovement __instance,
                Vector3 __state, uint simulation, int recov, int input_x, int input_y, float look_x, float look_y,
                bool inputJump, bool inputSprint, float deltaTime)
            {
                if (__instance.transform.position == __state)
                    return;

                OnPlayerMovementChanged?.Invoke(__instance.player, __state);
            }

            [HarmonyPatch(typeof(Player), "teleportToLocationUnsafe")]
            [HarmonyPrefix]
            internal static bool OnPlayerTeleportedInvoker(Player __instance,
                out Tuple<bool, Vector3> __state, ref Vector3 position, ref float yaw)
            {
                var shouldAllow = true;
                OnPrePlayerTeleported?.Invoke(__instance, ref position, ref yaw, ref shouldAllow);
                __state = new Tuple<bool, Vector3>(shouldAllow, __instance.transform.position);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Player), "teleportToLocationUnsafe")]
            [HarmonyPostfix]
            internal static void OnPlayerTeleportedInvoker(Tuple<bool, Vector3> __state, Player __instance, Vector3 position, float yaw)
            {
                if (__state is not {Item1: true})
                    return;

                OnPlayerTeleported?.Invoke(__instance, __state.Item2, position, yaw);
            }

            [HarmonyPatch(typeof(AnimalManager), "sendAnimalAlive")]
            [HarmonyPrefix]
            internal static bool OnPreAnimalSpawnedInvoker(out bool __state, Animal animal, ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreAnimalSpawned?.Invoke(animal, ref newPosition, ref newAngle, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(AnimalManager), "sendAnimalAlive")]
            [HarmonyPostfix]
            internal static void OnPreAnimalSpawnedInvoker(bool __state, Animal animal, Vector3 newPosition,
                byte newAngle)
            {
                if (!__state)
                    return;

                OnAnimalSpawned?.Invoke(animal, newPosition, newAngle);
            }

            [HarmonyPatch(typeof(ItemManager), "ReceiveItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemSpawnedInvoker(out bool __state, byte x, byte y, ref ushort id, ref byte amount,
                ref byte quality,
                ref byte[] state, ref Vector3 point, uint instanceID)
            {
                var shouldAllow = true;
                OnPreItemSpawned?.Invoke(x, y, ref id, ref amount, ref quality, ref state, ref point, instanceID,
                    ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ItemManager), "ReceiveItem")]
            [HarmonyPostfix]
            internal static void OnPreItemSpawnedInvoker(bool __state, byte x, byte y, ushort id, byte amount,
                byte quality, byte[] state, Vector3 point, uint instanceID)
            {
                if (!__state)
                    return;

                OnItemSpawned?.Invoke(x, y, id, amount, quality, state, point, instanceID);
            }

            [HarmonyPatch(typeof(ResourceManager), "ServerSetResourceAlive")]
            [HarmonyPrefix]
            internal static bool OnPreResourceSpawnedInvoker(out bool __state, byte x, byte y, ushort index)
            {
                var shouldAllow = true;
                OnPreResourceSpawned?.Invoke(LevelGround.trees[x, y][index], ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ResourceManager), "ServerSetResourceAlive")]
            [HarmonyPostfix]
            internal static void OnPreResourceSpawnedInvoker(bool __state, byte x, byte y, ushort index)
            {
                if (!__state)
                    return;

                OnResourceSpawned?.Invoke(LevelGround.trees[x, y][index]);
            }

            [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
            [HarmonyPrefix]
            internal static bool OnPreVehicleSpawnedFromSpawnpointInvoker(out bool __state, VehicleSpawnpoint spawn)
            {
                var shouldAllow = true;
                OnPreVehicleSpawnedFromSpawnpoint?.Invoke(spawn, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
            [HarmonyPostfix]
            internal static void OnPreVehicleSpawnedFromSpawnpointInvoker(bool __state,
                InteractableVehicle __result, VehicleSpawnpoint spawn)
            {
                if (!__state)
                    return;

                OnVehicleSpawnedFromSpawnpoint?.Invoke(spawn, __result);
            }

            [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset),typeof(ushort),typeof(ushort),typeof(float),typeof(Vector3),typeof(Quaternion),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(ushort),typeof(ushort),typeof(ushort),typeof(CSteamID),typeof(CSteamID),typeof(bool),typeof(byte[][]),typeof(byte))]
            [HarmonyPrefix]
            internal static bool OnPreVehicleSpawnedInvoker(out bool __state, ref VehicleAsset asset, ref ushort skinID,
                ref ushort mythicID, ref float roadPosition, ref Vector3 point, ref Quaternion angle, ref bool sirens,
                ref bool blimp,
                ref bool headlights, ref bool taillights, ref ushort fuel, ref ushort health, ref ushort batteryCharge,
                ref CSteamID owner,
                ref CSteamID group, ref bool locked, ref byte[][] turrets, ref byte tireAliveMask)
            {
                var shouldAllow = true;
                OnPreVehicleSpawned?.Invoke(ref asset, ref skinID, ref mythicID, ref roadPosition, ref point, ref angle,
                    ref sirens, ref blimp, ref headlights, ref taillights, ref fuel, ref health, ref batteryCharge,
                    ref owner, ref group, ref locked, ref turrets, ref tireAliveMask, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset), typeof(ushort), typeof(ushort), typeof(float), typeof(Vector3), typeof(Quaternion), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(ushort), typeof(ushort), typeof(ushort), typeof(CSteamID), typeof(CSteamID), typeof(bool), typeof(byte[][]), typeof(byte))]
            [HarmonyPostfix]
            internal static void OnPreVehicleSpawnedInvoker(bool __state,
                InteractableVehicle __result,
                VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle,
                bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health,
                ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask)
            {
                if (!__state)
                    return;

                OnVehicleSpawned?.Invoke(__result);
            }

            [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset),typeof(ushort),typeof(ushort),typeof(float),typeof(Vector3),typeof(Quaternion),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(ushort),typeof(ushort),typeof(ushort),typeof(CSteamID),typeof(CSteamID),typeof(bool),typeof(byte[][]),typeof(byte),typeof(Color32))]
            [HarmonyPrefix]
            internal static bool OnPreVehicleSpawnedInvoker(out bool __state, ref VehicleAsset asset, ref ushort skinID,
                ref ushort mythicID, ref float roadPosition, ref Vector3 point, ref Quaternion angle, ref bool sirens,
                ref bool blimp,
                ref bool headlights, ref bool taillights, ref ushort fuel, ref ushort health, ref ushort batteryCharge,
                ref CSteamID owner,
                ref CSteamID group, ref bool locked, ref byte[][] turrets, ref byte tireAliveMask, ref Color32 paintColor)
            {
                var shouldAllow = true;
                OnPreVehicleSpawned?.Invoke(ref asset, ref skinID, ref mythicID, ref roadPosition, ref point, ref angle,
                    ref sirens, ref blimp, ref headlights, ref taillights, ref fuel, ref health, ref batteryCharge,
                    ref owner, ref group, ref locked, ref turrets, ref tireAliveMask, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset),typeof(ushort),typeof(ushort),typeof(float),typeof(Vector3),typeof(Quaternion),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(ushort),typeof(ushort),typeof(ushort),typeof(CSteamID),typeof(CSteamID),typeof(bool),typeof(byte[][]),typeof(byte),typeof(Color32))]
            [HarmonyPostfix]
            internal static void OnPreVehicleSpawnedInvoker(bool __state,
                InteractableVehicle __result,
                VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle,
                bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health,
                ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask, Color32 paintColor)
            {
                if (!__state)
                    return;

                OnVehicleSpawned?.Invoke(__result);
            }

            [HarmonyPatch(typeof(ZombieManager), "sendZombieAlive")]
            [HarmonyPrefix]
            internal static bool OnPreZombieSpawnedInvoker(out bool __state, Zombie zombie, ref byte newType,
                ref byte newSpeciality, ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear,
                ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreZombieSpawned?.Invoke(zombie, ref newType, ref newSpeciality, ref newShirt, ref newPants,
                    ref newHat, ref newGear, ref newPosition, ref newAngle, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ZombieManager), "sendZombieAlive")]
            [HarmonyPostfix]
            internal static void OnPreZombieSpawnedInvoker(bool __state, Zombie zombie, byte newType,
                byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear,
                Vector3 newPosition, byte newAngle)
            {
                if (!__state)
                    return;

                OnZombieSpawned?.Invoke(zombie, newType, newSpeciality, newShirt, newPants,
                    newHat, newGear, newPosition, newAngle);
            }

            [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
                typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(float), typeof(float),
                typeof(float))]
            [HarmonyPrefix]
            internal static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance,
                out Vector3 __state,
                Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
                Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
            {
                // var shouldAllow = true;
                // OnPreVehicleMovementChangedByPlayer?.Invoke(__instance,
                //     __instance.passengers.ElementAtOrDefault(0)?.player?.player, ___lastUpdatedPos, ref shouldAllow);
                // return shouldAllow;
                __state = ___lastUpdatedPos;
            }

            [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
                typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(float), typeof(float),
                typeof(float))]
            [HarmonyPostfix]
            internal static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance, Vector3 __state, uint simulation, int recov, bool inputStamina,
                Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
            {
                OnVehicleMovementChangedByPlayer?.Invoke(__instance,
                    __instance.passengers.ElementAtOrDefault(0)?.player?.player, __state);
            }

            [HarmonyPatch(typeof(InteractableVehicle), "UpdateSafezoneStatus")]
            [HarmonyPrefix]
            internal static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance, out Vector3 __state,
                Vector3 ___lastUpdatedPos, float deltaSeconds)
            {
                __state = ___lastUpdatedPos;
            }

            [HarmonyPatch(typeof(InteractableVehicle), "UpdateSafezoneStatus")]
            [HarmonyPostfix]
            internal static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance,
                Vector3 __state, float deltaSeconds)
            {
                if (__instance.transform.position == __state)
                    return;

                OnVehicleMovementChanged?.Invoke(__instance, __state);
            }

            [HarmonyPatch(typeof(Zombie), "tick")]
            [HarmonyPrefix]
            internal static void OnZombieMovementChangedInvoker(Zombie __instance, out Vector3 __state,
                Vector3 ___lastUpdatedPos)
            {
                // var shouldAllow = true;
                // OnPreZombieMovementChanged?.Invoke(__instance, ___lastUpdatedPos, ref shouldAllow);
                // return shouldAllow;
                __state = ___lastUpdatedPos;
            }

            [HarmonyPatch(typeof(Zombie), "tick")]
            [HarmonyPostfix]
            internal static void OnZombieMovementChangedInvoker(Zombie __instance, Vector3 __state)
            {
                if (__instance.transform.position == __state)
                    return;

                OnZombieMovementChanged?.Invoke(__instance, __state);
            }

            [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalDead")]
            [HarmonyPostfix]
            internal static void OnAnimalKilledInvoker(bool __state, ushort index, ref Vector3 newRagdoll,
                ref ERagdollEffect newRagdollEffect)
            {
                if (!__state)
                    return;

                OnAnimalKilled?.Invoke(AnimalManager.animals.ElementAtOrDefault(index), ref newRagdoll,
                    ref newRagdollEffect);
            }

            [HarmonyPatch(typeof(Animal), "askDamage")]
            [HarmonyPrefix]
            internal static bool OnPreAnimalKilledInvoker(out bool __state, Animal __instance, ushort ___health, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref ERagdollEffect ragdollEffect)
            {
                __state = true;
                if (___health > amount)
                    return true;

                var shouldAllow = true;
                OnPreAnimalKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                    ref ragdollEffect, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Animal), "askDamage")]
            [HarmonyPostfix]
            internal static void OnAnimalKilledInvoker(bool __state, Animal __instance, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref ERagdollEffect ragdollEffect)
            {
                if (!__state)
                    return;

                OnAnimalDamaged?.Invoke(__instance, amount, kill, xp);
            }

            [HarmonyPatch(typeof(BarricadeManager), "InternalSetBarricadeTransform")]
            [HarmonyPostfix]
            internal static void OnBarricadeTransformedInvoker(byte x, byte y, ushort plant,
                BarricadeDrop barricade, Vector3 point, Quaternion rotation)
            {
                OnBarricadeTransformed?.Invoke(x, y, plant, barricade, point, rotation);
            }

            [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade")]
            [HarmonyPatch(new[] {typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort)})]
            [HarmonyPrefix]
            internal static bool OnPreBarricadeDestroyedInvoker(out bool __state, BarricadeDrop barricade, byte x, byte y, ushort plant)
            {
                var shouldAllow = true;
                OnPreBarricadeDestroyed?.Invoke(barricade, x, y, plant, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade")]
            [HarmonyPatch(new[] {typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort)})]
            [HarmonyPostfix]
            internal static void OnBarricadeDestroyedInvoker(bool __state, BarricadeDrop barricade, byte x,
                byte y, ushort plant)
            {
                if (!__state)
                    return;

                OnBarricadeDestroyed?.Invoke(barricade, x, y, plant);
            }

            // [HarmonyLib.HarmonyPatch(typeof(UseableFisher), "ReceiveCatch")]
            // [HarmonyLib.HarmonyPrefix]
            // internal static bool OnPreFishCaughtInvoker(UseableFisher __instance, ref bool ___isCatch,
            //     float ___lastLuck, float ___luckTime, bool ___hasLuckReset)
            // {
            //     if (OnPrePlayerCaughtFish == null)
            //         return true;
            //     if (Time.realtimeSinceStartup - ___lastLuck > ___luckTime - 2.4f ||
            //         ___hasLuckReset && Time.realtimeSinceStartup - ___lastLuck < 1f)
            //         ___isCatch = true;
            //     OnPrePlayerCaughtFish.Invoke(__instance.player, ref ___isCatch);
            //     return false;
            // }

            [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.forceAddItem), typeof(Item), typeof(bool))]
            [HarmonyPrefix]
            internal static bool ForceAddItem(PlayerInventory __instance, Item item, bool auto)
            {
                if (__instance.player.equipment.isBusy && auto == false && item.state.Length == 0)
                {
                    var giveFish = true;
                    OnPrePlayerCaughtFish?.Invoke(__instance.player, ref giveFish);
                    return giveFish;
                }

                return true;
            }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveDragItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemDraggedInvoker(PlayerInventory __instance, byte page_0, byte x_0, byte y_0,
                byte page_1, byte x_1, byte y_1, byte rot_1)
            {
                var shouldAllow = true;
                OnPrePlayerDraggedItem?.Invoke(__instance, page_0, x_0, y_0, page_1, x_1, y_1, rot_1, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveSwapItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemSwappedInvoker(PlayerInventory __instance, byte page_0, byte x_0, byte y_0,
                byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
            {
                var shouldAllow = true;
                OnPrePlayerSwappedItem?.Invoke(__instance, page_0, x_0, y_0, rot_0, page_1, x_1, y_1, rot_1,
                    ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(UseableMelee), "fire")]
            [HarmonyPostfix]
            internal static void OnPlayerAttackInvoker(UseableMelee __instance)
            {
                OnPlayerAttack?.Invoke(__instance.player);
            }

            [HarmonyPatch(typeof(UseableGun), "jab")]
            [HarmonyPostfix]
            internal static void OnPlayerAttackInvoker2(UseableGun __instance)
            {
                OnPlayerAttack?.Invoke(__instance.player);
            }

            [HarmonyPatch(typeof(PlayerEquipment), "ReceiveEquip")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChangedEquipmentInvoker(PlayerEquipment __instance, byte page, byte x,
                byte y, ref Guid newAssetGuid, ref byte newQuality, ref byte[] newState, ref NetId useableNetId)
            {
                var shouldAllow = true;
                OnPrePlayerChangedEquipment?.Invoke(__instance.player, page, x, y, ref newAssetGuid, ref newQuality,
                    ref newState, ref useableNetId, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveDropItem")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerDroppedItemInvoker(PlayerInventory __instance, byte page, byte x,
                byte y)
            {
                var shouldAllow = true;
                OnPrePlayerDroppedItem?.Invoke(__instance.player, page, x, y, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "doDamage")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerKilledInvoker(out bool __state, PlayerLife __instance, byte amount, ref Vector3 newRagdoll,
                ref EDeathCause newCause, ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill,
                ref bool trackKill, ref ERagdollEffect newRagdollEffect, bool canCauseBleeding = true)
            {
                __state = true;
                if (__instance.health > amount)
                    return true;

                var shouldAllow = true;
                OnPrePlayerKilled?.Invoke(__instance.player, ref newRagdoll, ref newCause, ref newLimb,
                    ref newKiller, ref kill, ref trackKill, ref newRagdollEffect, ref shouldAllow);
                __state = false;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "doDamage")]
            [HarmonyPostfix]
            internal static void OnPlayerKilledInvoker(bool __state, PlayerLife __instance, byte amount,
                ref Vector3 newRagdoll,
                ref EDeathCause newCause, ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill,
                ref bool trackKill, ref ERagdollEffect newRagdollEffect, bool canCauseBleeding = true)
            {
                if (!__state)
                    return;

                OnPlayerDamaged?.Invoke(__instance.player, amount, newCause, newLimb, newKiller, kill);
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveResourceDead")]
            [HarmonyPostfix]
            internal static void OnResourceDeadInvoker(byte x, byte y, ushort index,
                ref Vector3 ragdoll)
            {
                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return;

                OnResourceDestroyed?.Invoke(resourceSpawnpoint, ref ragdoll);
                if (resourceSpawnpoint.asset.isForage)
                    OnResourceForaged?.Invoke(resourceSpawnpoint, ref ragdoll);
                if (resourceSpawnpoint.asset.hasDebris && !resourceSpawnpoint.asset.isForage)
                    OnResourceChopped?.Invoke(resourceSpawnpoint, ref ragdoll);
                if (!resourceSpawnpoint.asset.hasDebris && !resourceSpawnpoint.asset.isForage)
                    OnResourceMined?.Invoke(resourceSpawnpoint, ref ragdoll);
            }

            [HarmonyPatch(typeof(ObjectManager), "ReceiveObjectRubble")]
            [HarmonyPostfix]
            internal static void ObjectRubbleInvoker(byte x, byte y, ushort index, byte section, bool isAlive,
                Vector3 ragdoll)
            {
                var levelObject = LevelObjects.objects[x, y]?[index];
                if (levelObject == null)
                    return;

                switch (isAlive)
                {
                    case true:
                        OnObjectSpawned?.Invoke(levelObject, section);
                        break;
                    case false:
                        OnObjectDestroyed?.Invoke(levelObject, section);
                        break;
                }
            }

            [HarmonyPatch(typeof(UseableGun), "ReceiveChangeFiremode")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerFiremodeChangedInvoker(out bool __state, UseableGun __instance, EFiremode newFiremode)
            {
                __state = false;
                var player = __instance.player;
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerFiremodeChanged?.Invoke(player, newFiremode, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(UseableGun), "ReceiveChangeFiremode")]
            [HarmonyPostfix]
            internal static void OnPlayerFiremodeChangedInvoker(bool __state, UseableGun __instance,
                EFiremode newFiremode)
            {
                if (!__state)
                    return;
                
                var player = __instance.player;
                if (player == null)
                    return;

                OnPlayerFiremodeChanged?.Invoke(player, newFiremode);
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveForageRequest")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerForagedResourceInvoker(out bool __state, in ServerInvocationContext context, byte x, byte y,
                ushort index)
            {
                __state = false;
                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return true;

                var player = context.GetPlayer();
                if (player == null)
                    return true;

                var shouldAllow = true;
                OnPrePlayerForagedResource?.Invoke(player, resourceSpawnpoint, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveForageRequest")]
            [HarmonyPostfix]
            internal static void OnPlayerForagedResourceInvoker(bool __state,
                in ServerInvocationContext context, byte x, byte y,
                ushort index)
            {
                if (!__state)
                    return;

                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerForagedResource?.Invoke(player, resourceSpawnpoint);
            }

            [HarmonyPatch(typeof(StructureManager), "InternalSetStructureTransform")]
            [HarmonyPostfix]
            internal static void OnStructureTransformedInvoker(byte x, byte y, StructureDrop drop,
                Vector3 point, Quaternion rotation)
            {
                OnStructureTransformed?.Invoke(x, y, drop, point, rotation);
            }

            [HarmonyPatch(typeof(StructureManager), "destroyStructure")]
            [HarmonyPatch(new[] {typeof(StructureDrop), typeof(byte), typeof(byte), typeof(Vector3)})]
            [HarmonyPrefix]
            internal static bool OnPreStructureDestroyedInvoker(out bool __state, StructureDrop structure, byte x, byte y,
                ref Vector3 ragdoll)
            {
                var shouldAllow = true;
                OnPreStructureDestroyed?.Invoke(structure, x, y, ref ragdoll, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(StructureManager), "destroyStructure")]
            [HarmonyPatch(new[] {typeof(StructureDrop), typeof(byte), typeof(byte), typeof(Vector3)})]
            [HarmonyPostfix]
            internal static void OnStructureDestroyedInvoker(bool __state, StructureDrop structure, byte x,
                byte y, ref Vector3 ragdoll)
            {
                if (!__state)
                    return;

                OnStructureDestroyed?.Invoke(structure, x, y, ragdoll);
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveDestroySingleVehicle")]
            [HarmonyPrefix]
            internal static bool OnPreVehicleDestroyedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
                if (vehicle == null)
                    return true;

                var flag = true;
                OnPreVehicleDestroyed?.Invoke(vehicle, ref flag);
                return flag;
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveVehicleExploded")]
            [HarmonyPrefix]
            internal static bool OnPreVehicleExplodedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);

                var flag = true;
                OnPreVehicleExploded?.Invoke(vehicle, ref flag);
                return flag;
            }

            [HarmonyPatch(typeof(ZombieManager), "ReceiveZombieDead")]
            [HarmonyPrefix]
            internal static void OnZombieKilledInvoker(byte reference, ushort id, ref Vector3 newRagdoll,
                ref ERagdollEffect newRagdollEffect)
            {
                var region = ZombieManager.regions.ElementAtOrDefault(reference);
                var zombie = region?.zombies.ElementAtOrDefault(id);
                OnZombieKilled?.Invoke(zombie, region, ref newRagdoll, ref newRagdollEffect);
            }

            [HarmonyPatch(typeof(ChatManager), "ReceiveChatRequest")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChattedInvoker(in ServerInvocationContext context, byte flags, string text)
            {
                var callingPlayer = context.GetCallingPlayer();
                if (callingPlayer == null || callingPlayer.player == null)
                    return true;

                var shouldAllow = true;
                var chatMode = (EChatMode) (flags & 127);
                OnPrePlayerChatted?.Invoke(callingPlayer.player, ref chatMode, ref text, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ChatManager), "serverSendMessage")]
            [HarmonyPrefix]
            internal static bool OnPreServerMessageSentInvoker(ref string text, ref Color color,
                ref SteamPlayer fromPlayer, ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL,
                ref bool useRichTextFormatting)
            {
                var shouldAllow = true;
                OnPreServerMessageSent?.Invoke(ref text, ref color, ref fromPlayer, ref toPlayer, ref mode, ref iconURL,
                    ref useRichTextFormatting, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerAnimator), "sendGesture")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChangedGestureInvoker(PlayerAnimator __instance, ref EPlayerGesture gesture,
                bool all)
            {
                var shouldAllow = true;
                OnPrePlayerChangedGesture?.Invoke(__instance.player, ref gesture, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerStance), "checkStance", new[] {typeof(EPlayerStance), typeof(bool)})]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChangedStanceInvoker(PlayerStance __instance, ref EPlayerStance newStance,
                ref bool all)
            {
                var shouldAllow = true;
                OnPrePlayerChangedStance?.Invoke(__instance.player, __instance.stance, ref newStance, ref all,
                    ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askStarve")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerStarvedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerStarved?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askDehydrate")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerDehydratedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerDehydrated?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askRadiate")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerRadiatedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerRadiated?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askInfect")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerInfectedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerInfected?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askTire")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerTiredInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerTired?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askSuffocate")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerSuffocatedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerSuffocated?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "askBlind")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerHallucinationBlindedInvoker(PlayerLife __instance, ref byte amount)
            {
                var shouldAllow = true;
                OnPrePlayerHallucinationBlinded?.Invoke(__instance.player, ref amount, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "simulatedModifyWarmth")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerWarmUpdatedInvoker(PlayerLife __instance, ref short delta)
            {
                var shouldAllow = true;
                OnPrePlayerWarmUpdated?.Invoke(__instance.player, ref delta, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Zombie), "askDamage")]
            [HarmonyPatch(new[] { typeof(ushort), typeof(Vector3), typeof(EPlayerKill), typeof(uint), typeof(bool), typeof(bool), typeof(EZombieStunOverride), typeof(ERagdollEffect) })]
            [HarmonyPrefix]
            internal static bool OnPreZombieKilledInvoker(out bool __state, Zombie __instance, ushort ___health, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref EZombieStunOverride stunOverride, ref ERagdollEffect ragdollEffect)
            {
                __state = true;
                if (___health > amount)
                    return true;

                var shouldAllow = true;
                OnPreZombieKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                    ref stunOverride, ref ragdollEffect, ref shouldAllow);
                __state = shouldAllow;
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Zombie), "askDamage")]
            [HarmonyPatch(new[] { typeof(ushort), typeof(Vector3), typeof(EPlayerKill), typeof(uint), typeof(bool), typeof(bool), typeof(EZombieStunOverride), typeof(ERagdollEffect) })]
            [HarmonyPostfix]
            internal static void OnZombieKilledInvoker(bool __state, Zombie __instance, ushort amount,
                Vector3 newRagdoll, EPlayerKill kill, uint xp, bool trackKill = true, bool dropLoot = true,
                EZombieStunOverride stunOverride = EZombieStunOverride.None, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
            {
                if (!__state)
                    return;

                OnZombieDamaged?.Invoke(__instance, amount, kill, xp);
            }
        }

        #endregion
    }
}