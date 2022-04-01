namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Data.DataConstants.League;
    using static Data.DataConstants.DefaultLengthForKeyGuid;

    public class League
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(LeagueNameMaxLength)]
        public string Name { get; set; }

        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
    }
}
