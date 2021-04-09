using System.ComponentModel.DataAnnotations;

namespace Masiv.Casino.Domain.Entities.Enums
{
    public enum RouletteStatus
    {
        [Display(Name = "Open", Description = "Bet is Open")]
        Open = 0,

        [Display(Name = "Close", Description = "Bet is Closed")]
        Close = 1,

        [Display(Name = "Pending", Description = "Bet is Pending")]
        Pending = 2
    }
}