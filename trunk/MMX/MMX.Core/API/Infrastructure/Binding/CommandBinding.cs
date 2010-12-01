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

namespace MMX.Core.API.Infrastructure.Binding
{
    public class CommandBinding : FrameworkElement
    {
        public CommandBinding()
        {
        }

        public string TargetCommand { get; set; }

        public string EventName { get; set; }
    }
}
