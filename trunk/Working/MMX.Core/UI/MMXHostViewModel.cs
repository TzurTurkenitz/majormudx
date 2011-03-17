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
using MMX.Common.API.UI;
using MMX.Common.API.Services;
using System.ComponentModel;

namespace MMX.Core.UI
{
    public class MMXHostViewModel : IMMXViewModel, INotifyPropertyChanged
    {
        IServiceLocator _locator;
        IMMXContentContainer _container;

        string _msg;

        public MMXHostViewModel()
        {
            _container = new MMXContentContainer();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Container"));
        }

        IServiceLocator IMMXViewModel.ServiceLocator
        {
            get
            {
                return _locator;
            }
        }

        public IMMXContentContainer Container
        {
            get
            {
                return _container;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
