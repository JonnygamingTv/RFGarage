using System;
using RFRocketLibrary.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
#pragma warning disable 612

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class BarricadeWrapper
    {
        public ushort Id { get; set; }
        public uint InstanceId { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public ushort Health { get; set; }
        public byte[] State { get; set; } = Array.Empty<byte>();
        public Vector3Wrapper Position { get; set; }
        public Vector3Wrapper Rotation { get; set; }

        public BarricadeWrapper()
        {
        }

        public BarricadeWrapper(ushort id, uint instanceId, ulong owner, ulong group, ushort health, byte[] state,
            Vector3Wrapper position, Vector3Wrapper rotation)
        {
            Id = id;
            InstanceId = instanceId;
            Owner = owner;
            Group = group;
            Health = health;
            State = state;
            Position = position;
            Rotation = rotation;
        }

        public static BarricadeWrapper Create(BarricadeDrop drop)
        {
            var data = drop.GetServersideData();
            return Create(drop, data);
        }

        public static BarricadeWrapper Create(BarricadeDrop drop, BarricadeData data)
        {
            return new BarricadeWrapper(data.barricade.asset.id, drop.instanceID, data.owner, data.group,
                data.barricade.health, data.barricade.state, new Vector3Wrapper(data.point),
                new Vector3Wrapper(drop.model.transform.localEulerAngles));
        }

        public ItemBarricadeAsset GetItemBarricadeAsset()
        {
            return (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, Id);
        }

        public Transform SpawnBarricade(Transform? hit = null)
        {
            var barricadeAsset = GetItemBarricadeAsset();
            var barricade = new Barricade(barricadeAsset, barricadeAsset.health, State);
            BarricadeUtil.ChangeOwnerAndGroup(barricade.asset.build, Owner, Group, ref barricade.state);
            
            BarricadeManager.onBarricadeSpawned += OnBarricadeSpawned;
            var spawnedBarricade = hit != null
                ? BarricadeManager.dropPlantedBarricade(hit, barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group)
                : BarricadeManager.dropNonPlantedBarricade(barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group);
            BarricadeManager.onBarricadeSpawned -= OnBarricadeSpawned;
            
            // BarricadeManager.updateReplicatedState(spawnedBarricade, barricade.state, barricade.state.Length);
            // BarricadeManager.changeOwnerAndGroup(spawnedBarricade, Owner, Group);
            return spawnedBarricade;
        }

        public Transform SpawnBarricade(UnturnedPlayer player, Transform? hit = null)
        {
            Owner = player.CSteamID.m_SteamID;
            Group = player.Player.quests.groupID.m_SteamID;
            return SpawnBarricade(hit);
        }

        private void OnBarricadeSpawned(BarricadeRegion region, BarricadeDrop drop)
        {
            var damage = drop.asset.health - Health;
            if (damage != 0)
                BarricadeManager.damage(drop.model, damage, 1, false);
        }
    }
}