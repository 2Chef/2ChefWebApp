using System.Reflection;
using Core.Kernel.DiReg;
using Core.Kernel.Setup;
using Telegram.Bot;
using WebApp.Application.Hosting.LongPooling;
using WebApp.Application.Hosting.WebHook;

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
                          .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables()
                          .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging()
                        .DiRegServices(Assembly.GetExecutingAssembly())
                        .AddHttpClient<ITelegramBotClient, TelegramBotClient>(client =>
                        {
                            string? token = hostContext.Configuration["Telegram:Bot:Token"];
                            if (string.IsNullOrWhiteSpace(token))
                                throw new InvalidOperationException("Bot token is missing in configuration.");
                            return new TelegramBotClient(token, client);
                        });

                    // Получаем аргументы командной строки
                    string[] commandLineArgs = Environment.GetCommandLineArgs();

                    if (commandLineArgs.Contains("-lp"))
                        services.AddHostedService<TelegramBotHosting>();
                    else
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
