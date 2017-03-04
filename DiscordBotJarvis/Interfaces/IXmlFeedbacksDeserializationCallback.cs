using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;

namespace DiscordBotJarvis.Interfaces
{
    public interface IXmlFeedbacksDeserializationCallback : IXmlDeserializationCallback
    {
        Feedback[] Feedbacks { get; set; }
    }
}