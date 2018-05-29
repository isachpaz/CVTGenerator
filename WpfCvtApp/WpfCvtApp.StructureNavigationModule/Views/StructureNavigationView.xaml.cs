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
using WpfCvtApp.StructureNavigationModule.ViewModels;

namespace WpfCvtApp.StructureNavigationModule.Views
{
    /// <summary>
    /// Interaction logic for StructureNavigationView.xaml
    /// </summary>
    public partial class StructureNavigationView : UserControl, IPanelInfo
    {
        public StructureNavigationView(StructureNavigationViewModel navigationViewModel)
        {
            InitializeComponent();
            AutoHide = false;
            ShowCloseButton = false;
            NavigationViewModel = navigationViewModel;
            DataContext = NavigationViewModel;
        }

        public string GetPanelCaption()
        {
            return $"Structures info";
        }

        public bool ShowCloseButton { get; set; }
        public bool AutoHide { get; set; }
        public StructureNavigationViewModel NavigationViewModel { get; }
    }
}
