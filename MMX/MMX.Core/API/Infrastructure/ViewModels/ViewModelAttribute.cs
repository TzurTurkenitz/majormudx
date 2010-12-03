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

namespace MMX.Core.API.Infrastructure.ViewModels
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelAttribute : Attribute
    {
        public string ID { get; set; }
        public ViewModelCreationMethod CreationMethod { get; set; }
    }

    public enum ViewModelCreationMethod
    {
        Instance,
        Singleton
    }
}
