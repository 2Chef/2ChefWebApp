using Telegram.Bot.Types;
using WebApp.Application.Contracts.Repositories;
using WebApp.Domain.Entities;
using WebApp.Kernel.ChatCommandReg;

namespace WebApp.Application.ChatCommands
{
    [TelegramCommand("/register")]
    internal sealed class RegisterCommand(ICustomerRepository customerRepository) : ITelegramCommand
    {
        private ICustomerRepository CustomerRepository { get; } = customerRepository;

        public async Task Execute(Message message, CancellationToken cancellationToken)
        {
            await CustomerRepository.RegisterUser(new Customer(message!.From!.Id, message.From.Username!));
        }
    }
}
