namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config print", Description = "Prints headless config")]
public class PrintHeadlessConfigCommand : CommandBase<GetHeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME", Description = "Config name to print")]
    public new required string? ConfigName { get; init; }

    protected override ValueTask Execute(IConsole console, GetHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigName, };
        plugin.RunPlugin();
        console.Output.WriteLine(plugin.Result);

        return default;
    }

    [Obsolete("This property is meaningless in this command", true)]
    public new string? ModelName { get; init; }

    [Obsolete("This property is meaningless in this command", true)]
    public new bool IsHeadlessMode { get; init; }
}
