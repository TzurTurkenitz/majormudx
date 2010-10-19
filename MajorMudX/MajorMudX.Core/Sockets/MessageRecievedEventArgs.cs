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
using MajorMudX.Core.UI.Text;

namespace MajorMudX.Core.Sockets
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public IFormattedTextSegment[] Segments { get; set; }
    }
}
