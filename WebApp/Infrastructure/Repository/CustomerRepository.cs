using Core.Kernel.DiReg;
using WebApp.Application.Contracts.Repositories;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Repository
{
    [DiReg(ServiceLifetime.Scoped, typeof(ICustomerRepository))]
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISet<Customer> _customers = new HashSet<Customer>();

        public void RegisterUser(Customer customers) => _customers.Add(customers);

        public IEnumerable<Customer> GetAllUsers() => _customers;
    }
}
