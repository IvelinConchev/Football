namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Position;
    using static Data.DataConstants.DefaultLengthForKeyGuid;
    public class Position
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(PositionNameMaxLength)]
        public string Name { get; set; }

        public IList<Player> Players { get; set; } = new List<Player>();
    }
}
