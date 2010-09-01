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
using MajorMudX.Core.Sockets;
using System.Reflection;
using MajorMudX.Core.Utilities.XapManagement;
using MajorMudX.Core.UI;

namespace MajorMudX
{
    public partial class HostPageLayout : UserControl
    {
        public HostPageLayout()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //if (Application.Current.IsRunningOutOfBrowser)
            //{
            string absolutePath = Application.Current.Host.Source.AbsoluteUri.Remove(
                Application.Current.Host.Source.AbsoluteUri.LastIndexOf('/') + 1);
            Uri uiApp = new Uri(absolutePath + "MajorMudX.UI.xap", UriKind.Absolute);

            XapLoader.LoadCompleted += new EventHandler(XapLoader_LoadCompleted);
            XapLoader.LoadXap(uiApp);
            //}
        }

        void XapLoader_LoadCompleted(object sender, EventArgs e)
        {

            if (XapLoader.Contains(typeof(CharacterHostAttribute)))
            {
                tabItem1.Content = XapLoader.Get(typeof(CharacterHostAttribute));
            }
        }
    }
}
