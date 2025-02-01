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
        private readonly ILogger<UpdateDistributor> _logger;
        private readonly TelegramCommandDispatcher _commandDispatcher;
        private readonly ButtonHandlerDispatcher _buttonHandlerDispatcher;

        public UpdateDistributor(ILogger<UpdateDistributor> logger,
            TelegramCommandDispatcher commandDispatcher, ButtonHandlerDispatcher buttonHandler)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _buttonHandlerDispatcher = buttonHandler;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update?.Message is not null)
            {
                await _commandDispatcher.HandleCommand(update, CancellationToken.None);
            }
            else if (update?.CallbackQuery?.Data is not null)
            {
                await _buttonHandlerDispatcher.Handle(update.CallbackQuery.Data, update, CancellationToken.None);
            }
        }

        public async Task HandleErrorAsync(Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            _logger.LogError(exception.Message);
        }

        Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
            HandleUpdateAsync(update, cancellationToken);

        Task IUpdateHandler.HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) =>
            HandleErrorAsync(exception, source, cancellationToken);
    }
}
