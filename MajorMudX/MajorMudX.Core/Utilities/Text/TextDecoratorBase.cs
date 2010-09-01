using System.Windows.Media;
using MajorMudX.Core.UI.Text;

namespace MajorMudX.Core.Utilities.Text
{
    public abstract class TextDecoratorBase : ITextDecorator
    {
        public virtual DisplayText[] ProcessText(string text)
        {
            return new DisplayText[] { new DisplayText() { Text = text, TextColor = Colors.LightGray } };
        }
    }
}
