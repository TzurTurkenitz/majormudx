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

        static RegexOptions _options = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        static MMXTextDecorator()
        {
            _damage = new Regex(".*damage[.|!]", _options);
            _combatMarker = new Regex(@"^\*Combat.*", _options);
            _movement = new Regex(@".*room.*from.*", _options);
        }

        public override IFormattedTextSegment[] ProcessText(string text)
        {
            IFormattedTextSegment[] tokens = Token.Tokenize(text);

            for (int i = 0; i < tokens.Length; ++i)
                tokens[i].TextColor = PaintToken(tokens[i].Text);

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

            return DefaultColor;
        }

        public new Color DefaultColor { get { return Colors.LightGray; } }
    }
}
