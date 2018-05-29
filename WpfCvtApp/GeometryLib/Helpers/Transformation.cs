using g3;

namespace GeometryLib.Helpers
{
    public class Transformation
    {
        public static Vector2d TranslatePointsCoordinationInsideTheBoundingBox(Vector2d point, AxisAlignedBox2d bounds)
        {
            Vector2d tmpPoint = new Vector2d();
            tmpPoint.x = bounds.Min.x + (bounds.Max.x - bounds.Min.x) * point.x;
            tmpPoint.y = bounds.Min.y + (bounds.Max.y - bounds.Min.y) * point.y;
            return tmpPoint;
        }

    }
}