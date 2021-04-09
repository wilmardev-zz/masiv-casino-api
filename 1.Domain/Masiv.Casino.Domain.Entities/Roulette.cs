using Masiv.Casino.Domain.Entities.Enums;
using System;

namespace Masiv.Casino.Domain.Entities
{
    public class Roulette
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string State { get; set; } = RouletteStatus.Pending.ToString();

        public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}