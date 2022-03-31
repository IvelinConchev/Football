namespace Football.Models.Teams
{
    using System.ComponentModel.DataAnnotations;

    using static Football.Infrastructure.Data.DataConstants.Team;
    public class AddTeamFormModel
    {
        [Display(Name = "Име")]
        [Required]
        [StringLength(TeamNameMaxLength, MinimumLength = TeamNameMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Name { get; set; }

        //[Display(Name = "Image URL")]
        //[Required]
        //[Url]
        public IFormFile Image { get; set; }

        [Display(Name = "Уеб сайт")]
        public string? WebSite { get; set; }

        [Display(Name = "Лого")]
        [Required]
        public string LogoUrl { get; set; }

        [Display(Name = "Домакин екип")]
        [StringLength(int.MaxValue,
            MinimumLength = TeamHomeKitMinLength,
            ErrorMessage = "The field HomeKit must be a string with a minimum length of {2}.")]
        public string HomeKit { get; set; }

        [Display(Name = "Гост екип")]
        [StringLength(int.MaxValue,
            MinimumLength = TeamAwayKitMinLength,
            ErrorMessage = "The field AwayKit must be a string with a minimum length of {2}.")]
        public string AwayKit { get; set; }

        [Display(Name = "Прякор")]
        [StringLength(int.MaxValue,
            MinimumLength = TeamNickNameMinLength,
            ErrorMessage = "The field AwayKit must be a string with a minimum length of {2}.")]
        public string NickName { get; set; }

        [Display(Name = "Описание")]
        [Required()]
        [StringLength(int.MaxValue,
            MinimumLength = TeamDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        [Display(Name = "Адрес")]
        [Required]
        [StringLength(TeamAddressMaxLength, MinimumLength = TeamAddressMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string Address { get; set; }

        [Display(Name = "Старши треньор")]
        [Required]
        [StringLength(TeamAddressMaxLength, MinimumLength = TeamAddressMinLength, ErrorMessage = "Minimum: {2}, Maximum: {1}")]
        public string HeadCoach { get; set; }

        [Display(Name = "Шампиони")]
        public int Champion { get; set; }

        [Display(Name = "Купи")]
        public int Cup { get; set; }

        [Display(Name = "Победи")]
        public int Win { get; set; }

        [Display(Name = "Загуби")]
        public int Defeats { get; set; }

        [Display(Name = "Отбори на играчи")]
        public Guid PlayerId { get; init; }

        public IEnumerable<TeamPlayerViewModel> Players { get; set; }
    }
}
