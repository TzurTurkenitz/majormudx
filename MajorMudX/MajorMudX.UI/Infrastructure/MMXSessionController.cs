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

        TelnetSocket _socket;

        int _loginIdx;
        string _overflow;

        Timer _loginTimer;

        public MMXSessionController(ISessionCredentials credentials, IBBSInfo bbs)
        {
            _credentials = credentials;
            _bbs = bbs;

            _socket = new TelnetSocket(bbs.Address);
            _socket.MessageRecieved += new TelnetSocket.IncomingMessageHandler(ProcessSocketMessage);

            CurrentState = SessionState.NOT_STARTED;
        }

        public void Write(string message)
        {
            if (_socket.IsConnected)
                _socket.Write(message);
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
            if (_socket.IsConnected) throw new InvalidOperationException("Login Sequence already executed!");

            CurrentState = _bbs.PreLoginSequence.Length > 0 ? SessionState.PRE_LOGIN : SessionState.LOGIN;
            _loginIdx = 0;
            _overflow = string.Empty;
            _socket.Connect();
            _loginTimer = new Timer(new TimerCallback(TimerElapsed), null, 100, -1);
        }

        void WriteSessionMessage(string message)
        {
            if (SessionMessageRecieved != null)
                SessionMessageRecieved(this, new SessionMessageEventArgs(message));
        }

        void TimerElapsed(object state)
        {
            _loginTimer = null;
            IRequestResponse resp = _bbs.LoginSequence[_loginIdx];
            if (_overflow.Contains(resp.Request))
            {
                lock (_overflow)
                {
                    _overflow = string.Empty;
                }

                CurrentState = _loginIdx++ == _bbs.LoginSequence.Length - 1 ? SessionState.CONNECTED : SessionState.LOGIN;

                _socket.Write(resp.Response);

                if (CurrentState == SessionState.CONNECTED) return;
            }
            else _socket.Write("\n");

            _loginTimer = new Timer(new TimerCallback(TimerElapsed), null, 100, -1);
        }

        public SessionState CurrentState { get; private set; }

        public event EventHandler<SessionMessageEventArgs> SessionMessageRecieved;
    }
}
