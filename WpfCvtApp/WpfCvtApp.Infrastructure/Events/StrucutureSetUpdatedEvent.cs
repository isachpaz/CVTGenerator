using System.Collections.Generic;
using System.Collections.ObjectModel;
using DicomLib.RtData;
using Prism.Events;

namespace WpfCvtApp.Infrastructure.Events
{
    public class StrucutureSetUpdatedEvent : PubSubEvent<StrucutureSetUpdatedEventArg>
    {
        
    }

    public class StrucutureSetUpdatedEventArg
    {
        public ObservableCollection<Structure> StructureSet { get; }

        public StrucutureSetUpdatedEventArg(ObservableCollection<Structure> structureSet)
        {
            this.StructureSet = structureSet;
        }
    }
}