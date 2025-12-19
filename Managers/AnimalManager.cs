using System;
using System.Linq;
using System.Reflection;
using Rocket.Core.Logging;
using SDG.Unturned;

namespace RFRocketLibrary.DatabaseManagers
{
    public static class AnimalManager
    {
        #region Methods

        public static bool Load()
        {
            if (!LevelSavedata.fileExists("/Animals.dat")) 
                return true;
            var river = LevelSavedata.openRiver("/Animals.dat", true);
            var ver = river.readByte();

            var packsCount = river.readUInt16();
            var animalManagerPacks = SDG.Unturned.AnimalManager.packs.Count;
            var packIndex = 0;

            var loadedAnimals = 0;

            for (var i = 0; i < packsCount; i++)
            {
                var createdNewPack = i >= animalManagerPacks;
                PackInfo selectedPack = createdNewPack ? AddNewPack() : SDG.Unturned.AnimalManager.packs[packIndex];

                var animalsCount = river.readUInt16();
                for (var j = 0; j < animalsCount; j++)
                {
                    var id = river.readUInt16();
                    if (id == 0)
                    {
                        //null animal detects custom animal pack without auto respawning
                        if (j == 0 && !createdNewPack) selectedPack = AddNewPack();
                        selectedPack.animals.Add(null);
                        continue;
                    }

                    var position = river.readSingleVector3();
                    var angle = river.readSingle();

                    if (animalsCount > selectedPack.animals.Count)
                    {
                        try
                        {
                            var aManager = typeof(SDG.Unturned.AnimalManager)
                                .GetField("manager", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as SDG.Unturned.AnimalManager;
                            var animal = typeof(SDG.Unturned.AnimalManager)
                                .GetMethod("addAnimal", BindingFlags.NonPublic | BindingFlags.Instance)
                                ?.Invoke(aManager, new object[] {id, position, angle, false}) as Animal;
                            if (animal != null)
                            {
                                animal.pack = selectedPack;
                                selectedPack.animals.Add(animal);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                            return false;
                        }
                    }
                    else
                    {
                        var animal = selectedPack.animals[j];
                        animal.id = id;
                        animal.transform.position = position;
                        animal.transform.rotation = UnityEngine.Quaternion.Euler(0.0f, angle, 0.0f);
                    }
                }

                if (!createdNewPack) packIndex++;

                loadedAnimals += animalsCount;
            }

            river.closeRiver();
            Logger.Log($"Loaded {packsCount} packs with {loadedAnimals} animals in total");

            return true;
        }

        public static void Save()
        {
            var river = LevelSavedata.openRiver("/Animals.dat", false);
            river.writeByte(SAVEDATA_VERSION);
            var packsToSave = SDG.Unturned.AnimalManager.packs
                .Where(pack => pack.animals.Any(a => a != null && !a.isDead)).ToList();
            river.writeUInt16((ushort) packsToSave.Count);

            foreach (var pack in packsToSave)
            {
                var aliveAnimals = pack.animals.Where((a) => a != null && !a.isDead).ToList();
                if (aliveAnimals.Count == 0) continue;
                if (pack.animals.Any(a => a == null)) aliveAnimals.Insert(0, null);

                river.writeUInt16((ushort) aliveAnimals.Count);
                foreach (var animal in aliveAnimals)
                {
                    if (animal == null) river.writeUInt16(0);
                    else
                    {
                        river.writeUInt16(animal.id);
                        river.writeSingleVector3(animal.transform.position);
                        river.writeSingle(animal.transform.rotation.eulerAngles.y);
                    }
                }
            }

            river.closeRiver();
            Logger.Log(
                $"Saved {packsToSave.Count} packs with {SDG.Unturned.AnimalManager.animals.Count} animals in total");
        }

        #endregion

        private const byte SAVEDATA_VERSION = 2;

        private static PackInfo NewPack()
        {
            var pack = new PackInfo();
            pack.spawns.Add(SDG.Unturned.AnimalManager
                .packs[UnityEngine.Random.Range(0, SDG.Unturned.AnimalManager.packs.Count)].spawns.First());
            return pack;
        }

        private static PackInfo AddNewPack()
        {
            var pack = NewPack();
            SDG.Unturned.AnimalManager.packs.Add(pack);
            return pack;
        }
    }
}