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
using System.ComponentModel;
using MMX.Core.API.Infrastructure.Control;

namespace MMX.Core.API.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Interface implementations

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get { return null; }
        }

        public IController Controller { get; set; }

        public string this[string columnName]
        {
            get { return Validate(columnName); }
        }

        #endregion

        #region Virtual Methods

        public virtual string Validate(string property)
        {
            return null;
        }

        #endregion

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
