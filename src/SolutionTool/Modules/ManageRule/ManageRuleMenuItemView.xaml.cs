using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace ManageRule
{
    /// <summary>
    /// Interaction logic for ManageRuleMenuItemView.xaml
    /// </summary>
    [ViewSortHint("01")]
    public partial class ManageRuleMenuItemView : UserControl
    {
        public ManageRuleMenuItemView(ManageRuleMenuItemViewmodel viewmodel)
        {
            InitializeComponent();
            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
