using g3;

namespace GeometryLib.Helpers
{
    public class InOutTest
    {
        public static bool IsInsidePolygon(Vector2d point, Polygon2d polygon)
        {
            return polygon.Contains(point);
        }
    }
}