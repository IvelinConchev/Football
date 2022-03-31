namespace Football.Models.Teams
{
    using System.ComponentModel.DataAnnotations;

    public class AllTeamQueryModel
    {
        public const int TeamPerPage = 3;
        public string Team { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public TeamSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalTeams { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<TeamListingViewModel> Teams { get; set; }
    }
}
