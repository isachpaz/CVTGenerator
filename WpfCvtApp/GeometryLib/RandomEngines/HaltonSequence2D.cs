using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Numerics;
using System.Threading;
using DicomLib.RtData;
using g3;

namespace GeometryLib.RandomEngines
{
    public class HaltonSequence2D : IRandom2D
    {
        public HaltonSequence2D()
        {
            RandomSamples = new List<Vector2d>();
        }

        public int Seed { get; set; }
        public virtual List<Vector2d> GetRandomNumbers(int numberOfSamples)
        {
            this.Reset();
            Vector3 position = Vector3.Zero;
            for (int i = 0; i < numberOfSamples; i++)
            {
                this.Increment();
                this.RandomSamples.Add(new Vector2d(m_CurrentPos.X, m_CurrentPos.Y));
            }
            return RandomSamples;
        }

        public List<Vector2d> GetRandomNumbers(int numberOfSamples, Polygon2d polygon)
        {
            this.Reset();
            while (RandomSamples.Count < numberOfSamples)
            {
                this.Increment();
                Vector2d point2D = new Vector2d(m_CurrentPos.X, m_CurrentPos.Y);
                var newPoint = Helpers.Transformation.TranslatePointsCoordinationInsideTheBoundingBox(point2D, polygon.Bounds);
                if (Helpers.InOutTest.IsInsidePolygon(newPoint, polygon))
                {
                    this.RandomSamples.Add(newPoint);
                }
                
            }
            return RandomSamples;
        }

      
       
        public List<Vector2d> RandomSamples { get; protected set; }

        protected Vector3 m_CurrentPos = new Vector3(0.0f, 0.0f, 0.0f);
        long m_Base2 = 0;
        long m_Base3 = 0;
        long m_Base5 = 0;

        protected long Increment()
        {
            float fOneOver3 = 1.0f / 3.0f;
            float fOneOver5 = 1.0f / 5.0f;

            long oldBase2 = m_Base2;
            m_Base2++;
            long diff = m_Base2 ^ oldBase2;

            float s = 0.5f;

            do
            {
                if ((oldBase2 & 1) == 1)
                    m_CurrentPos.X -= s;
                else
                    m_CurrentPos.X += s;

                s *= 0.5f;

                diff = diff >> 1;
                oldBase2 = oldBase2 >> 1;
            }
            while (diff > 0);

            long bitmask = 0x3;
            long bitadd = 0x1;
            s = fOneOver3;

            m_Base3++;

            while (true)
            {
                if ((m_Base3 & bitmask) == bitmask)
                {
                    m_Base3 += bitadd;
                    m_CurrentPos.Y -= 2 * s;

                    bitmask = bitmask << 2;
                    bitadd = bitadd << 2;

                    s *= fOneOver3;
                }
                else
                {
                    m_CurrentPos.Y += s;
                    break;
                }
            };
            bitmask = 0x7;
            bitadd = 0x3;
            long dmax = 0x5;

            s = fOneOver5;

            m_Base5++;

            while (true)
            {
                if ((m_Base5 & bitmask) == dmax)
                {
                    m_Base5 += bitadd;
                    m_CurrentPos.Z -= 4 * s;

                    bitmask = bitmask << 3;
                    dmax = dmax << 3;
                    bitadd = bitadd << 3;

                    s *= fOneOver5;
                }
                else
                {
                    m_CurrentPos.Z += s;
                    break;
                }
            };

            return m_Base2;
        }

        protected void Reset()
        {
            m_CurrentPos.X = 0.0f;
            m_CurrentPos.Y = 0.0f;
            m_CurrentPos.Z = 0.0f;
            m_Base2 = 0;
            m_Base3 = 0;
            m_Base5 = 0;
        }

    }
}