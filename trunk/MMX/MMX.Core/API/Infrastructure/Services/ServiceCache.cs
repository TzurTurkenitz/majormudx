using System;
using System.Linq;
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

namespace MMX.Core.API.Infrastructure.Services
{
    public class ServiceCache
    {
        Dictionary<Type, object> _services;
        IController _parent;

        internal ServiceCache(IController parent)
        {
            _services = new Dictionary<Type, object>();
            _parent = parent;
        }

        public T Get<T>()
            where T : class
        {
            // Create a null instance
            T svc = null;
            Type serviceType = typeof(T);

            // Validate typing and check if the services contains the type
            if (serviceType.IsInterface &&
                serviceType.IsDefined(typeof(ServiceAttribute), true) &&
                _services.ContainsKey(serviceType))
            {
                svc = _services[serviceType] as T;
            }

            // Check if the parent has access to the service
            if (svc == null && _parent != null)
            {
                svc = _parent.Services.Get<T>();
            }

            // Return the service instance
            return svc;
        }

        public void Add<T>(object svc)
        {
            Type t = typeof(T);
            Type oType = svc.GetType();

            if (t.IsInterface && t.IsDefined(typeof(ServiceAttribute), true)
                && t.IsAssignableFrom(oType))
            {
                if (!_services.ContainsKey(t))
                    _services.Add(t, null);
                _services[t] = svc;
            }
        }
    }
}
