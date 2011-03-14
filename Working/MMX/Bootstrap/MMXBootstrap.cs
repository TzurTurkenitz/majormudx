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
                    else if (att.ServiceType == typeof(IMMXHost))
                        locator.RegisterMMXHost(o as IMMXHost, att.Id);
                    else if (att.ServiceType == typeof(ITelnetSocket))
                        locator.RegisterTelnet(o as ITelnetSocket, att.Id);
                }
            }
        }
    }
}
