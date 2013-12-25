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

namespace ManageRule
{
    /// <summary>
    /// Interaction logic for ManageRuleMenuItemView.xaml
    /// </summary>
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
