using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace ManageTemplate
{
    /// <summary>
    /// Interaction logic for ManageTemplateMenuItemView.xaml
    /// </summary>
    [ViewSortHint("02")]
    public partial class ManageTemplateMenuItemView : UserControl
    {
        public ManageTemplateMenuItemView(ManageTemplateMenuItemViewmodel viewmodel)
        {
            InitializeComponent();
            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
