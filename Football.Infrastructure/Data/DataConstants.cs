namespace Football.Infrastructure.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int ApplicationUserFirstNameMaxLength = 50;
            public const int ApplicationUserLaststNameMaxLength = 50;
        }
        public class DefaultLengthForKeyGuid
        {
            public const int DefaultLength = 36;
        }

        public class League
        {
            public const int LeagueNameMinLength = 3;
            public const int LeagueNameMaxLength = 30;
            public const int LeagueDescriptionMinLength = 2;
        }

        public class Team
        {
            public const int TeamNameMinLength = 3;
            public const int TeamNameMaxLength = 30;
            public const int TeamWebSiteMaxLength = 100;
            public const int TeamHomeKitMinLength = 2;
            public const int TeamHomeKitMaxLength = 100;
            public const int TeamAwayKitMinLength = 2;
            public const int TeamAwayKitMaxLength = 100;
            public const int TeamNickNameMinLength = 2;
            public const int PlayerNickNameMaxLength = 50;
            public const int TeamAddressMinLength = 5;
            public const int TeamAddressMaxLength = 100;
            public const int TeamDescriptionMinLength = 2;
            public const int TeamHeadCoachMaxLength = 100;
        }

        public class Stadium
        {
            public const int StadiumNameMinLength = 3;
            public const int StadiumNameMaxLength = 100;
            public const int StadiumAddressMinLength = 30;
            public const int StadiumAddressMaxLength = 100;
        }

        public class Player
        {
            public const int PlayerFirstNameMinLength = 2;
            public const int PlayerFirstNameMaxLength = 100;
            public const int PlayerMiddleNameMinLength = 2;
            public const int PlayerMiddleNameMaxLength = 100;
            public const int PlayerLastNameMinLength = 2;
            public const int PlayerLastNameMaxLength = 100;
            public const int PlayerPositionMaxLength = 20;
            public const int PlayerNationalityMinLength = 4;
            public const int PlayerNationalityMaxLength = 60;
            public const int PlayerDescriptionMinLength = 2;
            public const int PlayerAgeMinLength = 14;
            public const int PlayerAgeMaxLength = 45;
            public const int PlayerShirtNumberMinValue = 1;
            public const int PlayerShirtNumberMaxValue = 99;
            public const int PlayerTeamMinLength = 4;
            public const int PlayerTeamMaxLength = 30;
        }

        public class City
        {
            public const int CityNameMinLength = 3;
            public const int CityNameMaxLength = 100;
            public const int CityPostCodeMinLength = 5;
            public const int CityPostCodeMaxLength = 10;
            public const int CityDescriptionMinLength = 2;
        }
        //European League
        public const int EuropeanLeagueMaxLength = 25;

        public class Position
        {
            public const int PositionNameMaxLength = 50;
        }

        //Country TRY
        public const int CountryCodeCapitalMaxLength = 85;

        public const int CountryNameMaxLength = 100;
        public const int CountryNameMinLength = 100;
    }
}
