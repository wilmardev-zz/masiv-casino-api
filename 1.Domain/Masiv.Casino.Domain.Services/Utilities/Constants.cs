namespace Masiv.Casino.Domain.Services.Utilities
{
    public static class Constants
    {
        public const string ROULETTE_NOT_FOUND = "ROULETTE_NOT_FOUND";
        public const string ROULETTE_NOT_FOUND_DESC = "The selected roulette not exist.";
        public const string ROULETTE_NOT_OPEN = "ROULETTE_NOT_OPEN";
        public const string ROULETTE_NOT_OPEN_DESC = "The selected roulette is not open to play.";
        public const string ROULETTE_IS_CLOSED = "ROULETTE_IS_CLOSED";
        public const string ROULETTE_IS_CLOSED_DESC = "The selected roulette has been closed.";
        public const string ROULETTE_ALREADY_OPEN = "ROULETTE_ALREADY_OPEN";
        public const string ROULETTE_ALREADY_OPEN_DESC = "The selected roulette is already open.";
        public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
        public const string INTERNAL_SERVER_ERROR_DESC = "Something went wrong. Please try again.";
        public const string SQL_EXCEPTION = "INTERNAL_DB_ERROR";
    }
}