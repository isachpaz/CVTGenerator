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
using WpfCvtApp.GraphModule.ViewModels;
using WpfCvtApp.Infrastructure.Adapters;

namespace WpfCvtApp.GraphModule.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl, IPanelInfo
    {
        public GraphView(GraphViewModel graphViewModel)
        {
            InitializeComponent();
            ShowCloseButton = false;
            AutoHide = false;
            GraphViewModel = graphViewModel;
            DataContext = GraphViewModel;
        }

        public string GetPanelCaption()
        {
            return $"Graph";
        }

        public bool ShowCloseButton { get; set; }
        public bool AutoHide { get; set; }
        public GraphViewModel GraphViewModel { get; }
    }
}
