namespace Football.Core.Contracts
{
    using Football.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();
    }
}
