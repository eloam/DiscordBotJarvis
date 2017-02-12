using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBotJarvis.Controllers.CommandsModule.EventArgs;
using DSharpPlus;

namespace DiscordBotJarvis.Controllers.CommandsModule
{
    public class CommandModule : IModule
    {
        internal static CommandModule Instance;

        internal CommandConfig Config { get; set; }

        public DiscordClient Client { get; set; }

        private readonly List<Command> _commands = new List<Command>();

        public CommandModule()
        {
            Config = new CommandConfig();

            Instance = this;
        }

        public CommandModule(CommandConfig config)
        {
            Config = config;

            Instance = this;
        }

        public void Setup(DiscordClient client)
        {
            Client = client;

            Client.MessageCreated += (sender, e) =>
            {
                if (((e.Message.Author.ID != Client.Me.ID && !Config.SelfBot) || (e.Message.Author.ID == Client.Me.ID && Config.SelfBot))
                        && e.Message.Content.StartsWith(Config.Prefix))
                {
                    string[] split = e.Message.Content.Split(new char[] { ' ' });
                    string cmdName = split[0].Substring(Config.Prefix.Length);

                    foreach (Command command in _commands)
                    {
                        if (command.Name == cmdName)
                        {
                            command.Execute(new CommandEventArgs(e.Message, command));
                        }
                    }
                }
            };
        }

        public Command AddCommand(string command, Func<CommandEventArgs, Task> Do)
        {
            Command cmd = new Command(command, Do);
            _commands.Add(cmd);

            Client.DebugLogger.LogMessage(LogLevel.Debug, "Command", $"Command added {command}", DateTime.Now);

            return cmd;
        }

        public Command AddCommand(string command, Action<CommandEventArgs> Do) => AddCommand(command, (x) => { Do(x); return Task.Delay(0); });
    }
}
