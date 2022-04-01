namespace Football.Models.Cities
{
    using System.ComponentModel.DataAnnotations;
    using static Football.Infrastructure.Data.DataConstants.City;

    public class AddCityFormModel
    {
        [Display(Name = "City Name")]
        [Required]
        [StringLength(CityNameMaxLength, MinimumLength = CityNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Name { get; set; }

        [Display(Name = "Post Code")]
        [Required]
        [StringLength(CityPostCodeMaxLength, MinimumLength = CityPostCodeMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string PostCode { get; set; }

        public IFormFile Image { get; set; }

        [Display(Name = "Описание")]
        [Required()]
        [StringLength(int.MaxValue,
            MinimumLength = CityDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Display(Name = "Team")]
        public Guid TeamId { get; init; }

        public IEnumerable<CityTeamViewModel> Teams { get; set; }
    }
}

