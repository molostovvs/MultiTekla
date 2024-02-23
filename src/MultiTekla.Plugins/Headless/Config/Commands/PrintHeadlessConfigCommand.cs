namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config print", Description = "Prints headless config")]
public class PrintHeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    [CommandOption("name", 'n', Description = "Name of the config file")]
    public string ConfigName { get; init; } = "default";

    public ValueTask ExecuteAsync(IConsole console)
    {
        var plugin = Plugin.Value;
        var config = plugin.GetConfigWithName(ConfigName);

        console.Output.WriteLine(config);

        return default;
    }

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; } = null!;
}