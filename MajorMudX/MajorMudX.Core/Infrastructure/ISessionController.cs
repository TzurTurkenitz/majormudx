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

namespace MajorMudX.Core.Infrastructure
{
    public interface ISessionController
    {
        void Login();
        void Write(string message);

        SessionState CurrentState { get; }

        event EventHandler<SessionMessageEventArgs> SessionMessageRecieved;
    }

    public class SessionMessageEventArgs : EventArgs
    {
        public SessionMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }

    public enum SessionState
    {
        NOT_STARTED,
        PRE_LOGIN,
        LOGIN,
        CONNECTED,
        DISCONNECTED
    }
}
