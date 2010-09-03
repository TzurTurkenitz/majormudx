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

namespace MajorMudX.Core.Infrastructure
{
    public interface ISessionController
    {
        void RegisterEvent<T>(string id, T handler) 
            where T : class;

        void RegisterService<TInt, TImp>(string id, TImp obj)
            where TInt : class
            where TImp : class;

        TInt GetService<TInt>(string id)
            where TInt : class;
    }
}
