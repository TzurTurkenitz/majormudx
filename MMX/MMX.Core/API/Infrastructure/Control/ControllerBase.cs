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
using MMX.Core.API.Infrastructure.Services;
using MMX.Core.API.Infrastructure.Views;

namespace MMX.Core.API.Infrastructure.Control
{
    public class ControllerBase : IController
    {
        public ControllerBase(IController parent)
        {
            Parent = parent;
            Controllers = new ControllerCache(parent);
            Services = new ServiceCache(parent);
            Views = new ViewCache(parent);
        }

        public ControllerCache Controllers { get; internal set; }

        public ServiceCache Services { get; internal set; }

        public IController Parent { get; protected set; }

        public ViewCache Views { get; internal set; }
    }
}
