using System;

namespace RFRocketLibrary.Models
{
    // Original author: John Jiyang Hou
    // Original from: https://www.codeproject.com/Articles/1070593/Point-Inside-D-Convex-Polygon-in-Csharp
    [Serializable]
    public struct Vector3ProjectionWrapper
    {
        public Vector3Wrapper StartPoint { get; set; } // vector begin point
        public Vector3Wrapper EndPoint { get; set; } // vector end point

        public float X => EndPoint.X - StartPoint.X; // vector x axis projection value
        public float Y => EndPoint.Y - StartPoint.Y; // vector y axis projection value
        public float Z => EndPoint.Z - StartPoint.Z; // vector z axis projection value 

        public Vector3ProjectionWrapper(Vector3Wrapper startPoint, Vector3Wrapper endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public static Vector3ProjectionWrapper operator *(Vector3ProjectionWrapper u, Vector3ProjectionWrapper v)
        {
            var x = u.Y * v.Z - u.Z * v.Y;
            var y = u.Z * v.X - u.X * v.Z;
            var z = u.X * v.Y - u.Y * v.X;

            var p0 = v.StartPoint;
            var p1 = p0 + new Vector3Wrapper(x, y, z);

            return new Vector3ProjectionWrapper(p0, p1);
        }
    }
}