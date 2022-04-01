namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Leagues;
    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : Controller
    {
        private readonly FootballDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        public LeaguesController(
            FootballDbContext _data,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.data = _data;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllLeagueQueryModel query)
        {
            var leagueQuery = this.data.Stadiums.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Team))
            {
                leagueQuery = leagueQuery.Where(l => l.Name == query.Team);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                leagueQuery = leagueQuery.Where(l => l.Name.ToLower().Contains(query.SearchTerm.ToLower())
                || l.Address.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            leagueQuery = query.Sorting switch
            {
                LeagueSorting.Name => leagueQuery.OrderBy(l => l.Name),
                _ => leagueQuery.OrderByDescending(l => l.Id)
            };

            var totalLeagues = leagueQuery.Count();

            var leagues = leagueQuery
                .Skip((query.CurrentPage - 1) * AllLeagueQueryModel.LeaguePerPage)
                .Take(AllLeagueQueryModel.LeaguePerPage)
                .Select(l => new LeagueListingViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    
                })
                .ToList();

            var leagueTeams = this.data
                .Leagues
                .Select(l => l.Name)
                .Distinct()
                .OrderBy(l => l)
                .ToList();

            query.TotalLeagues = totalLeagues;
            query.Teams = leagueTeams;
            query.Leagues = leagues;

            return View(query);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add() => View(new AddLeagueFormModel
        {
            Teams = this.GetLaegueTeams()
        });

        [HttpPost]
        public IActionResult Add(AddLeagueFormModel league)
        {
            if (!ModelState.IsValid)
            {
                league.Teams = this.GetLaegueTeams();

                return View(league);
            }

            string stringFileName = UploadFile(league);

            var leagueData = new League
            {
                Name = league.Name,
                Image = stringFileName,
                Description = league.Description,
                TeamId = league.TeamId,
            };

            this.data.Add(leagueData);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private string UploadFile(AddLeagueFormModel model)
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

        private IEnumerable<LeagueTeamViewModel> GetLaegueTeams()
            => this.data
            .Teams
            .OrderBy(t => t.Name)
            .Select(t => new LeagueTeamViewModel
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToList();

    }
}
