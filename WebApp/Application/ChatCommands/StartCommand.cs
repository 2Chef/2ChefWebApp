using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.ChatCommands
{
    [TelegramCommand("/start")]
    internal sealed class StartCommand : ITelegramCommand
    {
        private ITelegramBotClient TelegramClient { get; }

        public StartCommand(ITelegramBotClient telegramClient)
        {
            TelegramClient = telegramClient;
        }

        public async Task Execute(Message message, CancellationToken cancellationToken)
        {
            if (message.From?.Id is null) return;

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            [
                [ InlineKeyboardButton.WithCallbackData("✅ Войти", "login"),  InlineKeyboardButton.WithCallbackData("🔢 Скалькулировать", "calculate") ],
                [ InlineKeyboardButton.WithUrl("🌐 Открыть сайт", "https://www.youtube.com/watch?v=mDFBTdToRmw") ],
            ]);

            ChatId chatId = new ChatId(message.From.Id);

            await TelegramClient.SendMessage(chatId, "Начинаем работу", replyMarkup: inlineKeyboard);
        }
    }
}
