using Telegram.Bot.Types;

namespace WebApp.Kernel.ChatCommandReg
{
    internal interface ITelegramCommand
    {
        Task Execute(Message message, CancellationToken cancellationToken);
    }
}
