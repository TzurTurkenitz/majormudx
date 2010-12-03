using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace MMX.Core.API.ViewModels
{
    public static class ViewModelLocator
    {
        static Dictionary<string, object> _registerredModels;

        static ViewModelLocator()
        {
            _registerredModels = new Dictionary<string, object>();
        }

        public static void Register(string key, object viewModel)
        {
            if (!_registerredModels.ContainsKey(key))
                _registerredModels.Add(key, null);
            _registerredModels[key] = viewModel;
        }

        public static object Get(string key)
        {
            if (_registerredModels.ContainsKey(key))
                return _registerredModels[key];
            return null;
        }
    }
}
