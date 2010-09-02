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
    public class AppHostController
    {
        AppHostController _parent;

        public AppHostController()
        {
            _parent = null;
        }

        public AppHostController(AppHostController parent)
        {
            _parent = parent;
        }

        public AppHostController Parent
        {
            get { return _parent; }
        }

        public virtual void Load()
        {

        }
    }
}
