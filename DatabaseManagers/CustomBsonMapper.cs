using LiteDB;
using RFGarage.Models;
using RFRocketLibrary.Models;
using System;
using UnityEngine;

namespace RFGarage.DatabaseManagers
{
    public class CustomBsonMapper : BsonMapper
    {
        public CustomBsonMapper()
        {
            /*
            RegisterType<VehicleWrapper>
            (
                serialize: obj =>
                {
                    var doc = new BsonDocument
                    {
                        ["Id"] = obj.Id,
                        ["InstanceId"] = obj.InstanceId,
                        ["SkinId"] = obj.SkinId,
                        ["MythicId"] = obj.MythicId,
                        ["RoadPosition"] = obj.RoadPosition,
                        ["Health"] = obj.Health,
                        ["Fuel"] = obj.Fuel,
                        ["BatteryCharge"] = obj.BatteryCharge,
                        ["Owner"] = obj.Owner,
                        ["Group"] = obj.Group,
                        ["Tires"] = new BsonArray(obj.Tires),
                        ["Turrets"] = new BsonArray(obj.Turrets.Select(t => new BsonValue(t))),
                        ["TrunkItems"] = Serialize(obj.TrunkItems),
                        ["Barricades"] = new BsonArray(obj.Barricades.Select(b => Serialize(b))),
                        ["Position"] = Serialize(obj.Position),
                        ["Rotation"] = Serialize(obj.Rotation),
                        ["PaintColor"] = Color32ToString(obj.PaintColor)
                    };
                    return doc;
                },
                deserialize: bson =>
                {
                    return new VehicleWrapper
                    (
                        id: bson["Id"].AsUInt16,
                        instanceId: bson["InstanceId"].AsUInt32,
                        skinId: bson["SkinId"].AsUInt16,
                        mythicId: bson["MythicId"].AsUInt16,
                        roadPosition: bson["RoadPosition"].AsFloat,
                        health: bson["Health"].AsUInt16,
                        fuel: bson["Fuel"].AsUInt16,
                        batteryCharge: bson["BatteryCharge"].AsUInt16,
                        owner: bson["Owner"].AsUInt64,
                        @group: bson["Group"].AsUInt64,
                        tires: bson["Tires"].AsArray.Select(t => t.AsBoolean).ToArray(),
                        turrets: bson["Turrets"].AsArray.Select(t => t.AsBinary).ToList(),
                        trunkItems: Deserialize<ItemsWrapper>(bson["TrunkItems"]),
                        barricades: bson["Barricades"].AsArray.Select(b => Deserialize<BarricadeWrapper>(b)).ToList(),
                        position: Deserialize<Vector3Wrapper>(bson["Position"]),
                        rotation: Deserialize<QuaternionWrapper>(bson["Rotation"]),
                        paintcolor: Color32FromString(bson["PaintColor"].AsString)
                    );
                }
            );
            */
        }

        private string Color32ToString(Color32 color)
        {
            return $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
        }

        private Color32 Color32FromString(string colorString)
        {
            if (ColorUtility.TryParseHtmlString(colorString, out Color color))
            {
                return (Color32)color;
            }
            return new Color32(255, 255, 255, 255); // Default to white if parsing fails
        }
    }
}