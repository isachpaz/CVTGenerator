using System.Collections.Generic;
using System.Windows.Media;

namespace DicomLib.RtData
{
    public class Structure
    {
        private readonly List<Contour> _contourCollection;
        public string Name { get; }
        public int RoiNumber { get; }

        public IEnumerable<Contour> ContourCollection => _contourCollection;

        public IReadOnlyCollection<Contour> Contours => _contourCollection;

        public Color Color { get; set; }

        public Structure(string name, int roiNumber)
        {
            Name = name;
            RoiNumber = roiNumber;
            _contourCollection = new List<Contour>();
        }

        public Structure(string name, int roiNumber, Color color) : this(name, roiNumber)
        {
            Color = color;
        }

        public void AddContour(Contour contour)
        {
            _contourCollection.Add(contour);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(RoiNumber)}: {RoiNumber}";
        }
    }
}