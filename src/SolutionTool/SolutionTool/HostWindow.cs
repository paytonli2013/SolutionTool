using System.Windows;

namespace Orc.SolutionTool
{
    public class HostWindow : IHostWindow
    {
        IStatusBar _statusBar;

        public HostWindow(Window window, IStatusBar statusBar = null)
        {
            _statusBar = statusBar;

            if (_statusBar == null)
                _statusBar = window as IStatusBar;
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            MessageBox.Show(message, catgory.ToString());
        }
    }
}
