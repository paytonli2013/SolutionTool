using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

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
