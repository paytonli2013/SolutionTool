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
