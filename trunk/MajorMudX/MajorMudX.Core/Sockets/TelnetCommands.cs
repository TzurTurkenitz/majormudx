using System.Reflection;
namespace MajorMudX.Core.Sockets
{
    internal enum TelnetCommands : byte
    {
        SubnegotiationEnd = 0xF0,
        NoOp = 0xF1,
        DataMark = 0xF2,
        Break = 0xF3,
        InterruptProcess = 0xF4,
        AbortOutput = 0xF5,
        AreYouThere = 0xF6,
        EraseCharacter = 0xF7,
        EraseLine = 0xF8,
        GoAhead = 0xF9,
        Subnegotation = 0xFA,
        Will = 0xFB,
        Wont = 0xFC,
        Do = 0xFD,
        Dont = 0xFE,
        IAC = 0xFF
    }

    internal static class TelnetCommandsExtensions
    {
        public static bool IsTelnetCommand(this byte b)
        {
            foreach (FieldInfo fi in typeof(TelnetCommands).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                TelnetCommands command = (TelnetCommands)fi.GetValue(null);
                if ((byte)command == b) return true;
            }

            return false;
        }

        public static TelnetCommands ToTelnetCommand(this byte b)
        {
            foreach (FieldInfo fi in typeof(TelnetCommands).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                TelnetCommands command = (TelnetCommands)fi.GetValue(null);
                if ((byte)command == b) return command;
            }

            return TelnetCommands.IAC;
        }

        public static TelnetOptions ToTelnetOption(this byte b)
        {
            foreach (FieldInfo fi in typeof(TelnetOptions).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                TelnetOptions option = (TelnetOptions)fi.GetValue(null);
                if ((byte)option == b) return option;
            }

            return TelnetOptions.SupressGoAhead;
        }

        public static byte Translate(this TelnetCommands command)
        {
            return (byte)command;
        }
    }
}
