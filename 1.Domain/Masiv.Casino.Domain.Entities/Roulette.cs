using Masiv.Casino.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Masiv.Casino.Domain.Entities
{
    public class Roulette
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string State { get; set; } = RouletteStatus.Pending.ToString();

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? CloseDate { get; set; }
    }
}