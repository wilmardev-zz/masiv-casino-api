namespace Masiv.Casino.Domain.Entities.Config
{
    public class AppSettings
    {
        public decimal MaxBetQuantityPerRoulette { get; set; }
        public decimal BetNumericFee { get; set; }
        public decimal BetColorFee { get; set; }
        public string RouletteCacheKey { get; set; }
        public string BetCacheKey { get; set; }
    }
}