using GymManagement.API.Authentication.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.API.Authentication.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
