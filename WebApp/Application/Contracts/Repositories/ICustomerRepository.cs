using WebApp.Domain.Entities;

namespace WebApp.Application.Contracts.Repositories
{
    public interface ICustomerRepository
    {
        Task RegisterUser(Customer customers);
    }
}
