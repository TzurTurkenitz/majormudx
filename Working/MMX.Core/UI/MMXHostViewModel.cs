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
        string _msg;

        public MMXHostViewModel()
        {
            _msg = "Hello World";
        }

        IServiceLocator IMMXViewModel.ServiceLocator
        {
            get
            {
                return _locator;
            }
        }

        public string Message
        {
            get { return _msg; }
            set { _msg = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Message")); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
