#region License
// ***********************************************************************
// Assembly : WpfCvtApp
// Project : WpfCvtApp.Infrastructure
// Author : Ilias Sachpazidis
// Filename: LayoutPanelAdapter.cs
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

using System.ComponentModel.Composition;
using DevExpress.Xpf.Docking;
using Prism.Regions;

namespace WpfCvtApp.Infrastructure.Adapters
{
    public class LayoutPanelAdapter : RegionAdapterBase<LayoutPanel>
    {
        [ImportingConstructor]
        public LayoutPanelAdapter(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutPanel regionTarget)
        {
            region.Views.CollectionChanged += (d, e) =>
            {
                if (e.NewItems != null)
                    regionTarget.Content = e.NewItems[0];
            };
        }
    }

}