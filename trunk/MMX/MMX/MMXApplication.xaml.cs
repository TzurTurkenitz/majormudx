﻿using System;
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
using MMX.Core.API.ViewModels;
using MMX.ViewModels;
using MMX.Views;
using MMX.Core.API.Infrastructure.Services;
using MMX.Core.API.UI;
using System.Threading;
using System.Windows.Threading;
using MMX.Core.API.Infrastructure.Control;

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
                ShellController.Services.Add<IMainContentControl>(new MainShellContent());

                ViewModelLocator.Register("Shell", new ShellViewModel() { Controller = new ControllerBase(ShellController) });

                // use the out of browser app
                this.RootVisual = new MMXShell();
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