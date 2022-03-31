namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Data.DataConstants.Team;
    using static Data.DataConstants.DefaultLengthForKeyGuid;

    public class Team
    {
        [Key]
        [StringLength(DefaultLength)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(TeamNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [StringLength(TeamWebSiteMaxLength)]
        public string? WebSite { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        [StringLength(TeamHomeKitMaxLength)]
        public string HomeKit { get; set; }

        [Required]
        [StringLength(TeamAwayKitMaxLength)]
        public string AwayKit { get; set; }

        [StringLength(PlayerNickNameMaxLength)]
        public string NickName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(TeamAddressMaxLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(TeamHeadCoachMaxLength)]
        public string HeadCoach { get; set; }

        public int Champion { get; set; }

        public int Cup { get; set; }

        public int Win { get; set; }

        public int Defeats { get; set; }

        public Guid PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public IList<League> Leagues { get; set; } = new List<League>();
    }
}
