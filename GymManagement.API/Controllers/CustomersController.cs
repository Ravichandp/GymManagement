using GymManagement.API.Authentication.Data;
using GymManagement.API.Authentication.Models.Domain;
using GymManagement.API.Models.Domain;
using GymManagement.API.Models.Dto;
using GymManagement.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICustomers iCustomers;

        public CustomersController(ApplicationDbContext dbContext,ICustomers iCustomers)
        {
            this.dbContext = dbContext;
            this.iCustomers = iCustomers;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await iCustomers.GetCustomerAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomerRequestDto addCustomer)
        {
            var customerDomain = new Customer
            {
                //Id = Guid.NewGuid(),
                FirstName = addCustomer.FirstName,
                LastName = addCustomer.LastName,
                MiddleName = addCustomer.MiddleName,
                Age = addCustomer.Age,
                Gender =addCustomer.Gender,
                City = addCustomer.City,
                UserName =addCustomer.UserName,
                Email=addCustomer.Email,
                Password=addCustomer.Password
            };

            customerDomain = await iCustomers.AddCustomerAsync(customerDomain);

            if (customerDomain == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Customer creation failed! Please check Customer details and try again." });
            }
            return Ok(new Response { Status = "Success", Message = "Customer created successfully!" });
        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid Id, [FromBody] CustomerDto customerDto)
        {
            var customerDomain = new Customer // converted to Domain 
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                MiddleName = customerDto.MiddleName,
                Age = customerDto.Age,
                City = customerDto.City,
                Gender = customerDto.Gender
            };

            customerDomain = await iCustomers.UpdateCustomerAsync(Id, customerDomain);

            var updatedCustomer = new CustomerDto // Converted back to DTO
            {
                FirstName = customerDomain.FirstName,
                LastName = customerDomain.LastName,
                MiddleName = customerDomain.MiddleName,
                Age = customerDomain.Age,
                City = customerDomain.City,
                Gender = customerDomain.Gender
            };

            return Ok(updatedCustomer);
        }

        [HttpGet]
        [Route("{CustomerId:guid}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid CustomerId)
        {
            var customerData = await iCustomers.GetCustomerByIdAsync(CustomerId);

            if (customerData == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Customer not Found!" });
            }

            var customerDto = new CustomerDto
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                MiddleName = customerData.MiddleName,
                Age = customerData.Age,
                City = customerData.City,
                Gender = customerData.Gender

            };
            return Ok(customerDto);
        }

        [HttpDelete]
        [Route("{CustomerId:guid}")]
        public async Task<IActionResult> DeleteCustomerById([FromRoute] Guid CustomerId)
        {
            

           var deletedCustomer =  await iCustomers.DeleteCustomerAsync(CustomerId);

            if (deletedCustomer == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Customer not Found!" });
            }
            return Ok(deletedCustomer);
        }
    }
}
