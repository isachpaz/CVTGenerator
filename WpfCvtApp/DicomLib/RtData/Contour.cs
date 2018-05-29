using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using g3;

namespace DicomLib.RtData
{
    public class Contour
    {
        private List<Vector3d> _points;

        public float ZPosition { get; protected set; }

        public override string ToString()
        {
            return $"{nameof(ZPosition)}: {ZPosition}, {nameof(Points)}: {Points.Count()}";
        }

        public IEnumerable<Vector3d> Points => _points;

        public Contour()
        {
            _points = new List<Vector3d>();
        }

        public Contour(Contour item) : this()
        {
            this._points = item._points;
            this.ZPosition = item.ZPosition;
        }

        public void SetZPosition(float z)
        {
            ZPosition = z;
        }
        public void AddPoint(float x, float y, float z)
        {
            _points.Add(new Vector3d(x, y, z));
        }

        public IReadOnlyCollection<Vector2d> ToVector2dList()
        {
            List<Vector2d> points =  new List<Vector2d>();
            foreach (Vector3d vector3D in this.Points)
            {
                points.Add(new Vector2d(vector3D.x, vector3D.y));
            }
            return points;
        }
    }
}