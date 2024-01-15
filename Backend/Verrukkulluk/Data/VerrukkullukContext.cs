using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public class VerrukkullukContext :
        IdentityDbContext<User, Role, int>
    {
        public VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : base(options) { }
    }
}
