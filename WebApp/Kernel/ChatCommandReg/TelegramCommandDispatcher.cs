using Core.Kernel;
using Core.Kernel.DiReg;
using Core.Kernel.Setup;
using System.Data;
using System.Reflection;
using Telegram.Bot.Types;

namespace WebApp.Kernel.ChatCommandReg
{
    [Setup]
    [DiReg(ServiceLifetime.Singleton)]
    public class TelegramCommandDispatcher(IServiceProvider serviceProvider, ILogger<TelegramCommandDispatcher> logger) : ISetup
    {
        private readonly Dictionary<string, Type> _commands = new();

        private IServiceProvider ServiceProvider { get; } = serviceProvider;
        private ILogger<TelegramCommandDispatcher> Logger { get; } = logger;

        public async Task HandleCommand(Update update, CancellationToken cancellationToken)
        {
            if (update?.Message?.Text is null) return;

            string commandKey = update.Message.Text.Split(' ')[0];

            if (_commands.TryGetValue(commandKey, out Type? typeCommand))
            {
                try
                {
                    // TODO logging
                    await ((ITelegramCommand)ServiceProvider.GetRequiredService(typeCommand))
                        .Execute(update.Message, cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Unhanldled exception Telegram-Command: {commandKey}");
                }
            }
        }

        void ISetup.Setup()
        {
            IEnumerable<Type> commandTypes = TypeProvider.Types.Where(t =>
                typeof(ITelegramCommand).IsAssignableFrom(t)
                && t.GetCustomAttribute<TelegramCommandAttribute>() is not null
                && !t.IsInterface
                && !t.IsAbstract);


            foreach (Type type in commandTypes)
            {
                TelegramCommandAttribute? attribute = type.GetCustomAttribute<TelegramCommandAttribute>();
                if (attribute is not null)
                {
                    if (!_commands.TryAdd(attribute.Command, type))
                        throw new InvalidOperationException($"Command '{attribute.Command}' is already registered.");
                }
            }
        }
    }
}
