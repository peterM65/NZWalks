using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreareJWTToken(IdentityUser user, List<string> roles);
    }
}
