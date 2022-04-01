namespace Football.Models.Leagues
{
    using System.ComponentModel.DataAnnotations;

    public class AllLeagueQueryModel
    {
        public const int LeaguePerPage = 3;
        public string Team { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public LeagueSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalLeagues { get; set; }
        public IEnumerable<string> Teams { get; set; }
        public IEnumerable<LeagueListingViewModel> Leagues { get; set; }
    }
}
