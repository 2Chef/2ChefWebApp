using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApp.Application.Hosting.WebHook
{
    /// <summary>
    /// Запускает соединение с Telegram через WebHook, не работает одновременно с LongPooling
    /// </summary>
    internal class WebhookSetupService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<WebhookSetupService> _logger;
        private readonly IConfiguration _configuration;

        public WebhookSetupService(ITelegramBotClient botClient, IConfiguration configuration, ILogger<WebhookSetupService> logger)
        {
            _botClient = botClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string webhookUrl = _configuration["Telegram:WebhookUrl"]!;

            // Сначала удаляем текущий вебхук
            await _botClient.DeleteWebhook(cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                _logger.LogError("Webhook URL is missing in configuration.");
                return;
            }

            _logger.LogInformation($"Setting webhook: {webhookUrl}");
            await _botClient.SetWebhook(webhookUrl, cancellationToken: cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
