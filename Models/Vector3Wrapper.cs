using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public struct Vector3Wrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3Wrapper(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Wrapper(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public static implicit operator Vector3Wrapper(Vector3 vector)
        {
            return new Vector3Wrapper(vector.x, vector.y, vector.z);
        }

        public static implicit operator Vector3(Vector3Wrapper vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Vector3Wrapper operator +(Vector3Wrapper vector1, Vector3Wrapper vector2)
        {
            return new Vector3Wrapper(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        }

        public static Vector3Wrapper Create(Vector3 vector)
        {
            return new Vector3Wrapper(vector);
        }

        public Vector3 ToVector3() => new(X, Y, Z);

        public override string ToString()
        {
            return string.Format("{0:F1}, {1:F1}, {2:F1}", new object[]
            {
                X, Y, Z
            });
        }

        public static bool TryParse(string? s, out Vector3Wrapper result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(s))
                return false;
            
            s = s.Replace(" ", string.Empty);
            var split = s.Split(',');
            if (!float.TryParse(split.ElementAtOrDefault(0), out var x))
                return false;
            
            if (!float.TryParse(split.ElementAtOrDefault(1), out var y))
                return false;
            
            if (!float.TryParse(split.ElementAtOrDefault(2), out var z))
                return false;

            result = new Vector3Wrapper(x, y, z);
            return true;
        }
    }
    
    public class Vector3WrapperConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            Vector3Wrapper.TryParse(reader.Value?.ToString(), out var result);
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector3Wrapper).IsAssignableFrom(objectType);
        }
    }
}