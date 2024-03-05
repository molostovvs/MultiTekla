namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config remove", Description = "Delete the config file")]
public class RemoveHeadlessConfigCommand : CommandBase<HeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME")]
    public required string ConfigNameToRemove { get; init; }

    protected override ValueTask Execute(IConsole console, HeadlessConfigPlugin plugin)
    {
        var res = plugin.Remove(ConfigNameToRemove);

        if (res.success)
            console.Output.WriteLine($"{res.configFileName} removed successfully");

        return default;
    }
}