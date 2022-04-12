namespace Football.Controllers
{
    using System.Diagnostics;
    using Football.Core.Constants;
    using Football.Core.Models;
    using Football.Infrastructure.Data;
    using Football.Models;
    using Football.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : BaseController
    {
        private readonly FootballDbContext data;

        private readonly ILogger<HomeController> logger;
        public HomeController(
            FootballDbContext _data,
            ILogger<HomeController> _logger)
        {
            this.data = _data;
            logger = _logger;
        }

        public IActionResult Index()
        {
            var totalPlayers = this.data.Players.Count();

            var players = this.data
                .Players
                .OrderByDescending(p => p.Id)
                .Select(p => new PlayerIndexViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    ImageUrl = p.Image,
                    Age = p.Age,
                    Nationality = p.Nationality,
                    Team = p.Team,
                    Position = p.Position.Name
                })
                .Take(3)
                .ToList();


            ViewData[MessageConstant.SuccessMessage] = "Браво";

            return View(new IndexViewModel
            {
                TotalPlayers = totalPlayers,
                Players = players
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}