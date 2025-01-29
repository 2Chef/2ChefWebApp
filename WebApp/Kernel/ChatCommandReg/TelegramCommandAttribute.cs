using Core.Kernel.DiReg;

namespace WebApp.Kernel.ChatCommandReg
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class TelegramCommandAttribute : DiRegAttribute
    {
        public string Command { get; }

        public TelegramCommandAttribute(string command) : base(ServiceLifetime.Transient)
        {
            Command = command;
        }

        public TelegramCommandAttribute(string command, ServiceLifetime serviceLifetime) : base(serviceLifetime)
        {
            Command = command;
        }
    }
}