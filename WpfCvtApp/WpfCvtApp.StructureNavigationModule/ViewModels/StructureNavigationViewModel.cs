#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp.StructureNavigationModule
// Author : Ilias Sachpazidis
// Filename: StructureNavigationViewModel.cs
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using DicomLib.RtData;
using Prism.Events;
using Prism.Mvvm;
using WpfCvtApp.Infrastructure.Events;

namespace WpfCvtApp.StructureNavigationModule.ViewModels
{
    public class StructureNavigationViewModel : BindableBase
    {
        private Structure _selectedStructure;
        public IEventAggregator EventAggregator { get; }
        public  ObservableCollection<Structure> Structures { get; set; }

        public Structure SelectedStructure
        {
            get { return _selectedStructure; }
            set
            {
                if (_selectedStructure != value)
                {
                    _selectedStructure = value;
                    OnSelectedStructure(_selectedStructure);
                }
            }
        }

        private void OnSelectedStructure(Structure selectedStructure)
        {
            Trace.WriteLine($"Selected item: {selectedStructure}");
            EventAggregator?.GetEvent<StructureSelectedEvent>()
                            .Publish(new StructureSelectedEventArg(selectedStructure));
        }

        public StructureNavigationViewModel(IEventAggregator eventAggregator)
        {
            Structures = new ObservableCollection<Structure>();
            SelectedStructure = new Structure("NULL", -1);
            EventAggregator = eventAggregator;
            EventAggregator.GetEvent<StrucutureSetUpdatedEvent>().Subscribe(StructureSetUpdated, true);
        }

        private void StructureSetUpdated(StrucutureSetUpdatedEventArg strucutureSetUpdatedEventArg)
        {
            EmptyStructureList();
            Trace.WriteLine("StructureSetUpdated called!");
            var structures = strucutureSetUpdatedEventArg.StructureSet;
            foreach (Structure item in structures)
            {
                Structures.Add(item);
            }
        }

        private void EmptyStructureList()
        {
            Structures.Clear();
        }
    }
}