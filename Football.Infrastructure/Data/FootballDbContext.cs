namespace Football.Infrastructure.Data
{
    using Football.Infrastructure.Data.Identity;
    using Football.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class FootballDbContext : IdentityDbContext<ApplicationUser>
    {
        public FootballDbContext(DbContextOptions<FootballDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StadiumCity>()
                .HasKey(s => new { s.StadiumId, s.CityId });

            builder.Entity<TeamCity>()
                .HasKey(t => new { t.TeamId, t.CityId });

            //builder.Entity<TeamLeague>()
            //    .HasKey(t => new { t.TeamId, t.LeagueId });
            //builder.Entity<Player>()
            //    .HasOne(p => p.Position)
            //    .WithMany(pl => pl.Players)
            //    .HasForeignKey(p => p.PositionId)
            //    .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Stadium> Stadiums { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<StadiumCity> StadiumCities { get; set; }

        public DbSet<TeamCity> TeamCities { get; set; }
    }
}