using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCvtApp.Infrastructure.Adapters;
using WpfCvtApp.SettingsModule.ViewModels;

namespace WpfCvtApp.SettingsModule.Views
{
    /// <summary>
    /// Interaction logic for VoronoiSettingsView.xaml
    /// </summary>
    public partial class VoronoiSettingsView : UserControl, IPanelInfo
    {
        public VoronoiSettingsView(VoronoiSettingsViewModel voronoiSettingsViewModel)
        {
            InitializeComponent();
            ShowCloseButton = false;
            AutoHide = true;
            VoronoiSettingsViewModel = voronoiSettingsViewModel;
            DataContext = VoronoiSettingsViewModel;
        }

        public string GetPanelCaption()
        {
            return $"Voronoi settings";
        }

        public bool ShowCloseButton { get; set; }
        public bool AutoHide { get; set; }
        public VoronoiSettingsViewModel VoronoiSettingsViewModel { get; }
    }
}
