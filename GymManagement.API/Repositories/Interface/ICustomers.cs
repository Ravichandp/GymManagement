using GymManagement.API.Models.Domain;

namespace GymManagement.API.Repositories.Interface
{
    public interface ICustomers
    {
        Task<List<Customer>> GetCustomerAsync();

        Task<Customer> AddCustomerAsync(Customer customer); 

        Task<Customer> UpdateCustomerAsync(Guid CustomerId, Customer customer);

        Task<Customer> DeleteCustomerAsync(Guid CustomerId);

        Task<Customer?> GetCustomerByIdAsync(Guid CustomerId);

    }
}
