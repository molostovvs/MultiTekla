namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command(
    "headless config list",
    Description = "Get the list of config files for headless tekla plugin"
)]
public class ListHeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        var pluginValue = Plugin.Value;

        var configs = pluginValue.GetAllConfigNames();
        foreach (var config in configs)
            console.Output.WriteLine(config);

        return default;
    }

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; } = null!;
}