namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command(
    "headless config list",
    Description = "Get the list of config files for headless tekla plugin"
)]
public sealed class ListHeadlessConfigCommand : CommandBase<ListHeadlessConfigPlugin>
{
    public override string ConfigName { get; init; } = null!;

    public override string? ModelName { get; init; }

    public override bool IsHeadlessMode { get; init; } = false;

    protected override ValueTask Execute(IConsole console, ListHeadlessConfigPlugin plugin)
    {
        plugin.RunPlugin();

        foreach (var configFileName in plugin.Result)
            console.Output.WriteLine(configFileName);

        return default;
    }
}
