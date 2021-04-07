using Masiv.Casino.Domain.Entities.Enums;
using System;

namespace Masiv.Casino.Domain.Entities
{
    public class Roulette
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public RouletteStatus State { get; set; } = RouletteStatus.Pending;
        public DateTime CreationDate { get; set; }
    }
}