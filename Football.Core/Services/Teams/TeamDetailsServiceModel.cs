namespace Football.Core.Services.Teams
{
    using System;

    public class TeamDetailsServiceModel : TeamServiceModel
    {
        public string Description { get; init; }

        public Guid PlayerId { get; init; }

        public Guid ManagerId { get; init; }

        public string ManagerName { get; init; }

        public string UserId { get; init; }

        public string Team { get; init; }
      }
}
