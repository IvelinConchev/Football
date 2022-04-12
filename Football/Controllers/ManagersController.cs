namespace Football.Controllers
{
    using Football.Extensions;
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Managers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ManagersController : Controller
    {
        private readonly FootballDbContext data;

        public ManagersController(FootballDbContext _data)
        {
            this.data = _data;
        }

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]

        public IActionResult Become(BecomeManagerFormModel manager)
        {
            var userId = this.User.Id();

            var userIdAlreadyManager = this.data
                .Managers
                .Any(m => m.UserId == userId);

            if (userIdAlreadyManager)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(manager);
            }

            var managerData = new Manager
            {
                Name = manager.Name,
                PhoneNumber = manager.PhoneNumber,
                UserId = userId,
            };

            this.data.Managers.Add(managerData);

            this.data.SaveChanges();

            return RedirectToAction("All", "Players");
        }
    }
}
