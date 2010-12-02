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
using System.Reflection;

namespace MMX.Core.API.Infrastructure.Binding
{
    using Ex = System.Linq.Expressions.Expression;
    using WinBinding = System.Windows.Data.Binding;
    public class BindingUtil
    {
        public static DependencyProperty EventBindingProperty = DependencyProperty.Register(
            "EventBinding", typeof(object), typeof(FrameworkElement), new PropertyMetadata(new PropertyChangedCallback(OnMultiBindingChanged)));

        public static EventBinding GetEventBinding(DependencyObject owner)
        {
            return owner.GetValue(EventBindingProperty) as EventBinding;
        }

        public static void SetEventBinding(DependencyObject owner, EventBinding binding)
        {
            FrameworkElement elem = owner as FrameworkElement;
            if (elem != null)
            {
                elem.Loaded += new RoutedEventHandler((o, e) =>
                {
                    binding.DataContext = ((FrameworkElement)o).DataContext;
                    UpdateBinding(o as DependencyObject, binding);
                });
            }

            owner.SetValue(EventBindingProperty, binding);
        }

        public static void OnMultiBindingChanged(DependencyObject owner, DependencyPropertyChangedEventArgs e)
        {
            EventBinding mbOld = e.OldValue as EventBinding;
            EventBinding mbNew = e.NewValue as EventBinding;

            // remove old bindings
            if (mbOld != null)
            {
            }

            UpdateBinding(owner, mbNew);
        }

        private static void UpdateBinding(DependencyObject owner, EventBinding mbNew)
        {
            if (mbNew.DataContext != null)
            {
                object ctx = mbNew.DataContext;

                BindingFlags flg = BindingFlags.Public | BindingFlags.Instance;
                Type ctlType = owner.GetType();
                Type ctxType = ctx.GetType();

                foreach (var binding in mbNew.Bindings)
                {
                    if (binding is CommandBinding)
                    {
                        CommandBinding cmdBinding = binding as CommandBinding;
                        EventInfo evt = ctlType.GetEvent(cmdBinding.EventName, flg);
                        if (evt == null) continue;
                        PropertyInfo prop = ctxType.GetProperty(cmdBinding.TargetCommand, flg);
                        if (prop == null) continue;

                        object cmd = prop.GetValue(ctx, null); // get the command

                        MethodInfo mthd = evt.EventHandlerType.GetMethod("Invoke"); // get the event handler

                        if (mthd != null)
                        {
                            var param = Ex.Parameter(typeof(EventArgs));
                            foreach (var p in mthd.GetParameters())
                            {
                                if (p.ParameterType != typeof(object))
                                {
                                    param = Ex.Parameter(p.ParameterType, p.Name);
                                    break;
                                }
                            }

                            var lam = Ex.Lambda(evt.EventHandlerType,
                                Ex.Call(Ex.Constant(cmd), "Execute", null, param),
                                Ex.Parameter(typeof(object)), param);

                            evt.AddEventHandler(owner, lam.Compile());
                        }
                    }
                }
            }
        }
    }
}
