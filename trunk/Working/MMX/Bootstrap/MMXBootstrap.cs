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
using System.Linq;
using MMX.Common.API.Services;
using MMX.Common.API.UI;
using MMX.Common.API.Communication;
using MMX.Common.API.Control;
using MMX.Common.API.Infrastructure;
using System.Reflection;
using System.Collections.Generic;

namespace MMX.Bootstrap
{
    internal static class MMXBootstrap
    {
        public static void LoadServices(IServiceLocator locator)
        {
            // find all the loaded assemblies
            var assemblies = Deployment.Current.Parts.Select(
                ap => Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative))).Select(
                    s => new AssemblyPart().Load(s.Stream)).ToArray();

            // Loop through the assemblies
            foreach (var asm in assemblies)
            {
                // Find all types decorated with the service registration
                var types = asm.GetTypes().Where(
                    t => t.IsDefined(typeof(ServiceRegistrationAttribute), true)).ToArray();

                // Iterate the types to load them on the fly
                foreach (Type t in types)
                {
                    // Get the service registration attribute
                    ServiceRegistrationAttribute att = t.GetCustomAttributes(
                        typeof(ServiceRegistrationAttribute), true).First() as ServiceRegistrationAttribute;

                    // Create an instance of the object
                    object o = Activator.CreateInstance(t);

                    // Store the object with the correct method
                    if (att.ServiceType == typeof(IMMXService))
                        locator.RegisterMMXService(o as IMMXService, att.Id);
                    else if (att.ServiceType == typeof(ITelnetSocket))
                        locator.RegisterTelnet(o as ITelnetSocket, att.Id);
                }
            }
        }

        public static void GenerateUI(IServiceLocator locator)
        {
            // find all the loaded assemblies
            var assemblies = Deployment.Current.Parts.Select(
                ap => Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative))).Select(
                    s => new AssemblyPart().Load(s.Stream)).ToArray();

            // Loop through the assemblies
            foreach (var asm in assemblies)
            {
                // Find all types decorated with the service registration
                var types = asm.GetTypes().Where(
                    t => t.IsDefined(typeof(ViewRegistrationAttribute), true)).ToArray();

                // Iterate the types to load them on the fly
                foreach (Type t in types)
                {
                    // Get the service registration attribute
                    ViewRegistrationAttribute att = t.GetCustomAttributes(
                        typeof(ViewRegistrationAttribute), true).First() as ViewRegistrationAttribute;

                    // Check for a view model requirement
                    ViewModelAttribute vmAtt = t.GetCustomAttributes(typeof(ViewModelAttribute), true).FirstOrDefault() as ViewModelAttribute;

                    // Load the view model if needed
                    if (vmAtt != null)
                    {
                        // Create the instance
                        IMMXViewModel vm = Activator.CreateInstance(vmAtt.ViewModelType) as IMMXViewModel;

                        // Register it with the locator
                        new ViewModelLocator()[vmAtt.Id] = vm;
                    }

                    // Create an instance of the object
                    object o = Activator.CreateInstance(t);

                    // Store the object with the correct method
                    if (att.ViewType == typeof(IMMXHost))
                        locator.RegisterMMXHost(o as IMMXHost, att.Id);
                }
            }
        }

        public static IEnumerable<IMMXController> BuildControllers(IServiceLocator locator)
        {
            // find all the loaded assemblies
            var assemblies = Deployment.Current.Parts.Select(
                ap => Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative))).Select(
                    s => new AssemblyPart().Load(s.Stream)).ToArray();

            // Loop through the assemblies
            foreach (var asm in assemblies)
            {
                // Find all types decorated with the service registration
                var types = asm.GetTypes().Where(
                    t => t.IsDefined(typeof(MMXControllerAttribute), true)).ToArray();

                // Iterate the types to load them on the fly
                foreach (Type t in types)
                {
                    // Get the service registration attribute
                    MMXControllerAttribute att = t.GetCustomAttributes(
                        typeof(MMXControllerAttribute), true).First() as MMXControllerAttribute;

                    ConstructorInfo ci =
                        t.GetConstructors().Where(c => c.GetCustomAttributes(typeof(MMXDefaultConstructorAttribute), true).Length > 0)
                        .FirstOrDefault();

                    if (ci != null)
                    {
                        ParameterInfo[] parameters = ci.GetParameters();
                        if (parameters.Length == 0)
                        {
                            yield return ci.Invoke(new object[] { }) as IMMXController;
                            continue;
                        }

                        object[] objs = new object[parameters.Length];
                        bool canCreate = true;
                        for (int i = 0; i < parameters.Length; ++i)
                        {
                            // Check for an inject new attribute
                            MMXInjectNewAttribute injectAtt = parameters[i].GetCustomAttributes(typeof(MMXInjectNewAttribute), true)
                                .FirstOrDefault() as MMXInjectNewAttribute;
                            if (injectAtt != null)
                            {
                                if (injectAtt.InjectionType == typeof(IServiceLocator))
                                    objs[i] = locator;
                                else canCreate = false; ;
                            }
                        }

                        if (canCreate)
                            yield return ci.Invoke(objs) as IMMXController;
                    }
                    else
                        yield return Activator.CreateInstance(t) as IMMXController;

                }
            }

            yield break;
        }
    }
}
