using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using WpfCvtApp.GraphModule.Views;
using WpfCvtApp.Infrastructure;

namespace WpfCvtApp.GraphModule
{
    public class GraphModule : IModule
    {
        public GraphModule(IEventAggregator eventAggregator, IRegionManager regionManager)
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
                RegionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(GraphView));
            }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            throw new NotImplementedException();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            throw new NotImplementedException();
        }
    }
}
