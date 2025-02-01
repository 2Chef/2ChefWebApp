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

        private IServiceProvider ServiceProvider { get; }
        private ILogger<ButtonHandlerDispatcher> Logger { get; }
        private ITelegramBotClient TelegramClient { get; }

        public ButtonHandlerDispatcher(IServiceProvider serviceProvider, ILogger<ButtonHandlerDispatcher> logger,
            ITelegramBotClient telegramClient)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            TelegramClient = telegramClient;
        }

        public async Task Handle(string callbackKey, Update update, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(callbackKey) || update?.CallbackQuery is null) return;

            if (_handlers.TryGetValue(callbackKey, out Type? typeCommand))
            {
                try
                {
                    // TODO logging 
                    await ((IButtonHandler)ServiceProvider.GetRequiredService(typeCommand))
                        .Execute(update.CallbackQuery);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Unhadnled exception Telegram ButtonHandler: {callbackKey}");
                }
                finally
                {
                    await TelegramClient.AnswerCallbackQuery(update.CallbackQuery.Id);
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
