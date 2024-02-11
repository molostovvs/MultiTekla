using System.Linq;
using System.Threading.Tasks;
using CliFx;
using MultiTekla.Core;

namespace MultiTekla.CLI;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var pluginComposition = new PluginManager().ConfigurePluginComposition();
        using var pluginsContainer = pluginComposition.CreateContainer();
        var cliCommands = pluginsContainer.GetExports<ICommand>().ToArray();

        return await ConfigureCliApp(cliCommands).RunAsync();
    }

    private static CliApplication ConfigureCliApp(ICommand[] cliCommands)
        => new CliApplicationBuilder()
           .AddCommands(cliCommands.Select(c => c.GetType()))
           .UseTypeActivator(
                type =>
                {
                    var export = cliCommands.First(c => c.GetType() == type);
                    return export;
                }
            )
           .Build();
}