using Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("login")]
    internal sealed class LoginHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _telegramClient;

        public LoginHandler(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Execute(CallbackQuery callbackData)
        {
            await _telegramClient.SendMessage(callbackData!.From!.Id, "Ну и куда ты хочешь войти блять");
        }
    }
}