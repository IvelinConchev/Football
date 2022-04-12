namespace Football.Core.Services.Teams
{
    using Football.Core.Contracts;
    using System;
    using System.Collections.Generic;
    using Football.Core.Models.Teams;
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;

    public class TeamService : ITeamService
    {
        private readonly FootballDbContext data;

        public TeamService(FootballDbContext _data)
            => this.data = _data;

        public TeamQueryServiceModel All(
            string name,
            string searchTerm,
            TeamSorting sorting,
            int currentPage,
            int teamsPerPage)
        {
            var teamsQuery = this.data.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                teamsQuery = teamsQuery.Where(t => t.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teamsQuery = teamsQuery.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())
                || p.NickName.ToLower().Contains(searchTerm.ToLower())
                || p.HeadCoach.ToLower().Contains(searchTerm.ToLower())
                || p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            teamsQuery = sorting switch
            {
                TeamSorting.Name => teamsQuery.OrderBy(p => p.Name),
                TeamSorting.Nickname => teamsQuery.OrderByDescending(p => p.NickName),
                TeamSorting.Cup => teamsQuery.OrderBy(p => p.Cup),
                TeamSorting.Champion => teamsQuery.OrderBy(p => p.Champion)
            };

            var totalTeams = teamsQuery.Count();

            var teams = GetTeams(teamsQuery
                .Skip((currentPage - 1) * teamsPerPage)
                .Take(teamsPerPage));

            return new TeamQueryServiceModel
            {
                TotalTeams = totalTeams,
                CurrentPage = currentPage,
                TeamsPerPage = teamsPerPage,
                Teams = teams
            };
        }

        public TeamDetailsServiceModel Details(Guid id)
            => this.data
            .Teams
            .Where(t => t.Id == id)
            .Select(t => new TeamDetailsServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                Address = t.Address,
                AwayKit = t.AwayKit,
                HomeKit = t.HomeKit,
                Image = t.Image,
                Champion = t.Champion,
                Cup = t.Cup,
                Defeats = t.Defeats,
                Description = t.Description,
                HeadCoach = t.HeadCoach,
                LogoUrl = t.LogoUrl,
                Win = t.Win,
                NickName = t.NickName,
                PlayerName = t.Player.Team,
                UserId = t.Player.Manager.UserId
            })
            .FirstOrDefault();

        public Guid Create(string name, string image, string webSite, string logoUrl, string homeKit, string awayKit, string nickName, string description, string address, string headCoach, int champion, int cup, int win, int defeats, Guid playerId, Guid managerId)
        {
            var teamData = new Team
            {
                Name = name,
                Image = image,
                WebSite = webSite,
                LogoUrl = logoUrl,
                HomeKit = homeKit,
                AwayKit = awayKit,
                NickName = nickName,
                Description = description,
                Address = address,
                HeadCoach = headCoach,
                Champion = champion,
                Cup = cup,
                Win = win,
                Defeats = defeats,
                PlayerId = playerId,
                //ManagerId = managerId
            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return teamData.Id;
        }

        public bool Edit(Guid id, string name, string image, string webSite, string logoUrl, string homeKit, string awayKit, string nickName, string description, string address, string headCoach, int champion, int cup, int win, int defeats, Guid playerId)
        {
            var teamData = this.data.Teams.Find(id);
            if (teamData == null)
            {
                return false;
            }

            teamData.Name = name;
            teamData.Image = image;
            teamData.WebSite = webSite;
            teamData.LogoUrl = logoUrl;
            teamData.HomeKit = homeKit;
            teamData.AwayKit = awayKit;
            teamData.NickName = nickName;
            teamData.Description = description;
            teamData.Address = address;
            teamData.HeadCoach = headCoach;
            teamData.Champion = champion;
            teamData.Cup = cup;
            teamData.Win = win;
            teamData.Defeats = defeats;
            teamData.PlayerId = playerId;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<TeamServiceModel> ByUser(string userId)
        => GetTeams(this.data
            .Teams
            .Where(t => t.Player.Manager.UserId == userId));

        public bool IsByManager(Guid teamId, Guid managerId)
        => this.data
            .Teams
            .Any(t => t.Id == teamId && t.Player.ManagerId == managerId);
        public IEnumerable<string> AllPlayers()
       => this.data
            .Teams
            .Select(t => t.Name)
            .Distinct()
            .OrderBy(t => t)
            .ToList();
        public IEnumerable<TeamPlayersServiceModel> AllTeams()
        => this.data
            .Players
            .Select(p => new TeamPlayersServiceModel
            {
                Id = p.Id,
                Name = p.Team,
            });


        public bool PlayerExists(Guid playerId)
        => this.data
            .Players
            .Any(p => p.Id == playerId);

        public bool ManagerExists(Guid managerId)
        => this.data
            .Managers
            .Any(m => m.Id == managerId);

        private static IEnumerable<TeamServiceModel> GetTeams(IQueryable<Team> teamQuery)
             => teamQuery
             .Select(t => new TeamServiceModel
             {
                 Id = t.Id,
                 Name = t.Name,
                 Address = t.Address,
                 AwayKit = t.AwayKit,
                 Champion = t.Champion,
                 Cup = t.Cup,
                 Defeats = t.Defeats,
                 Description = t.Description,
                 HeadCoach = t.HeadCoach,
                 HomeKit = t.HomeKit,
                 Image = t.Image,
                 LogoUrl = t.LogoUrl,
                 NickName = t.NickName,
                 WebSite = t.WebSite,
                 Win = t.Win,
                 PlayerName = t.Player.FirstName
             })
            .ToList();

    }
}
