#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp
// Author : Ilias Sachpazidis
// Filename: Bootstrapper.cs
// Created : 01/11/2014  
//   
// Last Modified By : isachpaz
// Last Modified On : 01/11/2014
// Description : 
//   
// Distributed under MIT License
// =============================================
// Copyright (c) 2018 - 2018 Medical Innovation and Technology
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files 
// (the "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sub license, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
// ***********************************************************************
#endregion

using System.Windows;
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using WpfCvtApp.GraphModule.ViewModels;
using WpfCvtApp.Infrastructure.Adapters;
using WpfCvtApp.Infrastructure.DataServices;
using WpfCvtApp.SettingsModule.ViewModels;
using WpfCvtApp.StructureNavigationModule.ViewModels;
using WpfCvtApp.ViewModels;
using WpfCvtApp.Views;

namespace WpfCvtApp.Startup
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<MainRibbonViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<StructureNavigationViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<GraphViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<VoronoiSettingsViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISettingsDataService, SettingsDataService>(new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            base.ConfigureModuleCatalog();
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;

            catalog.AddModule(typeof(StructureNavigationModule.StructureNavigationModule));
            catalog.AddModule(typeof(GraphModule.GraphModule));
            catalog.AddModule(typeof(SettingsModule.SettingsModule));

        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            if (mappings != null)
            {
                mappings.RegisterMapping(typeof(LayoutPanel), Container.Resolve<LayoutPanelAdapter>());
                mappings.RegisterMapping(typeof(LayoutGroup), Container.Resolve<LayoutGroupAdapter>());
                mappings.RegisterMapping(typeof(DocumentGroup), Container.Resolve<DocumentGroupAdapter>());
                mappings.RegisterMapping(typeof(TabbedGroup), Container.Resolve<TabbedGroupAdapter>());
            }
            return mappings;
            //return base.ConfigureRegionAdapterMappings();
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainRibbonView>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = Shell as Window;
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Show();
        }
    }
}