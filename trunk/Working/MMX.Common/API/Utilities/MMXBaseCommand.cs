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

namespace MMX.Common.API.Utilities
{
    public class MMXBaseCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _validate;

        public MMXBaseCommand(Action<object> action)
            : this(action, (o) => { return true; })
        {
        }

        public MMXBaseCommand(Action<object> action, Func<object, bool> validator)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return _validate(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
