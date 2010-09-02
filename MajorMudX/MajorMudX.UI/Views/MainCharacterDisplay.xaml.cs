using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MajorMudX.Core.UI;
using MajorMudX.Core.Sockets;
using System.Text;
using MajorMudX.Core.UI.Text;
using MajorMudX.UI.Utilities;
using MajorMudX.Core.Injection;
using MajorMudX.Core.Utilities.Text;

namespace MajorMudX.UI.Views
{
    [CharacterHost]
    public partial class MainCharacterDisplay : UserControl
    {
        TelnetSocket _socket;
        ObjectResolver _resolver;
        IFormattedTextSegment _lastSegment;

        public MainCharacterDisplay()
        {
            InitializeComponent();

            if (Application.Current.IsRunningOutOfBrowser)
            {
                _resolver = new ObjectResolver();
                _resolver.Create<ITextDecorator, MMXTextDecorator>();
                _socket = new TelnetSocket("MajorMUD.DontExist.com", 23);
                _socket.MessageRecieved += new TelnetSocket.IncomingMessageHandler(_socket_MessageRecieved);
                _socket.Connect();
                textTrapping.Focus();
            }
        }

        void _socket_MessageRecieved(string message)
        {
            AddNewRun(message);
        }

        void AddNewRun(string message)
        {
            if (this.Dispatcher.CheckAccess())
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in message)
                {
                    if (c == '\b' && sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    else if (c == '\b')
                    {
                        Run prev = MainTextArea.Inlines[MainTextArea.Inlines.Count - 1] as Run;
                        if (prev != null)
                            prev.Text = prev.Text.Remove(prev.Text.Length - 1);
                    }
                    else if (c != '\r')
                        sb.Append(c);
                }

                ITextDecorator decorator = _resolver.Get<ITextDecorator>();
                IFormattedTextSegment[] segments = decorator.ProcessText(sb.ToString());

                for (int i = 0; i < segments.Length; ++i)
                {
                    if (segments[i].Text.Length == 0) continue;

                    if (_lastSegment != null && !_lastSegment.Complete && MainTextArea.Inlines.Count > 0)
                    {
                        Run r = MainTextArea.Inlines[MainTextArea.Inlines.Count - 1] as Run;
                        r.Text += segments[i].Text;
                        if (((SolidColorBrush)r.Foreground).Color == decorator.DefaultColor)
                            r.Foreground = new SolidColorBrush(segments[i].TextColor);
                    }
                    else
                    {
                        Run r = new Run();
                        r.Text = segments[i].Text;
                        r.Foreground = new SolidColorBrush(segments[i].TextColor);
                        MainTextArea.Inlines.Add(r);
                    }

                    _lastSegment = segments[i];
                }

                MainTextViewer.UpdateLayout();
                MainTextViewer.ScrollToVerticalOffset(double.MaxValue);
            }
            else
                Dispatcher.BeginInvoke(() => { AddNewRun(message); });
        }

        private void textTrapping_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textTrapping.Text))
                return;
            string s = textTrapping.Text;
            textTrapping.Text = string.Empty;

            if (_socket != null)
                _socket.Write(s);
        }

        private void textTrapping_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _socket.Write("\n");
            else if (e.Key == Key.Back)
                _socket.Write("\b");
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            textTrapping.Focus();
        }
    }
}
