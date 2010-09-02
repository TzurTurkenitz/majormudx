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
using MajorMudX.Core.UI.Text;

namespace MajorMudX.UI.Utilities.TextProcessing
{
    public sealed class Token : IFormattedTextSegment
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public bool Complete { get { return Text.EndsWith("\n") || Text.EndsWith(":"); } }

        public static Token[] Tokenize(string s)
        {
            // Start by breaking down into lines
            string[] lines = s.Split('\n');
            for (int i = 0; i < lines.Length - 1; ++i)
                lines[i] += "\n";

            Token[] tokens = new Token[0];

            for (int i = 0; i < lines.Length; ++i)
            {
                // Break down into line tokens
                string[] lineTokens = lines[i].Split(':');
                for (int n = 0; n < lineTokens.Length - 1; ++n)
                    lineTokens[n] += ":";

                // Append each one to the array
                int l = tokens.Length;
                Array.Resize<Token>(ref tokens, l + lineTokens.Length);
                for (int n = 0; n < lineTokens.Length; ++n)
                    tokens[l + n] = new Token() { Text = lineTokens[n] };
            }

            // Return the list of token text blocks
            return tokens;
        }
    }
}
