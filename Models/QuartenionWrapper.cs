using System;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    //Typo lol
    [Serializable]
    public struct QuartenionWrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public QuartenionWrapper(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        public QuartenionWrapper(Quaternion quaternion)
        {
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
            W = quaternion.w;
        }

        public static QuartenionWrapper Create(Quaternion quaternion)
        {
            return new QuartenionWrapper(quaternion);
        }

        public Quaternion ToQuaternion() => new(X, Y, Z, W);
    }
}