using Core.Kernel.Setup;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Data;
using System.Reflection;

namespace Core.Kernel.DiReg
{
    /// <summary>
    /// Основная задача: провалидировать регистрацию зависимостей: 1 класс -> 1 сервис. Но потом можно будет достать все зарегистрированные сервисы из этого же класса.
    /// На самом же деле зависимости будут предоставляться через <see cref="IServiceCollection"/>
    /// </summary>
    [Setup]
    [DiReg(ServiceLifetime.Singleton, typeof(IDiRegTypesProvider))]
    public class DiRegSetup : ISetup, IDiRegTypesProvider
    {
        private readonly Dictionary<Type, Type> _diServices = new();

        private ImmutableDictionary<Type, Type>? _servicesTypes;

        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/> где TKey - внешний тип, интерфейс. TValue - реализация.
        /// </summary>
        public IImmutableDictionary<Type, Type> ServicesTypes => _servicesTypes!;

        void ISetup.Setup()
        {
            IEnumerable<Type> commandTypes = TypeProvider.Types
                .Where(t => t.GetCustomAttribute<DiRegAttribute>() is not null);

            foreach (Type type in commandTypes)
            {
                DiRegAttribute? attribute = type.GetCustomAttribute<DiRegAttribute>();
                if (attribute is not null)
                {
                    Type registerType = attribute.ForeignType ?? type;
                    if (!_diServices.TryAdd(registerType, type))
                        throw new InvalidOperationException($"Service '{registerType}' is already registered.");
                }
            }

            // переписываем все зарегестрированные типы, для дальнейшей иммутабельности
            _servicesTypes = ImmutableDictionary.CreateRange(_diServices);
        }
    }
}
