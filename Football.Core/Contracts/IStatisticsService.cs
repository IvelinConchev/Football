namespace Football.Core.Contracts
{
    using Football.Core.Services.Statistics;

    public interface IStatisticsService
    {
        StatisticsServiceModel Total();
    }
}
