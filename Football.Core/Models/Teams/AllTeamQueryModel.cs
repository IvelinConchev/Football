namespace Football.Core.Models.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Football.Core.Services.Teams;

    public class AllTeamQueryModel
    {
        public const int TeamPerPage = 3;

        public string Team { get; init; }


        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public TeamSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalTeams { get; set; }

        public IEnumerable<string> TeamCities { get; set; }

        public IEnumerable<TeamServiceModel> Teams { get; set; }
    }
}
