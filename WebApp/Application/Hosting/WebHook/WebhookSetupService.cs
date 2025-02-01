using Telegram.Bot;

namespace WebApp.Application.Hosting.WebHook
{
    /// <summary>
    /// Запускает соединение с Telegram через WebHook, не работает одновременно с LongPooling
    /// </summary>
    internal class WebhookSetupService : IHostedService
    {
        private ITelegramBotClient BotClient { get; }
        private ILogger<WebhookSetupService> Logger { get; }
        private IConfiguration Configuration { get; }

        public WebhookSetupService(ITelegramBotClient botClient, IConfiguration configuration, ILogger<WebhookSetupService> logger)
        {
            BotClient = botClient;
            Configuration = configuration;
            Logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string webhookUrl = Configuration["Telegram:WebhookUrl"]!;

            // Сначала удаляем текущий вебхук
            await BotClient.DeleteWebhook(cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                Logger.LogError("Webhook URL is missing in configuration.");
                return;
            }

            Logger.LogInformation($"Setting webhook: {webhookUrl}");
            await BotClient.SetWebhook(webhookUrl, dropPendingUpdates: true, cancellationToken: cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
