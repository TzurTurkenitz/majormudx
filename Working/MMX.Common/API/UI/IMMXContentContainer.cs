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
    public interface IMMXContentContainer
    {
        bool Register<T>(T obj, string id) where T : UserControl;
        bool Remove(string id);
        void Activate(string id);
    }
}
