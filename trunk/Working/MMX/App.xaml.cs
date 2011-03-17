using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MMX.Bootstrap;
using MMX.Common.API.Services;
using MMX.Core;
using MMX.Core.Services;
using MMX.Common.API.Control;
using MMX.Core.Control;

namespace MMX
{
    public partial class App : Application
    {
        IServiceLocator _locator;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (IsRunningOutOfBrowser && HasElevatedPermissions)
            {
                _locator = new ServiceLocator();

                // Invoke the bootstrapper
                //MMXBootstrap.LoadServices(_locator);

                // Load the UI elements
                //MMXBootstrap.GenerateUI(_locator);

                var controllers = MMXBootstrap.BuildControllers(_locator);

                IMMXController[] ctl = controllers.ToArray();

                if (ctl[0] is MMXHostController)
                    RootVisual = (ctl[0] as MMXHostController).HostUI;
                else
                    // Set the root visual
                    RootVisual = new DefaultPage();// _locator.GetInstance<UIElement>(ServiceConstants.MMXHost);
            }
            else
            {
                RootVisual = new DefaultPage();
            }
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                if (Application.Current.HasElevatedPermissions && Application.Current.IsRunningOutOfBrowser)
                    ; // handle errors here
                else
                    Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
