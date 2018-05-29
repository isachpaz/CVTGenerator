using System;
using System.Collections.ObjectModel;
using GeometryLib.RandomEngines;
using Prism.Events;
using Prism.Mvvm;
using WpfCvtApp.Infrastructure.Data;
using WpfCvtApp.Infrastructure.DataServices;

namespace WpfCvtApp.SettingsModule.ViewModels
{
    public class VoronoiSettingsViewModel : BindableBase
    {
        private int _numberOfGenerators;
        private int _numberOfSamplingPoints;
        private string _selectedSamplingMethod;

        public VoronoiSettingsViewModel(IEventAggregator eventAggregator,
                                        ISettingsDataService settingsDataService)
        {
            EventAggregator = eventAggregator;
            SettingsDataService = settingsDataService;
            SamplingMethods = new ObservableCollection<string>();
            //SelectedSamplingMethod = String.Empty;

            DefaultSettings(SettingsDataService.VoronoiSettings);
        }

        private void DefaultSettings(VoronoiSettings vs)
        {
            if (vs ==null) return;
            
            NumberOfGenerators = vs.NumberOfGenerators;
            NumberOfSamplingPoints = vs.NumberOfSamplingPoints;
            foreach (var item in vs.SamplingMethods)
            {
                SamplingMethods.Add(item);
            }
            SelectedSamplingMethod = vs.SelectedSamplingMethod.ToString();
        }

        public IEventAggregator EventAggregator { get; }
        public ISettingsDataService SettingsDataService { get; }

        public int NumberOfGenerators
        {
            get { return _numberOfGenerators; }
            set
            {
                if (_numberOfGenerators != value)
                {
                    _numberOfGenerators = value;
                    SettingsDataService.VoronoiSettings.NumberOfGenerators = _numberOfGenerators;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberOfSamplingPoints
        {
            get { return _numberOfSamplingPoints; }
            set
            {
                if (_numberOfSamplingPoints != value)
                {
                    _numberOfSamplingPoints = value;
                    SettingsDataService.VoronoiSettings.NumberOfSamplingPoints = _numberOfSamplingPoints;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> SamplingMethods { get; set; }

        public string SelectedSamplingMethod
        {
            get { return _selectedSamplingMethod; }
            set
            {
                if (_selectedSamplingMethod != value)
                {
                    _selectedSamplingMethod = value;
                    RandomEngine re = (RandomEngine)Enum.Parse(typeof(RandomEngine), _selectedSamplingMethod, true);
                    SettingsDataService.VoronoiSettings.SelectedSamplingMethod = re;
                    OnPropertyChanged();
                }
            }
        }
    }
}