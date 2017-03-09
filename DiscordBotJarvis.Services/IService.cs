namespace DiscordBotJarvis.Services
{
    public interface IService
    {
        string Name { get; }
        string Do(string query, string queryProcessed, string currentCulture);
    }
}
