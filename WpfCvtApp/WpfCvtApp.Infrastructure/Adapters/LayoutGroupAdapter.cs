#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp.Infrastructure
// Author : Ilias Sachpazidis
// Filename: LayoutGroupAdapter.cs
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
    public class LayoutGroupAdapter : RegionAdapterBase<LayoutGroup>
    {
        private bool _lockItemsChanged;
        private bool _lockViewsChanged;

        [ImportingConstructor]
        public LayoutGroupAdapter(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
            regionTarget.Items.CollectionChanged += (s, e) => OnItemsCollectionChanged(region, regionTarget, s, e);
        }


        private void OnItemsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (_lockItemsChanged)
                return;

            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    _lockViewsChanged = true;
            //    var lp = (LayoutPanel)e.OldItems[0];
            //    var view = lp.Content;
            //    lp.Content = null;
            //    region.Remove(view);
            //    _lockViewsChanged = false;
            //}
        }

        private void OnViewsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (_lockViewsChanged)
                return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var panel = new LayoutPanel { Content = view };
                    if (view is IPanelInfo)
                    {
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                        panel.ShowCloseButton = ((IPanelInfo)view).ShowCloseButton;
                        panel.AutoHidden = ((IPanelInfo)view).AutoHide;
                    }
                    else
                        panel.Caption = "new Page";

                    _lockItemsChanged = true;
                    regionTarget.Items.Add(panel);
                    _lockItemsChanged = false;

                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    LayoutPanel viewPanel = null;
                    foreach (LayoutPanel panel in regionTarget.Items)
                    {
                        if (panel.Content == view)
                        {
                            viewPanel = panel;
                            break;
                        }
                    }
                    if (viewPanel == null) continue;
                    viewPanel.Content = null;
                    _lockItemsChanged = true;
                    regionTarget.Items.Remove(viewPanel);
                    _lockItemsChanged = false;
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }
    }

}