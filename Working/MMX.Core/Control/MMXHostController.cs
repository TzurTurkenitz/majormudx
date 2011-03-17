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
using MMX.Core.UI;
using MMX.Common.API.UI;

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

            new ViewModelLocator()[ServiceConstants.MMXHostVM] = new MMXHostViewModel();

            (new ViewModelLocator()[ServiceConstants.MMXHostVM] as MMXHostViewModel).Container.Register(new TestView(), "test");
            (new ViewModelLocator()[ServiceConstants.MMXHostVM] as MMXHostViewModel).Container.Activate("test");

            HostUI = new MMXHost();
            
        }

        public MMXHost HostUI { get; set; }
    }
}
