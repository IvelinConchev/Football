namespace Football.Core.Services.Positions
{
    public class PositionDetailsServiceModel : PositionServiceModel
    {
        //public Guid Id { get; set; }
        public string Description { get; init; }

        public Guid PositionId { get; init; }

        public Guid ManagerId { get; init; }

        public string ManagerName { get; init; }

        public string UserId { get; init; }
    }
}
