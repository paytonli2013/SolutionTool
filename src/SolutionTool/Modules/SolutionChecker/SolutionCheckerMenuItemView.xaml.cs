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

namespace SolutionChecker
{
    /// <summary>
    /// Interaction logic for SolutionCheckerMenuItemView.xaml
    /// </summary>
    [ViewSortHint("00")]
    public partial class SolutionCheckerMenuItemView : UserControl
    {
        public SolutionCheckerMenuItemView(SolutionCheckerMenuItemViewmodel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
