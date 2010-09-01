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
using System.Collections.Generic;

namespace MajorMudX.Core.Injection
{
    public class ObjectResolver
    {
        Dictionary<Type, object> _objectCache;

        public ObjectResolver()
        {
            _objectCache = new Dictionary<Type, object>();
        }

        public TObj Create<TInt, TObj>()
            where TObj : class
            where TInt : class
        {
            TObj obj = (TObj)Activator.CreateInstance(typeof(TObj));

            _objectCache.Remove(typeof(TInt));
            _objectCache.Add(typeof(TInt), obj);

            return obj;
        }

        public TInt Get<TInt>()
            where TInt : class
        {
            TInt obj = null;

            if (_objectCache.ContainsKey(typeof(TInt)))
                obj = _objectCache[typeof(TInt)] as TInt;

            return obj;
        }
    }
}
