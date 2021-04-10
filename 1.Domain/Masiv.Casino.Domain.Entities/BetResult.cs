using System;
using System.Collections.Generic;

namespace Masiv.Casino.Domain.Entities
{
    public class BetResult
    {
        public string RouletteId { get; set; }
        public List<Bet> Bets { get; set; }
        public int? WinnerNumber { get; set; }
        public string WinnerColor { get; set; }
        public bool HasWinner { get; set; }
        public decimal TotalBetWinners { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}