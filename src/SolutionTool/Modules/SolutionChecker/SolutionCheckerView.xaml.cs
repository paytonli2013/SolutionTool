using System.Windows.Controls;

namespace SolutionChecker
{
    /// <summary>
    /// Interaction logic for SolutionCheckerView.xaml
    /// </summary>
    public partial class SolutionCheckerView : UserControl
    {
        public SolutionCheckerView(SolutionCheckerViewmodel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
