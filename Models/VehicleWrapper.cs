using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

#nullable disable

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class VehicleWrapper
    {
        public ushort Id { get; set; }
        public Guid assetId { get; set; }
        public uint InstanceId { get; set; }
        public ushort SkinId { get; set; }
        public ushort MythicId { get; set; }
        public float RoadPosition { get; set; }
        public ushort Health { get; set; }
        public ushort Fuel { get; set; }
        public ushort BatteryCharge { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public bool[] Tires { get; set; } = Array.Empty<bool>();
        public List<byte[]> Turrets { get; set; } = new();
        public ItemsWrapper TrunkItems { get; set; } = new();
        public List<BarricadeWrapper> Barricades { get; set; } = new();
        public List<StructureWrapper> Structures { get; set; } = new();
        public Vector3Wrapper Position { get; set; }
        public QuartenionWrapper Rotation { get; set; }
        public byte[] PaintColor_ { get; set; } = new byte[4] { 0,0,0,0 };
        //public Color PaintColor { get; set; } = new Color(0, 0, 0, 1);
        // public QuaternionWrapper Rotation { get; set; }

        public VehicleWrapper()
        {
        }

        public VehicleWrapper(ushort id, uint instanceId, ushort skinId, ushort mythicId, float roadPosition,
            ushort health, ushort fuel, ushort batteryCharge, ulong owner, ulong @group, bool[] tires,
            List<byte[]> turrets, ItemsWrapper trunkItems, List<BarricadeWrapper> barricades, Vector3Wrapper position,
            QuartenionWrapper rotation, Guid assetid = default(Guid))
        {
            Id = id;
            assetId = assetid;
            InstanceId = instanceId;
            SkinId = skinId;
            MythicId = mythicId;
            RoadPosition = roadPosition;
            Health = health;
            Fuel = fuel;
            BatteryCharge = batteryCharge;
            Owner = owner;
            Group = group;
            Tires = tires;
            Turrets = turrets;
            TrunkItems = trunkItems;
            Barricades = barricades;
            Position = position;
            Rotation = rotation;
        }
        public VehicleWrapper(ushort id, uint instanceId, ushort skinId, ushort mythicId, float roadPosition,
    ushort health, ushort fuel, ushort batteryCharge, ulong owner, ulong @group, bool[] tires,
    List<byte[]> turrets, ItemsWrapper trunkItems, List<BarricadeWrapper> barricades, Vector3Wrapper position,
    QuartenionWrapper rotation, byte[] paintcolor, Guid assetid = default(Guid))
        {
            Id = id;
            assetId = assetid;
            InstanceId = instanceId;
            SkinId = skinId;
            MythicId = mythicId;
            RoadPosition = roadPosition;
            Health = health;
            Fuel = fuel;
            BatteryCharge = batteryCharge;
            Owner = owner;
            Group = group;
            Tires = tires;
            Turrets = turrets;
            TrunkItems = trunkItems;
            Barricades = barricades;
            Position = position;
            Rotation = rotation;
            PaintColor_ = paintcolor;
        }

        public static VehicleWrapper Create(InteractableVehicle vehicle)
        {
            var vehicleTurret = new List<byte[]>();
            if (vehicle.turrets != null && vehicle.turrets?.Length != 0)
            {
                byte index = 0;
                while (index < vehicle.turrets?.Length)
                {
                    vehicleTurret.Add(vehicle.turrets.ElementAtOrDefault(index) != null
                        ? vehicle.turrets[index].state
                        : Array.Empty<byte>());
                    index++;
                }
            }

            var result = new VehicleWrapper
            {
                Health = vehicle.health,
                Fuel = vehicle.fuel,
                BatteryCharge = vehicle.batteryCharge,
                Id = vehicle.id,
                assetId = vehicle.asset.GUID,
                TrunkItems = vehicle.trunkItems?.items != null && vehicle.trunkItems.items.Count != 0
                    ? ItemsWrapper.Create(vehicle.trunkItems)
                    : new ItemsWrapper(),
                Tires = vehicle.tires?.Select(c => c.isAlive).ToArray() ?? Array.Empty<bool>(),
                Turrets = vehicleTurret,
                Group = vehicle.lockedGroup.m_SteamID,
                Owner = vehicle.lockedOwner.m_SteamID,
                PaintColor_ = vehicle.PaintColor == Color.clear || vehicle.PaintColor.a == 0 ? new byte[4]{ 0, 0, 0, 0 } : new byte[4] { vehicle.PaintColor.r, vehicle.PaintColor.g, vehicle.PaintColor.b, vehicle.PaintColor.a }
            };

            if (BarricadeManager.tryGetPlant(vehicle.transform, out _, out _, out _, out var region))
            {
                foreach (var barricade in from barricadeDrop in region.drops
                                          where !barricadeDrop.GetServersideData().barricade.isDead
                                          select BarricadeWrapper.Create(barricadeDrop))
                {
                    result.Barricades.Add(barricade);
                }
            }

            if (StructureManager.tryGetRegion(vehicle.transform, out _, out _, out var reg))
            {
                foreach (var structure in from structureDrop in reg.drops
                                          where !structureDrop.GetServersideData().structure.isDead && StructureGetVehicle(structureDrop.GetServersideData().structure.asset.structure.transform) != null
                                          select StructureWrapper.Create(structureDrop))
                {
                    result.Structures.Add(structure);
                }
            }

            return result;
        }
        static Transform StructureGetVehicle(Transform structure)
        {
            Transform transform = structure;
            while((transform = transform.parent) != null)
            {
                if (transform.gameObject.GetComponent<InteractableVehicle>() != null) return transform;
            }
            return null;
        }

        public VehicleAsset GetVehicleAsset()
        {
            return (VehicleAsset) Assets.FindBaseVehicleAssetByGuidOrLegacyId(assetId, Id);
        }

        public InteractableVehicle SpawnVehicle(UnturnedPlayer player)
        {
            // Spawn Vehicle
            Owner = player.CSteamID.m_SteamID;
            Group = player.Player.quests.groupID.m_SteamID;
            return SpawnVehicle(true);
        }

        public InteractableVehicle SpawnVehicle(bool changeBarricadeOwner = false)
        {
            var owner = new CSteamID(Owner);
            var group = new CSteamID(Group);
            // Spawn Vehicle
            InteractableVehicle vehicle;
            if (!PaintColor_.Equals(new byte[] { 0, 0, 0, 0 }))
            {
                Color32 Paint = new Color32(PaintColor_[0], PaintColor_[1], PaintColor_[2], PaintColor_[3]);
                vehicle = VehicleManager.SpawnVehicleV3(GetVehicleAsset(), SkinId, MythicId, RoadPosition, Position.ToVector3(),
    Rotation.ToQuaternion(), false, false, false, false, Fuel, Health, BatteryCharge,
    owner, group, Owner != 0, null, byte.MaxValue, Paint);
            }
            else
            {
                vehicle = VehicleManager.SpawnVehicleV3(GetVehicleAsset(), SkinId, MythicId, RoadPosition, Position.ToVector3(),
                Rotation.ToQuaternion(), false, false, false, false, Fuel, Health, BatteryCharge,
                owner, group, Owner != 0, null, byte.MaxValue);
            }
            if (vehicle != null)
            {

                // Set Tires
                for (var i = 0; i < (Tires?.Length ?? 0); i++)
                    if (Tires != null)
                        if (vehicle.tires.ElementAtOrDefault(i) != null)
                            vehicle.tires[i].isAlive = Tires.ElementAtOrDefault(i);

                vehicle.sendTireAliveMaskUpdate();

                // Spawn Trunk Items
                if (vehicle.trunkItems != null && TrunkItems.Items?.Count != 0)
                    foreach (var item in TrunkItems.Items)
                        vehicle.trunkItems.addItem(item.X, item.Y, item.Rotation, item.Item.ToItem());

                // Spawn Barricades
                if (Barricades.Count != 0)
                    foreach (var barricade in Barricades.Where(barricade => barricade.Id != 0))
                    {
                        if (changeBarricadeOwner)
                        {
                            barricade.Owner = Owner;
                            barricade.Group = Group;
                        }

                        barricade.SpawnBarricade(vehicle.transform);
                    }

                // Set Turrets
                if (vehicle.turrets != null && Turrets.Count == vehicle.turrets.Length)
                {
                    byte index = 0;
                    while (index < vehicle.turrets.Length)
                    {
                        if (vehicle.turrets.ElementAtOrDefault(index) != null && Turrets.ElementAtOrDefault(index) != null)
                            vehicle.turrets[index].state = Turrets[index];
                        index += 1;
                    }
                }
                else
                {
                    byte index = 0;
                    while (index < vehicle.turrets?.Length)
                    {
                        if (vehicle.turrets?.ElementAtOrDefault(index) != null)
                        {
                            var vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, Id);
                            var itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, vehicleAsset.turrets[index].itemID);
                            vehicle.turrets[index].state = itemAsset.getState();
                        }

                        index += 1;
                    }
                }
            }
            return vehicle;
        }
    }
}