namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config remove", Description = "Delete the config file")]
public class RemoveHeadlessConfigCommand : CommandBase<RemoveHeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME")]
    public required string ConfigNameToRemove { get; init; }

    protected override ValueTask Execute(IConsole console, RemoveHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigNameToRemove, };
        plugin.RunPlugin();

        console.Output.WriteLine(
            plugin.Result.Success ? "Removed successfully"
                : "Fail to remove" + $"config with name {ConfigNameToRemove}"
        );

        return default;
    }
}
