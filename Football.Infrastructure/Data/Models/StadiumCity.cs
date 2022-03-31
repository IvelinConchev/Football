namespace Football.Infrastructure.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class StadiumCity
    {
        public Guid StadiumId { get; set; }

        [ForeignKey(nameof(StadiumId))]
        public Stadium Stadium { get; set; }

        public Guid CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
