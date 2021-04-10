using System.ComponentModel.DataAnnotations;

namespace Masiv.Casino.Application.Contracts.DTO
{
    public class BetDTO
    {
        [Required]
        public string RouletteId { get; set; }

        public int? Number { get; set; }
        public string Color { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}