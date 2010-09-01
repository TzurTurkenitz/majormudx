using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using MajorMudX.Core.UI;

namespace MajorMudX.Core.Utilities.XapManagement
{
    public static class XapLoader
    {
        public static event EventHandler LoadCompleted;

        static XapLoader()
        {
            _temp = new Dictionary<Type, object>();
        }

        public static void LoadXap(Uri xapUri)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(OpenReadCompletedHandler);
            client.OpenReadAsync(xapUri);
        }

        static Dictionary<Type, object> _temp;

        public static bool Contains(Type t) { return _temp.ContainsKey(t); }

        public static object Get(Type t) { if (_temp.ContainsKey(t)) return _temp[t]; return null; }

        static void OpenReadCompletedHandler(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // need to handle the error
                return;
            }

            StreamResourceInfo info = Application.GetResourceStream(
                new StreamResourceInfo(e.Result, null),
                new Uri("AppManifest.xaml", UriKind.Relative));

            string appManifest = new StreamReader(info.Stream).ReadToEnd();
            XmlReader xReader = XmlReader.Create(new StringReader(appManifest));
            Type objType = null;
            while (xReader.Read())
            {
                if (xReader.IsStartElement("AssemblyPart"))
                {
                    xReader.MoveToAttribute("Source");
                    if (xReader.ReadAttributeValue())
                    {
                        StreamResourceInfo asmInfo = new StreamResourceInfo(e.Result, "application/binary");
                        StreamResourceInfo asmStream = Application.GetResourceStream(asmInfo, new Uri(xReader.Value, UriKind.Relative));
                        AssemblyPart part = new AssemblyPart();
                        Assembly asm = part.Load(asmStream.Stream);

                        foreach (Type t in asm.GetTypes())
                        {
                            if (t.IsDefined(typeof(CharacterHostAttribute), false))
                                objType = t;
                        }
                    }
                }
            }

            if (objType != null)
                _temp.Add(typeof(CharacterHostAttribute), Activator.CreateInstance(objType));

            if (LoadCompleted != null)
                LoadCompleted(null, EventArgs.Empty);
        }
    }
}
