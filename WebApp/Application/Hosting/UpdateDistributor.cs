using Core.Kernel.DiReg;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.Hosting
{
    [DiReg(ServiceLifetime.Singleton)]
    public class UpdateDistributor : IUpdateHandler
    {
        private ILogger<UpdateDistributor> Logger { get; }
        private TelegramCommandDispatcher CommandDispatcher { get; }
        private ButtonHandlerDispatcher ButtonHandlerDispatcher { get; }

        public UpdateDistributor(ILogger<UpdateDistributor> logger,
            TelegramCommandDispatcher commandDispatcher, ButtonHandlerDispatcher buttonHandler)
        {
            Logger = logger;
            CommandDispatcher = commandDispatcher;
            ButtonHandlerDispatcher = buttonHandler;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update?.Message is not null)
            {
                await CommandDispatcher.HandleCommand(update, CancellationToken.None);
            }
            else if (update?.CallbackQuery?.Data is not null)
            {
                await ButtonHandlerDispatcher.Handle(update.CallbackQuery.Data, update, CancellationToken.None);
            }
        }

        public Task HandleErrorAsync(Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            Logger.LogError(exception.Message);
            return Task.CompletedTask;
        }

        Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
            HandleUpdateAsync(update, cancellationToken);

        Task IUpdateHandler.HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) =>
            HandleErrorAsync(exception, source, cancellationToken);
    }
}
