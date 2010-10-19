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
using System.Collections.Generic;
using System.Text;

namespace MajorMudX.Core.Sockets
{
    internal class AnsiProcessor
    {
        public static byte ESC = 0x1b;
        public static byte LB = 0x5b;

        public static HashSet<char> EndMarkers = new HashSet<char>()
            {
                'A','B','C','D','E','F','G','H','J','K','S','T','f','m','n','s','u','l'
            };

        public static bool IsANSICommand(byte b, byte b1) { return b == 0x1b && b1 == 0x5b; }
        public static bool IsTerminating(byte b) { return b >= 0x40 && b <= 0x7e; }
        public static bool IsParameter(byte b) { return b >= 0x30 && b <= 0x3f; }
        public static bool IsIntermediate(byte b) { return b >= 0x20 && b <= 0x2f; }

        static bool fbright = false;
        static bool bbright = false;

        static Dictionary<int, Color> colorsNormal = new Dictionary<int, Color>()
        {
            {30, Color.FromArgb(0xFF, 0x0, 0x0, 0x0)},
            {31, Color.FromArgb(0xFF, 0x80, 0x0, 0x0)},
            {32, Color.FromArgb(0xFF, 0x0, 0x80, 0x0)},
            {33, Color.FromArgb(0xFF, 0x80, 0x80, 0x0)},
            {34, Color.FromArgb(0xFF, 0x0, 0x0, 0x80)},
            {35, Color.FromArgb(0xFF, 0x80, 0x0, 0x80)},
            {36, Color.FromArgb(0xFF, 0x0, 0x80, 0x80)},
            {37, Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0)}
        };

        static Dictionary<int, Color> colorsBright = new Dictionary<int, Color>()
        {
            {30, Color.FromArgb(0xFF, 0x80, 0x80, 0x80)},
            {31, Color.FromArgb(0xFF, 0xFF, 0x0, 0x0)},
            {32, Color.FromArgb(0xFF, 0x0, 0xFF, 0x0)},
            {33, Color.FromArgb(0xFF, 0xFF, 0xFF, 0x0)},
            {34, Color.FromArgb(0xFF, 0x0, 0x0, 0xFF)},
            {35, Color.FromArgb(0xFF, 0xFF, 0x0, 0xFF)},
            {36, Color.FromArgb(0xFF, 0x0, 0xFF, 0xFF)},
            {37, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)}
        };

        /* ----------------------------------------------------------
         * ANSI Command Sequence
         * ----------------------------------------------------------
         * 1. All ANSI Commands start with "\e[" (bytes : 0x1b,0x5b).
         * 2. Commands will then have a collection of parameter bytes
         * 3. Parameter bytes are followed by intermediate bytes
         * 4. Itermediate bytes will be followed by a final byte
         * 
         * Any command that doesn't follow this sequence is invalid
         */

        public static int ReadCommand(byte[] buffer, int offset, ref int cursorRow, ref int cursorColumn, ref Color backgroundColor, ref Color foregroundColor)
        {
            int read = -1;

            // make sure there is enough to make up a valid command
            if (offset + 3 < buffer.Length)
            {
                // validate this is a proper CSI command
                if (IsANSICommand(buffer[offset], buffer[offset + 1]))
                {
                    int idx = offset + 2;
                    StringBuilder parameters = new StringBuilder();
                    StringBuilder intermediates = new StringBuilder();
                    char final;

                    // read the parameters
                    while (idx < buffer.Length && IsParameter(buffer[idx]))
                        parameters.Append((char)buffer[idx++]);

                    // read the intermediate options
                    while (idx < buffer.Length && IsIntermediate(buffer[idx]))
                        intermediates.Append((char)buffer[idx++]);

                    // read the final character
                    if (idx < buffer.Length && IsTerminating(buffer[idx]))
                    {
                        // get the final character
                        final = (char)buffer[idx];

                        // TODO: add the processing here
                        if (final == 'm') // graphic processing
                        {
                            if (parameters.Length == 0) // reset to defaults
                            {
                                foregroundColor = colorsNormal[37];
                            }
                            else
                            {
                                string[] values = parameters.ToString().Split(';');

                                int bright = -1;
                                int v;
                                foreach (string value in values)
                                {
                                    if (int.TryParse(value, out v))
                                    {
                                        if (v < 30) // brightness
                                        {
                                            bright = v;
                                        }
                                        else if (v < 40) // foreground
                                        {
                                            fbright = bright == 1;

                                            if (v > 37)
                                                v = 37;

                                            foregroundColor = fbright ? colorsBright[v] : colorsNormal[v];

                                            bright = -1; // clear value
                                        }
                                        else // background
                                        {
                                            bbright = bright == 1;

                                            if (v > 47)
                                                v = 47;

                                            v -= 10;

                                            backgroundColor = bbright ? colorsBright[v] : colorsNormal[v];

                                            bright = -1; // clear value
                                        }
                                    }
                                }
                            }
                        }

                        // mark the number of bytes read on invalid command
                        read = idx - offset;

                    }
                    else if (idx < buffer.Length && parameters.Length == 0) // no parameters
                    {
                        // mark the number of bytes read on invalid command
                        read = idx - offset;
                    }
                }
                else // advance past the escape sequence
                    read = 2;
            }

            return read;
        }
    }
}
