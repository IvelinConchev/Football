namespace Football.Core.Models.Positions
{
    using Football.Core.Services.Positions;
    using System.ComponentModel.DataAnnotations;
    using static Football.Infrastructure.Data.DataConstants.Position;
    public class PositionFormModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(PositionNameMaxLength)]
        public string? Name { get; set; }

        public IEnumerable<PositionServiceModel> Positions { get; set; }
    }
}
