namespace Football.Core.Services.Statistics
{
    using Football.Core.Contracts;
    using Football.Infrastructure.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatisticsService : IStatisticsService
    {
        private readonly FootballDbContext data;

        public StatisticsService(FootballDbContext _data)
        {
            this.data = _data;
        }

        public StatisticsServiceModel Total()
        {
            var totalPlayers = this.data.Players.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalPlayers = totalPlayers,
                TotalUsers = totalUsers,
            };
        }
    }
}
