﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MMX.Common.API.UI
{
    public sealed partial class MMXContentContainer : MMXContentSite
    {
        public MMXContentContainer()
        {
            InitializeComponent();
            _root = LayoutRoot;
        }
    }
}
