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

namespace MajorMudX.Core.UI.Text
{
    public class DisplayText
    {
        string _innerText;
        Color _textColor;

        public string Text
        {
            get { return _innerText; }
            set { _innerText = value; }
        }

        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public DisplayText()
        {
        }
    }
}
