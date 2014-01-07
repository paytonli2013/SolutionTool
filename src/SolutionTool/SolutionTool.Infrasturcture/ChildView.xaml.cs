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
    /// Interaction logic for ChildView.xaml
    /// </summary>
    public partial class ChildView : UserControl,IMessageService
    {
        private IWindowHost host;
        private string viewName;
        private string title;
        private Action<CloseResult> onClosed;

        private ChildView()
        {
            InitializeComponent();
        }

        public ChildView(IWindowHost host, string viewName, string title, Action<CloseResult> onClosed) :this()
        {
            // TODO: Complete member initialization
            this.host = host;
            this.viewName = viewName;
            this.title = title;
            this.onClosed = onClosed;
        }

        public void SetContent(object content)
        {
            this.content.Content = content;
        }

        public void Show(string message)
        {
            //throw new NotImplementedException();
        }
    }
}
