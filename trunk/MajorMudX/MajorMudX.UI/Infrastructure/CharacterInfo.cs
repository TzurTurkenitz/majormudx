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
using MajorMudX.Core.Infrastructure;

namespace MajorMudX.UI.Infrastructure
{
    public class CharacterInfo : ISessionCredentials
    {
        public string Username
        {
            get { return "mmxtest"; }
        }

        public string Password
        {
            get { return "pa55w0rd"; }
        }
    }
}
