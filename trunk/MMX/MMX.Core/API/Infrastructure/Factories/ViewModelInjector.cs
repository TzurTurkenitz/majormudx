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

namespace MMX.Core.API.Infrastructure.Factories
{
    public class ViewModelInjector
    {
        public ViewModelInjector()
        {
        }

        public string ViewModelKey { get; set; }
        public bool AttachAsDataContext { get; set; }
    }
}
