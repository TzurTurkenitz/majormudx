using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Media;
using MajorMudX.Core.UI.Text;

namespace MajorMudX.Core.Sockets
{
    public class TelnetSocket
    {
        private Queue<byte[]> _buffer;
        private Socket _socket;
        private EndPoint _address;
        private TelnetOptionFlags _flags;
        private TelnetOptionFlags _negotiated;
        private AutoResetEvent _writeLock;
        private byte[] _overflow;
        private bool _nawsEnabled = false;

        public string IPAddress { get; set; }

        public TelnetSocket(string address)
        {
            _address = new DnsEndPoint(address, 23);
            _buffer = new Queue<byte[]>();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _flags = TelnetOptionFlags.Echo | TelnetOptionFlags.SupressGoAhead | TelnetOptionFlags.NAWS;
            _negotiated = TelnetOptionFlags.None;
            _writeLock = new AutoResetEvent(false);
        }

        public TelnetSocket(string address, TelnetOptionFlags options)
            : this(address)
        {
            _flags |= options;
        }

        public bool IsEnabled(TelnetOptionFlags flag) { return (_flags & flag) > 0; }
        public void Enable(TelnetOptionFlags flag) { _flags |= flag; }
        public void Disable(TelnetOptionFlags flag) { _flags &= ~flag; }

        public bool Connected { get { return _socket.Connected; } }

        public event EventHandler<TelnetNegotiationEventArgs> NegotiationCompleted;
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Connect()
        {
            if (_socket.Connected) return;

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = _socket;
            args.RemoteEndPoint = _address;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncCompletedHandler);

            // initialize the overflow
            _overflow = new byte[0];

            // start a new thread to watch for responses
            new Thread(new ThreadStart(ParseResponse)).Start();

            _socket.ConnectAsync(args);
        }

        public void Write(string message)
        {
            if (_socket.Connected)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                message = message.Replace("\n", "\n\r");
                byte[] data = new byte[message.Length];
                for (int i = 0; i < data.Length; ++i)
                {
                    char c = message[i];
                    if (c == '\b')
                    {
                        Array.Resize<byte>(ref data, data.Length + 1);
                        data[i++] = (byte)TelnetCommands.IAC;
                        data[i] = (byte)TelnetCommands.EraseCharacter;
                    }
                    else
                        data[i] = (byte)c;
                }

                args.SetBuffer(data, 0, data.Length);
                args.UserToken = _socket;
                args.RemoteEndPoint = _address;
                args.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncCompletedHandler);

