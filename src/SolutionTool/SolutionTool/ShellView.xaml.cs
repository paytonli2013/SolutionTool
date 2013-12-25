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

namespace Orc.SolutionTool
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl, IStatusBar,IMessageService
    {
        ShellViewmodel _viewmodel;
        public ShellView(ShellViewmodel viewmodel)
        {
            InitializeComponent();
            _viewmodel = viewmodel;
            viewmodel.View = this;
            DataContext = viewmodel;
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            //throw new NotImplementedException();
            _viewmodel.PostStatusMessage(catgory, message);
        }

        public void Show(string message)
        {
            _viewmodel.Show(message);
        }
    }
}
