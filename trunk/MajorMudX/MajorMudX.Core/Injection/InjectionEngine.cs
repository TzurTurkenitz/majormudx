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

namespace MajorMudX.Core.Injection
{
    public sealed class InjectionEngine
    {
        static InjectionEngine _engine;

        public static InjectionEngine Instance
        {
            get
            {
                if (_engine == null)
                    _engine = new InjectionEngine();

                return _engine;
            }
        }

        private InjectionEngine()
        {
        }
    }
}
