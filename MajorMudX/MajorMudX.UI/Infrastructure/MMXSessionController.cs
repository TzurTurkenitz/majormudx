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
using MajorMudX.Core.Infrastructure;
using MajorMudX.Core.Sockets;
using System.Threading;

namespace MajorMudX.UI.Infrastructure
{
    public class MMXSessionController : ISessionController
    {
        ISessionCredentials _credentials;
        IBBSInfo _bbs;


        int _loginIdx;
        string _overflow;

        Timer _loginTimer;

        public MMXSessionController(ISessionCredentials credentials, IBBSInfo bbs)
        {
            _credentials = credentials;
            _bbs = bbs;


            CurrentState = SessionState.NOT_STARTED;
        }

        public void Write(string message)
        {
        }

        void ProcessSocketMessage(string message)
        {
            switch (CurrentState)
            {
                case SessionState.NOT_STARTED: break;
                case SessionState.PRE_LOGIN: break;
                case SessionState.LOGIN:
                    WriteSessionMessage(message);
                    _overflow += message; break;
                case SessionState.CONNECTED:
                    WriteSessionMessage(message);
                    break;
                default: break;
            }
        }

        public void Login()
        {
        }

        void WriteSessionMessage(string message)
        {
            if (SessionMessageRecieved != null)
                SessionMessageRecieved(this, new SessionMessageEventArgs(message));
        }

        void TimerElapsed(object state)
        {
        }

        public SessionState CurrentState { get; private set; }

        public event EventHandler<SessionMessageEventArgs> SessionMessageRecieved;
    }
}
