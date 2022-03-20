namespace Football.Services
{
    using Football.Core.Contracts;
    using Football.Core.Models;
    using Football.Infrastructure.Data.Identity;
    using Football.Infrastructure.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly IFootballDbRepository repo;

        public UserService(IFootballDbRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo.All<ApplicationUser>()
                .Select(u => new UserListViewModel
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();
        }
    }
}
