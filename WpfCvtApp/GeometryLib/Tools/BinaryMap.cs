using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using g3;
using GeometryLib.Primitives;

namespace GeometryLib.Tools
{
    public class BinaryMap
    {
        public int Id { get; set; }
        public BinaryMapSettings Settings { get; }

        protected int[,] _binaryMap = null;
        protected int[,] _trackingMap = null;
        protected Vertex StartVertex = null;
        protected List<Vertex> BoundaryVertices = new List<Vertex>();
        protected List<Offset> _offsets = new List<Offset>();
        protected List<Vector2d> _points = new List<Vector2d>();
        protected Polygon2d _polyline = new Polygon2d();

        private int _increase_bounding_box = 10;
        public int SizeX { get; }
        public int SizeY { get; }
        public Vector2d Min { get; protected set; }
        public Vector2d Max { get; protected set; }

        
        public BinaryMap(BinaryMapSettings settings)
        {
            Settings = settings;
            SizeX = Settings.SizeX;
            SizeY = Settings.SizeY;
            IncreaseGridSize(_increase_bounding_box);

            _binaryMap = new int[SizeX, SizeY];
            _trackingMap = new int[SizeX, SizeY];
            Initilize();
        }

        private void IncreaseGridSize(int increaseBoundingBox)
        {
            Min = Settings.BoundingBox.Min;
            Max = Settings.BoundingBox.Max + this._increase_bounding_box;
        }

        private void Initilize()
        {
            Parallel.For(0, SizeX,
                i =>
                {
                    for (int j = 0; j < SizeY; j++)
                    {
                        _binaryMap[i, j] = 0;
                        _trackingMap[i, j] = 0;
                    }
                });
        }
        
        public void SetContour(IReadOnlyCollection<Vector2d> contourPoints)
        {
            double stepX = Math.Abs((Max.x - Min.x) / SizeX);
            double stepY = Math.Abs((Max.y - Min.y) / SizeY);

            foreach (var item in contourPoints)
            {
                this._polyline.AppendVertex(new Vector2d(item.x, item.y));
            }

            for (int i = 0; i < SizeX; i++)
            {
                double x = Min.x + i * stepX;
                for (int j = 0; j < SizeY; j++)
                {
                    double y = Min.y + j * stepY;
                    if (this._polyline.Contains(new Vector2d(x, y)))
                    {
                        this._binaryMap[i, j] = 1;
                    }
                    else
                    {
                        this._binaryMap[i, j] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// OR operation on Binarymaps
        /// </summary>
        /// <param name="map"></param>
        public void OR(BinaryMap map)
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    _binaryMap[i, j] = _binaryMap[i, j] | map._binaryMap[i, j];
                }
            }
        }

        public void WriteToFile(string cTmpBinaryTxt)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(cTmpBinaryTxt);

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    file.Write(_binaryMap[i, j]);
                }
                file.WriteLine(" ");
            }
            file.Close();
        }

        public List<Vertex> GetBoundary()
        {
            BinaryMap bm = new BinaryMap(this.Settings);
            var result = new List<Vertex>();

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    int value = Get4Neighbours(i, j);
                    bm.SetBinaryValue(i, j, value);
                }
            }

            // bm.WriteToFile(@"c:\temp\contour.txt");

            GetContinousBoundaryPoints(bm);

            double stepX = Math.Abs((Max.x - Min.x) / SizeX);
            double stepY = Math.Abs((Max.y - Min.y) / SizeY);

            foreach (Vertex vertex in BoundaryVertices)
            {
                double x = Min.x + stepX * vertex.I;
                double y = Min.y + stepY * vertex.J;
                result.Add(new Vertex(x, y));
            }

            return result;
        }


        private void SetBinaryValue(int i, int j, int value)
        {
            this._binaryMap[i, j] = value;
        }

        private int Get4Neighbours(int i, int j)
        {
            int value0 = GetValue(i, j);
            int value1 = GetValue(i - 1, j);
            int value2 = GetValue(i + 1, j);
            int value3 = GetValue(i, j - 1);
            int value4 = GetValue(i, j + 1);

            return value0 + value1 + value2 + value3 + value4;
        }

        public int GetValue(int i, int j)
        {
            try
            {
                if (i < 0 || i >= SizeX)
                {
                    return 0;
                }

                if (j < 0 || j >= SizeY)
                {
                    return 0;
                }
                return this._binaryMap[i, j];
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                throw;
            }
        }


        void GetContinousBoundaryPoints(BinaryMap bm)
        {
            //  +----------+----------+----------+
            //  |          |          |          |
            //  |(x-1,y-1) | (x,y-1)  |(x+1,y-1) |
            //  +----------+----------+----------+
            //  |(x-1,y)   |  (x,y)   |(x+1,y)   |
            //  |          |          |          |
            //  +----------+----------+----------+
            //  |          | (x,y+1)  |(x+1,y+1) |
            //  |(x-1,y+1) |          |          |
            //  +----------+----------+----------+

            this._offsets = new List<Offset>
            {
                new Offset() {DeltaI = -1, DeltaJ = 1},
                new Offset() {DeltaI = -1, DeltaJ = 0},
                new Offset() {DeltaI = -1, DeltaJ = -1},
                new Offset() {DeltaI = 0, DeltaJ = -1},
                new Offset() {DeltaI = 0, DeltaJ = 1},
                new Offset() {DeltaI = 1, DeltaJ = 1},
                new Offset() {DeltaI = 1, DeltaJ = 0},
                new Offset() {DeltaI = 1, DeltaJ = -1},
            };

            StartVertex = bm.FindStart();
            BoundaryVertices.Add(StartVertex);
            _trackingMap[StartVertex.I, StartVertex.J] = 1;
            Vertex tmp = null;
            bool bStop = false;
            while (bStop == false)
            {
                if (tmp == null)
                {
                    tmp = StartVertex;
                }
                var vertex = SearchForNextNeighbour(tmp, bm);

                if (vertex.I == 0 && vertex.J == 0)
                {
                    bStop = true;
                    BoundaryVertices.Add(StartVertex);
                }
                tmp = vertex;
            }
        }

        private Vertex SearchForNextNeighbour(Vertex currentVertex, BinaryMap bm)
        {

            try
            {
                int i0 = currentVertex.I;
                int j0 = currentVertex.J;

                foreach (Offset offset in this._offsets)
                {
                    var value = bm.GetValue(i0 + offset.DeltaI, j0 + offset.DeltaJ);
                    if (value == 1 || value == 2)
                    {
                        if (_trackingMap[i0 + offset.DeltaI, j0 + offset.DeltaJ] == 0)
                        {
                            _trackingMap[i0 + offset.DeltaI, j0 + offset.DeltaJ] = 1;
                        }
                        else
                        {
                            continue;
                        }

                        var vertex = new Vertex(i0 + offset.DeltaI, j0 + offset.DeltaJ);
                        //if (BoundaryVertices.Contains(vertex))
                        //    continue;
                        //Trace.WriteLine($"BPoint = ({i0 + offset.DeltaI}, {j0 + offset.DeltaJ}");
                        BoundaryVertices.Add(vertex);

                        return vertex;
                    }
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return new Vertex(0, 0);
        }

        private Vertex FindStart()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    var value = GetValue(i, j);
                    if (value == 1 || value == 2)
                    {
                        return new Vertex(i, j);
                    }
                }
            }
            return new Vertex(0, 0);
        }
    }
}