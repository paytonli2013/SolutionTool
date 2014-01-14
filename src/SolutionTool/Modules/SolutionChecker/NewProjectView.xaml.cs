using System.Windows.Controls;

namespace SolutionChecker
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView(NewProjectViewmodel viewmodel)
        {
            InitializeComponent();
            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
