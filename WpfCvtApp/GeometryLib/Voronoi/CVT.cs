

using System;
using System.Collections.Generic;
using System.Diagnostics;
using g3;
using GeometryLib.Primitives;
using GeometryLib.RandomEngines;

namespace GeometryLib.Voronoi
{
    public class Cvt
    {
        public Cvt(IReadOnlyCollection<Vertex> polygon, CvtSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentException("Please, define CvtSettings object");
            }
            Polygon = new Polygon2d();
            foreach (Vertex vertex in polygon)
            {
                Polygon.AppendVertex(new Vector2d(vertex.X, vertex.Y));
            }

            Settings = settings;
        }

        public Polygon2d Polygon { get; }
        public CvtSettings Settings { get; }

        protected List<Vertex> _generators = new List<Vertex>();

        public IReadOnlyCollection<Vertex> GetGenerators()
        {
            
            int numberOfGenerators = Settings.NumberOfGenerators;
            int maxNumberOfIterations = Settings.MaxNumberOfIterations;

            IRandom2D random2DSamplingPoints = RandomEngineFactory.Create(Settings.SelectedSamplingMethod);
            var samplingPoints = random2DSamplingPoints.GetRandomNumbers(Settings.NumberOfSamplingPoints, Polygon);
            
            IRandom2D random2DGenerators = RandomEngineFactory.Create(Settings.SelectedSamplingMethod);
            var generators = random2DGenerators.GetRandomNumbers(numberOfGenerators, Polygon);

            //Initialize voronoi areas, for each generator
            var voronoiArea = new List<List<Vector2d>>();
            for (int i = 0; i < numberOfGenerators; i++)
            {
                voronoiArea.Add(new List<Vector2d>());
            }

            for (int kTimes = 0; kTimes < maxNumberOfIterations; kTimes++)
            {
                UpdateVoronoiAreas(generators, samplingPoints, ref voronoiArea);
                
                for (int i = 0; i < voronoiArea.Count; i++)
                {
                    var area = voronoiArea[i];
                    var newGenerator1 = FindMassCenter(area);
                    generators[i] = newGenerator1;

                    //g3.ConvexHull2 areaConvexHull2 = new ConvexHull2(area, 0.1, QueryNumberType.QT_DOUBLE);
                    //var newGenerator = areaConvexHull2.GetHullPolygon().Bounds.Center;
                    //double areaXX = areaConvexHull2.GetHullPolygon().SignedArea;
                    //Trace.WriteLine($"Area of voronoi (id={i}) = {areaXX}");
                }
                double energyFunction = ComputeEnergy(generators, voronoiArea);
                Trace.WriteLine($"Total energy = {energyFunction}");
                Trace.WriteLine($"Step: {kTimes}");
               
            }

            _generators.Clear();
            foreach (Vector2d item in generators)
            {
                _generators.Add(new Vertex(item.x, item.y));
            }

            return _generators;
        }

        private Vector2d FindMassCenter(List<Vector2d> area)
        {
            double xMean = 0.0;
            double yMean = 0.0;
            int totalPoints = area.Count;

            foreach (Vector2d p in area)
            {
                xMean += p.x;
                yMean += p.y;
            }

            xMean /= totalPoints;
            yMean /= totalPoints;

            return new Vector2d(xMean, yMean);
        }

        private double ComputeEnergy(List<Vector2d> generatorList, List<List<Vector2d>> voronoiArea)
        {
            List<double> localEnergies = new List<double>();
            int totalSamplingPoints = 0;

            for (int i = 0; i < generatorList.Count; i++)
            {
                Vector2d generator = generatorList[i];
                double localEnergy = 0.0;
                for (int j = 0; j < voronoiArea[i].Count; j++)
                {
                    ++totalSamplingPoints;
                    double distance = Distance(generator, voronoiArea[i][j]);
                    localEnergy += (distance * distance);
                }
                localEnergies.Add(localEnergy);
            }

            double globalEnergy = 0.0;
            foreach (double energy in localEnergies)
            {
                globalEnergy += energy;
            }
            globalEnergy /= totalSamplingPoints;
            globalEnergy = Math.Log10(globalEnergy);
            return globalEnergy;
        }


        private void UpdateVoronoiAreas(List<Vector2d> generatorList,
            List<Vector2d> samplingPoints,
            ref List<List<Vector2d>> voronoiArea)
        {
            EmptyPoints(voronoiArea);
            for (int i = 0; i < samplingPoints.Count; i++)
            {
                double distance0 = Double.MaxValue;
                int posJ = -1;
                for (int j = 0; j < generatorList.Count; j++)
                {
                    double distance = Distance(generatorList[j], samplingPoints[i]);
                    if (distance < distance0)
                    {
                        distance0 = distance;
                        posJ = j;
                    }
                }
                voronoiArea[posJ].Add(samplingPoints[i]);
            }
        }

        private void EmptyPoints(List<List<Vector2d>> voronoiArea)
        {
            for (int i = 0; i < voronoiArea.Count; i++)
            {
                voronoiArea[i].Clear();
            }
        }

        private double Distance(Vector2d generator, Vector2d samplingPoint)
        {
            var dxx = (generator.x - samplingPoint.x) * (generator.x - samplingPoint.x);
            var dyy = (generator.y - samplingPoint.y) * (generator.y - samplingPoint.y);
            var distance = Math.Sqrt(dxx + dyy);
            return distance;
        }


      

        
    }
}