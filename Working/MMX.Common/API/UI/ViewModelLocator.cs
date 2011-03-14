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
using System.Collections.Generic;

namespace MMX.Common.API.UI
{
    public sealed class ViewModelLocator
    {
        static Dictionary<string, IMMXViewModel> _viewModels;

        static ViewModelLocator()
        {
            _viewModels = new Dictionary<string, IMMXViewModel>();
        }

        public ViewModelLocator()
        {
        }

        public IMMXViewModel this[string index]
        {
            get
            {
                return _viewModels[index];
            }
            set
            {
                if (!_viewModels.ContainsKey(index))
                    _viewModels.Add(index, null);
                _viewModels[index] = value;
            }
        }
    }
}
