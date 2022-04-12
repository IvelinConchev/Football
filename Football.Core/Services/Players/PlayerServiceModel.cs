namespace Football.Core.Services.Players
{
    public class PlayerServiceModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Team { get; set; }

        public int Age { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string Image { get; set; }

        public int Goal { get; set; }

        public byte ShirtNumber { get; set; }

        public string Nationality { get; set; }

        public string PositionName { get; set; }
    }
}
