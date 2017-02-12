using System;
using System.Threading.Tasks;
using DiscordBotJarvis.Controllers.CommandsModule.EventArgs;

namespace DiscordBotJarvis.Controllers.CommandsModule
{
    public class Command
    {
        public string Name { get; set; }
        public Func<CommandEventArgs, Task> Func { get; set; }

        public Command(string command, Func<CommandEventArgs, Task> func)
        {
            Name = command;
            Func = func;
        }

        public void Execute(CommandEventArgs args)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Func(args);
                }
                catch (NotSupportedException ex)
                {
                    await args.Message.Respond($":warning: An error occurred: {ex.Message}");
                }
            });
        }
    }
}
