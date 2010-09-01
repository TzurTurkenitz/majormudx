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

namespace MajorMudX.Core.ServerManagement
{
    public sealed class ServerDetails
    {
        string _name, _dnsAddress;
        int _port;

        public ServerDetails()
        {
            _name = string.Empty;
            _dnsAddress = string.Empty;
            _port = -1;
        }

        public ServerDetails(string name, string address, int port)
        {
            _name = name;
            _dnsAddress = address;
            _port = port;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Address
        {
            get { return _dnsAddress; }
            set { _dnsAddress = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
    }
}
