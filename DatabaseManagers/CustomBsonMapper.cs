using LiteDB;
using LiteDB.Async;
using RFGarage.Models;
using System;

namespace RFGarage.DatabaseManagers
{
    public class CustomBsonMapper : BsonMapper
    {
        public CustomBsonMapper()
        {
            RegisterTypeMapper();
        }

        private void RegisterTypeMapper()
        {
            /*
            // Register a custom mapper for your PlayerGarage type
            RegisterType<PlayerGarage>
            (
                serialize: obj =>
                {
                    var doc = new BsonDocument
                    {
                        ["SteamId"] = obj.SteamId
                    // Add other properties here
                };

                // Handle nullable properties
                if (obj.Color)
                        doc["SomeNullableProperty"] = obj.SomeNullableProperty.Value;
                    else
                        doc["SomeNullableProperty"] = BsonValue.Null;

                    return doc;
                },
                deserialize: bson =>
                {
                    var obj = new PlayerGarage
                    {
                        SteamId = bson["SteamId"].AsUInt64
                    // Initialize other properties here
                };

                    if (!bson["SomeNullableProperty"].IsNull)
                        obj.SomeNullableProperty = bson["SomeNullableProperty"].AsInt32;

                    return obj;
                }
            );
            */
        }
    }
}