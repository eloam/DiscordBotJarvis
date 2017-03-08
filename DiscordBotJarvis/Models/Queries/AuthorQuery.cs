namespace DiscordBotJarvis.Models.Queries
{
    public class AuthorQuery
    {
        public string UserId { get; }

        public string UserName { get; }

        public bool IsBot { get; }

        public AuthorQuery(string userId, string userName, bool isBot = false)
        {
            UserId = userId;
            UserName = userName;
            IsBot = isBot;
        }
    }
}
