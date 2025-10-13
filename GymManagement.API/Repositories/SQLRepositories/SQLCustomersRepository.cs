using GymManagement.API.Authentication.Models.Domain;
using GymManagement.API.Authentication.Data;
using GymManagement.API.Models.Domain;
using GymManagement.API.Models.Dto;
using GymManagement.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Data.Common;

namespace GymManagement.API.Repositories.SQLRepositories
{
    public class SQLCustomersRepository : ICustomers
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public SQLCustomersRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {

            var registerUser = new AddRegisterToCustomerDto()
            {
                Email = customer.Email,
                UserName=customer.UserName,
                Password =customer.Password
            };

            var userExits = await userManager.FindByNameAsync(registerUser.UserName);
            if (userExits != null)
            {
                return null;
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> DeleteCustomerAsync(Guid CustomerId)
        {
                var customerExists = await dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == CustomerId);

            if (customerExists == null)
            {
                return null;
            }
            dbContext.Customers.Remove(customerExists);
            await dbContext.SaveChangesAsync();
            return customerExists;
        }

        public async Task<List<Customer>> GetCustomerAsync()
        {
            var customerDataList = await dbContext.Customers.ToListAsync();

            return customerDataList;
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid CustomerId)
        {
            return await dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == CustomerId);
        }

        public async Task<Customer> UpdateCustomerAsync(Guid CustomerId, Customer customer)
        {
            var customerData = await dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == CustomerId);
            if (customerData is null)
            {
                return null;
            }
            customerData.FirstName = customer.FirstName;
            customerData.MiddleName = customer.MiddleName;
            customerData.LastName = customer.LastName;
            customerData.Gender = customer.Gender;
            customerData.Age = customer.Age;
            customerData.City = customer.City;

            await dbContext.SaveChangesAsync();

            return customerData;
        }
    }
}
