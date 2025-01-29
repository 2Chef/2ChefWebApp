using Telegram.Bot.Types;

namespace WebApp.Kernel.ButtonHandlerReg
{
    public interface IButtonHandler
    {
        Task Execute(CallbackQuery callbackData);
    }
}
