using Core.Kernel.DiReg;
using WebApp.Application.Contracts.Repositories;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Repository
{
    [DiReg(ServiceLifetime.Scoped, typeof(ICustomerRepository))]
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISet<Customer> _customers = new HashSet<Customer>();

        public async Task RegisterUser(Customer customers) =>
            await Task.Run(() => _customers.Add(customers));
    }
}
