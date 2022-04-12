namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Football.Infrastructure.Data.DataConstants.Manager;
    public class Manager
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(ManagerNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength()]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Player> Players { get; set; } = new List<Player>();
    }
}
