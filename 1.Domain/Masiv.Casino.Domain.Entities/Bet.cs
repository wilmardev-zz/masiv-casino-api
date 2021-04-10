using System;

namespace Masiv.Casino.Domain.Entities
{
    public class Bet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string RouletteId { get; set; }
        public string UserId { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalBetWinner { get; set; }
    }
}