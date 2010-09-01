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
using MajorMudX.Core.Sockets;
using MajorMudX.Core.ServerManagement;

namespace MajorMudX.Core.CharacterManagement
{
    public abstract class CharacterBase : ICharacter
    {
        protected TelnetSocket _socket;
        protected ServerDetails _server;

        public virtual void Connect()
        {
            if (_socket != null && !_socket.IsConnected)
            {
                _socket.Connect();
            }
        }
    }
}
