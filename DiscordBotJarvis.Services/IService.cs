using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotJarvis.Services
{
    public interface IService
    {
        string Name { get; }
        string Do(string query, string queryProcessed, string currentCulture);
    }
}
