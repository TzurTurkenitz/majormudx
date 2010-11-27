using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MMX.Core.API.Infrastructure.Command
{
    using Ex = System.Linq.Expressions.Expression;

    public static class EventBinder
    {
        public static DependencyProperty EventProperty = DependencyProperty.RegisterAttached(
            "Event", typeof(EventBinding), typeof(EventBinder), new PropertyMetadata(new PropertyChangedCallback(OnEventChanged)));

        public static EventBinding GetEvent(DependencyObject owner)
        {
            return owner.GetValue(EventProperty) as EventBinding;
        }

        public static void SetEvent(DependencyObject owner, ICommand cmd)
        {
            owner.SetValue(EventProperty, cmd);
        }

        static void OnEventChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
        {
            EventBinding oBinding = args.OldValue as EventBinding;
            EventBinding nBinding = args.NewValue as EventBinding;

            Type oType = owner.GetType();

            EventInfo targetEvent = oType.GetEvent(nBinding.TargetEvent, BindingFlags.Instance | BindingFlags.Public);

            // TODO: add remove event handler for changes

            if (targetEvent != null) // validate class implements event
            {
                MethodInfo minfo = targetEvent.EventHandlerType.GetMethod("Invoke");
                if (minfo != null)
                {
                    var param = Ex.Parameter(typeof(string));
                    foreach (var p in minfo.GetParameters())
                    {
                        if (p.ParameterType != typeof(object))
                        {
                            param = Ex.Parameter(p.ParameterType, p.Name);
                            break;
                        }
                    }

                    var lam = Ex.Lambda(targetEvent.EventHandlerType,
                        Ex.Call(Ex.Constant(nBinding.Command), "Execute", null, param),
                        Ex.Parameter(typeof(object)), param);

                    targetEvent.AddEventHandler(owner, lam.Compile());
                }
            }
        }
    }
}
