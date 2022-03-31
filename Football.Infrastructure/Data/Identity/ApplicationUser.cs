namespace Football.Infrastructure.Data.Identity
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static Data.DataConstants.User;

    public class ApplicationUser : IdentityUser
    {
        [StringLength(ApplicationUserFirstNameMaxLength)]
        public string? FirstName { get; set; }

        [StringLength(ApplicationUserLaststNameMaxLength)]
        public string? LastName { get; set; }
    }
}
