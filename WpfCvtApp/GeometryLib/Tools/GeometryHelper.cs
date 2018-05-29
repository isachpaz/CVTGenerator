using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using DicomLib.RtData;
using g3;
using GeometryLib.Primitives;

namespace GeometryLib.Tools
{
    public static class GeometryHelper
    {
        const double EquityTolerance = 0.000000001;

        public static bool IsEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) <= EquityTolerance;
        }

        public static List<Vertex> GetOuterContour(IReadOnlyCollection<Contour> contours, 
                                                    int gridResolution)
        {
            List<Vertex> outerVertices = new List<Vertex>();

            var convexHull = GetConvexHull(contours);
            var hullPolygon = convexHull.GetHullPolygon();
            
            ConcurrentBag<BinaryMap> Maps = new ConcurrentBag<BinaryMap>();
            BinaryMapSettings settings = new BinaryMapSettings
            {
                SizeX = gridResolution,
                SizeY = gridResolution,
                BoundingBox = hullPolygon.GetBounds()
            };

            for (int i = 0; i < contours.Count; i++)
            {
                BinaryMap bm = new BinaryMap(settings);
                bm.SetContour(contours.ElementAt(i).ToVector2dList());
                bm.Id = i;
                Maps.Add(bm);
            }
            
            BinaryMap refMap = Maps.FirstOrDefault();
            foreach (BinaryMap map in Maps)
            {
               refMap?.OR(map);
            }
            outerVertices = refMap?.GetBoundary();
            return outerVertices;
        }

        private static ConvexHull2 GetConvexHull(IReadOnlyCollection<Contour> contours)
        {
            List<Vector2d> points = new List<Vector2d>();
            foreach (Contour contour in contours)
            {
                foreach (var point in contour.Points)
                {
                    points.Add(new Vector2d(point.x, point.y));
                }
            }
            ConvexHull2 convexHull2 = new ConvexHull2(points, EquityTolerance, QueryNumberType.QT_DOUBLE);
            return convexHull2;
        }
    }
}
