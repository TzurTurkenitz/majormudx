using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MMX.Common.API.Services;

namespace MMX.Common.API.UI
{
    public abstract class ViewModelBase : IMMXViewModel
    {
        protected IServiceLocator _locator;

        IServiceLocator IMMXViewModel.ServiceLocator
        {
            get { return _locator; }
        }
    }
}
