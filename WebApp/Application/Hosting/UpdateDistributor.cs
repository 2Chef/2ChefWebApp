using Core.Kernel.DiReg;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.Hosting
{
    /// <summary>
    /// Обрабатывает любое входящие обновление от телеграм, по факту распределяет действия для других распределителей, определяет что конкретно случилось
    /// </summary>
    /// <param name="logger"> Залогируем всё что нужно </param>
    /// <param name="commandDispatcher"> Если пользователь ввел команду, нам понадобится распределитель действий для команд</param>
    /// <param name="buttonHandler"> Если пользователь нажал на кнопку, нам понадобится распределитель действий для кнопок </param>
    [DiReg(ServiceLifetime.Singleton)]
    public class UpdateDistributor(ILogger<UpdateDistributor> logger,
        TelegramCommandDispatcher commandDispatcher, ButtonHandlerDispatcher buttonHandler) : IUpdateHandler
    {
        private ILogger<UpdateDistributor> Logger { get; } = logger;
        private TelegramCommandDispatcher CommandDispatcher { get; } = commandDispatcher;
        private ButtonHandlerDispatcher ButtonHandlerDispatcher { get; } = buttonHandler;

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update?.Message is not null)
            {
                await CommandDispatcher.HandleCommand(update, cancellationToken);
            }
            else if (update?.CallbackQuery?.Data is not null)
            {
                await ButtonHandlerDispatcher.Handle(update.CallbackQuery.Data, update, cancellationToken);
            }
        }

        public async Task HandleErrorAsync(Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            await Task.Run(() => Logger.LogError("An error occurred: {ErrorMessage}", exception.Message), cancellationToken);
        }

        Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
            HandleUpdateAsync(update, cancellationToken);

        Task IUpdateHandler.HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) =>
            HandleErrorAsync(exception, source, cancellationToken); 
    }
}
