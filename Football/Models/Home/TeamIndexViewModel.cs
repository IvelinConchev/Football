namespace Football.Models.Home
{
    public class TeamIndexViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string? WebSite { get; set; }

        public string LogoUrl { get; set; }

        public string HomeKit { get; set; }

        public string AwayKit { get; set; }

        public string NickName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string HeadCoach { get; set; }

        public int Champion { get; set; }

        public int Cup { get; set; }

        public int Win { get; set; }

        public int Defeats { get; set; }
    }
}
