using System;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public struct QuaternionWrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public QuaternionWrapper(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        public QuaternionWrapper(Quaternion quaternion)
        {
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
            W = quaternion.w;
        }

        public static QuaternionWrapper Create(Quaternion quaternion)
        {
            return new QuaternionWrapper(quaternion);
        }

        public Quaternion ToQuaternion() => new(X, Y, Z, W);
    }
}