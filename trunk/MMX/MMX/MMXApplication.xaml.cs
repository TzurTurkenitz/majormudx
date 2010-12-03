using System;
using System.Linq;
using System.Windows;
using MMX.Core.API.Infrastructure.Control;
using MMX.Core.API.Infrastructure.ViewModels;
using MMX.ViewModels;
using MMX.Views;
using System.Collections.Generic;
using System.Reflection;

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
                ReflectViewModels();

                //ViewModelLocator.Register(Constants.ShellViewModel, new ShellViewModel() { Controller = new ControllerBase(ShellController) });

                // use the out of browser app
                this.RootVisual = ShellController.Views.Create<MMXShell>("MainShell");
            }
            else
            {
                // use the web installer
                this.RootVisual = new WebLandingPage();
            }
        }

        private void ReflectViewModels()
        {
            List<Assembly> assemblies = Deployment.Current.Parts.Select(
                    ap => Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative))).Select(
                        stream => new AssemblyPart().Load(stream.Stream)).ToList();

            foreach (Assembly asm in assemblies)
            {
                foreach (Type t in asm.GetTypes().Where((at) => { return at.IsDefined(typeof(ViewModelAttribute), true); }))
                {
                    ViewModelAttribute vmAtt = t.GetCustomAttributes(true).FirstOrDefault(
                        (a) => { return a is ViewModelAttribute; }) as ViewModelAttribute;

                    object instance = Activator.CreateInstance(t);
                    PropertyInfo pController = t.GetProperty("Controller", BindingFlags.Instance | BindingFlags.Public);
                    if (pController != null)
                        pController.SetValue(instance, ShellController, null);
                    ViewModelLocator.Register(vmAtt.ID, instance);
                }
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
