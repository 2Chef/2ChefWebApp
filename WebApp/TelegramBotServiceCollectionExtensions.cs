using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using WebApp.Kernel.BotConfigProvider;

namespace WebApp.Kernel.TelegramBot
{
    public static class TelegramBotServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection services)
        {
            services.AddHttpClient<ITelegramBotClient, TelegramBotClient>()
                .AddTypedClient<ITelegramBotClient>((client, servicesProvider) =>
                {
                    IBotConfigProvider botConfig = servicesProvider.GetRequiredService<IBotConfigProvider>();
                    return new TelegramBotClient(botConfig.GetTlgBotToken(), client);
                });

            return services;
        }
    }
}
