using Football.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Football.Infrastructure.Data
{
    public class FootballDbContext : IdentityDbContext<ApplicationUser>
    {
        public FootballDbContext(DbContextOptions<FootballDbContext> options)
            : base(options)
        {
        }
    }
}