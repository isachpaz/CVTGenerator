using System;

namespace GeometryLib.RandomEngines
{
    public class RandomEngineFactory
    {
        public static IRandom2D Create(RandomEngine randomEngine)
        {
            switch (randomEngine)
            {
                case RandomEngine.HALTONSEQUENCE:
                    return new HaltonSequence2D();
                case RandomEngine.UNIFORMDISTRIBUTION:
                    return new UniformDistribution2D();
                default:
                    throw new ArgumentException("You should never received this exception (RandomEngineFactory)");
            }
        }
    }
}