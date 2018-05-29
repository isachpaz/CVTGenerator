using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using WpfCvtApp.Infrastructure;
using WpfCvtApp.StructureNavigationModule.Views;

namespace WpfCvtApp.StructureNavigationModule
{
    public class StructureNavigationModule : IModule
    {
        public StructureNavigationModule(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            EventAggregator = eventAggregator;
            RegionManager = regionManager;
        }

        public IEventAggregator EventAggregator { get; }
        public IRegionManager RegionManager { get; }

        public void Initialize()
        {
            if (RegionManager?.Regions?.Count() > 0)
            {
                RegionManager.RegisterViewWithRegion(RegionNames.LeftRegion, typeof(StructureNavigationView));
            }
        }
    }
}
