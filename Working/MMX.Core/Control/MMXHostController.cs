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
using MMX.Common.API.Control;
using MMX.Common.API.Infrastructure;
using MMX.Common.API.Services;

namespace MMX.Core.Control
{
    [MMXController]
    public class MMXHostController : IMMXController
    {
        IServiceLocator _locator;

        [MMXDefaultConstructor]
        public MMXHostController([MMXInjectNew(InjectionType = typeof(IServiceLocator))] IServiceLocator locator)
        {
            _locator = locator;
        }
    }
}
