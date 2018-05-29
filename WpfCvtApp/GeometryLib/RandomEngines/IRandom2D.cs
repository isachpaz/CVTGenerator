using System.Collections.Generic;
using DicomLib.RtData;
using g3;

namespace GeometryLib.RandomEngines
{
    public interface IRandom2D
    {
        int Seed { get; set; }
        List<Vector2d> GetRandomNumbers(int numberOfSamples);

        List<Vector2d> GetRandomNumbers(int numberOfSamples, Polygon2d polygon);
    }
}