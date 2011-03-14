using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MMX.Common.API.Communication;
using MMX.Common.API.Services;

namespace MMX.Core.Telnet
{
    [ServiceRegistration(Id = ServiceConstants.TelnetSocket, ServiceType = typeof(ITelnetSocket))]
    public class TelnetSocket : ITelnetSocket
    {
        EndPoint _endpoint;
        Socket _socket;
        int _bufferSize;
        event EventHandler<TelnetEventArgs> _opening, _opened, _closing, _closed;

        static readonly int TELNET_PORT = 23;

        List<ITelnetProcessor> _processors;

        public TelnetSocket()
            : this("time-a.nist.gov")
        {
        }

        public TelnetSocket(string address)
        {
            _processors = new List<ITelnetProcessor>();
            _endpoint = new DnsEndPoint(address, TELNET_PORT);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        void Connect()
        {
            // Prevent multiple connections
            if (_socket.Connected) return;

            // Create the event args
            SocketAsyncEventArgs args = CreateAsyncArgs();

            // Connect asynchronously
            _socket.ConnectAsync(args);
        }

        void Disconnect()
        {
            // Close the socket
            _socket.Close(500);

            // Notify listeners
            if (_closed != null)
                _closed(this, new TelnetEventArgs());
        }

        SocketAsyncEventArgs CreateAsyncArgs()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = _socket;
            args.RemoteEndPoint = _endpoint;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAsyncComplete);

            return args;
        }

        void OnAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            if (e.ConnectByNameError != null)
            {
            }
            else if (e.SocketError != SocketError.Success)
            {
            }
            else
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Connect:
                        // Notify all listeners
                        if (_opened != null) _opened(this, new TelnetEventArgs());
                        break;
                    case SocketAsyncOperation.Send: break;
                    case SocketAsyncOperation.Receive:
                        // Get the data
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);

                        // Lock the processors to make sure data isn't misaligned
                        lock (_processors)
                        {
                            // Notify the processors
                            for (int i = 0; i < _processors.Count; ++i)
                                _processors[i].ProcessBytes(data);
                        }

                        break;
                    case SocketAsyncOperation.None: break;
                }

                // Create new event args
                SocketAsyncEventArgs args = CreateAsyncArgs();

                // Setup the buffer
                args.SetBuffer(new byte[_bufferSize], 0, _bufferSize);

                // Continue listening
                _socket.ReceiveAsync(args);
            }
        }

        #region ITelnetSocket

        int ITelnetSocket.BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        int ITelnetSocket.Write(byte[] buffer, int offset, int length)
        {
            // Create the args
            SocketAsyncEventArgs args = CreateAsyncArgs();

            // Limit the length to the buffer size
            int len = Math.Min(length, _bufferSize);

            // Create the buffer
            byte[] data = new byte[len];
            Array.Copy(buffer, offset, data, 0, len);

            // Bind the buffer to the args
            args.SetBuffer(data, 0, len);

            // Send the response
            _socket.SendAsync(args);

            // Return the number of bytes written
            return len;
        }

        void ITelnetSocket.Open()
        {
            // Notify listeners the socket is connecting
            if (_opening != null)
                _opening(this, new TelnetEventArgs());

            // Try to connect
            Connect();
        }

        void ITelnetSocket.Close()
        {
            if (_closing != null)
            {
                // Notify listeners that the socket will close
                TelnetEventArgs args = new TelnetEventArgs() { Cancel = false };
                _closing(this, args);

                // Check to see if anyone wanted to cancel the event
                if (args.Cancel)
                    return;
            }

            Disconnect();
        }

        bool ITelnetSocket.RegisterProcessor<T>(T processor)
        {
            throw new NotImplementedException();
        }

        event EventHandler<TelnetEventArgs> ITelnetSocket.OnClosed
        {
            add { _closed += value; }
            remove { _closed -= value; }
        }

        event EventHandler<TelnetEventArgs> ITelnetSocket.OnOpened
        {
            add { _opened += value; }
            remove { _opened -= value; }
        }

        event EventHandler<TelnetEventArgs> ITelnetSocket.OnOpening
        {
            add { _opening += value; }
            remove { _opening -= value; }
        }

        event EventHandler<TelnetEventArgs> ITelnetSocket.OnClosing
        {
            add { _closing += value; }
            remove { _closing -= value; }
        }

        #region IDisposable
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
