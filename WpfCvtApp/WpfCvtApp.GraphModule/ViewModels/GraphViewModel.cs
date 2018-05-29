using System;
using System.Collections.ObjectModel;
using GeometryLib.Primitives;
using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using WpfCvtApp.Infrastructure.Events;

namespace WpfCvtApp.GraphModule.ViewModels
{
    public class GraphViewModel : BindableBase
    {
        private string _title;
        public ObservableCollection<DataPoint> ExternalDataPoints { get; set; }
        public ObservableCollection<ScatterPoint> GeneratorScatterPoints { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                base.OnPropertyChanged();
            }
        }

        public GraphViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            ExternalDataPoints = new ObservableCollection<DataPoint>();
            GeneratorScatterPoints = new ObservableCollection<ScatterPoint>();
            Title = "Maximum external contour";
            EventAggregator.GetEvent<ExternalContourUpdatedEvent>().Subscribe(ExternalContourUpdated, true);
            EventAggregator.GetEvent<GeneratorsUpdatedEvent>().Subscribe(GeneratorsUpdated, true);
            
        }

        private void GeneratorsUpdated(GeneratorsUpdatedEventArg generatorsUpdatedEventArg)
        {
            var generators = generatorsUpdatedEventArg.Generators;
            GeneratorScatterPoints.Clear();

            foreach (Vertex item in generators)
            {
                GeneratorScatterPoints.Add(new ScatterPoint(item.X, item.Y));
            }
        }

        private void ExternalContourUpdated(ExternalContourUpdatedEventArg externalContourUpdatedEventArg)
        {
            ExternalDataPoints.Clear();
            var points = externalContourUpdatedEventArg.ExternalContour;
            foreach (var item in points)
            {
                ExternalDataPoints.Add(new DataPoint(item.X, item.Y));
            }
        }

        public IEventAggregator EventAggregator { get; }
    }
}