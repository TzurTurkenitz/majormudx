using System.Windows.Media;
using MajorMudX.Core.UI.Text;

namespace MajorMudX.Core.Utilities.Text
{
    public abstract class TextDecoratorBase : ITextDecorator
    {
        public virtual IFormattedTextSegment[] ProcessText(string text)
        {
            return new IFormattedTextSegment[] { new DisplayText() { Text = text, TextColor = Colors.LightGray } };
        }

        public Color DefaultColor { get { return Colors.LightGray; } }
    }
}
