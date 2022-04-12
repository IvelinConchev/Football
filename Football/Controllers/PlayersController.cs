namespace Football.Controllers
{
    using Football.Core.Contracts;
    using Football.Core.Models.Players;
    using Football.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    public class PlayersController : Controller
    {
        private readonly IPlayerService players;
        private readonly IManagerService managers;

        private readonly IWebHostEnvironment webHostEnvironment;

        public PlayersController(
           IManagerService managers,
            IPlayerService players,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.managers = managers;
            this.players = players;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All([FromQuery]
           AllPlayerQueryModel query)
        {
            var queryResult = this.players.All(
                query.Team,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllPlayerQueryModel.PlayerPerPage
                );

            var playersTeams = this.players.AllTeams();

            query.Teams = playersTeams;
            query.TotalPlayers = queryResult.TotalPlayers;
            query.Players = queryResult.Players;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myPlayers = this.players.ByUser(this.User.Id());

            return View(myPlayers);
        }

        [Authorize]
        public IActionResult Add()
        {
            //if (!this.managers.isManager(this.User.Id()))
            //{
            //    return RedirectToAction(nameof(ManagersController.Become), "Managers");
            //}

            return View(new PlayerFormModel
            {
                Positions = this.players.AllPositions()
            });
        }

        [HttpPost]
        [Authorize]

        public IActionResult Add(PlayerFormModel player)
        {
            //TODO
            var managerId = this.managers.IdByUser(this.User.Id());

            if (managerId.Equals(Guid.Empty))
            {
                return RedirectToAction(nameof(ManagersController.Become), "Managers");
            }

            if (!this.players.PositionExists(player.PositionId))
            {
                this.ModelState.AddModelError(nameof(player.PositionId), "Player does not existt.");
            }

            if (ModelState.IsValid)
            {
                player.Positions = this.players.AllPositions();

                return View(player);
            }

            string stringFileName = UploadFile(player);

            this.players.Create(
            player.FirstName,
            player.MiddleName,
            player.LastName,
            player.Age,
            player.Description,
            player.Goal,
            player.Team,
            player.Height,
            player.Weight,
            stringFileName,
            player.Nationality,
            player.ShirtNumber,
            player.PositionId,
            managerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(Guid id)
        {
            var userId = this.User.Id();

            if (!this.managers.isManager(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ManagersController.Become), "Managers");
            }

            var player = this.players.Details(id);

            if (player.UserId != userId)
            {
                return Unauthorized();
            }

            return View(new PlayerFormModel
            {

                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                LastName = player.LastName,
                Age = player.Age,
                Description = player.Description,
                Goal = player.Goal,
                Team = player.Team,
                Height = player.Height,
                Weight = player.Weight,
                //Image = IFormFile,
                Nationality = player.Nationality,
                ShirtNumber = player.ShirtNumber,
                PositionId = player.PositionId,
                Positions = this.players.AllPositions()
            }); ;
        }

        [HttpPost]
        [Authorize]

        public IActionResult Edit(Guid id, PlayerFormModel player)
        {
            var managerId = this.managers.IdByUser(
                this.User.Id());

            string stringFileName = UploadFile(player);

            if (managerId.Equals(0) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ManagersController.Become), "Managers");
            }

            if (!this.players.PositionExists(player.PositionId))
            {
                this.ModelState.AddModelError(nameof(player.PositionId), "Position does not exist.");
            }

            if (ModelState.IsValid)
            {
                player.Positions = this.players.AllPositions();

                return View(player);
            }

            if (!this.players.IsByManager(id, managerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.players.Edit(
                id,
               player.FirstName,
               player.MiddleName,
               player.LastName,
               player.Age,
               player.Description,
               player.Goal,
               player.Team,
               player.Weight,
               player.Height,
               stringFileName,
               player.Nationality,
               player.ShirtNumber,
               player.PositionId);

            return RedirectToAction(nameof(All));
        }

        private string UploadFile(PlayerFormModel model)
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
