using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public class VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : 
        IdentityDbContext<User, Role, int>(options)
    {

    }
}
