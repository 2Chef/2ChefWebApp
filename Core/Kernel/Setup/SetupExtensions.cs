using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Core.Kernel.Setup
{
    public static class SetupExtensions
    {
        public static IHost Setup(this IHost host, Assembly assembly)
        {
            IEnumerable<Type> dispatcherTypes = TypeProvider.GetTypesByAssembly(assembly).Where(t =>
                typeof(ISetup).IsAssignableFrom(t)
                && t.GetCustomAttribute<SetupAttribute>() is not null
                && t.IsClass);

            foreach (Type type in dispatcherTypes)
            {
                object? service = host.Services.GetService(type);
                if (service is ISetup hostStartup)
                {
                    hostStartup.Setup();
                }
            }
            return host;
        }
    }
}
