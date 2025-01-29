using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("calculate")]
    internal sealed class CalculateHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _telegramClient;

        public CalculateHandler(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Execute(CallbackQuery callbackData)
        {
            await _telegramClient.SendMessage(callbackData!.From!.Id, "Хуй тебе а не калькулирование");
        }
    }
}