namespace MultiTekla.Plugins.Headless.Config;

[Command(
    "headless config list",
    Description = "Get the list of config files for headless tekla plugin"
)]
public class ListHeadlessConfigCommand : CommandBase<ListHeadlessConfigPlugin>
{
    protected override ValueTask Execute(IConsole console, ListHeadlessConfigPlugin plugin)
    {
        plugin.RunPlugin();

        foreach (var configFileName in plugin.Result)
            console.Output.WriteLine(configFileName);

        return default;
    }

    [Obsolete("This property is meaningless in this command", true)]
    public new string? ConfigName { get; init; }

    [Obsolete("This property is meaningless in this command", true)]
    public new string? ModelName { get; init; }

    [Obsolete("This property is meaningless in this command", true)]
    public new bool IsHeadlessMode { get; init; }
}
