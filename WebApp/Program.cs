using Core.Kernel.DiReg;
using Core.Kernel.Setup;
using System.Reflection;
using Telegram.Bot;
using WebApp.Application.Hosting.LongPooling;
using WebApp.Application.Hosting.WebHook;
using WebApp.Kernel.BotConfigProvider;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Setup(Assembly.GetExecutingAssembly()).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables()
                          .AddUserSecrets<Program>()
                          .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging()
                        .DiRegServices(Assembly.GetExecutingAssembly())
                        .AddHttpClient<ITelegramBotClient, TelegramBotClient>().AddTypedClient<ITelegramBotClient>((client, servicesProvider) =>
                        {
                            IBotConfigProvider botConfig = servicesProvider.GetRequiredService<IBotConfigProvider>();
                            return new TelegramBotClient(botConfig.GetTlgBotToken(), client);
                        });

                    if (CmdSettings.IsLongPoolingConnection)
                        services.AddHostedService<TelegramBotHosting>();
                    else if (CmdSettings.IsWebHookConnection)
                        services.AddHostedService<WebhookSetupService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel()
                        .UseStartup<WebHostStartup>();
                })
                .UseConsoleLifetime();
        }
    }
}
