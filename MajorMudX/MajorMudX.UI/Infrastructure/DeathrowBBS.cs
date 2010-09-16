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
    public class DeathrowBBS : IBBSInfo
    {
        public string Address
        {
            get { return "deathrow.dyndns.org"; }
        }

        public IRequestResponse[] LoginSequence
        {
            get
            {
                return new IRequestResponse[] 
                {
                    new BBSRequestResponse(){Request="Identity:", Response="mmxtest\n"},
                    new BBSRequestResponse(){Request="Password:", Response="pa55w0rd\n"},
                    new BBSRequestResponse(){Request="(C)ontinue?", Response="N\n"},
                    new BBSRequestResponse(){Request=", or X to exit):", Response="R\n"},
                    new BBSRequestResponse(){Request="[MAJORMUD]:", Response="E\n"}
                };
            }
        }

        public IRequestResponse[] PreLoginSequence
        {
            get { return new IRequestResponse[] { }; }
        }
    }
}
