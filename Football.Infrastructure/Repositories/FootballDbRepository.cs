namespace Football.Infrastructure.Repositories
{
    using Football.Infrastructure.Common;
    using Football.Infrastructure.Data;

    public class FootballDbRepository : Repository, IFootballDbRepository
    {
        public FootballDbRepository(FootballDbContext context)
        {
            this.Context = context;
        }
    }
}
