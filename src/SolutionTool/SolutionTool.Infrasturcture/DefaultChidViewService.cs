using System;
using System.Windows;
using Microsoft.Practices.Unity;

namespace Orc.SolutionTool
{
    public class DefaultChiLdViewService : IChildViewService
    {
        IUnityContainer _container;

        public DefaultChiLdViewService(IUnityContainer container)
        {
            _container = container;
        }

        public void OpenChildView(IWindowHost host, string viewName, string title, Action<CloseResult> onClosed, ViewOptions option = null)
        {
            if (option == null)
                option = ViewOptions.Default;

            var window = new Window()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Height = option.Height,
                Width = option.Width,
                Title = title
            };

            if (host!=null)
            {
                var owner = host.GetRealWindow() as Window;

                window.Owner = owner;
            }

            host = new HostWindow(window, null, onClosed);

            var cv = new ChildView(host, viewName, title, onClosed);

            var shellService = new ShellService(host, cv, _container, null);
            var childContainer = _container.CreateChildContainer();

            childContainer.RegisterInstance<IShellService>(shellService);

            var view = childContainer.Resolve<object>(viewName);

            if (option.Payload!=null && view is IChildView)
            {
                ((IChildView)view).SetPayload(option.Payload);
            }

            cv.SetContent(view);

            window.Content = cv;

            window.ShowDialog();
        }
    }
}
