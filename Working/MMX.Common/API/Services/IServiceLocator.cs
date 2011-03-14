using MMX.Common.API.Communication;
using MMX.Common.API.UI;

namespace MMX.Common.API.Services
{
    public interface IServiceLocator
    {
        void RegisterMMXService<T>(T obj, string id)
            where T : class, IMMXService;

        string RegisterMMXService<T>(T obj)
            where T : class, IMMXService;

        void RegisterTelnet<T>(T obj, string id)
            where T : class, ITelnetSocket;

        string RegisterTelnet<T>(T obj)
            where T : class, ITelnetSocket;

        void RegisterMMXHost<T>(T obj, string id)
            where T : class, IMMXHost;

        string RegisterMMXHost<T>(T obj)
            where T : class, IMMXHost;

        bool IsRegisterred(string id);

        bool IsRegisterred<T>(string id);

        T GetInstance<T>(string id)
            where T : class;

        void RemoveObject(string id);
    }
}
