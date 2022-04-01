namespace Football.Models.Leagues
{
    using System.ComponentModel.DataAnnotations;

    using static Football.Infrastructure.Data.DataConstants.League;

    public class AddLeagueFormModel
    {
        [Display(Name = "Име")]
        [Required]
        [StringLength(LeagueNameMaxLength, MinimumLength = LeagueNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Name { get; set; }

        [Display(Name = "Снимка")]
        public IFormFile Image { get; set; }

        [Display(Name = "Описание")]
        [Required()]
        [StringLength(int.MaxValue,
            MinimumLength = LeagueDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        [Display(Name = "Отбор")]
        public Guid TeamId { get; init; }
        public IEnumerable<LeagueTeamViewModel> Teams { get; set; }
    }
}