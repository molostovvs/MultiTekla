namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command(
    "headless config list",
    Description = "Get the list of config files for headless tekla plugin"
)]
public class ListHeadlessConfigCommand : CommandBase<HeadlessConfigPlugin>
{
    protected override ValueTask Execute(IConsole console, HeadlessConfigPlugin plugin)
    {
        var configs = plugin.GetAllConfigNames();

        foreach (var config in configs)
            console.Output.WriteLine(config);

        return default;
    }
}