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

            Loaded += ShellView_Loaded;
        }

        void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(new Action(() => 
                {
                    _viewmodel.Avtive(0);
                }));
            }
            else
                _viewmodel.Avtive(0);

            _viewmodel.PostStatusMessage(StatusCatgory.None,"Ready");
            //throw new NotImplementedException();
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

        public void Confirm(string message, Action<MessageBoxResult> onConfirmed)
        {
            var result = MessageBox.Show(message, "alert", MessageBoxButton.YesNo);

            var result2 = (MessageBoxResult)((int)result);

            if (onConfirmed != null)
            {
                onConfirmed.Invoke(result2);
            }
        }
    }
}
