using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fluent;
using WpfCvtApp.ViewModels;


namespace WpfCvtApp.Views
{
    /// <summary>
    /// Interaction logic for MainRibbonView.xaml
    /// </summary>
    public partial class MainRibbonView : RibbonWindow
    {
        public MainRibbonView(MainRibbonViewModel mainRibbonViewModel)
        {
            InitializeComponent();
            MainRibbonViewModel = mainRibbonViewModel;
            DataContext = MainRibbonViewModel;
        }

        public MainRibbonViewModel MainRibbonViewModel { get; }
    }
}
