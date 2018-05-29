using System.Collections.Generic;
using GeometryLib;
using GeometryLib.Primitives;
using Prism.Events;

namespace WpfCvtApp.Infrastructure.Events
{
    public class ExternalContourUpdatedEvent : PubSubEvent<ExternalContourUpdatedEventArg>
    {
        
    }

    public class ExternalContourUpdatedEventArg
    {
        public List<Vertex> ExternalContour { get; protected set; }

        public ExternalContourUpdatedEventArg(List<Vertex> externalContour)
        {
            ExternalContour = externalContour;
        }
    }
}