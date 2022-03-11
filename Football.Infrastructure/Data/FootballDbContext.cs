using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Football.Infrastructure.Data
{
    public class FootballDbContext : IdentityDbContext
    {
        public FootballDbContext(DbContextOptions<FootballDbContext> options)
            : base(options)
        {
        }
    }
}