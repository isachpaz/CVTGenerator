using System.Collections.Generic;
using GeometryLib.RandomEngines;

namespace GeometryLib.Voronoi
{
    public class CvtSettings
    {
        public int NumberOfGenerators { get; set; }
        public int NumberOfSamplingPoints { get; set; }
        public RandomEngine SelectedSamplingMethod { get; set; }
        public int MaxNumberOfIterations { get; set; }

        public CvtSettings()
        {
            NumberOfGenerators = 10;
            NumberOfSamplingPoints = 10000;
            SelectedSamplingMethod = RandomEngine.HALTONSEQUENCE;
            MaxNumberOfIterations = 100;
        }
    }
}