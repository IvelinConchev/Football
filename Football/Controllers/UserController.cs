namespace Football.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(
            RoleManager<IdentityRole> _roleManager)
        {
            this.roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateRole()
        {
           //await roleManager.CreateAsync(new IdentityRole()
           // {
           //     Name = "Administrator"
           // });

            return Ok();
        }
    }
}
