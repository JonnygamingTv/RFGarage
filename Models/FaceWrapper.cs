using System;
using System.Collections.Generic;

namespace RFRocketLibrary.Models
{
    // Original author: John Jiyang Hou
    // Original from: https://www.codeproject.com/Articles/1070593/Point-Inside-D-Convex-Polygon-in-Csharp
    [Serializable]
    public struct FaceWrapper
    {
        // Points in one face of the polygon
        public List<Vector3Wrapper> Points { get; set; }

        public FaceWrapper(IEnumerable<Vector3Wrapper> points)
        {
            Points = new List<Vector3Wrapper>();
            Points.AddRange(points);
        }
    }
}