namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config remove", Description = "Delete the config file")]
public class RemoveHeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME")]
    public required string ConfigNameToRemove { get; init; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var pluginValue = Plugin.Value;

        var res = pluginValue.Remove(ConfigNameToRemove);
        if (res.success)
            console.Output.WriteLine($"{res.configFileName} removed successfully");
        return default;
    }

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; } = null!;
}