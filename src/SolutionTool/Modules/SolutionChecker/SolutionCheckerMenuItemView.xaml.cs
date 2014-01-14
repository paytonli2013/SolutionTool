using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

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
