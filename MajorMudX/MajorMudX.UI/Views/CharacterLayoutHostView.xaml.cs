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
using MajorMudX.Core.Infrastructure;
using MajorMudX.Core.Sockets;
using MajorMudX.Core.UI.Text;
using MajorMudX.UI.Utilities.TextProcessing;
using MajorMudX.UI.Infrastructure;

namespace MajorMudX.UI.Views
{
    [CharacterHost]
    public partial class CharacterLayoutHostView : UserControl
    {
        TelnetSocket _socket;
        ISessionController _session;
        string _lastToken;

        public void Write(string msg)
        {
            _session.Write(msg);
        }

        public CharacterLayoutHostView()
        {
            InitializeComponent();
            _lastToken = null;
            _session = new MMXSessionController(new CharacterInfo(), new DeathrowBBS());
        }

        void _socket_MessageRecieved(string message)
        {
            Token[] tokens = Token.Tokenize(message);

            if (tokens.Length > 0 && TextRenderer != null)
            {
                if (_lastToken != null)
                    _lastToken += tokens[0].Text;

                for (int i = 0; i < tokens.Length; ++i)
                {
                    if (_lastToken != null)
                    {
                        if (tokens[i].Complete)
                            _lastToken = null;
                    }
                    else if (tokens[i].Text.StartsWith("[HP"))
                    {
                        string current = tokens[i].Text.Remove(tokens[i].Text.IndexOf(']')).Split('=')[1];
                        if (current.Contains(' '))
                            current = current.Remove(current.IndexOf(' '));
                        int n;
                        if (int.TryParse(current, out n))
                        {
                            if (Status != null)
                                Status.CurrentHP = n;
                            else
                                TextRenderer.RenderText(new DisplayText() { Text = "Bad Monkey", TextColor = Colors.Blue });
                        }
                        else
                            TextRenderer.RenderText(new DisplayText() { Text = "Bad Monkey", TextColor = Colors.Red });
                        continue; // don't show status updates
                    }
                    else
                        tokens[i].TextColor = Colors.Gray;

                    TextRenderer.RenderText(tokens[i]);
                }
            }
        }

        //http://www.majormud.com/mudhelp_gamesettings.html

        public ITextRenderer TextRenderer { get; set; }
        public StatusView Status { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.HasElevatedPermissions && Application.Current.IsRunningOutOfBrowser)
            {
                try
                {
                    _session.SessionMessageRecieved += new EventHandler<SessionMessageEventArgs>(_session_SessionMessageRecieved);

                    if (_session.CurrentState == SessionState.NOT_STARTED)
                        _session.Login();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
                }
            }
        }

        void _session_SessionMessageRecieved(object sender, SessionMessageEventArgs e)
        {
            _socket_MessageRecieved(e.Message);
        }
    }
}
