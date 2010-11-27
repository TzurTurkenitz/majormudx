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

namespace MMX.Core.API.Infrastructure.Command
{
    public class EventBinding
    {
        ICommand _cmd;
        string _targetEvent;

        public EventBinding(ICommand cmd, string targetEvent)
        {
            _cmd = cmd;
            _targetEvent = targetEvent;
        }

        public ICommand Command { get { return _cmd; } }
        public string TargetEvent { get { return _targetEvent; } }
    }
}
