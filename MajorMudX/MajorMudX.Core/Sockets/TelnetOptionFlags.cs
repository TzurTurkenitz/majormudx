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

namespace MajorMudX.Core.Sockets
{
    public enum TelnetOptionFlags : int
    {
        None = 0,
        TransmitBinary = 1,
        Echo = 2,
        SupressGoAhead = 4,
        Status = 8,
        TimingMark = 16,
        NAOCRD = 32,
        NAOHTS = 64,
        NAOHTD = 128,
        NAOFFD = 256,
        NAOVTS = 512,
        NAOVTD = 1024,
        NAOLFD = 2048,
        ExtendedAscii = 4096,
        TerminalType = 8192,
        NAWS = 16384,
        TerminalSpeed = 32768,
        ToggleFlowControl = 65536,
        LienMode = 131072,
        Authentication = 262144,
        ANSI = 524288
    }
}
