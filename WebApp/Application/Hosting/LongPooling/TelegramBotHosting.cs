using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WebApp.Application.Hosting.LongPooling
{
    /// <summary>
    /// Запускает соединение с Telegram через LongPooling, не работает одновременно с WebHook
    /// </summary>
    internal class TelegramBotHosting : IHostedService
    {
        private ILogger<TelegramBotHosting> Logger { get; init; }

        private ITelegramBotClient TelegramClient { get; init; }

        private UpdateHandler Handler { get; init; }

        public TelegramBotHosting(ILogger<TelegramBotHosting> logger, ITelegramBotClient botClient,
            UpdateHandler handler)
        {
            Logger = logger;
            TelegramClient = botClient;
            Handler = handler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await TelegramClient.DeleteWebhook(cancellationToken: cancellationToken);

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery], // Поддерживаемые типы обновлений
                DropPendingUpdates = true // Удалить все старые обновления
            };

            TelegramClient.StartReceiving(Handler, receiverOptions);
            Logger.LogInformation("Telegram Bot Hosting started.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Telegram Bot Hosting stopped.");
            return Task.CompletedTask;
        }
    }
}
