using DicomLib.RtData;
using Prism.Events;

namespace WpfCvtApp.Infrastructure.Events
{
    public class StructureSelectedEvent : PubSubEvent<StructureSelectedEventArg>
    {
        
    }

    public class StructureSelectedEventArg
    {
        public Structure Structure { get; }
        public StructureSelectedEventArg(Structure structure)
        {
            Structure = structure;
        }
    }
}