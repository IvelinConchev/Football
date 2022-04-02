namespace Football.Models.Positions
{
    using System.ComponentModel.DataAnnotations;
    using static Football.Infrastructure.Data.DataConstants.Position;
    public class AddPositionFormModel
    {
        [Required]
        [StringLength(PositionNameMaxLength)]
        public string? Name { get; set; }
    }
}
