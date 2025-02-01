using Microsoft.Extensions.DependencyInjection;

namespace Core.Kernel.DiReg
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DiRegAttribute : Attribute
    {
        public ServiceLifetime LifeTime { get; }

        public Type? ForeignType { get; }

        public DiRegAttribute(ServiceLifetime lifeTime) : this(lifeTime, null) { }

        public DiRegAttribute(ServiceLifetime lifeTime, Type? foreignType)
        {
            LifeTime = lifeTime;
            ForeignType = foreignType;
        }
    }
}
