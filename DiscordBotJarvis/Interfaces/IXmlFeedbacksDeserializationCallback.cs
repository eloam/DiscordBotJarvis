using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;

namespace DiscordBotJarvis.Interfaces
{
    public interface IXmlFeedbacksDeserializationCallback : IXmlDeserializationCallback
    {
        Feedback[] Feedbacks { get; set; }
    }
}