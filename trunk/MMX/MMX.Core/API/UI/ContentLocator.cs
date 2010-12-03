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
using System.Windows.Data;
using MMX.Core.API.Infrastructure.Services;
using MMX.Core.API.Infrastructure.Control;

namespace MMX.Core.API.UI
{
    public class ContentLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object ctl = null;

            IController cntl = value as IController;
            if (cntl != null)
            {
                ctl = cntl.Services.Get<IMainContentControl>();
            }

            return ctl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
