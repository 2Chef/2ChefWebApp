using Telegram.Bot;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;

namespace WebApp.Application.ButtonHandlers
{
    [ButtonHandler("susi")]
    internal sealed class SusiHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _telegramClient;

        public SusiHandler(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Execute(CallbackQuery callbackData)
        {
            await _telegramClient.SendMessage(callbackData!.From!.Id, "🍣🍱🍤🥢🥡");
        }
    }
}