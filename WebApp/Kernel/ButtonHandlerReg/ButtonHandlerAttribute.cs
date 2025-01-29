using Core.Kernel.DiReg;

namespace WebApp.Kernel.ButtonHandlerReg
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ButtonHandlerAttribute : DiRegAttribute
    {
        public string ButtonCode { get; }

        public ButtonHandlerAttribute(string command) : base(ServiceLifetime.Transient)
        {
            ButtonCode = command;
        }

        public ButtonHandlerAttribute(string command, ServiceLifetime serviceLifetime) : base(serviceLifetime)
        {
            ButtonCode = command;
        }
    }
}