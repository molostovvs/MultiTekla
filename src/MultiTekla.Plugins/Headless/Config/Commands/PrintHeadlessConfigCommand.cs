namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config print", Description = "Prints headless config")]
public sealed class PrintHeadlessConfigCommand : CommandBase<GetHeadlessConfigPlugin>
{
    public override string? ModelName { get; init; }

    public override bool IsHeadlessMode { get; init; } = false;

    [CommandParameter(0, Name = "CONFIG NAME", Description = "Config name to print")]
    public override required string ConfigName { get; init; }

    protected override ValueTask Execute(IConsole console, GetHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigName, };
        plugin.RunPlugin();
        console.Output.WriteLine(plugin.Result);

        return default;
    }
}
