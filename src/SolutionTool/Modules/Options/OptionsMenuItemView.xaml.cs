using Microsoft.Practices.Prism.Regions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Options
{
    /// <summary>
    /// Interaction logic for OptionsMenuItemView.xaml
    /// </summary>
    [ViewSortHint("90")]
    public partial class OptionsMenuItemView : UserControl
    {
        public OptionsMenuItemView(OptionsMenuItemViewmodel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
