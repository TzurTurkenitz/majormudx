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
    public class ViewModelLocator : IValueConverter
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

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string key = parameter as string;
            if (!string.IsNullOrEmpty(key) && 
                _registerredModels.ContainsKey(key))
                    return _registerredModels[key];

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
