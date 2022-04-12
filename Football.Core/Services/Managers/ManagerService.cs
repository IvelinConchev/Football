namespace Football.Core.Services.Managers
{
    using Football.Core.Contracts;
    using Football.Infrastructure.Data;
    using System;
    using System.Linq;

    public class ManagerService : IManagerService
    {
        private readonly FootballDbContext data;

        public ManagerService(FootballDbContext _data)
        {
            this.data = _data;
        }

        public Guid IdByUser(string userId)
        => this.data
            .Managers
            .Where(m => m.UserId == userId)
            .Select(m => m.Id)
            .FirstOrDefault();

        public bool isManager(string userId)
        => this.data
            .Managers
            .Any(m => m.UserId == userId);

    }
}
