using System.Collections.Immutable;

namespace Core.Kernel.DiReg
{
    public interface IDiRegTypesProvider
    {
        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/> где TKey - внешний тип, интерфейс. TValue - реализация.
        /// </summary>
        IImmutableDictionary<Type, Type> ServicesTypes { get; }
    }
}
