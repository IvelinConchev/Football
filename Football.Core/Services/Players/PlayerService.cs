namespace Football.Core.Services.Players
{
    using Football.Core.Contracts;
    using Football.Core.Models.Players;
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerService : IPlayerService
    {
        private readonly FootballDbContext data;

       //private readonly IWebHostEnvironment webHostEnvironment;

        public PlayerService(FootballDbContext _data)
         => this.data = _data;

        public PlayerQueryServiceModel All(
            string team,
            string searchTerm,
            PlayerSorting sorting,
            int currentPage,
            int playersPerPage)
        {
            var playersQuery = this.data.Players.AsQueryable();

            if (!string.IsNullOrWhiteSpace(team))
            {
                playersQuery = playersQuery.Where(c => c.Team == team);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                playersQuery = playersQuery.Where(
                    p => (p.FirstName + " " + p.MiddleName + " " + p.LastName).ToLower().Contains(searchTerm.ToLower())
                    || (p.FirstName + " " + p.LastName).ToLower().Contains(searchTerm.ToLower())
                    || (p.MiddleName + " " + p.LastName).ToLower().Contains(searchTerm.ToLower())
                    || (p.FirstName + " " + p.LastName + " " + p.Team).ToLower().Contains(searchTerm.ToLower())
                    || p.Team.ToLower().Contains(searchTerm.ToLower())
                    || p.Position.Name.ToLower().Contains(searchTerm.ToLower())
                    || p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            playersQuery = sorting switch
            {
                PlayerSorting.FirstAndMiddleAndLastName => playersQuery.OrderBy(p => p.FirstName)
                .ThenBy(p => p.MiddleName).ThenBy(p => p.LastName),
                PlayerSorting.Team => playersQuery.OrderByDescending(p => p.Team),
                PlayerSorting.Position => playersQuery.OrderBy(p => p.Position),
                PlayerSorting.Description or _ => playersQuery.OrderByDescending(p => p.Id)
                //CarSorting.DateCreated or _ => playerQuery.OrderByDescending(p => p.Id)
            };

            var totalPlayers = playersQuery.Count();

            var players = GetPlayers(playersQuery
                .Skip((currentPage - 1) * playersPerPage)
                .Take(playersPerPage));

            return new PlayerQueryServiceModel
            {
                TotalPlayers = totalPlayers,
                CurrentPage = currentPage,
                PlayersPerPage = playersPerPage,
                Players = players
            };
        }

        public PlayerDetailsServiceModel Details(Guid id)
            => this.data
            .Players
            .Where(p => p.Id == id)
            .Select(p => new PlayerDetailsServiceModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Age = p.Age,
                Description = p.Description,
                Goal = p.Goal,
                Team = p.Team,
                Height = p.Height,
                Weight = p.Weight,
                Image = p.Image,
                Nationality = p.Nationality,
                ShirtNumber = p.ShirtNumber,
                PositionName = p.Position.Name,
                UserId = p.Manager.UserId
            })
            .FirstOrDefault();

        public Guid Create(string firstName, string middleName, string lastName, int age, string description, int goal, string team, double height, double weight, string image, string nationality, byte shirtNumber, Guid positionId, Guid managerId)
        {
            var playerData = new Player
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Age = age,
                Description = description,
                Goal = goal,
                Height = height,
                Weight = weight,
                Image = image,
                Team = team,
                Nationality = nationality,
                ShirtNumber = shirtNumber,
                PositionId = positionId,
                ManagerId = managerId
            };

            this.data.Players.Add(playerData);
            this.data.SaveChanges();

            return playerData.Id;
        }

        public bool Edit(Guid id, string firstName, string middleName, string lastName, int age, string description, int goal, string team, double height, double weight, string image, string nationality, byte shirtNumber, Guid positionId)
        {
            var playerData = this.data.Players.Find(id);

            if (playerData == null)
            {
                return false;
            }

            playerData.FirstName = firstName;
            playerData.MiddleName = middleName;
            playerData.LastName = lastName;
            playerData.Age = age;
            playerData.Description = description;
            playerData.Goal = goal;
            playerData.Team = team;
            playerData.Height = height;
            playerData.Weight = weight;
            playerData.Image = image;
            playerData.Nationality = nationality;
            playerData.ShirtNumber = shirtNumber;
            playerData.PositionId = positionId;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<PlayerServiceModel> ByUser(string userId)
            => GetPlayers(this.data
                .Players
                .Where(p => p.Manager.UserId == userId));

        public bool IsByManager(Guid playerId, Guid managerId)
            => this.data
            .Players
            .Any(p => p.Id == playerId && p.ManagerId == managerId);


        public IEnumerable<string> AllTeams()
            => this.data
            .Players
            .Select(p => p.Team)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        public IEnumerable<PlayerPositionsServiceModel>AllPositions()
            => this.data
            .Positions
            .Select(p => new PlayerPositionsServiceModel
            {
                Id = p.Id,
                Name = p.Name,
            })
            .ToList();

        public bool PositionExists(Guid positionId)
            => this.data
            .Positions
            .Any(p => p.Id == positionId);

        public bool ManagerExists(Guid managerId)
          => this.data
          .Managers
          .Any(m => m.Id == managerId);

        private static IEnumerable<PlayerServiceModel>
            GetPlayers(IQueryable<Player> playerQuery)
            => playerQuery
            .Select(p => new PlayerServiceModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Age = p.Age,
                //Description = p.Description,
                Goal = p.Goal,
                Team = p.Team,
                Height = p.Height,
                Weight = p.Weight,
                Image = p.Image,
                Nationality = p.Nationality,
                ShirtNumber = p.ShirtNumber,
                PositionName = p.Position.Name,
            })
            .ToList();



        //private string UploadFile(AddPlayerFormModel model)
        //{
        //    string fileName = null;
        //    if (model.Image != null)
        //    {
        //        string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "img");
        //        fileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
        //        string filePath = Path.Combine(uploadDir, fileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.Image.CopyTo(fileStream);
        //        }
        //    }

        //    return fileName;
        //}
    }
}

