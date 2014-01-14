using System.Windows.Controls;

namespace ManageRule
{
    /// <summary>
    /// Interaction logic for ManageRuleView.xaml
    /// </summary>
    public partial class ManageRuleView : UserControl
    {
        public ManageRuleView(ManageRuleViewmodel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
