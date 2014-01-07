using System.Windows;

namespace Orc.SolutionTool
{
    public class HostWindow : IWindowHost
    {
        IStatusBar _statusBar;
        Window _window;
        private System.Action<CloseResult> _onClosed;
        public HostWindow(Window window, IStatusBar statusBar = null, System.Action<CloseResult> onClosed = null)
        {
            _statusBar = statusBar;
            _window = window;
            _onClosed = onClosed;
            if (_statusBar == null)
                _statusBar = window as IStatusBar;
        }

        public void PostStatusMessage(StatusCatgory catgory, string message)
        {
            if (_statusBar != null)
                _statusBar.PostStatusMessage(catgory, message);
            else
                MessageBox.Show(message, catgory.ToString());
        }

        public string Name
        {
            get { return _window.Name; }
        }

        public void Refresh()
        {

        }

        public void Close(CloseResult result)
        {
            _window.Close();

            if (_onClosed != null)
            {
                _onClosed.Invoke(result);
            }
        }

        public object GetRealWindow()
        {
            return _window;
        }
    }
}
