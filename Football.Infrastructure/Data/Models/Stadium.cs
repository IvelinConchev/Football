namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Stadium;
    using static Data.DataConstants.DefaultLengthForKeyGuid;
    public class Stadium
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(StadiumAddressMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int Capacity { get; set; }

        [Required]
        [StringLength(StadiumAddressMaxLength)]
        public string Address { get; set; }
    }
}
