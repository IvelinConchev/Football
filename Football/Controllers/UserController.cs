﻿namespace Football.Controllers
{
    using Football.Core.Constants;
    using Football.Core.Contracts;
    using Football.Infrastructure.Data.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserService service;

        public UserController(
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager, 
            IUserService _service)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
            this.service = _service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await service.GetUsers();

            return Ok(users);
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
