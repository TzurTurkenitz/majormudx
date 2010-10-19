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

namespace MajorMudX.Core.Sockets
{
    public class TelnetNegotiationEventArgs : EventArgs
    {
        public TelnetOptionFlags Option { get; set; }
        public bool Success { get; set; }
    }
}
