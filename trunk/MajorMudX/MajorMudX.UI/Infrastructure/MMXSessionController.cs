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
using MajorMudX.Core.Infrastructure;

namespace MajorMudX.UI.Infrastructure
{
    public class MMXSessionController : ISessionController
    {
        Dictionary<string, object> _registerredEvents;
        Dictionary<string, object> _registerredServices;

        ISessionController Parent { get; set; }

        public MMXSessionController()
        {
            _registerredEvents = new Dictionary<string, object>();
            _registerredServices = new Dictionary<string, object>();
        }

        public void RegisterEvent<T>(string id, T handler) where T : class
        {
            if (typeof(T).IsAssignableFrom(typeof(Delegate)))
            {
            }
        }

        public void RegisterService<TInt, TImp>(string id, TImp obj)
            where TInt : class
            where TImp : class
        {
            throw new NotImplementedException();
        }

        public TInt GetService<TInt>(string id) where TInt : class
        {
            throw new NotImplementedException();
        }
    }
}
