namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Players;
    using Microsoft.AspNetCore.Mvc;
    using FileSystem = System.IO.File;
    public class PlayersController : Controller
    {
        private readonly FootballDbContext data;

        public PlayersController(FootballDbContext _data)
        {
            this.data = _data;
        }

        public IActionResult All([FromQuery] AllPlayerQueryModel query)
        {
            var playerQuery = this.data.Players.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Team))
            {
                playerQuery = playerQuery.Where(p => p.Team == query.Team);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                playerQuery = playerQuery.Where(
                    p => (p.FirstName + " " + p.MiddleName + " " + p.LastName).ToLower().Contains(query.SearchTerm.ToLower())
                    || (p.FirstName + " " + p.LastName).ToLower().Contains(query.SearchTerm.ToLower())
                    || (p.MiddleName + " " + p.LastName).ToLower().Contains(query.SearchTerm.ToLower())
                    || (p.FirstName + " " + p.LastName + " " + p.Team).ToLower().Contains(query.SearchTerm.ToLower())
                    || p.Team.ToLower().Contains(query.SearchTerm.ToLower())
                    || p.Position.Name.ToLower().Contains(query.SearchTerm.ToLower())
                    || p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            playerQuery = query.Sorting switch
            {
                PlayerSorting.FirstAndMiddleAndLastName => playerQuery.OrderBy(p => p.FirstName)
                .ThenBy(p => p.MiddleName).ThenBy(p => p.LastName),
                PlayerSorting.Team => playerQuery.OrderByDescending(p => p.Team),
                PlayerSorting.Position => playerQuery.OrderBy(p => p.Position),
                PlayerSorting.Description  or _ => playerQuery.OrderByDescending(p => p.Id)
                //CarSorting.DateCreated or _ => playerQuery.OrderByDescending(p => p.Id)
            };

            var totalPlayers = playerQuery.Count();

            var players = playerQuery
                .Skip((query.CurrentPage - 1) * AllPlayerQueryModel.PlayerPerPage)
                .Take(AllPlayerQueryModel.PlayerPerPage)
                .Select(p => new PlayerListingViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    Image = p.Image,
                    Age = p.Age,
                    Team = p.Team,
                    ShirtNumber = p.ShirtNumber,
                    Nationality = p.Nationality,
                    Position = p.Position.Name
                })
                .ToList();

            var playerTeams = this.data
                .Players
                .Select(p => p.Team)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            query.TotalPlayers = totalPlayers;
            query.Teams = playerTeams;
            query.Players = players;

            return View(query);
        }

        public IActionResult Add() => View(new AddPlayerFormModel
        {
            Positions = this.GetPlayerPositions()
        });

        [HttpPost]
        public IActionResult Add(AddPlayerFormModel player, IFormFile image)
        {
            //if (image != null || image.Length > 2 * 1024 * 1024)
            //{
            //    this.ModelState.AddModelError("Image", "The image is not valid. It is required should be less than 2 MB.");
            //}

            //var imageInMemory = new MemoryStream();
            //image.CopyTo(imageInMemory);
            //var imageBytes = imageInMemory.ToArray();

            //image.CopyTo(FileSystem.OpenWrite($"/images/{image.FileName}"));

            if (!this.data.Positions.Any(p => p.Id == player.PositionId))
            {
                this.ModelState.AddModelError(nameof(player.PositionId), "Position does not exist");
            }

            if (ModelState.IsValid)
            {
                player.Positions = this.GetPlayerPositions();

                return View(player);
            }

            var playerData = new Player
            {
                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                LastName = player.LastName,
                Weight = player.Weight,
                Height = player.Height,
                Goal = player.Goal,
                Age = player.Age,
                Image = player.Image,
                Nationality = player.Nationality,
                ShirtNumber = player.ShirtNumber,
                Description = player.Description,
                Team = player.Team,
                PositionId = player.PositionId,
            };

            this.data.Players.Add(playerData);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<PlayerPositionViewModel> GetPlayerPositions()
       => this.data
            .Positions
            .OrderBy(p => p.Name)
            .Select(p => new PlayerPositionViewModel
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToList();
    }
}
