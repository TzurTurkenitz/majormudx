using System.Collections.Generic;
using System;
using System.Reflection;

namespace MajorMudX.Core.Sockets
{
    internal enum TelnetOptions : byte
    {
        TransmitBinary = 0x0,
        Echo = 0x1,
        SupressGoAhead = 0x3,
        Status = 0x5,
        TimingMark = 0x6,
        NAOCRD = 0xA,
        NAOHTS = 0xB,
        NAOHTD = 0xC,
        NAOFFD = 0xD,
        NAOVTS = 0xE,
        NAOVTD = 0xF,
        NAOLFD = 0x10,
        ExtendedAscii = 0x11,
        TerminalType = 0x18,
        NAWS = 0x1F,
        TerminalSpeed = 0x20,
        ToggleFlowControl = 0x21,
        LienMode = 0x22,
        Authentication = 0x23
    }

    /// <summary>
    /// Internal class for translating between public flag enum to byte representation.
    /// </summary>
    internal static class TelnetOptionFlagsExtensions
    {
        public static TelnetOptions[] GetFlags(this TelnetOptionFlags flag)
        {
            List<TelnetOptions> options = new List<TelnetOptions>();

            foreach (TelnetOptionFlags tflag in flag.ParseOptions())
                options.Add(tflag.Translate());

            return options.ToArray();
        }

        public static TelnetOptionFlags[] ParseOptions(this TelnetOptionFlags flag)
        {
            List<TelnetOptionFlags> flags = new List<TelnetOptionFlags>();

            foreach (FieldInfo fi in typeof(TelnetOptionFlags).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                TelnetOptionFlags option = (TelnetOptionFlags)fi.GetValue(null);
                if (flag.Contains(option)) flags.Add(option);
            }

            return flags.ToArray();
        }

        public static TelnetOptions Translate(this TelnetOptionFlags flag)
        {
            string name = Enum.GetName(typeof(TelnetOptionFlags), flag);

            return (TelnetOptions)Enum.Parse(typeof(TelnetOptions), name, true);
        }

        public static TelnetOptionFlags ToFlag(this TelnetOptions option)
        {
            string name = Enum.GetName(typeof(TelnetOptions), option);

            return (TelnetOptionFlags)Enum.Parse(typeof(TelnetOptionFlags), name, true);
        }

        public static bool Contains(this TelnetOptionFlags current, TelnetOptionFlags flag)
        {
            return (current & flag) > 0;
        }

        public static byte Translate(this TelnetOptions option)
        {
            return (byte)option;
        }
    }
}
