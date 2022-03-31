namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Players;
    using Football.Models.Teams;
    using Microsoft.AspNetCore.Mvc;
    using FileSystem = System.IO.File;
    public class PlayersController : Controller
    {
        private readonly FootballDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        public PlayersController(
            FootballDbContext _data,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.data = _data;
            this.webHostEnvironment = _webHostEnvironment;
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
        public IActionResult Add(AddPlayerFormModel player)
        {

            if (!this.data.Positions.Any(p => p.Id == player.PositionId))
            {
                this.ModelState.AddModelError(nameof(player.PositionId), "Position does not exist");
            }

            if (!ModelState.IsValid)
            {
                player.Positions = this.GetPlayerPositions();

                return View(player);
            }

            string stringFileName = UploadFile(player);

            var playerData = new Player
            {
                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                LastName = player.LastName,
                Weight = player.Weight,
                Height = player.Height,
                Goal = player.Goal,
                Age = player.Age,
                Image = stringFileName,
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

        private string UploadFile(AddPlayerFormModel model)
        {
            string fileName = null;
            if (model.Image != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return fileName;
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
