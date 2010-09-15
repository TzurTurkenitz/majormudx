using System;
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
using MajorMudX.Core.Infrastructure;
using MajorMudX.Core.UI.Text;

namespace MajorMudX.UI.Views
{
    public partial class TextWindowView : UserControl, ITextRenderer
    {
        List<IFormattedTextSegment> _incompleteSegments;
        CharacterLayoutHostView _host;

        public TextWindowView()
        {
            InitializeComponent();
            _incompleteSegments = new List<IFormattedTextSegment>();
        }

        private void LayoutRoot_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbxCaptureText.Focus();
        }

        private void tbxCaptureText_KeyDown(object sender, KeyEventArgs e)
        {
            string s = tbxCaptureText.Text;
            if (e.Key == Key.Enter)
                s += "\n";
            else if (e.Key == Key.Back)
                s += "\b";
            ClearText();

            if (_host != null)
                _host.Write(s);
        }

        void ClearText()
        {
            if (Dispatcher.CheckAccess()) tbxCaptureText.Text = string.Empty;
            else Dispatcher.BeginInvoke(() => { ClearText(); });
        }

        public void ProcessBackspace()
        {
            if (Dispatcher.CheckAccess())
            {
                if (tblkMainContent.Inlines.Count == 0) return;

                Run current = tblkMainContent.Inlines[tblkMainContent.Inlines.Count - 1] as Run;
                if (current == null) return;
                if (current.Text.Length == 0) return;
                current.Text = current.Text.Remove(current.Text.Length - 2);
            }
            else Dispatcher.BeginInvoke(() => { ProcessBackspace(); });
        }

        public void RenderText(IFormattedTextSegment segment)
        {
            if (Dispatcher.CheckAccess())
            {
                if (!segment.Complete)
                {
                    _incompleteSegments.Add(segment);
                }
                else
                {
                    int start = tblkMainContent.Inlines.Count - _incompleteSegments.Count;
                    start = start < 0 ? 0 : start;

                    for (int i = start; i < tblkMainContent.Inlines.Count; ++i)
                    {
                        Run prev = tblkMainContent.Inlines[i] as Run;
                        if (prev == null) continue;
                        prev.Foreground = new SolidColorBrush(segment.TextColor);
                    }

                    _incompleteSegments.Clear();
                }

                Run r = new Run();
                r.Text = segment.Text;
                r.Foreground = new SolidColorBrush(segment.TextColor);

                tblkMainContent.Inlines.Add(r);

                svMainContent.UpdateLayout();
                svMainContent.ScrollToVerticalOffset(double.MaxValue);
            }
            else Dispatcher.BeginInvoke(() => { RenderText(segment); });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = Parent;
            while (parent != null && !typeof(CharacterLayoutHostView).IsAssignableFrom(parent.GetType()))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                _host = parent as CharacterLayoutHostView;
                if (_host != null)
                    _host.TextRenderer = this;
            }
        }
    }
}
