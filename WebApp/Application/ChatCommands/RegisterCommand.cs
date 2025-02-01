using Telegram.Bot.Types;
using WebApp.Application.Contracts.Repositories;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.ChatCommands
{
    [TelegramCommand("/register")]
    internal sealed class RegisterCommand : ITelegramCommand
    {
        private ICustomerRepository CustomerRepository { get; }

        public RegisterCommand(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public async Task Execute(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
