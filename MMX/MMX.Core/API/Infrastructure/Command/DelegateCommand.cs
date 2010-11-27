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
using System.Windows.Threading;

namespace MMX.Core.API.Infrastructure.Command
{
    public class DelegateCommand : ICommand
    {
        Action<object> _executeAction;
        Func<object, bool> _checkExecuteAction;
        Dispatcher _ownerDispatch;

        public DelegateCommand(Action<object> action, Func<object, bool> validator)
        {
            _checkExecuteAction = validator;
            _executeAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return _checkExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
