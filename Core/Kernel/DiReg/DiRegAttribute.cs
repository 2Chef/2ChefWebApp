using Microsoft.Extensions.DependencyInjection;

namespace Core.Kernel.DiReg
{
    /// <summary>
    /// Использования данного атрибута автоматически предоставляет вам возможность
    /// "подставлять" свой класс в качестве реализации при запросе зависимости типа <see cref="foreignType"/>
    /// </summary>
    /// <param name="lifeTime"></param>
    /// <param name="foreignType"></param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DiRegAttribute(ServiceLifetime lifeTime, Type? foreignType) : Attribute
    {
        public ServiceLifetime LifeTime { get; } = lifeTime;

        public Type? ForeignType { get; } = foreignType;

        public DiRegAttribute(ServiceLifetime lifeTime) : this(lifeTime, null) { }
    }
}
