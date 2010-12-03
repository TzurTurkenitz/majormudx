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
using MMX.Core.API.Infrastructure.Services;
using MMX.Core.API.Infrastructure.Views;

namespace MMX.Core.API.Infrastructure.Control
{
    public interface IController
    {
        ControllerCache Controllers { get; }
        IController Parent { get; }
        ServiceCache Services { get; }
        ViewCache Views { get; }
    }
}
