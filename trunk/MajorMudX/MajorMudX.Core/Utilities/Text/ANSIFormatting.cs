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
using System.Text.RegularExpressions;
using MajorMudX.Core.UI.Text;

namespace MajorMudX.Core.Utilities.Text
{
    public class ANSIFormatting : ITextDecorator
    {
        static char cESC = (char)0x1B;

        static Regex rxResetNormal = new Regex(@"\[0;\d*m.*", RegexOptions.Singleline);
        static Regex rxShorthandNormal = new Regex(@"\[\d*m.*", RegexOptions.Singleline);
        static Regex rxBright = new Regex(@"\[1;\d*m.*", RegexOptions.Singleline);

        static Regex rxCursorUp = new Regex(@"\[\d*A.*", RegexOptions.Singleline);

        static Regex rxANSICmd = new Regex(@"\[(\d*;)*(\d*)+[ABCDEFGHJKSTfmnsul].*", RegexOptions.Singleline);

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

        Color _currentColor = colorsNormal[37];
        string _overflow = string.Empty;

        public IFormattedTextSegment[] ProcessText(string text)
        {
            IFormattedTextSegment[] formattedText = new IFormattedTextSegment[] { };

            lock (_overflow)
            {
                // append any previous overflow
                string input = _overflow + text;

                // clear the overflow for the next pass
                _overflow = string.Empty;

                // find any escape sequence
                int nextEsc = input.IndexOf(cESC);

                // process the sequence if found
                if (nextEsc >= 0)
                {
                    string[] tokens = text.Split(cESC);

                    foreach (string token in tokens)
                    {
                        string parsedText = string.Empty;

                        if (rxANSICmd.IsMatch(token)) // process the command
                        {
                            if (token.Contains(";")) // check for multiple parameters
                            {
                                int m = token.IndexOf('m');
                                int first = int.MaxValue;
                                foreach (char c in "ABCDEFGHJKSTfmnsul")
                                {
                                    int idx = token.IndexOf(c);
                                    if (idx < 0) continue;
                                    first = Math.Min(idx, first);
                                }
                                if (first < m) // not SGR so just remove
                                    parsedText = token.Substring(first + 1);
                                else
                                {
                                    string[] values = token.Split(';');
                                    int cformat = int.Parse(values[0].Substring(1));

                                    for (int i = 1; i < values.Length; ++i)
                                    {
                                        int fm = values[i].IndexOf('m');
                                        int val = fm < 0 ? int.Parse(values[i]) : int.Parse(values[i].Substring(0, fm));
                                        if (val >= 30 && val <= 37)
                                        {
                                            _currentColor = cformat == 0 ? colorsNormal[val] : colorsBright[val];
                                            break;
                                        }
                                    }

                                    parsedText = values[values.Length - 1].Substring(values[values.Length - 1].IndexOf('m') + 1);
                                }
                            }
                            else if (rxShorthandNormal.IsMatch(token)) // check for SGR
                            {
                                int val = int.Parse(token.Substring(1, token.IndexOf('m') - 1));
                                if (val >= 30 && val <= 37)
                                    _currentColor = colorsNormal[val];
                                parsedText = token.Substring(token.IndexOf('m') + 1);
                            }
                            else
                            {
                                // remove the ansi junk and just process the normal text
                                int first = int.MaxValue;
                                foreach (char c in "ABCDEFGHJKSTfmnsul")
                                {
                                    int idx = token.IndexOf(c);
                                    if (idx < 0) continue;
                                    first = Math.Min(idx, first);
                                }
                                parsedText = token.Substring(first + 1);
                            }
                        }
                        else
                            parsedText = token;

                        Array.Resize<IFormattedTextSegment>(ref formattedText, formattedText.Length + 1);
                        formattedText[formattedText.Length - 1] = new DisplayText() { Text = parsedText, TextColor = DefaultColor };
                    }
                }
                else // use the last known color
                {
                    Array.Resize<IFormattedTextSegment>(ref formattedText, 1);
                    formattedText[0] = new DisplayText() { Text = input, TextColor = DefaultColor };
                }
            }

            return formattedText;
        }

        public Color DefaultColor
        {
            get { return _currentColor; }
        }
    }
}
