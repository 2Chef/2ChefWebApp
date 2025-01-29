using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Kernel.DiReg
{
    public static class DiRegExtension
    {
        public static IServiceCollection DiRegServices(this IServiceCollection services, Assembly currentAssembly)
        {
            IEnumerable<Type> serviceTypes = TypeProvider.GetTypesByAssembly(currentAssembly)
                .Where(t => t.GetCustomAttribute<DiRegAttribute>() is not null);

            foreach (Type providedType in serviceTypes)
            {
                DiRegAttribute? attribute = providedType.GetCustomAttribute<DiRegAttribute>();

                if (attribute is not null)
                {
                    Type registerType = attribute.ForeignType ?? providedType;
                    switch (attribute.LifeTime)
                    {
                        case ServiceLifetime.Singleton: services.AddSingleton(registerType, providedType); break;
                        case ServiceLifetime.Scoped: services.AddScoped(registerType, providedType); break;
                        case ServiceLifetime.Transient: services.AddTransient(registerType, providedType); break;
                        default: throw new InvalidOperationException("Unsupported service lifetime.");
                    }
                }
            }
            return services;
        }
    }
}