                _socket.SendAsync(args);
            }
        }

        void AsyncCompletedHandler(object sender, SocketAsyncEventArgs e)
        {
            if (e.ConnectByNameError != null)
                return; // handle the error later

            if (e.SocketError != SocketError.Success)
                return; // handle the error later

            if (e.LastOperation == SocketAsyncOperation.Receive)
                EnqueueResponse(e);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = _socket;
            args.RemoteEndPoint = _address;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncCompletedHandler);
            args.SetBuffer(new byte[1024], 0, 1024);
            _socket.ReceiveAsync(args);
        }

        void EnqueueResponse(SocketAsyncEventArgs e)
        {
            lock (_buffer)
            {
                byte[] data = new byte[e.BytesTransferred];
                Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                _buffer.Enqueue(data);
            }

            // notify the parser that it can proceed
            _writeLock.Set();
        }

        const byte ESC = 0x1B;

        void ParseResponse()
        {
            // track the current color that should be applied to the text
            Color currentColor = Colors.Cyan;//Color.FromArgb(0xFF, 0x0, 0x0, 0x0);

            do
            {
                // wait for input
                _writeLock.WaitOne();

                // reset the lock
                _writeLock.Reset();

                // create an array to hold the bytes
                byte[] data = new byte[_overflow.Length];

                // copy any overflow bytes into the array
                Array.Copy(_overflow, data, _overflow.Length);

                // clear the overflow
                _overflow = new byte[0];

                // lock the buffer to keep additional data entry
                lock (_buffer)
                {
                    // append each block in the buffer to the data array
                    while (_buffer.Count > 0)
                    {
                        byte[] next = _buffer.Dequeue();
                        Array.Resize<byte>(ref data, data.Length + next.Length);
                        Array.Copy(next, 0, data, data.Length - next.Length, next.Length);
                    }
                }

                // begin processing the bytes

                // check if consumer supports ansi
                bool ansi = _flags.Contains(TelnetOptionFlags.ANSI);

                // create a list to hold the segments
                List<IFormattedTextSegment> segments = new List<IFormattedTextSegment>();

                // create a string builder to construct segments
                StringBuilder segment = new StringBuilder();

                // create a byte array to store any response to server requests
                byte[] response = new byte[0];

                // process the entire byte stream
                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == ESC) // ansi escape
                    {
                        Color background = Colors.Black;
                        int row = 0, col = 0;

                        if (segment.Length > 0)
                        {
                            segments.Add(new DisplayText() { Text = segment.ToString(), TextColor = currentColor });
                            segment.Clear();
                        }

                        int processed = AnsiProcessor.ReadCommand(data, i, ref row, ref col, ref background, ref currentColor);

                        if (processed < 0) // fragment
                        {
                            _overflow = new byte[data.Length - i];
                            Array.Copy(data, i, _overflow, 0, _overflow.Length);
                            break;
                        }


                        i += processed; // advance past the command

                        continue;
                    }
                    else if (data[i] == TelnetCommands.IAC.Translate()) // process the telnet command
                    {
                        TelnetCommands command = data[++i].ToTelnetCommand();

                        // subnegotiation so there will be more data
                        if (command == TelnetCommands.Subnegotation)
                        {
                        }
                        // delete a character
                        else if (command == TelnetCommands.EraseCharacter)
                        {
                        }
                        // delete the line
                        else if (command == TelnetCommands.EraseLine)
                        {
                        }
                        else
                        {
                            // regular negotiation
                            TelnetOptions option = data[++i].ToTelnetOption();

                            Array.Resize<byte>(ref response, response.Length + 3);
                            response[response.Length - 3] = TelnetCommands.IAC.Translate();

                            // check to see if that option is supported
                            if (_flags.Contains(option.ToFlag()))
                            {
                                if (command == TelnetCommands.Will || command == TelnetCommands.Do)
                                    response[response.Length - 2] = TelnetCommands.Will.Translate();
                                else
                                    response[response.Length - 2] = TelnetCommands.Wont.Translate();

                                if (option == TelnetOptions.NAWS && (command == TelnetCommands.Will))
                                    _nawsEnabled = true;
                            }
                            else // tell the server not to support this
                            {
                                if (command == TelnetCommands.Will)
                                    response[response.Length - 2] = TelnetCommands.Dont.Translate();
                                else
                                    response[response.Length - 2] = TelnetCommands.Wont.Translate();
                            }

                            response[response.Length - 1] = option.Translate();

                            _negotiated |= option.ToFlag();
                        }
                        continue;
                    }

                    // append the data to the segment
                    char c = (char)data[i];
                    if (c == '\b')
                    {
                        if (segment.Length > 0)
                            segment.Remove(segment.Length - 1, 1);
                    }
                    else
                        segment.Append(c);
                }

                // check for server negotiation
                if (response.Length > 0)
                {
                    // add any additional flags that haven't been negotiated
                    foreach (TelnetOptionFlags option in _flags.ParseOptions())
                    {
                        // this has already been negotiated
                        if (_negotiated.Contains(option)) continue;

                        // request the ones that we are comfortable with so far
                        switch (option)
                        {
                            case TelnetOptionFlags.SupressGoAhead: goto case TelnetOptionFlags.NAWS;
                            case TelnetOptionFlags.Echo: goto case TelnetOptionFlags.NAWS;
                            case TelnetOptionFlags.NAWS:
                                Array.Resize<byte>(ref response, response.Length + 3);

                                response[response.Length - 3] = TelnetCommands.IAC.Translate();
                                response[response.Length - 2] = TelnetCommands.Do.Translate();
                                response[response.Length - 1] = option.Translate().Translate();

                                break;
                        }

                        _negotiated |= option;
                    }

                    // create the socket event args
                    SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                    args.UserToken = _socket;
                    args.RemoteEndPoint = _address;
                    args.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncCompletedHandler);

                    // put the response in the buffer
                    args.SetBuffer(response, 0, response.Length);

                    // send the response to the server
                    _socket.SendAsync(args);
                }

                // notify any listeners
                if (MessageRecieved != null)
                {
                    segments.Add(new DisplayText() { Text = segment.ToString(), TextColor = currentColor });
                    MessageRecieved(this, new MessageRecievedEventArgs()
                    {
                        Segments = segments.ToArray()
                    });
                }

                // end byte processing
            } while (_socket.Connected);
        }
    }
}
