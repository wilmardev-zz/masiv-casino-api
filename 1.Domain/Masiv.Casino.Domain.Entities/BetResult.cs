using Masiv.Casino.Domain.Entities.Enums;
using System;

namespace Masiv.Casino.Domain.Entities
{
    public class BetResult
    {
        public string RouletteId { get; set; }
        public string BetId { get; set; }
        public int? WinnerNumber { get; set; }
        public BetColor? WinnerColor { get; set; }
        public bool HasWinner { get; set; }
        public decimal TotalBet { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}