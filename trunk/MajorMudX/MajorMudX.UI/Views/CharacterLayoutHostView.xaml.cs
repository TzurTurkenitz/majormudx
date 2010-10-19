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
using MajorMudX.Core.Utilities.Text;

namespace MajorMudX.UI.Views
{
    [CharacterHost]
    public partial class CharacterLayoutHostView : UserControl
    {
        ISessionController _session;
        TelnetSocket _socket;
        string _lastToken;
        ANSIFormatting _ansiFormatter = new ANSIFormatting();

        public void Write(string msg)
        {
            _socket.Write(msg);
        }

        public CharacterLayoutHostView()
        {
            InitializeComponent();
            _lastToken = null;
            _session = new MMXSessionController(new CharacterInfo(), new MudRevBBS());
            _socket = new TelnetSocket("MajorMud.DontExist.com");
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
                    _socket.MessageRecieved += new EventHandler<MessageRecievedEventArgs>(socket_MessageRecieved);
                    _socket.Connect();
                    //_session.SessionMessageRecieved += new EventHandler<SessionMessageEventArgs>(_session_SessionMessageRecieved);

                    //if (_session.CurrentState == SessionState.NOT_STARTED)
                    //    _session.Login();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
                }
            }
        }

        void socket_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            foreach (IFormattedTextSegment token in e.Segments)
                TextRenderer.RenderText(token);
        }
    }
}
