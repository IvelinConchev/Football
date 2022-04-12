namespace Football.Core.Contracts
{
    using Football.Core.Models.Players;
    using Football.Core.Services.Players;
    using System.Collections.Generic;

    public interface IPlayerService
    {
        PlayerQueryServiceModel All(
               string name,
               string searchTerm,
               PlayerSorting sorting,
               int currentPage,
               int playersPerPage);

        PlayerDetailsServiceModel Details(Guid playerId);

        //public PositionsServiceModel Details(Guid id)

        Guid Create(
            string firstName,
            string middleName,
            string lastName,
            int age,
            string description,
            int goal,
            string team,
            double height,
            double weight,
            string image,
            string nationality,
            byte shirtNumber,
            Guid positionId,
            Guid managerId);

        bool Edit(Guid id,
           string firstName,
            string middleName,
            string lastName,
            int age,
            string description,
            int goal,
            string team,
            double height,
            double weight,
            string image,
            string nationality,
            byte shirtNumber,
            Guid positionId
          );

        IEnumerable<PlayerServiceModel> ByUser(string userId);

        bool IsByManager(Guid playerId, Guid managerId);

        IEnumerable<string> AllTeams();

        IEnumerable<PlayerPositionsServiceModel> AllPositions();

        bool PositionExists(Guid positionId);
    }
}