using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("calculate")]
    internal sealed class CalculateHandler : IButtonHandler
    {
        private ITelegramBotClient TelegramClient { get; }

        public CalculateHandler(ITelegramBotClient telegramClient)
        {
            TelegramClient = telegramClient;
        }

        public async Task Execute(CallbackQuery callbackData)
        {
            await TelegramClient.SendMessage(callbackData!.From!.Id, "Хуй тебе а не калькулирование");
        }
    }
}