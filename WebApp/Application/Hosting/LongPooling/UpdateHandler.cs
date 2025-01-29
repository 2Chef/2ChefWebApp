using Core.Kernel.DiReg;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WebApp.Kernel.ButtonHandlerReg;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.Hosting.LongPooling
{
    [DiReg(ServiceLifetime.Singleton)]
    internal class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> _logger;
        private readonly TelegramCommandDispatcher _commandDispatcher;
        private readonly ButtonHandlerDispatcher _buttonHandlerDispatcher;

        public UpdateHandler(ILogger<UpdateHandler> logger,
            TelegramCommandDispatcher commandDispatcher, ButtonHandlerDispatcher buttonHandler)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _buttonHandlerDispatcher = buttonHandler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            _logger.LogError(exception.Message);
        }
    }
}
