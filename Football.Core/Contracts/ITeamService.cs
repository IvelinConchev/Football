namespace Football.Core.Contracts
{
    using Football.Core.Models.Teams;
    using Football.Core.Services.Teams;
    using System;

    public interface ITeamService
    {
        TeamQueryServiceModel All(
            string name,
            string searchTerm,
            TeamSorting sorting,
            int currentPage,
            int teamsPerPage);

        TeamDetailsServiceModel Details(Guid teamId);

        Guid Create(
            string name,
            string image,
            string webSite,
            string logoUrl,
            string homeKit,
            string awayKit,
            string nickName,
            string description,
            string address,
            string headCoach,
            int champion,
            int cup,
            int win,
            int defeats,
            Guid playerId,
            Guid managerId);

        bool Edit(Guid id,
            string name,
            string image,
            string webSite,
            string logoUrl,
            string homeKit,
            string awayKit,
            string nickName,
            string description,
            string address,
            string headCoach,
            int champion,
            int cup,
            int win,
            int defeats,
            Guid playerId);

        IEnumerable<TeamServiceModel> ByUser(string userId);

        bool IsByManager(Guid teamId, Guid managerId);

        // IEnumerable<string> AllTeams();
        IEnumerable<string> AllPlayers();

        IEnumerable<TeamPlayersServiceModel> AllTeams();

        bool PlayerExists(Guid playerId);
    }
}
