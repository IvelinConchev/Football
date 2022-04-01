namespace Football.Models.Stadiums
{
    using System.ComponentModel.DataAnnotations;

    public class AllStadiumQueryModel
    {
        public const int StadiumPerPage = 3;
        public string City { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public StadiumSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalStadiums { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<StadiumListingViewModel> Stadiums { get; set; }
    }
}

