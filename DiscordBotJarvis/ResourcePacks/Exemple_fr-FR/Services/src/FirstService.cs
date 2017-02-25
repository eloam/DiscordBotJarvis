using DiscordBotJarvis.Services;

namespace ServiceTest
{
    public class FirstService : IService
    {
        public string Name => "Service de test";

        public string Do(string query, string queryProcessed, string currentCulture)
        {
            return "Ceci est un plugin test de Jarvis.";
        }
    }
}