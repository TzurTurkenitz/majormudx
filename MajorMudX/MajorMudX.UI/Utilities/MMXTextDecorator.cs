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
using MajorMudX.Core.Utilities.Text;

using System.Text.RegularExpressions;
using MajorMudX.Core.UI.Text;
using System.Collections.Generic;
using MajorMudX.UI.Utilities.TextProcessing;

namespace MajorMudX.UI.Utilities
{
    public class MMXTextDecorator : TextDecoratorBase
    {
        static Regex _damage;
        static Regex _combatMarker;
        static Regex _movement;
        static Regex _alsoHere;
        static Regex _exits;
        static Regex _status;

        static RegexOptions _options = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        string overflow = string.Empty;
        bool nextSet = false;
        Color nextColor = Colors.Black;

        static MMXTextDecorator()
        {
            _damage = new Regex(".*damage[.|!]$", _options);
            _combatMarker = new Regex(@"^\*Combat.*", _options);
            _movement = new Regex(@".*room.*from.*", _options);
            _alsoHere = new Regex(@"^Also here:", _options);
            _exits = new Regex(@"^Obvious exits:", _options);
            _status = new Regex(@"^\[.*\]:$", _options);
        }

        public override IFormattedTextSegment[] ProcessText(string text)
        {
            IFormattedTextSegment[] tokens = Token.Tokenize(text);

            Token prev = null;
            for (int i = 0; i < tokens.Length; ++i)
            {
                if (prev != null && prev.Complete && nextSet)
                {
                    tokens[i].TextColor = nextColor;
                    nextSet = !tokens[i].Complete;
                }
                else
                {
                    if (i == 0 && overflow.Length > 0)
                        tokens[i].TextColor = PaintToken(overflow + tokens[i].Text);
                    else
                        tokens[i].TextColor = PaintToken(tokens[i].Text);
                }

                prev = (Token)tokens[i];

                if (i == tokens.Length - 1 && !(tokens[i].Text.EndsWith("\n") || tokens[i].Text.EndsWith(":")))
                    overflow = tokens[i].Text;
                else
                    overflow = string.Empty;
            }

            return tokens;
        }

        Color PaintToken(string token)
        {
            if (_damage.IsMatch(token))
                return Colors.Red;
            if (_movement.IsMatch(token))
                return Colors.Yellow;
            if (_combatMarker.IsMatch(token))
                return Colors.Orange;
            if (_alsoHere.IsMatch(token))
            {
                nextSet = true;
                nextColor = Colors.Magenta;
                return Colors.Purple;
            }
            if (_exits.IsMatch(token))
            {
                nextSet = true;
                nextColor = Colors.Green;
                return Colors.Cyan;
            }

            return DefaultColor;
        }

        public new Color DefaultColor { get { return Colors.LightGray; } }
    }
}
