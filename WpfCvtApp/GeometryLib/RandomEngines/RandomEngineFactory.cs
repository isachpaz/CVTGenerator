using System;

namespace GeometryLib.RandomEngines
{
    public class RandomEngineFactory
    {
        public static IRandom2D Create(RandomEngineType randomEngineType)
        {
            switch (randomEngineType)
            {
                case RandomEngineType.HALTONSEQUENCE:
                    return new HaltonSequence2D();
                case RandomEngineType.UNIFORMDISTRIBUTION:
                    return new UniformDistribution2D();
                default:
                    throw new ArgumentException("You should never received this exception (RandomEngineFactory)");
            }
        }
    }
}