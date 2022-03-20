namespace Football.Infrastructure.Data.Identity
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class ApplicationUser : IdentityUser
    {
        [StringLength(ApplicationUserFirstNameMaxLength)]
        public string? FirstName { get; set; }

        [StringLength(ApplicationUserLaststNameMaxLength)]
        public string? LastName { get; set; }
    }
}
