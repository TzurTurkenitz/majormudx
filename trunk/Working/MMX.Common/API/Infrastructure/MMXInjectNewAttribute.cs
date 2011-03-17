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

namespace MMX.Common.API.Infrastructure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MMXInjectNewAttribute : Attribute
    {
        public string Id { get; set; }
        public Type InjectionType { get; set; }
    }
}
