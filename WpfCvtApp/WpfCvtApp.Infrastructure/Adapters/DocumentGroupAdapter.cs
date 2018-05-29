#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp.Infrastructure
// Author : Ilias Sachpazidis
// Filename: DocumentGroupAdapter.cs
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

using System.Collections.Specialized;
using System.ComponentModel.Composition;
using DevExpress.Xpf.Docking;
using Prism.Regions;

namespace WpfCvtApp.Infrastructure.Adapters
{
    public class DocumentGroupAdapter : RegionAdapterBase<DocumentGroup>
    {
        [ImportingConstructor]
        public DocumentGroupAdapter(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, DocumentGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => { OnViewsCollectionChanged(region, regionTarget, s, e); };
        }

        private void OnViewsCollectionChanged(IRegion region, DocumentGroup regionTarget, object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var manager = regionTarget.GetDockLayoutManager();
                    var panel = manager.DockController.AddDocumentPanel(regionTarget);
                    panel.Content = view;
                    if (view is IPanelInfo)
                    {
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                        panel.ShowCloseButton = ((IPanelInfo)view).ShowCloseButton;
                    }
                    else panel.Caption = "new Page";
                    manager.DockController.Activate(panel);
                }
            }
        }
    }

}