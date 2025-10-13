using GymManagement.API.Authentication.Models;
using GymManagement.API.Authentication.Models.Domain;
using GymManagement.API.Authentication.Models.Dto;
using GymManagement.API.Authentication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ITokenRepository tokenRepository;

        public AuthenticateController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            LoginModel loginmodle = new LoginModel()
            {
                UserName = loginDto.UserName,
                Password = loginDto.Password
            };

            var user = await userManager.FindByNameAsync(loginmodle.UserName); //Find UserName 

            var passwordCheck = await userManager.CheckPasswordAsync(user, loginmodle.Password); //CheckPassword
            if (user is not null & passwordCheck)
            {
                var userRoles =await userManager.GetRolesAsync(user); //Get Roles of the user

                var jwtToken = tokenRepository.CreateJWTToken(user, userRoles.ToList());
                var response = new LoginResponseDto
                {
                    JwtToken = jwtToken
                };
                return Ok(response);
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            RegisterModel registerModel = new RegisterModel()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password
            };
            var userExits = await userManager.FindByNameAsync(registerModel.UserName);
            if (userExits != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "UserName Already Exists" });
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            RegisterModel registerModel = new RegisterModel()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password
            };
            var userExits = await userManager.FindByNameAsync(registerModel.UserName);
            if (userExits != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "UserName Already Exists" });
            }


            IdentityUser user = new IdentityUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            var adminroleExists = await roleManager.RoleExistsAsync(UserRoles.Admin);
            if (!adminroleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            var userroleExists = await roleManager.RoleExistsAsync(UserRoles.User);

            if (!userroleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            var roleExists = await roleManager.RoleExistsAsync(UserRoles.Admin);

            if (roleExists)
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }
    }
}
