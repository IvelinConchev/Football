namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Data.DataConstants.Player;
    using static Data.DataConstants.DefaultLengthForKeyGuid;

    public class Player
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(PlayerFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(PlayerMiddleNameMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(PlayerLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(PlayerTeamMaxLength)]
        public string Team { get; set; }

        public int Age { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int Goal { get; set; }

        public byte ShirtNumber { get; set; }

        [Required]
        [StringLength(PlayerNationalityMaxLength)]
        public string Nationality { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; }

        public IList<Team> Teams { get; set; } = new List<Team>();
    }
}
