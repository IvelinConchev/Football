namespace Football.Models.Players
{
    using System.ComponentModel.DataAnnotations;

    using static Football.Infrastructure.Data.DataConstants.Player;
    public class AddPlayerFormModel
    {
        [Display(Name = "First Name")]
        [Required]
        [StringLength(PlayerFirstNameMaxLength, MinimumLength = PlayerFirstNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string FirstName { get; init; }

        [Display(Name = "Middle Name")]
        [Required]
        [StringLength(PlayerMiddleNameMaxLength, MinimumLength = PlayerMiddleNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string MiddleName { get; init; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(PlayerLastNameMaxLength, MinimumLength = PlayerLastNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string LastName { get; init; }

        [Range(PlayerAgeMinLength, PlayerAgeMaxLength)]
        public int Age { get; init; }

        public double Weight { get; init; }

        public double Height { get; init; }

        [Display(Name = "Image")]
        [Required]
        [Url]
        public string Image { get; init; }

        public int Goal { get; init; }

        [Display(Name = "Shirt Number")]
        [Range(PlayerShirtNumberMinValue, PlayerShirtNumberMaxValue, ErrorMessage = "Minimum: {1}, Maximum: {2}")]
        public byte ShirtNumber { get; init; }

        [Required]
        [StringLength(PlayerNationalityMaxLength, MinimumLength = PlayerNationalityMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Nationality { get; init; }

        [Required]
        [StringLength(PlayerTeamMaxLength, MinimumLength = PlayerTeamMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Team { get; init; }


        [Required()]
        [StringLength(int.MaxValue,
            MinimumLength = PlayerDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Display(Name = "Position")]
        public Guid PositionId { get; init; }

        public IEnumerable<PlayerPositionViewModel> Positions { get; set; }
    }
}
