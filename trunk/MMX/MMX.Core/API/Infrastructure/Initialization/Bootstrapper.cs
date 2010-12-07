using System;
using System.Collections.Generic;
using System.Linq;
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
using MMX.Core.API.Infrastructure.ViewModels;
using MMX.Core.API.Infrastructure.Control;

namespace MMX.Core.API.Infrastructure.Initialization
{
    public class Bootstrapper
    {
        public static IController ShellController { get; set; }

        public static void ReflectAssemblies(System.Windows.Deployment current)
        {
            List<Assembly> assemblies = current.Parts.Select(
                    ap => Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative))).Select(
                        stream => new AssemblyPart().Load(stream.Stream)).ToList();

            foreach (Assembly asm in assemblies)
            {
                foreach (Type t in asm.GetTypes().Where((at) => { return at.IsDefined(typeof(ViewModelAttribute), true); }))
                {
                    ViewModelAttribute vmAtt = t.GetCustomAttributes(true).FirstOrDefault(
                        (a) => { return a is ViewModelAttribute; }) as ViewModelAttribute;

                    object instance = Activator.CreateInstance(t);
                    PropertyInfo pController = t.GetProperty("Controller", BindingFlags.Instance | BindingFlags.Public);
                    if (pController != null)
                        pController.SetValue(instance, ShellController, null);
                    ViewModelLocator.Register(vmAtt.ID, instance);
                }
            }
        }
    }
}
