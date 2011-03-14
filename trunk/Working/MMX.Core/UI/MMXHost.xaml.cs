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
using System.Windows.Navigation;
using MMX.Common.API.UI;
using MMX.Common.API.Services;

namespace MMX.Core.UI
{
    [ViewRegistration(Id = ServiceConstants.MMXHost, ViewType = typeof(IMMXHost))]
    [ViewModel(ViewModelType = typeof(MMXHostViewModel), Id = ServiceConstants.MMXHostVM, CreateNew = true)]
    public partial class MMXHost : Page, IMMXHost
    {
        public MMXHost()
        {
            InitializeComponent();
        }
    }
}
