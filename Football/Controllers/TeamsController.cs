namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : Controller
    {
        public readonly FootballDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        public TeamsController(FootballDbContext _data, IWebHostEnvironment _webHostEnvironment)
        {
            this.data = _data;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult All([FromQuery] AllTeamQueryModel query)
        {
            var teamQuery = this.data.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Team))
            {
                teamQuery = teamQuery.Where(p => p.Name == query.Team);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                teamQuery = teamQuery.Where(p => p.Name.ToLower().Contains(query.SearchTerm.ToLower())
                || p.NickName.ToLower().Contains(query.SearchTerm.ToLower())
                || p.HeadCoach.ToLower().Contains(query.SearchTerm.ToLower())
                || p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            teamQuery = query.Sorting switch
            {
                TeamSorting.Name => teamQuery.OrderBy(p => p.Name),
                TeamSorting.Nickname => teamQuery.OrderByDescending(p => p.NickName),
                TeamSorting.Cup => teamQuery.OrderBy(p => p.Cup),
                TeamSorting.Champion => teamQuery.OrderBy(p => p.Champion)
            };

            var totalTeams = teamQuery.Count();

            var teams = teamQuery
                .Skip((query.CurrentPage - 1) * AllTeamQueryModel.TeamPerPage)
                .Take(AllTeamQueryModel.TeamPerPage)
                .Select(t => new TeamListingViewModel
                {
                    Id = t.Id,
                    Address = t.Address,
                    AwayKit = t.AwayKit,
                    Champion = t.Champion,
                    Cup = t.Cup,
                    Defeats = t.Defeats,
                    Description = t.Description,
                    HeadCoach = t.HeadCoach,
                    HomeKit = t.HomeKit,
                    Image = t.Image,
                    LogoUrl = t.LogoUrl,
                    Name = t.Name,
                    NickName = t.NickName,
                    WebSite = t.WebSite,
                    Win = t.Win
                })
                .ToList();

            var teamP = this.data
                .Teams
                .Select(t => t.Name)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            query.TotalTeams = totalTeams;
            query.Names = teamP;
            query.Teams = teams;


            return View(query);
        }
        public IActionResult Add() => View(new AddTeamFormModel
        {
            Players = this.GetPlayers(),
        });


        [HttpPost]
        public IActionResult Add(AddTeamFormModel team)
        {
            if (!ModelState.IsValid)
            {
                team.Players = this.GetPlayers();

                return View(team);
            }

            string stringFileName = UploadFile(team);

            var teamData = new Team
            {
                Name = team.Name,
                Address = team.Address,
                Image = stringFileName,
                WebSite = team.WebSite,
                HomeKit = team.HomeKit,
                AwayKit = team.AwayKit,
                Champion = team.Champion,
                Cup = team.Cup,
                Defeats = team.Defeats,
                Description = team.Description,
                HeadCoach = team.HeadCoach,
                NickName = team.NickName,
                LogoUrl = team.LogoUrl,
                Win = team.Win,
                PlayerId = team.PlayerId

            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private string UploadFile(AddTeamFormModel model)
        {
            string fileName = null;
            if (model.Image != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        private IEnumerable<TeamPlayerViewModel> GetPlayers()
       => this.data
            .Players
            .OrderBy(p => p.Team)
            .Select(p => new TeamPlayerViewModel
            {
                Id = p.Id,
                Team = p.Team
            })
            .ToList();
    }
}

