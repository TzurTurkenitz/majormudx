using System;
using System.Linq;
using System.Windows;
using MMX.Core.API.Infrastructure.Control;
using MMX.Core.API.Infrastructure.ViewModels;
using MMX.ViewModels;
using MMX.Views;
using System.Collections.Generic;
using System.Reflection;
using MMX.Core.API.Infrastructure.Initialization;

namespace MMX
{
    public partial class MMXApplication : Application
    {
        public IController ShellController { get; private set; }

        public MMXApplication()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Application.Current.HasElevatedPermissions && Application.Current.IsRunningOutOfBrowser)
            {
                ShellController = new ControllerBase(null);
                ShellController.Views.Create<MainShellContent>("MainShellContent");

                // view model loader
                Bootstrapper.ShellController = ShellController;
                Bootstrapper.ReflectAssemblies(Deployment.Current);

                // use the out of browser app
                this.RootVisual = ShellController.Views.Create<MMXShell>("MainShell");
            }
            else
            {
                // use the web installer
                this.RootVisual = new WebLandingPage();
            }
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

            // mark the error as handled
            e.Handled = true;
        }
    }
}
