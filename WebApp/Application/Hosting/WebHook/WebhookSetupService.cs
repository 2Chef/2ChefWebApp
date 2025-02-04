using Telegram.Bot;
using WebApp.Kernel.DomainPathProvider;

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
        private IDomainPathProvider DomainPathProvider { get; }

        public WebhookSetupService(ITelegramBotClient botClient, IConfiguration configuration, ILogger<WebhookSetupService> logger,
            IDomainPathProvider domainPathProvider)
        {
            BotClient = botClient;
            Configuration = configuration;
            Logger = logger;
            DomainPathProvider = domainPathProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string domain = await DomainPathProvider.GetDomain()
                ?? throw new InvalidOperationException("Не найден текущий домен");

            string route = Configuration["Telegram:UpdateRoute"]
                ?? throw new InvalidOperationException("Настройка Telegram:UpdateRoute пуста");

            await BotClient.DeleteWebhook(cancellationToken: cancellationToken);

            string fullWebhookUrl = domain + route;

            Logger.LogInformation($"Setting webhook: {fullWebhookUrl}");

            await BotClient.SetWebhook(fullWebhookUrl, dropPendingUpdates: true, cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
