﻿using System;
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

namespace MajorMudX.Core.Utilities.Text
{
    public interface ITextDecorator
    {
        IFormattedTextSegment[] ProcessText(string text);
        Color DefaultColor { get; }
    }
}
