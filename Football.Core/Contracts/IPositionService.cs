namespace Football.Core.Contracts
{
    using System;
    using Football.Core.Services.Positions;

    public interface IPositionService
    {
        PositionDetailsServiceModel Details(Guid positionid);
        Guid Create(
           string Name);

        bool Edit(Guid id,
          string Name);

      

        IEnumerable<PositionServiceModel> ByUser(string userId);

        bool IsByManager(Guid positionId, Guid managerId);
    }
}
