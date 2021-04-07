using System.ComponentModel;

namespace Masiv.Casino.Domain.Entities.Enums
{
    public enum RouletteStatus
    {
        [Description("Open")]
        Open = 0,

        [Description("Close")]
        Close = 1,

        [Description("Pending")]
        Pending = 2
    }
}