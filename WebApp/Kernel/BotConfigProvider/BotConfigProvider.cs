using Core.Kernel.DiReg;

namespace WebApp.Kernel.BotConfigProvider
{
    [DiReg(ServiceLifetime.Singleton, typeof(IBotConfigProvider))]
    internal class BotConfigProvider : IBotConfigProvider
    {
        private IWebHostEnvironment Enviroment { get; }
        private IConfiguration Configuration { get; }

        public BotConfigProvider(IWebHostEnvironment enviroment, IConfiguration configuration)
        {
            Enviroment = enviroment;
            Configuration = configuration;
        }

        public string GetTlgBotToken()
        {
            if (Enviroment.IsDevelopment())
            {
                return Configuration["Telegram:Bot:Token"] ??
                    throw new InvalidOperationException("Не заданы значения User Sercrets для локального тестирования.\n" +
                    "Для установки токена своего бота используйте команду в консоли {dotnet user-secrets set \"Telegram:Bot:Token\" \"your-secret-token\"}");
            }
            else
            {
                throw new NotImplementedException("Получение Telegram Bot Token для продакшена еще не реализовано");
            }
        }
    }
}
