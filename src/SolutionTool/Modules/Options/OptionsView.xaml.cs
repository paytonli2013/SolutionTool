using System.Windows.Controls;

namespace Options
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    public partial class OptionsView : UserControl
    {
        public OptionsView(OptionsViewModel viewmodel)
        {
            InitializeComponent();

            viewmodel.View = this;
            DataContext = viewmodel;
        }
    }
}
