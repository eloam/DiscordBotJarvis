using DiscordBotJarvis.Models.Commands;
using DSharpPlus;

namespace DiscordBotJarvis.EventArgs
{
    public class CommandEventArgs : System.EventArgs
    {
        public DiscordMessage Message { get; }
        public Command Command { get; }
        public string[] Arguments { get; }

        public CommandEventArgs(DiscordMessage message, Command command)
        {
            Message = message;
            Command = command;
            if (message.Content.Length > (CommandModule.Instance.Config.Prefix.Length + command.Name.Length))
            {
                string args = message.Content.Substring(CommandModule.Instance.Config.Prefix.Length + command.Name.Length + 1);
                Arguments = args.Split(new char[] { ' ' });
            }
        }
    }
}
