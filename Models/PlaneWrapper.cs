using System;

namespace RFRocketLibrary.Models
{
    // Original author: John Jiyang Hou
    // Original from: https://www.codeproject.com/Articles/1070593/Point-Inside-D-Convex-Polygon-in-Csharp
    [Serializable]
    public struct PlaneWrapper
    {
        public PlaneWrapper(float a, float b, float c, float d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public PlaneWrapper(Vector3Wrapper vector1, Vector3Wrapper vector2, Vector3Wrapper vector3)
        {
            var v = new Vector3ProjectionWrapper(vector1, vector2);
            var u = new Vector3ProjectionWrapper(vector1, vector3);
            var n = u * v;

            // normal vector		
            A = n.X;
            B = n.Y;
            C = n.Z;				
            D = -(A * vector1.X + B * vector1.Y + C * vector1.Z);
        }

        // Plane Equation: a * x + b * y + c * z + d = 0
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }

        public static PlaneWrapper operator -(PlaneWrapper plane) 
        {
            return new PlaneWrapper(-plane.A, -plane.B, -plane.C, -plane.D);
        }

        public static float operator *(Vector3Wrapper vector, PlaneWrapper plane) 
        {
            return (vector.X * plane.A + vector.Y * plane.B + vector.Z * plane.C + plane.D);
        }
    }
}