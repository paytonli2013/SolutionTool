using Microsoft.Practices.Prism.UnityExtensions;
using SolutionTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Unity;

namespace Orc.SolutionTool
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            var shell = Container.Resolve<ShellView>();

            var window = new Window() { Content = shell };

            Container.RegisterInstance<IHostWindow>(new HostWindow(window, shell as IStatusBar));
            Container.RegisterInstance<IMessageService>(shell);
            Container.RegisterInstance<IStatusBar>(shell);

            App.Current.MainWindow = window;

            return shell;
             //throw new NotImplementedException();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProjectManager, ProjectManager>(new ContainerControlledLifetimeManager());
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog catalog = new ConfigurationModuleCatalog();
            return catalog;
        }
    }
}
