using Masiv.Casino.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Masiv.Casino.Domain.Entities
{
    public class Roulette
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString().ToUpper();

        public string State { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ClosedDate { get; set; }
    }
}