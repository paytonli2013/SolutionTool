using System.Windows;
using System.Windows.Controls;
using Orc.SolutionTool;

namespace SolutionChecker
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : UserControl, IChildView
    {
        ReportViewerViewmodel _viewmodel;
        public ReportViewer(ReportViewerViewmodel viewmodel)
        {
            InitializeComponent();
            _viewmodel = viewmodel;
            viewmodel.View = this;
            DataContext = viewmodel;

            Loaded += ReportViewer_Loaded;
        }

        void ReportViewer_Loaded(object sender, RoutedEventArgs e)
        {
            wbPlain.Navigate(_viewmodel.ReportText);
            wb.Navigate(_viewmodel.ReportHtml);
        }

        public void SetPayload(object data)
        {
            _viewmodel.SetPayload(data);
            //throw new NotImplementedException();
        }
    }
}
