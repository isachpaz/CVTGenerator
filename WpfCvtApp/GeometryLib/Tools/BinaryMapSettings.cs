using g3;

namespace GeometryLib.Tools
{
    public class BinaryMapSettings
    {
        public BinaryMapSettings()
        {
            SizeX = SizeY = 1000; // Default values
        }

        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public AxisAlignedBox2d BoundingBox { get; set; }
    }
}