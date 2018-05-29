#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp
// Author : Ilias Sachpazidis
// Filename: MainRibbonViewModel.cs
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DicomLib;
using DicomLib.RtData;
using GeometryLib.Primitives;
using GeometryLib.Tools;
using GeometryLib.Voronoi;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using WpfCvtApp.Infrastructure.DataServices;
using WpfCvtApp.Infrastructure.Events;

namespace WpfCvtApp.ViewModels
{
    public class MainRibbonViewModel : BindableBase
    {
        public DelegateCommand LoadStructureCommand { get; set; }
        public DelegateCommand CvtGenerationCommand { get; set; }
        public IEventAggregator EventAggregator { get; }
        public ISettingsDataService SettingsDataService { get; }
        public bool InitialState { get; set; }
        public ObservableCollection<Structure> StructureSet { get; set; }

        private List<Vertex> CurrentExternalContour { get; set; }

        public MainRibbonViewModel(IEventAggregator eventAggregator, 
                                    ISettingsDataService settingsDataService)
        {
            EventAggregator = eventAggregator;
            SettingsDataService = settingsDataService;
            LoadStructureCommand = new DelegateCommand(LoadStructureExecute);
            CvtGenerationCommand = new DelegateCommand(CvtGenerationExecute);
            StructureSet = new ObservableCollection<Structure>();
            CurrentExternalContour = new List<Vertex>();
            EventAggregator.GetEvent<StructureSelectedEvent>().Subscribe(NewStructureSelected, true);
        }

        private void CvtGenerationExecute()
        {
            if (CurrentExternalContour.Count == 0)
            {
                DXMessageBox.Show("Please, load a structure, and generate a contour, first!");
                return;
            }
            var voronoiSettings = SettingsDataService.VoronoiSettings;
            DXSplashScreen.Show<SplashScreenView1>();
            DXSplashScreen.SetState("Calculating centroidal voronoi tessellations...");

            var cvtSettings = new CvtSettings
            {
                NumberOfGenerators = voronoiSettings.NumberOfGenerators,
                NumberOfSamplingPoints = voronoiSettings.NumberOfSamplingPoints,
                SelectedSamplingMethod = voronoiSettings.SelectedSamplingMethod,
                MaxNumberOfIterations = 90,
            };

            Cvt cvt = new Cvt(CurrentExternalContour, cvtSettings);
            
            var generators = cvt.GetGenerators();
            EventAggregator.GetEvent<GeneratorsUpdatedEvent>().Publish(new GeneratorsUpdatedEventArg(generators));
            Trace.WriteLine(generators);
            DXSplashScreen.Close();
        }

        private void NewStructureSelected(StructureSelectedEventArg structureSelectedEventArg)
        {
            var structure = structureSelectedEventArg.Structure;
            if (structure == null)
            {
                return;
            }
            try
            {
                DXSplashScreen.Show<SplashScreenView1>();
                DXSplashScreen.SetState("Calculating the maximun external contour...");

                //1. Get the maximum external contour of the structure, available from all contours.
                //2. Plot the points on 2D.

                var contours = structure.Contours;
                var maxExternalContour = GeometryHelper.GetOuterContour(contours, 1000);
                this.CurrentExternalContour = maxExternalContour;
                EventAggregator.GetEvent<ExternalContourUpdatedEvent>()
                    .Publish(new ExternalContourUpdatedEventArg(maxExternalContour));
               
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                throw;
            }
            finally
            {
                DXSplashScreen.Close();
            }
           
        }

        private void LoadStructureExecute()
        {
            Debug.WriteLine("LoadStructure pressed!");
            InitialState = false;

            StructureSet.Clear();
            string sFile = @"DicomSamples\RSAnonymized.dcm";
            DicomLib.RtStructureDicom rtStructureDicom = RtStructureDicom.ImportFromFile(sFile);
            var ss = rtStructureDicom.StructureSet;
            foreach (Structure item in ss.StructureCollection)
            {
                StructureSet.Add(item);
                Trace.WriteLine(item);
            }

            var structure = ss.StructureCollection.FirstOrDefault(s => s.Name == "Prostate" || s.Name == "Prostata");
            EventAggregator.GetEvent<StrucutureSetUpdatedEvent>().Publish(new StrucutureSetUpdatedEventArg(StructureSet));
        }

        
    }
}