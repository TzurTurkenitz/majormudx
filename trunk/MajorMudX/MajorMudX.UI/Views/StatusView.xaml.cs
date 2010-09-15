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
using System.ComponentModel;

namespace MajorMudX.UI.Views
{
    public partial class StatusView : UserControl, INotifyPropertyChanged
    {
        CharacterLayoutHostView _host;

        public StatusView()
        {
            InitializeComponent();
            MaxHP = 34;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = Parent;
            while (parent != null && !typeof(CharacterLayoutHostView).IsAssignableFrom(parent.GetType()))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                _host = parent as CharacterLayoutHostView;
                if (_host != null)
                    _host.Status = this;
            }
        }

        int _maxHp;
        public int MaxHP
        {
            get { return _maxHp; }
            set { _maxHp = value; Notify("MaxHP"); }
        }

        int _currentHp;
        public int CurrentHP
        {
            get { return _currentHp; }
            set { _currentHp = value; Notify("CurrentHP"); }
        }

        void Notify(string propName)
        {
            if (Dispatcher.CheckAccess())
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
            else Dispatcher.BeginInvoke(() => { Notify(propName); });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
