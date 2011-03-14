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
using MMX.Common.API.Services;
using System.Collections.Generic;

namespace MMX.Core.Services
{
    public class ServiceLocator : IServiceLocator
    {
        Dictionary<string, object> _services;

        public ServiceLocator()
        {
            _services = new Dictionary<string, object>();
        }

        void Add(object obj, string id)
        {
            if (!_services.ContainsKey(id))
                _services.Add(id, null);

            _services[id] = obj;
        }

        void IServiceLocator.RegisterMMXService<T>(T obj, string id)
        {
            Add(obj, id);
        }

        string IServiceLocator.RegisterMMXService<T>(T obj)
        {
            string id = Guid.NewGuid().ToString();
            Add(obj, id);
            return id;
        }

        void IServiceLocator.RegisterTelnet<T>(T obj, string id)
        {
            Add(obj, id);
        }

        string IServiceLocator.RegisterTelnet<T>(T obj)
        {
            string id = Guid.NewGuid().ToString();
            Add(obj, id);
            return id;
        }

        void IServiceLocator.RegisterMMXHost<T>(T obj, string id)
        {
            Add(obj, id);
        }

        string IServiceLocator.RegisterMMXHost<T>(T obj)
        {
            string id = Guid.NewGuid().ToString();
            Add(obj, id);
            return id;
        }

        bool IServiceLocator.IsRegisterred(string id)
        {
            return _services.ContainsKey(id);
        }

        bool IServiceLocator.IsRegisterred<T>(string id)
        {
            if (_services.ContainsKey(id))
                return _services[id].GetType().IsInstanceOfType(typeof(T));
            return false;
        }

        T IServiceLocator.GetInstance<T>(string id)
        {
            return _services[id] as T;
        }

        void IServiceLocator.RemoveObject(string id)
        {
            _services.Remove(id);
        }
    }
}
