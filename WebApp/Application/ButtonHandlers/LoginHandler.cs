using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("login")]
    internal sealed class LoginHandler : IButtonHandler
    {
        private ITelegramBotClient TelegramClient { get; }

        public LoginHandler(ITelegramBotClient telegramClient)
        {
            TelegramClient = telegramClient;
        }

        public async Task Execute(CallbackQuery callbackData)
        {
            await TelegramClient.SendMessage(callbackData!.From!.Id, "Ну и куда ты хочешь войти блять");
        }
    }
}