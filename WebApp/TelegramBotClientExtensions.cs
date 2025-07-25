using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using WebApp.Kernel.BotConfigProvider;

namespace WebApp.Application.Extensions
{
    public static class TelegramBotClientExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection services)
        {
            return services
                .AddHttpClient<ITelegramBotClient, TelegramBotClient>()
                .AddTypedClient<ITelegramBotClient>((client, serviceProvider) =>
                {
                    IBotConfigProvider botConfig = serviceProvider.GetRequiredService<IBotConfigProvider>();
                    return new TelegramBotClient(botConfig.GetTlgBotToken(), client);
                });
        }
    }
}
