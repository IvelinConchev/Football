namespace Football.Core.Services.Players
{
    public class PlayerDetailsServiceModel : PlayerServiceModel
    {
        public string Description { get; init; }

        public Guid PositionId { get; init; }

        public Guid ManagerId { get; init; }

        public string ManagerName { get; init; }

        public string UserId { get; init; }
    }
}