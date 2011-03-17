using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MMX.Common.API.UI
{
    public class MMXContentSite : UserControl, IMMXContentContainer
    {
        Dictionary<string, UserControl> _views;
        protected Canvas _root;

        public MMXContentSite()
        {
            _views = new Dictionary<string, UserControl>();
            _root = null;
        }

        bool IMMXContentContainer.Register<T>(T obj, string id)
        {
            if (!_views.ContainsKey(id))
                _views.Add(id, null);

            _views[id] = obj;

            return true;
        }

        bool IMMXContentContainer.Remove(string id)
        {
            return _views.Remove(id);
        }

        void IMMXContentContainer.Activate(string id)
        {
            if (_root != null && _views.ContainsKey(id))
                _root.Dispatcher.BeginInvoke(
                    () => { _root.Children.Clear(); _root.Children.Add(_views[id]); });
        }
    }
}
