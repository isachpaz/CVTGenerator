using System.Collections.Generic;
using GeometryLib.Primitives;
using Prism.Events;

namespace WpfCvtApp.Infrastructure.Events
{
    public class GeneratorsUpdatedEvent : PubSubEvent<GeneratorsUpdatedEventArg>
    {
        
    }

    public class GeneratorsUpdatedEventArg
    {
        public GeneratorsUpdatedEventArg(IReadOnlyCollection<Vertex> generators)
        {
            Generators = generators;
        }

        public IReadOnlyCollection<Vertex> Generators { get; }
    }
}