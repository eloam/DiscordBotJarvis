using DiscordBotJarvis.Extensions;

namespace DiscordBotJarvis.Models.Queries
{
    public class UserQuery
    {
        public string Query { get; }

        public string QueryProcessed { get; }

        public AuthorQuery Author { get; }

        public UserQuery(string query, AuthorQuery author)
        {
            Query = query;
            QueryProcessed = query.NormalizeUserQuery();
            Author = author;
        }
    }
}
