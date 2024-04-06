namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config delete", Description = "Delete the config file")]
public sealed class DeleteHeadlessConfigCommand : CommandBase<DeleteHeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME", Description = "Config name to delete")]
    public override required string ConfigName { get; init; }

    public override string? ModelName { get; init; }

    public override bool IsHeadlessMode { get; init; } = false;

    protected override ValueTask Execute(IConsole console, DeleteHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigName, };
        plugin.RunPlugin();

        console.Output.WriteLine(
            plugin.Result.Success ? "Removed successfully"
                : "Fail to remove" + $"config with name {ConfigName}"
        );

        return default;
    }
}
