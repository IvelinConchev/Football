namespace Football.Models.Cities
{
    using System.ComponentModel.DataAnnotations;

    public class AllCityQueryModel
    {

        public const int CityPerPage = 3;
        public string Team { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public CitySorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalCities { get; set; }
        public IEnumerable<string> Teams { get; set; }
        public IEnumerable<CityListingViewModel> Cities { get; set; }
    }
}
