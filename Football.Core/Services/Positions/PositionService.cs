namespace Football.Core.Services.Positions
{
    using Football.Core.Contracts;
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using System;
    using System.Collections.Generic;

    public class PositionService : IPositionService
    {
        private readonly FootballDbContext data;

        public PositionService(FootballDbContext _data)
            => this.data = _data;

        public PositionDetailsServiceModel Details(Guid id)
            => this.data
            .Positions
            .Where(p => p.Id == id)
            .Select(p => new PositionDetailsServiceModel
            {
                Id = p.Id,
                Name = p.Name
            })
            .FirstOrDefault();

        public Guid Create(string Name)
        {
            var positionData = new Position
            {
                Name = Name,
            };

            this.data.Positions.Add(positionData);
            this.data.SaveChanges();

            return positionData.Id;
        }

        public bool Edit(Guid id, string name)
        {
            var positionData = this.data.Positions.Find(id);

            if (positionData == null)
            {
                return false;
            }

            positionData.Name = name;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<PositionServiceModel> ByUser(string userId)

            => GetPositions(this.data
            .Positions
            .Where(p => p.Name == userId));


        public bool IsByManager(Guid positionId, Guid managerId)

            => this.data
            .Positions
            .Any(p => p.Id == positionId);

        //public IEnumerable<PositionServiceModel>
            
        private static IEnumerable<PositionServiceModel> GetPositions(IQueryable<Position> positionQuery)
            => positionQuery
            .Select(p => new PositionServiceModel
            {
                Id = p.Id,
                Name = p.Name,
            })
            .ToList();

    }
}
