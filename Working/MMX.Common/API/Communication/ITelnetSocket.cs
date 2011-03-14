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

namespace MMX.Common.API.Communication
{
    public interface ITelnetSocket : IDisposable
    {
        void Open();
        void Close();

        int BufferSize { get; set; }

        int Write(byte[] buffer, int offset, int length);

        bool RegisterProcessor<T>(T processor)
            where T : ITelnetProcessor;

        event EventHandler<TelnetEventArgs> OnOpening;
        event EventHandler<TelnetEventArgs> OnOpened;
        event EventHandler<TelnetEventArgs> OnClosing;
        event EventHandler<TelnetEventArgs> OnClosed;
    }
}
