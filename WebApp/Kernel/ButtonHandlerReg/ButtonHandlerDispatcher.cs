using Core.Kernel;
using Core.Kernel.DiReg;
using Core.Kernel.Setup;
using System.Data;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApp.Kernel.ButtonHandlerReg
{
    [Setup]
    [DiReg(ServiceLifetime.Singleton)]
    public class ButtonHandlerDispatcher : ISetup
    {
        private readonly Dictionary<string, Type> _handlers = new();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ButtonHandlerDispatcher> _logger;
        private readonly ITelegramBotClient _telegramClient;

        public ButtonHandlerDispatcher(IServiceProvider serviceProvider, ILogger<ButtonHandlerDispatcher> logger,
            ITelegramBotClient telegramClient)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _telegramClient = telegramClient;
        }

        public async Task Handle(string callbackKey, Update update, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(callbackKey) || update?.CallbackQuery is null) return;

            if (_handlers.TryGetValue(callbackKey, out Type? typeCommand))
            {
                try
                {
                    // TODO logging 
                    await ((IButtonHandler)_serviceProvider.GetRequiredService(typeCommand))
                        .Execute(update.CallbackQuery);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unhadnled exception Telegram ButtonHandler: {callbackKey}");
                }
                finally
                {
                    await _telegramClient.AnswerCallbackQuery(update.CallbackQuery.Id);
                }
            }
        }

        void ISetup.Setup()
        {
            IEnumerable<Type> commandTypes = TypeProvider.Types.Where(t =>
                typeof(IButtonHandler).IsAssignableFrom(t)
                && !t.IsInterface
                && !t.IsAbstract
                && t.GetCustomAttribute<ButtonHandlerAttribute>() is not null);

            foreach (Type type in commandTypes)
            {
                ButtonHandlerAttribute? attribute = type.GetCustomAttribute<ButtonHandlerAttribute>();
                if (attribute is not null)
                {
                    if (!_handlers.TryAdd(attribute.ButtonCode, type))
                        throw new InvalidOperationException($"Button handler '{attribute.ButtonCode}' is already registered.");
                }
            }
        }
    }
}
