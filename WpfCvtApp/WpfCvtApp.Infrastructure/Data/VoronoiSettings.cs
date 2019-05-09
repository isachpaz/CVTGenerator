using System;
using System.Collections.Generic;
using System.ComponentModel;
using GeometryLib.RandomEngines;

namespace WpfCvtApp.Infrastructure.Data
{
    public class VoronoiSettings
    {
        public int NumberOfGenerators { get; set; }
        public int NumberOfSamplingPoints { get; set; }

        public List<string> SamplingMethods { get; set; }

        public RandomEngineType SelectedSamplingMethod { get; set; }

        public VoronoiSettings()
        {
            SamplingMethods = new List<string>();
            Initialization();
        }

        private void Initialization()
        {
            NumberOfGenerators = 20;
            NumberOfSamplingPoints = 10000;
            SelectedSamplingMethod = RandomEngineType.HALTONSEQUENCE;
            SamplingMethods.Clear();
            foreach (var item in Enum.GetNames(typeof(RandomEngineType)) )
            {
                SamplingMethods.Add(item);
            }
        }
    }
}