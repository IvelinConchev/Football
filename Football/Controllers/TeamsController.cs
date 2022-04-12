namespace Football.Controllers
{
    using Football.Core.Contracts;
    using Football.Core.Models.Teams;
    using Football.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : Controller
    {
        public readonly ITeamService teams;
        private readonly IManagerService managers;

        private readonly IWebHostEnvironment webHostEnvironment;

        public TeamsController(
            ITeamService _teams,
            IManagerService _managers,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.teams = _teams;
            this.managers = _managers;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All([FromQuery]
               AllTeamQueryModel query)
        {
            var queryResult = this.teams.All(
                query.Team,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllTeamQueryModel.TeamPerPage);

            var teamsPlayers = this.teams.AllPlayers();

            //TODO
            query.TeamCities = teamsPlayers;
            query.TotalTeams = queryResult.TotalTeams;
            query.Teams = queryResult.Teams;

            return View(query);
        }

        [Authorize]

        public IActionResult Mine()
        {
            var myTeams = this.teams.ByUser(this.User.Id());

            return View(myTeams);
        }

        [Authorize]

        public IActionResult Add()
        {
            //if (!this.managers.isManager(this.User.Id()))
            //{
            //    return RedirectToAction(nameof(ManagersController.Become),
            //        "Managers");
            //}

            return View(new TeamFormModel
            {
                //TODO
                Players = this.teams.AllTeams()
            });
        }

        [HttpPost]
        [Authorize]

        public IActionResult Add(TeamFormModel team)
        {
            var managerId = this.managers.IdByUser(this.User.Id());

            if (managerId.Equals(Guid.Empty))
            {
                return RedirectToAction(nameof(ManagersController.Become),
                    "Managers");
            }

            if (!this.teams.PlayerExists(team.PlayerId))
            {
                this.ModelState.AddModelError(nameof(team.PlayerId),
                    "Team does not exist.");
            }

            if (ModelState.IsValid)
            {
                //team.Players = this.teams.AllPlayers();

                return View(team);
            }

            string stringFileName = UploadFile(team);

            this.teams.Create(
                team.Name,
                stringFileName,
                team.WebSite,
                team.LogoUrl,
                team.HomeKit,
                team.AwayKit,
                team.NickName,
                team.Description,
                team.Address,
                team.HeadCoach,
                team.Champion,
                team.Cup,
                team.Win,
                team.Defeats,
                team.PlayerId,
                managerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]

        public IActionResult Edit(Guid id)
        {
            var userId = this.User.Id();

            if (!this.managers.isManager(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ManagersController.Become),
                    "Managers");
            }

            var team = this.teams.Details(id);

            if (team.UserId != userId)
            {
                return Unauthorized();
            }

            return View(new TeamFormModel
            {
                Name = team.Name,
                //Image = team.Image,
                WebSite = team.WebSite,
                LogoUrl = team.LogoUrl,
                HomeKit = team.HomeKit,
                AwayKit = team.AwayKit,
                NickName = team.NickName,
                Description = team.Description,
                Address = team.Address,
                HeadCoach = team.HeadCoach,
                Champion = team.Champion,
                Cup = team.Cup,
                Win = team.Win,
                Defeats = team.Defeats,
                PlayerId = team.PlayerId,
                Players = this.teams.AllTeams()
            });
        }

        [HttpPost]
        [Authorize]

        public IActionResult Edit(Guid id, TeamFormModel team)
        {
            var managerId = this.managers.IdByUser(this.User.Id());

            string stringFileName = UploadFile(team);

            if (managerId.Equals(Guid.Empty) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ManagersController.Become), "Managers");
            }

            if (!this.teams.PlayerExists(team.PlayerId))
            {
                this.ModelState.AddModelError(nameof(team.PlayerId),
                    "Player does not exist.");
            }

            if (ModelState.IsValid)
            {
                team.Players = this.teams.AllTeams();

                return View(team);
            }

            if (!this.teams.IsByManager(id, managerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.teams.Edit(
                id,
                team.Name,
                stringFileName,
                team.WebSite,
                team.LogoUrl,
                team.HomeKit,
                team.AwayKit,
                team.NickName,
                team.Description,
                team.Address,
                team.HeadCoach,
                team.Champion,
                team.Cup,
                team.Win,
                team.Defeats,
                team.PlayerId);

            return RedirectToAction(nameof(All));
        }

        private string UploadFile(TeamFormModel model)
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
    }
}

