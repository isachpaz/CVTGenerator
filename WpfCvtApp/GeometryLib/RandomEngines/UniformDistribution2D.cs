using System;
using System.Collections.Generic;
using g3;

namespace GeometryLib.RandomEngines
{
    public class UniformDistribution2D : IRandom2D
    {
        public UniformDistribution2D()
        {
            RandomSamples = new List<Vector2d>();
        }

        public int Seed { get; set; }
        public virtual List<Vector2d> GetRandomNumbers(int numberOfSamples)
        {

            Random xRandom = new Random(Seed);
            Random yRandom = new Random(Seed + 100);

            for (int i = 0; i < numberOfSamples; i++)
            {
                var x = xRandom.NextDouble();
                var y = yRandom.NextDouble();

                Vector2d vertex = new Vector2d(x, y);
                RandomSamples.Add(vertex);
            }
            return RandomSamples;
        }

        public List<Vector2d> GetRandomNumbers(int numberOfSamples, Polygon2d polygon)
        {
            Random xRandom = new Random(Seed);
            Random yRandom = new Random(Seed + 100);

            while (RandomSamples.Count < numberOfSamples)
            {
                var x = xRandom.NextDouble();
                var y = yRandom.NextDouble();

                Vector2d point2D = new Vector2d(x,y);
                var newPoint = Helpers.Transformation.TranslatePointsCoordinationInsideTheBoundingBox(point2D, polygon.Bounds);
                if (Helpers.InOutTest.IsInsidePolygon(newPoint, polygon))
                {
                    this.RandomSamples.Add(newPoint);
                }

            }
            return RandomSamples;
        }

        public List<Vector2d> RandomSamples { get; protected set; }
    }
}