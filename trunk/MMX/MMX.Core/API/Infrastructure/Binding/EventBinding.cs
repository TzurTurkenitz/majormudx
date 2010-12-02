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
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MMX.Core.API.Infrastructure.Binding
{
    using System.Windows.Markup;

    [ContentProperty("Bindings")]
    public class EventBinding : FrameworkElement
    {
        public EventBinding()
        {
            Bindings = new ObservableCollection<CommandBinding>();
        }

        public ObservableCollection<CommandBinding> Bindings { get; set; }
    }
}
