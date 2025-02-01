using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.ChatCommands
{
    [TelegramCommand("/start")]
    internal sealed class StartCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _telegramClient;

        public StartCommand(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Execute(Message message, CancellationToken cancellationToken)
        {
            if (message.From?.Id is null) return;

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            [
                [ InlineKeyboardButton.WithCallbackData("✅ Войти", "login"),  InlineKeyboardButton.WithCallbackData("🔢 Скалькулировать", "calculate") ],
                [ InlineKeyboardButton.WithUrl("🌐 Открыть сайт", "https://www.youtube.com/watch?v=mDFBTdToRmw") , InlineKeyboardButton.WithCallbackData("🍣 SUSI", "susi") ]
            ]);

            ChatId chatId = new ChatId(message.From.Id);

            await _telegramClient.SendMessage(chatId, "Начинаем работу", replyMarkup: inlineKeyboard);
        }
    }
}
