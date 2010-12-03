using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MMX.Core.API.Infrastructure.Control;
using MMX.Core.API.Infrastructure.Factories;
using MMX.Core.API.Infrastructure.ViewModels;

namespace MMX.Core.API.Infrastructure.Views
{
    public class ViewCache
    {
        Dictionary<string, UserControl> _views;
        IController _parent;

        internal ViewCache(IController parent)
        {
            _views = new Dictionary<string, UserControl>();
            _parent = parent;
        }

        public T Create<T>(string id)
            where T : UserControl
        {
            // check current chain to find the view
            object v = this[id];
            if (v != null)
                return v as T;

            T instance = Activator.CreateInstance<T>();

            ViewModelInjector injector = null;

            foreach (object key in instance.Resources.Keys)
                if (instance.Resources[key] is ViewModelInjector)
                {
                    injector = instance.Resources[key] as ViewModelInjector;
                    break;
                }

            if (injector != null && injector.AttachAsDataContext)
            {
                instance.DataContext = ViewModelLocator.Get("ShellViewModel");
            }

            _views.Add(id, instance);

            return instance;
        }

        public UserControl this[string key]
        {
            get
            {
                UserControl view = null;

                if(_views.ContainsKey(key))
                    return _views[key];

                if (_parent != null)
                    view = _parent.Views[key];
                
                return view;
            }
            set
            {
                if (!_views.ContainsKey(key))
                    _views.Add(key, null);
                _views[key] = value;
            }
        }
    }
}
