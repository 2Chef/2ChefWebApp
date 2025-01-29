using Core.Kernel.Setup;
using System.Data;
using System.Reflection;

namespace Core.Kernel.DiReg
{
    [Setup]
    public class DiRegSetup : ISetup
    {
        private readonly Dictionary<Type, Type> _diServices = new();

        void ISetup.Setup()
        {
            IEnumerable<Type> commandTypes = TypeProvider.Types
                .Where(t => t.GetCustomAttribute<DiRegAttribute>() is not null);

            // На самом деле основная задача: провалидировать. 1 класс -> 1 сервис.
            // Но потом можно будет достать все зарегистрированные сервисы из этого же класса.
            // Достаточно повесить на него этот же [DiReg]
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
        }
    }
}
