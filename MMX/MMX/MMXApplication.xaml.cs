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
using MMX.Core.API.Infrastructure.Factories;

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
                ShellController = Generate<ControllerBase>();
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

        IController Generate<T>() where T : class
        {
            IController instance;

            Type t = typeof(T);
            ConstructorInfo cInfo = null;
            foreach (ConstructorInfo ci in t.GetConstructors())
            {

                ParameterInfo[] pArr = ci.GetParameters();
                if (pArr.Length == 0)
                {
                    cInfo = ci;
                    break;
                }
                bool valid = true;
                foreach (ParameterInfo pi in pArr)
                    if (!pi.IsDefined(typeof(CreateNewAttribute), false))
                        valid = false;
                if (valid)
                {
                    cInfo = ci;
                    break;
                }
            }

            if (cInfo != null)
            {
                ParameterInfo[] piArr = cInfo.GetParameters();
                if (piArr.Length == 0)
                    instance = cInfo.Invoke(null) as IController;
                else
                {
                    object[] objs = new object[piArr.Length];
                    for (int i = 0; i < objs.Length; ++i)
                        objs[i] = null;
                    instance = cInfo.Invoke(objs) as IController;
                }
            }
            else
                instance = default(T) as IController;

            return instance;
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
