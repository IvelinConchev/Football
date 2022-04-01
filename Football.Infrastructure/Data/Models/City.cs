namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.City;
    using static Data.DataConstants.DefaultLengthForKeyGuid;

    public class City
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(CityNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CityPostCodeMaxLength)]
        public string PostCode { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Desctription { get; set; }
    }
}