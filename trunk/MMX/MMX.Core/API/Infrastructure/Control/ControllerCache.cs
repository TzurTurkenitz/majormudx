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

namespace MMX.Core.API.Infrastructure.Control
{
    public class ControllerCache
    {
        private Dictionary<string, IController> _controllers;
        IController _parent;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <remarks>
        /// Can only be constructed by the core API to prevent functionality
        /// pollution.
        /// </remarks>
        /// <param name="parent"></param>
        internal ControllerCache(IController parent)
        {
            _parent = parent;
            _controllers = new Dictionary<string, IController>();
        }

        public IController this[string key]
        {
            get
            {
                // check the current cache first
                if (_controllers.ContainsKey(key))
                    return _controllers[key];

                // check the parent cache if applicable
                if (_parent != null)
                    return _parent.Controllers[key];

                return null;
            }
            set
            {
                if (!_controllers.ContainsKey(key))
                    _controllers.Add(key, null);
                _controllers[key] = value;
            }
        }
    }
}
