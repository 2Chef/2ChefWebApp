using Telegram.Bot.Types;
using WebApp.Infrastructure.Repository;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.ChatCommands
{
    [TelegramCommand("/register")]
    internal sealed class RegisterCommand : ITelegramCommand
    {
        CustomerRepository CustomerRepository { get; set; }

        public RegisterCommand(CustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public async Task Execute(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
