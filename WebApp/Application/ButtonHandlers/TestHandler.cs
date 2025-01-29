using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("test")]
    internal sealed class TestHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _telegramClient;

        public TestHandler(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        /// <summary>
        /// Попадаем в этот метод, когда пользователь нажал на кнопку с CallbackData = 'test'
        /// </summary>
        public async Task Execute(CallbackQuery callbackData)
        {
            await _telegramClient.SendMessage(callbackData.From.Id, "Test success");
        }
    }
}