using System.Collections.Generic;
using RFGarage.Enums;
using RFGarage.Models;
using Rocket.API;

namespace RFGarage
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool SafeDb;
        public bool Enabled;
        public EDatabase Database;
        public Newtonsoft.Json.Formatting JsonFormatting;
        public string MySqlConnectionString;
        public string MessageColor;
        public string MessageIconUrl;
        public bool AutoClearDestroyedVehicles;
        public bool AutoAddOnDrown;
        public bool AutoGarageOnLeave_IgnoreMaxStorage;
        public string AutoAddOnDrownPermission;
        public float VehicleAddDist;
        public float AutoGarageOnLeave;
        public string Permission_Ignore_AutoGarageOnLeave;
        public bool AllowTrain;
        public int DefaultGarageSlot;
        public string GarageSlotPermissionPrefix;
        public List<Blacklist> Blacklists;
        
        public void LoadDefaults()
        {
            SafeDb = false;
            Enabled = true;
            Database = EDatabase.LITEDB;
            JsonFormatting = Newtonsoft.Json.Formatting.Indented;
            MySqlConnectionString = "SERVER=/var/run/mysqld/mysqld.sock;Protocol=unix;DATABASE=unturned;UID=root;PASSWORD=123456;PORT=3306;TABLENAME=rfgarage;";
            MessageColor = "magenta";
            MessageIconUrl = "https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/Announcer.png";
            AutoClearDestroyedVehicles = true;
            AutoAddOnDrown = true;
            AutoAddOnDrownPermission = "garagedrown";
            AutoGarageOnLeave_IgnoreMaxStorage = true;
            VehicleAddDist = float.MaxValue;
            AutoGarageOnLeave = -1;
            Permission_Ignore_AutoGarageOnLeave = "noautogarage";
            AllowTrain = false;
            DefaultGarageSlot = 5;
            GarageSlotPermissionPrefix = "garageslot";
            Blacklists = new List<Blacklist>
            {
                new Blacklist
                {
                    Type = EBlacklistType.BARRICADE,
                    BypassPermission = "garagebypass.barricade.example",
                    IdList = new List<ushort> {1, 2}
                },
                new Blacklist
                {
                    Type = EBlacklistType.ITEM,
                    BypassPermission = "garagebypass.item.example",
                    IdList = new List<ushort> {1, 2}
                },
                new Blacklist
                {
                    Type = EBlacklistType.VEHICLE,
                    BypassPermission = "garagebypass.vehicle.example",
                    IdList = new List<ushort> {1, 2}
                },
                new Blacklist
                {
                    Type = EBlacklistType.BARRICADE,
                    BypassPermission = "garagebypass.barricade.example2",
                    IdList = new List<ushort> {3, 4}
                },
                new Blacklist
                {
                    Type = EBlacklistType.ITEM,
                    BypassPermission = "garagebypass.item.example2",
                    IdList = new List<ushort> {3, 4}
                },
                new Blacklist
                {
                    Type = EBlacklistType.VEHICLE,
                    BypassPermission = "garagebypass.vehicle.example2",
                    IdList = new List<ushort> {3, 4}
                },
            };
        }
    }
}