using System.Windows.Controls;

namespace ManageTemplate
{
    /// <summary>
    /// Interaction logic for ManageTemplateView.xaml
    /// </summary>
    public partial class ManageTemplateView : UserControl
    {
        public ManageTemplateView(ManageTemplateViewmodel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            this.DataContext = viewmodel;
        }
    }
}
