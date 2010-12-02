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

namespace MMX.Core.API.Infrastructure.Services
{
    public class ServiceLocator
    {
        static Dictionary<Type, object> _services;

        static ServiceLocator()
        {
            _services = new Dictionary<Type, object>();
        }

        public ServiceLocator()
        {
        }

        public bool IsRegisterred(Type t)
        {
            return _services.ContainsKey(t);
        }

        public void Register<T>(object obj)
        {
            Type t = typeof(T);
            if (t.IsInterface && t.IsAssignableFrom(obj.GetType()))
            {
                if (!_services.ContainsKey(t))
                    _services.Add(t, null);
                _services[t] = obj;
            }
        }

        public T Get<T>()
            where T : class
        {
            Type t = typeof(T);
            if (t.IsInterface && _services.ContainsKey(t))
                return _services[t] as T;

            return null;
        }
    }
}
