namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TeamCity
    {
        public Guid TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }

        public Guid CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
