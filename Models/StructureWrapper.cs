using System;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class StructureWrapper
    {
        public ushort Id { get; set; }
        public uint InstanceId { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public ushort Health { get; set; }
        public Vector3Wrapper Position { get; set; }
        public Vector3Wrapper Rotation { get; set; }

        public StructureWrapper()
        {
        }

        public StructureWrapper(ushort id, uint instanceId, ulong owner, ulong @group, ushort health,
            Vector3Wrapper position, Vector3Wrapper rotation)
        {
            Id = id;
            InstanceId = instanceId;
            Owner = owner;
            Group = @group;
            Health = health;
            Position = position;
            Rotation = rotation;
        }

        public static StructureWrapper Create(StructureDrop drop)
        {
            var data = drop.GetServersideData();
            return Create(drop, data);
        }

        public static StructureWrapper Create(StructureDrop drop, StructureData data)
        {
            var structure = new StructureWrapper
            {
                Id = data.structure.asset.id,
                InstanceId = drop.instanceID,
                Position = new Vector3Wrapper(data.point),
                Rotation = new Vector3Wrapper(drop.model.transform.localEulerAngles),
                Owner = data.owner,
                Group = data.group,
                Health = data.structure.health,
            };

            return structure;
        }

        public ItemStructureAsset GetItemStructureAsset()
        {
            return (ItemStructureAsset) Assets.find(EAssetType.ITEM, Id);
        }

        public bool SpawnStructure()
        {

            StructureManager.onStructureSpawned += OnStructureSpawned;
            var structureAsset = GetItemStructureAsset();
            var structure = new Structure(structureAsset, structureAsset.health);
            var flag = StructureManager.dropReplicatedStructure(structure, Position.ToVector3(),
                Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group);
            StructureManager.onStructureSpawned -= OnStructureSpawned;
            return flag;
        }

        public bool SpawnStructure(UnturnedPlayer player)
        {
            Owner = player.CSteamID.m_SteamID;
            Group = player.Player.quests.groupID.m_SteamID;
            return SpawnStructure();
        }

        private void OnStructureSpawned(StructureRegion region, StructureDrop drop)
        {
            var damage = drop.asset.health - Health;
            if (damage != 0)
                StructureManager.damage(drop.model, Vector3.zero, damage, 1, false);
        }
    }
}