namespace Football.Models.Managers
{
    using System.ComponentModel.DataAnnotations;

    using static Football.Infrastructure.Data.DataConstants.Manager;

    public class BecomeManagerFormModel
    {
        [Required]
        [StringLength(ManagerNameMaxLength, MinimumLength = ManagerNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
