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

namespace MMX.Common.API.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class ViewModelAttribute : Attribute
    {
        public Type ViewModelType { get; set; }
        public string Id { get; set; }
        public bool CreateNew { get; set; }
    }
}
